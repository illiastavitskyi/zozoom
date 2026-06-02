using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoZoom.Pages
{
    [Authorize]
    public class ChatModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ChatModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string CurrentUserId { get; set; }
        public string CurrentUserName { get; set; }
        public List<IdentityUser> OtherUsers { get; set; } = new();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            CurrentUserId = user?.Id;
            CurrentUserName = user?.UserName;

            OtherUsers = _userManager.Users.Where(u => u.Id != CurrentUserId).ToList();
        }
    }
}