using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBlueRay.Models
{
    public class BluRay
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Release Year is required.")]
        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "Language is required.")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Run Time is required.")]
        public int RunTime { get; set; }

        public virtual ICollection<UserBlueRay> UserBlueRays { get; set; }
    }
}