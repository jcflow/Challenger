using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Team
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public Team()
        {
        }
    }
}
