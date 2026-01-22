using Microsoft.AspNetCore.Identity;

namespace TemplateJwtProject.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Playlist> PlayLists { get; set; } = new List<Playlist>();

}
