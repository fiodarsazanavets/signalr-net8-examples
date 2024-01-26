using OpenIddict.Abstractions;
using SsoIdp.Data;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace SsoIdp;

public class InitialDataSeed : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public InitialDataSeed(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("aspNetCoreAuth", cancellationToken) == null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "aspNetCoreAuth",
                ClientSecret = "some_secret",
                ConsentType = ConsentTypes.Explicit,
                DisplayName = "ASP.NET Core client application",
                RedirectUris =
                {
                    new Uri("https://localhost:5000/signin-oidc")
                },
                PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:5000/signout-oidc")
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                },
                Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange
                }
            }, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}