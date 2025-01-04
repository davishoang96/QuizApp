using Auth0.AspNetCore.Authentication;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FastEndpoints;
using FastEndpoints.ClientGen;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using NJsonSchema.CodeGeneration.CSharp;
using QuizApp;
using QuizApp.Api;
using QuizApp.AuthenticationStateSyncer;
using QuizApp.Components;
using QuizApp.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new RepositoryModule());
        });

#region Auth0 Setup
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenHandler>();
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
});

var baseUrl = builder.Configuration["BaseUrl"];

builder.Services.AddHttpClient("QuizAppApi", client =>
{
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<IQuizApiClient>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("QuizAppApi");
    return new QuizApiClient(baseUrl, httpClient);
});

builder.Services.AddFastEndpoints().SwaggerDocument(o =>
{
    o.ShortSchemaNames = true; // prevent adding namespace as prefix to classes.
    o.DocumentSettings = s => s.DocumentName = "QuizAppApi"; //must match doc name below
});

#endregion

builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var dataSource = builder.Configuration["DataSource"];
builder.Services.AddDbContext<QuizContext>(options =>
    options.UseSqlite($"Data Source={dataSource}"));

var app = builder.Build();

// Apply migration
using (var scope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<QuizContext>().Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Enable Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(QuizApp.Client._Imports).Assembly);

app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

    await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("/Account/Logout", async (HttpContext httpContext) =>
{
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri("/")
            .Build();

    await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

// Use FastEndpoints
app.UseFastEndpoints(c =>
{
    c.Endpoints.ShortNames = true;
    c.Serializer.Options.PropertyNamingPolicy = null;
}).UseSwaggerGen();

await app.GenerateClientsAndExitAsync(
    documentName: "QuizAppApi",
    destinationPath: "../QuizApp.Api/",
    csSettings: c =>
    {
        c.ClassName = "QuizApiClient";
        c.InjectHttpClient = true;
        c.GenerateClientInterfaces = true;
        c.GenerateDtoTypes = false;
        c.CSharpGeneratorSettings.Namespace = "QuizApp.Api";
        c.CSharpGeneratorSettings.JsonLibrary = CSharpJsonLibrary.NewtonsoftJson;
    },
    tsSettings: null);

app.Run();
