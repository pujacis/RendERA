using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace RendERA.DB.ViewModels
{
    public class DocumentVM
    {
        public DocumentVM()
        {
            Partners = new List<SelectListItem>();
            Teams = new List<SelectListItem>();
            Projects = new List<SelectListItem>();
           
        }
        public Microsoft.AspNetCore.Http.IFormFile file  { get;set; }
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUploadUrl { get; set; }
        public string FileDownloadUrl { get; set; }
        public string UploadedByName { get; set; }
        public Guid UploadedBy { get; set; }
        public DateTime DatePosted { get; set; }
        public Guid? AssignedPartnerId { get; set; }
        public string TrackingId { get; set; }
        public int UserType { get; set; }
        public int status { get; set; }
        //public string status { get; set; }
        public bool IsParameter { get; set; }
        public int? MessageId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AssignedTeamId { get; set; }
        public int? AssignedProjectId { get; set; }
        public bool? IsSubmitted { get; set; }
        public string TransactionId { get; set; }
        public IList<SelectListItem> Partners { get; set; }
        public IList<SelectListItem> Teams { get; set; }
        public IList<SelectListItem> Projects { get; set; }
        public List<ParametersVM> Parameters { get; set; }

    }
}
