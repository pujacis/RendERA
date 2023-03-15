using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Services.WebApi;
using PagedList.Core;

namespace RendERA.DB.ViewModels 
{
    public class MessageReplyVM
    {
        public MessageReplyVM()
        {
            UsersByType = new List<SelectListItem>();
        }
        private List<MessageReplyModel> _replies = new List<MessageReplyModel>();
        public MessageReplyModel Reply { get; set; }

        public int PageNumber { get; set; }
        public MessageModel Message { get; set; }

        public List<MessageReplyModel> Replies
        {
            get { return _replies; }
            set { _replies = value; }
        }

        public PagedList.Core.IPagedList<MessageModel> Messages { get; set; }

        public Guid SelectedUserId{ get; set;}
        public IList<SelectListItem> UsersByType { get; set; }
        public string SearchQuery { get; set; }

    }
    public class MessageReplyModel
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string MessageDetails { get; set; }
        public string ReplyFrom { get; set; }
        public Guid FromId { get; set; }
        public string ReplyMessage { get; set; }
        public DateTime ReplyDateTime { get; set; }
    }
    public class MessageModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string MessageToPost { get; set; }
        public Guid To { get; set; }
        public Guid From { get; set; }
        public Guid? JobUploadedBy { get; set; }
        public string  ToName { get; set; }
        public string JobUploadedByName { get; set; }
        public string FromName { get; set; }
        public DateTime DatePosted { get; set; }
    }


    public class EmailToUser { 
       public Guid UserId { get; set; }
        public string user { get; set; }
    }

    public class MessagesList {
        public PagedList.Core.IPagedList<MessageModel> Messages { get; set; }
        public int TotalCount { get; set; }
    }
}
