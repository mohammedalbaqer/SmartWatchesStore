using Microsoft.AspNetCore.Identity;

namespace SmartWatchesStore.Models
{
    public class AppUser : IdentityUser
    {
        public string? Occupation { get; set; }
    }
}
