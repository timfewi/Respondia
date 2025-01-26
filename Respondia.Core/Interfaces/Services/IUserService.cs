using Respondia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Interfaces.Services
{
    /// <summary>
    /// Provides methods for managing and retrieving user information.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Asynchronously retrieves the current application user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the current <see cref="AppUser"/>.</returns>
        Task<AppUser?> GetCurrentUserAsync();

        /// <summary>
        /// Asynchronously retrieves the identifier of the current application user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the current user.</returns>
        Task<int?> GetCurrentUserIdAsync();
    }
}
