using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Entities
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? KhatmaId { get; set; }
        public virtual Khatma Khatma  { get; set; }
        //public int? UserCount { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
}
