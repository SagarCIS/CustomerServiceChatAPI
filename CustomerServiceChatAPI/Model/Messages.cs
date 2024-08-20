using System.ComponentModel.DataAnnotations;

namespace CustomerServiceChatAPI.Model
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }
        public string SenderName {  get; set; }
        public string Message { get; set; }
        public string ReceiverName {  get; set; }
        public DateTime DateTime { get; set; }

    }
}
