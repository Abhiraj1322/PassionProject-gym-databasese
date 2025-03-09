using System.ComponentModel.DataAnnotations;
namespace GymDatabase.Models
{
    public class Trainee
    {
        [Key]
        public int TrainerId { get; set; } 
        public string Name { get; set; } 
        public string Specialty { get; set; }

        public ICollection<MemeberTraining> MemberTrainings { get; set; }

    }
    public class TraineeDTO
    {
        public int TrainerId { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }

    }
}

