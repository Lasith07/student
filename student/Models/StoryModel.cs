using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoAPI.Models
{
    public class StoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long storyid { get; set; }

        public string title { get; set; }
        public string content { get; set; }
        public long UserId { get; set; }
    }
}
