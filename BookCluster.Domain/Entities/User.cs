using BookCluster.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookCluster.Domain.Entities
{
    public class User : IHaveId
    {
        [Key]
        public int Id { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
