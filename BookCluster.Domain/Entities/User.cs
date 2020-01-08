using System;
using System.Collections.Generic;
using System.Text;

namespace BookCluster.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
