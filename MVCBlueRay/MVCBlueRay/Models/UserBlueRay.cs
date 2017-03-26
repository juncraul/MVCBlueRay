using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBlueRay.Models
{
    public class UserBlueRay
    {
        [Key]
        public int Id { get; set; }
        public int BluRayId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual BluRay BluRay { get; set; }
    }
}