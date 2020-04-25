using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Tournament> Tournaments { get; set; }
    }
}
