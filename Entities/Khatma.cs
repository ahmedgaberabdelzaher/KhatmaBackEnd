using System;
using System.ComponentModel.DataAnnotations;
using KhatmaBackEnd.Utilites.Enums;

namespace KhatmaBackEnd.Entities
{
    public class Khatma
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public KhatmaTypeEnum Type { get; set; }
    }
}
