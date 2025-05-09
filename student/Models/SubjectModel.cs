using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoAPI.Models
{
    [Table("Subject")]
    public class SubjectModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long subject_id { get; set; }

        [Required]
        public string subject_code { get; set; }

        [Required]
        public string subject_name { get; set; }

        [Required]
        public string in_charge { get; set; }
    }
}
