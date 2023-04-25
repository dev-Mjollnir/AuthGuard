using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace AuthGuard.AuthServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        readonly IOpenIddictApplicationManager _openIddictApplicationManager;

        public ClientController(IOpenIddictApplicationManager openIddictApplicationManager)
        {
            _openIddictApplicationManager = openIddictApplicationManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(Client model)
        {
            var client = await _openIddictApplicationManager.FindByClientIdAsync(model.ClientId);
            if (client is null)
            {
                await _openIddictApplicationManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = model.ClientId,
                    ClientSecret = model.ClientSecret,
                    DisplayName = model.DisplayName,
                    Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "read",
                    OpenIddictConstants.Permissions.Prefixes.Scope + "write",
                    OpenIddictConstants.Permissions.Prefixes.Scope + "delete",

                }
                });
                return Ok();
            }
            return Ok();
        }
    }
}
