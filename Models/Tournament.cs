using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Tournament
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
    }
}
