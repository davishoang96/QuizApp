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

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
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

app.Run();
