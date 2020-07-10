using KhatmaBackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.ViewModels
{
    public class GroupViewModel:GroupBase
    {
        public User admin { get; set; }
    }
}
