using Microsoft.AspNetCore.Mvc;
using ZoZoom.Data;
using ZoZoom.Models;

namespace ZoZoom.Controllers
{
    public class MeetingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MeetingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Meetings.ToList());

        //public IActionResult Create() => View();

        public IActionResult Create(string date)
        {
            var meeting = new Meeting();

            if (!string.IsNullOrEmpty(date))
            {
                // 🔹 Підставляємо вибраний день у поле початку
                meeting.StartTime = DateTime.Parse(date);
                meeting.EndTime = meeting.StartTime.AddHours(1); // наприклад, тривалість 1 година
            }

            return View(meeting);
        }


        [HttpPost]
        public IActionResult Create(Meeting meeting)
        {
            // Оскільки OrganizerId та MeetingLink заповнюються в коді, 
            // видаляємо їх з валідації моделі, щоб ModelState не ламався
            ModelState.Remove(nameof(meeting.OrganizerId));
            ModelState.Remove(nameof(meeting.MeetingLink));

            if (ModelState.IsValid)
            {
               
                meeting.OrganizerId = User.Identity?.Name ?? "admin";
                meeting.ParticipantIds = meeting.ParticipantIds ?? new List<string>();

                meeting.MeetingLink = string.Empty;

                _context.Meetings.Add(meeting);
                _context.SaveChanges();

                meeting.MeetingLink = Url.Action("Details", "Meeting", new { id = meeting.Id }, Request.Scheme);
                _context.SaveChanges();

                System.Diagnostics.Debug.WriteLine($"Title: {meeting.Title}");
                System.Diagnostics.Debug.WriteLine($"StartTime: {meeting.StartTime}");
                System.Diagnostics.Debug.WriteLine($"EndTime: {meeting.EndTime}");

                return RedirectToAction("Index", "Meeting");
            }

            return View(meeting);
        }
        public IActionResult Details(int id)
        {
            var meeting = _context.Meetings.FirstOrDefault(m => m.Id == id);
            if (meeting == null) return NotFound();

            return View(meeting);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var meeting = _context.Meetings.FirstOrDefault(m => m.Id == id);
            if (meeting == null) return NotFound();

            _context.Meetings.Remove(meeting);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public JsonResult GetEvents()
        {
            var meetings = _context.Meetings.Select(m => new {
                id = m.Id,
                title = m.Title,
                start = m.StartTime,
                end = m.EndTime,
                url = Url.Action("Details", "Meeting", new { id = m.Id })
            }).ToList();

            return Json(meetings);

        }

    }

}
