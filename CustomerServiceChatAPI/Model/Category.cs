using System.ComponentModel.DataAnnotations;

namespace CustomerServiceChatAPI.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Service { get; set; }
    }
}
