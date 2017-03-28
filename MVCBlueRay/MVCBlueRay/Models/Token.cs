using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBlueRay.Models
{
    public class Token
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Code { get; set; }
    }
}