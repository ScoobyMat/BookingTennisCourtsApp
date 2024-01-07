using System;
using System.ComponentModel.DataAnnotations;

namespace BookingTennisCourts.Data
{
    public class Reservation : BaseDomainEntity
    {

        public int? CourtId { get; set; }

        public virtual Court? Court { get; set; }

        public DateTime Data { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string? UserId { get; set; }
    }
}
