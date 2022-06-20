using System.ComponentModel.DataAnnotations;

namespace GameServerApi.Models
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }

        public int UserId { get; set; }

    }
}
