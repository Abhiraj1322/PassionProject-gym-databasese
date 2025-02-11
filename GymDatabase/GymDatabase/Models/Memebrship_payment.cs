using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GymDatabase.Models
{
    public class Memebrship_payment
    {
        [Key]
        public int PaymentId { get; set; }

        [ForeignKey("Membership")]
        public int MembershipId { get; set; }  
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
    public class Memebrship_paymentdto
    {
        
        public int PaymentId { get; set; }
        public int MembershipId { get; set; }
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
