namespace BookingTennisCourts.Data
{
    public class Court : BaseDomainEntity
    {
        public string Name { get; set; } 

        public string Info { get; set; } 

        public CourtStatus Status { get; set; }

        public float PricePerHour { get; set; }
    }

    public enum CourtStatus
    {
        Available,
        NotAvailable,
    }
}
