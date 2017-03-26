using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBlueRay.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<CheckBloxViewModel> BluRays { get; set; }
    }
}