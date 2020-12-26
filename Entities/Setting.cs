using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Entities
{
    public class Setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int KhatmaCount { get; set; }
        public int? LastDistributedPage { get; set; }
        public int? KhatmaId { get; set; }
        public virtual Khatma Khatma { get; set; }
    }
}
