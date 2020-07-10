﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.ViewModels
{
    public class UserForAdd
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? PageNo { get; set; }
        public int GroupId { get; set; }
        public bool? IsRead { get; set; }
    }
}
