using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Respondia.Core.Attributes;
using Respondia.Core.Entities;
using Respondia.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.BL.Services.UserServices
{
    [Injectable(ServiceLifetime.Scoped)]
    /// <inheritdoc/>
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILogger<UserService> _logger;
        private readonly NavigationManager _navigationManager;

        public UserService(
            UserManager<AppUser> userManager,
            AuthenticationStateProvider authenticationStateProvider,
            ILogger<UserService> logger,
            NavigationManager navigationManager
            )
        {
            _userManager = userManager;
            _authenticationStateProvider = authenticationStateProvider;
            _logger = logger;
            _navigationManager = navigationManager;
        }

        /// <inheritdoc/>
        public async Task<AppUser?> GetCurrentUserAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var principal = authState.User;

            if (principal?.Identity?.IsAuthenticated != true)
            {
                _logger.LogWarning("No authenticated user found.");
                return null;
            }

            var appUser = await _userManager.GetUserAsync(principal);
            if (appUser != null)
            {
                _logger.LogInformation("Authenticated user found: {UserName}", appUser.UserName);
                return appUser;
            }

            _logger.LogWarning("Authenticated user could not be found.");
            return null;
        }


        /// <inheritdoc/>
        public async Task<int?> GetCurrentUserIdAsync()
        {
            var user = await GetCurrentUserAsync();
            return user?.Id;
        }
    }
}
