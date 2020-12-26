using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhatmaBackEnd.Entities
{
    public class UserPages
    {

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public int? PageNo { get; set; }
            public bool? IRead { get; set; }
            public DateTime? PageDistributedDate  { get; set; }
            public DateTime? ReadedDate { get; set; }
    }
}
