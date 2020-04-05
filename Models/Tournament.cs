using System.Collections.Generic;

namespace Models
{
    public class Tournament
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<Bracket> Brackets { get; set; }

        public Tournament()
        {
            this.Brackets = new HashSet<Bracket>();
        }
    }
}
