using System.ComponentModel.DataAnnotations;

namespace ZoZoom.Data
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; } // Links to ApplicationUser.Id

        public string ReceiverId { get; set; } // Null if it's a group message
        public string GroupName { get; set; } // Null if it's a private message

        public string Content { get; set; }
        public string FileUrl { get; set; } // Populated if a file was sent

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Navigation property for the sender
        public virtual ApplicationUser Sender { get; set; }
    }
}