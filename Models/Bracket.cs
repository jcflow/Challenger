using System;

namespace Models
{
    public class Bracket
    {
        public int ID { get; set; }

        public bool Finished { get; set; }

        public int Level { get; set; }

        public Tournament Tournament { get; set; }

        public Bracket()
        {
        }
    }
}
