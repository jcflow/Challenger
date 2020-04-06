using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Bracket
    {
        [Key]
        public int ID { get; set; }

        public bool Finished { get; set; }

        public int Level { get; set; }

        public Tournament Tournament { get; set; }

        [ForeignKey("Tournament")]
        public int TournamentID { get; set; }

        public Bracket()
        {
        }
    }
}
