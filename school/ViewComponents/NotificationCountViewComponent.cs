using Microsoft.AspNetCore.Mvc;
using school.Services;
using System.Security.Claims;

namespace school.ViewComponents
{
    public class NotificationCountViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService;

        public NotificationCountViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdClaim = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                int count = await _notificationService.GetUnreadCountAsync(userId);
                return View(count);
            }

            return View(0);
        }
    }
}
