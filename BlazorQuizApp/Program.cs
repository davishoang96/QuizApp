using Auth0.AspNetCore.Authentication;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlazorQuizApp.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QuizApp.Database;
using Radzen;
using FastEndpoints;
using FastEndpoints.Swagger;
using FastEndpoints.ClientGen;
using NJsonSchema.CodeGeneration.CSharp;
using QuizApp.Api;
using QuizApp.Services;
using Microsoft.AspNetCore.Authorization;
using BlazorQuizApp.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserService, UserService>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterModule(new RepositoryModule());
        });

var dataSource = builder.Configuration["DataSource"];
builder.Services.AddDbContext<QuizContext>(options =>
    options.UseSqlite($"Data Source={dataSource}"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddRadzenComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
});

builder.Services.AddFastEndpoints().SwaggerDocument(o =>
{
    o.ShortSchemaNames = true; // prevent adding namespace as prefix to classes.
    o.DocumentSettings = s => s.DocumentName = "QuizAppApi"; //must match doc name below
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CookieHandler>();

var baseUrl = builder.Configuration["BaseUrl"];
builder.Services.AddHttpClient("QuizAppApi",
      client => client.BaseAddress = new Uri(baseUrl))
      .AddHttpMessageHandler<CookieHandler>();
builder.Services.AddTransient<CookieHandler>();
builder.Services.AddScoped<IQuizApiClient>(sp =>
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("QuizAppApi");
    return new QuizApiClient(baseUrl, httpClient);
});

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

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapGet("/Account/Login", async (HttpContext httpContext, string redirectUri = "/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(redirectUri)
            .Build();

    await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("/Account/Logout", async (HttpContext httpContext, string redirectUri = "/") =>
{
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri(redirectUri)
            .Build();

    await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorQuizApp.Client._Imports).Assembly);

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
        c.AdditionalNamespaceUsages = ["QuizApp.Common.DTO", "QuizApp.Common.Request"];
        c.CSharpGeneratorSettings.Namespace = "QuizApp.Api";
        c.CSharpGeneratorSettings.JsonLibrary = CSharpJsonLibrary.NewtonsoftJson;
    },
    tsSettings: null);

app.Run();
