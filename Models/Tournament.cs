using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Tournament
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public TournamentCategory Category { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public User Administrator { get; set; }

        [ForeignKey("Administrator")]
        public int AdministratorID { get; set; }
    }
}
