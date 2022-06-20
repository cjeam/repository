using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GameServerApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }




    }
}
