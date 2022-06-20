using System.ComponentModel.DataAnnotations;

namespace GameServerApi.Models
{
    public class SpawnedObjects
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int UserId { get; set; }
    }
}
