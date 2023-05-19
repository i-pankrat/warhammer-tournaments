using Microsoft.AspNetCore.Mvc;

namespace WarhammerTournaments.Models;

public class ProfileViewModel
{
    public string UserId { get; set; }

    [Remote("IsUserNameValid", "Profile", HttpMethod = "post")]
    public string UserName { get; set; }
}