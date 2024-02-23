using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTennisCourts
{
    public class Reservation : BaseDomainEntity
    {
        [ForeignKey("Court")]
        public int CourtId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public float FullPrice { get; set; }
        public Court Court { get; set; }
        public User User { get; set; }

    }
}
