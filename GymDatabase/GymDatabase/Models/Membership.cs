using GymDatabase.Data.Migrations;
using System.ComponentModel.DataAnnotations;
namespace GymDatabase.Models
{
    public class Membership
    {
        [Key]
        public int MembershipId { get; set; } 
        public string MemberName { get; set; }
        public string PlanType { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<MemeberTraining> MemberTrainings { get; set; }
        public ICollection<Memebrship_payment> Memebrship_payments { get; set; }

    }
    public class Membershipdto {
        public int MembershipId { get; set; }
        public string MemberName { get; set; }
        public string PlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

       



    }
}
