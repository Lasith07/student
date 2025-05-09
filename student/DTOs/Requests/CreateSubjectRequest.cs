using System.ComponentModel.DataAnnotations;

namespace DemoAPI.DTOs.Requests

{

    public class CreateSubjectRequest

    {

        [Required]

        public long subject_id { get; set; }

        [Required]

        public string subject_code { get; set; }

        [Required]

        public string subject_name { get; set; }

        [Required]

        public string in_charge { get; set; }

    }
}