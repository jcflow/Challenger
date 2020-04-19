using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class TournamentCategory
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
    }
}
