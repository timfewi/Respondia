using Microsoft.AspNetCore.Identity;

namespace Respondia.Core.Entities
{
    public class AppUser : IdentityUser<int>
    {
        [PersonalData]
        public string? DisplayName { get; set; }
        public string? ExternalProvider { get; set; }
        public string? ProviderKey { get; set; }

        public ICollection<Campaign>? Campaigns { get; set; } = new List<Campaign>();
    }

}
