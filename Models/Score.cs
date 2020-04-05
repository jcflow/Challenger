using System;

namespace Models
{
    public class Score
    {
        public int ID { get; set; }

        public int Value { get; set; }

        public Team Team { get; set; }

        public Bracket Bracket { get; set; }

        public Score()
        {
        }
    }
}
