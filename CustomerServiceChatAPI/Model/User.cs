using System.ComponentModel.DataAnnotations;

namespace CustomerServiceChatAPI.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }


        public string status { get; set; } = "process";
    }
}
