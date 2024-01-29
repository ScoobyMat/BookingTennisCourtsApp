namespace BookingTennisCourts.Data.Entities
{
    public class Court : BaseDomainEntity
    {
        public string Name { get; set; }

        public string Info { get; set; }

        public string Status { get; set; }

        public float PricePerHour { get; set; }
    }
}
