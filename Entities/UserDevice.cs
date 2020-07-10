using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Entities
{
    public class UserDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public string DeviceToken { get; set; }
        public User User { get; set; }

    }
}
