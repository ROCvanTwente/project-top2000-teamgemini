using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using TemplateJwtProject.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Playlist> PlayLists { get; set; } = new List<Playlist>();
}
