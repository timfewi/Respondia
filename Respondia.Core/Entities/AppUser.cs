using Microsoft.AspNetCore.Identity;

namespace Respondia.Core.Entities
{
    /// <summary>
    /// Represents an application user in the system.
    /// This class extends the IdentityUser class provided by ASP.NET Core Identity.
    /// </summary>
    public class AppUser : IdentityUser<int>
    {
        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        [PersonalData]
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the external provider used for authentication.
        /// </summary>
        public string? ExternalProvider { get; set; }

        /// <summary>
        /// Gets or sets the provider key for the external authentication provider.
        /// </summary>
        public string? ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the collection of campaigns associated with the user.
        /// </summary>
        public ICollection<Campaign>? Campaigns { get; set; } = new List<Campaign>();
    }

}
