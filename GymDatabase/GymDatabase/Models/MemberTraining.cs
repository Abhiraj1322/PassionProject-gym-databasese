using GymDatabase.Data.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymDatabase.Models
{
    public class MemberTraining
    {
        [Key]
        public int MemberTrainingId { get; set; }
        [ForeignKey("Membership")]
        public int MembershipId { get; set; }
        [ForeignKey("Trainee")]
        public int TrainerId { get; set; }  
        public string TrainingType { get; set; }
        public Membership Membership { get; set; }
        public Trainee Trainee { get; set; }
    }
    public class MemberTrainingdto
    {
        public int MemberTrainingId { get; set; }
        [ForeignKey("Membership")]
        public int MembershipId { get; set; }
        [ForeignKey("Trainee")]
        public int TrainerId { get; set; }
        public string TrainingType { get; set; }
        public Membership Membership { get; set; }
        public Trainee Trainee { get; set; }

    }
}
