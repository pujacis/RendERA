using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RendERA.DB.ViewModels
{
    public class ProjectVM
    {
        public ProjectVM()
        {
            StatusList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select status")]
        public int Status { get; set; }
        public string PartnerName { get; set; }
        public Guid PartnerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public IList<SelectListItem> StatusList { get; set; }
    }
}
