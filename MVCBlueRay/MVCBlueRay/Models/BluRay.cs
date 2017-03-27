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
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Release Year is required.")]
        [Range(1500, 3000, ErrorMessage = "Please enter valid year")]
        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "Language is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Language { get; set; }

        [Required(ErrorMessage = "Run Time is required.")]
        [Range(0, 1000000, ErrorMessage = "Please enter valid run time")]
        public int RunTime { get; set; }

        public virtual ICollection<UserBlueRay> UserBlueRays { get; set; }
    }
}