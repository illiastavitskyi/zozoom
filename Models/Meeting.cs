namespace ZoZoom.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Meeting
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the name of the meeting")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Add a description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Enter the start date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Enter the end date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }


        public string? OrganizerId { get; set; }
        public List<string>? ParticipantIds { get; set; }
        public string? MeetingLink { get; set; }
    }


}
