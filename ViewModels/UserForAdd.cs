using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhatmaBackEnd.Utilites.Enums;

namespace KhatmaBackEnd.ViewModels
{
    public class UserForAdd
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
        public int? PageNo { get; set; }
        public int? GroupId { get; set; }
        public bool? IsRead { get; set; }
        public int? KhatmaId { get; set; }
      public DateTime? PageDistributedDate { get; set; }
    }
}
