using System;
using KhatmaBackEnd.Utilites.Enums;

namespace KhatmaBackEnd.ViewModels
{
    public class UserData
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

        public string Password { get; set; }
        public Roles Role { get; set; }
        public int? PageNo { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? PageDistributedDate { get; set; }
        public DateTime? ReadedDate { get; set; }
        public int? GroupId { get; set; }
        public int KhatmaId { get; set; }
    }
}
