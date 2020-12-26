using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration.Annotations;
using KhatmaBackEnd.Utilites.Enums;
using Newtonsoft.Json;

namespace KhatmaBackEnd.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

        public string Password { get; set; }
        [Required]
        public Roles Role { get; set; }
       public int? PageNo { get; set; }
       public bool? IsRead { get; set; }
        public DateTime? PageDistributedDate { get; set; }
        public DateTime? ReadedDate { get; set; }
        [ForeignKey("GroupId")]
        public int? GroupId { get; set; }
        [JsonIgnore]
        public virtual Group group { get; set; }

        [ForeignKey("KhatmaId")]
        public int KhatmaId { get; set; }
       [JsonIgnore]
        public virtual Khatma khatma { get; set; }

        //      public virtual ICollection<UserDevice> Devices { get; set; }
    }
}
