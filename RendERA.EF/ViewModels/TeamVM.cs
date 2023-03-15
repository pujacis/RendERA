using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RendERA.DB.ViewModels
{
    public class TeamVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Degination")]
        public string Degination { get; set; }

        public string PartnerName { get; set; }
        public Guid PartnerId { get; set; }
        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int assignJobCount { get; set; }
    }
}
