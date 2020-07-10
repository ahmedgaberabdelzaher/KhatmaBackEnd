using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.ViewModels
{
    public class ChangeReadStatusViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
