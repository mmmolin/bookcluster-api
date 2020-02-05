using BookCluster.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BookCluster.Domain.Entities
{
    public class User : IHaveId
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}
