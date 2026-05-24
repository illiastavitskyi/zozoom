using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZoZoom.Data;

namespace ZoZoom.Pages
{
    [Authorize] // Requires login
    public class ChatModel : PageModel
    {
        private readonly UserManager<ApplicationUser>
        _userManager;

        public ChatModel(UserManager<ApplicationUser>
        userManager)
        {
            _userManager = userManager;
        }

        public string CurrentUserId { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            CurrentUserId = user?.Id;
        }
    }
}