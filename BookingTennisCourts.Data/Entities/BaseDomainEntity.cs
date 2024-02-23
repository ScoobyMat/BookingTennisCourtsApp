using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTennisCourts
{
    public abstract class BaseDomainEntity
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateCreated { get; set; }
    }
}
