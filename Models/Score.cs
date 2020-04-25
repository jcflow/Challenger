using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Score
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int Value { get; set; }

        public Team Team { get; set; }

        [ForeignKey("Team")]
        public int TeamID { get; set; }

        public Bracket Bracket { get; set; }

        [ForeignKey("Bracket")]
        public int BracketID { get; set; }

        public Score()
        {
        }
    }
}
