using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RendERA.DB.ViewModels
{
    public class ServerListVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Server Name")]
        [Display(Name = "Server Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter IP Address")]
        [Display(Name = "IP Address")]
        public string IpAddress { get; set; }
    }
}
