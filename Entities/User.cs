using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public int? PageNo { get; set; }
        public bool? IsRead { get; set; }

        [ForeignKey("GroupId")]
        public int GroupId { get; set; }

  //      public virtual ICollection<UserDevice> Devices { get; set; }
    }
}
