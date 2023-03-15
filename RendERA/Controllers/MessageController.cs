using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RendERA.ServiceManager.IServices;
using RendERA.DB.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MessageBoardApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessagesSrv _messagesService;
        private readonly IMessageReplySrv _messageReplysService;
        private RendERA.ServiceManager.IServices.IAccountSrv _accountService;
        private readonly IHttpContextAccessor _contextAccessor; //for session
        public MessageController( IHttpContextAccessor httpContextAccessor, IMessageReplySrv messageReplysService, IMessagesSrv messagesService, RendERA.ServiceManager.IServices.IAccountSrv accountService, IHttpContextAccessor contextAccessor)
        {
            _accountService = accountService;
            _contextAccessor = contextAccessor;
            _messageReplysService = messageReplysService;
            _messagesService = messagesService;
        }

        public IActionResult Index(string query, int? Id, int? page)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                   RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var User = _accountService.GetUser(new Guid(value));
            //pagination
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;

            //get messages list
            MessagesList msglist = new MessagesList();
            msglist = _messagesService.GetAllMsgsbyPage(query, User.UserId, pageNumber, pageSize);
            if (msglist == null)
            {
                return null;
            }
            var count = msglist.TotalCount;
            int totalPages = count / pageSize;
            if (count % pageSize > 0) {
                totalPages = totalPages + 1;
            }
            ViewBag.TotalPages = totalPages == 0 ? 1 : totalPages;
            ViewBag.PageNumber = pageNumber;
            
            MessageReplyVM vm = new MessageReplyVM();
            vm.Messages = msglist.Messages;         
            if (Id != null)
            {   
                var replies = _messageReplysService.GetMsgsByMsgId(Id.Value);
                var msg = _messagesService.GetMsgById(Id.Value);
                if (msg == null)
                {
                    msg = new MessageModel();
                    msg.MessageToPost = "";
                }
                if (replies != null)
                {
                    foreach (var rep in replies)
                    {
                        MessageReplyModel reply = new MessageReplyModel();
                        reply.MessageId = rep.MessageId;
                        reply.Id = rep.Id;
                        reply.ReplyMessage = rep.ReplyMessage;
                        reply.ReplyDateTime = rep.ReplyDateTime;
                        reply.MessageDetails = msg.MessageToPost == null ? "" : msg.MessageToPost;
                        reply.ReplyFrom = rep.ReplyFrom;
                        reply.FromId = rep.FromId;
                        vm.Replies.Add(reply);
                    }
                }
                else
                {
                    vm.Replies.Add(null);
                }
                vm.Message = msg;
                ViewBag.MessageId = Id.Value;
            }
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            ViewBag.usertype = User.UserType;
            return View(vm);
        }

        public IActionResult UserMessageList(string query, int? Id, int? page)
        {
            var value = _contextAccessor.HttpContext.Session.GetString(
                   RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
            if (value == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var User = _accountService.GetUser(new Guid(value));

            //pagination
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;

            //get messages list
            MessagesList msglist = new MessagesList();
            msglist = _messagesService.GetAllMsgsbyPage(query, User.UserId, pageNumber, pageSize);
            if (msglist == null)
            {
                return null;
            }
            var count = msglist.TotalCount;
            int totalPages = count / pageSize;
            ViewBag.TotalPages = totalPages == 0 ? 1 : totalPages;
            ViewBag.PageNumber = pageNumber;

            MessageReplyVM vm = new MessageReplyVM();
            vm.Messages = msglist.Messages;
            if (Id != null)
            {
                var replies = _messageReplysService.GetMsgsByMsgId(Id.Value);
                var msg = _messagesService.GetMsgById(Id.Value);
                if (msg == null)
                {
                    msg = new MessageModel();
                    msg.MessageToPost = "";
                }
                if (replies != null)
                {
                    foreach (var rep in replies)
                    {
                        MessageReplyModel reply = new MessageReplyModel();
                        reply.MessageId = rep.MessageId;
                        reply.Id = rep.Id;
                        reply.ReplyMessage = rep.ReplyMessage;
                        reply.ReplyDateTime = rep.ReplyDateTime;
                        reply.MessageDetails = msg.MessageToPost == null ? "" : msg.MessageToPost;
                        reply.ReplyFrom = rep.ReplyFrom;
                        reply.FromId = rep.FromId;
                        vm.Replies.Add(reply);
                    }
                }
                else
                {
                    vm.Replies.Add(null);
                }
                vm.Message = msg;
                ViewBag.MessageId = Id.Value;
            }
            if (TempData["Msg"] != null)
                ViewBag.Msg = TempData["Msg"] as string;
            return View(vm);
        }

        [HttpPost]
        public ActionResult PostMessage(MessageReplyVM vm)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                   RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null) {
                   return RedirectToAction("Login","Account");
                }
                var User = _accountService.GetUser(new Guid(value));

                MessageModel messagetoPost = new MessageModel();
                if (vm.Message.Subject != string.Empty && vm.Message.MessageToPost != string.Empty)
                {
                    messagetoPost.DatePosted = DateTime.Now;
                    messagetoPost.Subject = vm.Message.Subject;
                    messagetoPost.MessageToPost = vm.Message.MessageToPost;
                    messagetoPost.To = vm.SelectedUserId;
                    messagetoPost.From = User.UserId;

                    _messagesService.InsertMsg(messagetoPost);
                    TempData["Msg"] = "Sent successfully";
                    return RedirectToAction("Index", "Message");
                }
                TempData["Msg"] = "Faild to send";
                return RedirectToAction("Index", "Message");
            }
            catch {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Index", "Message");
            }
        }

        [HttpPost]
        public ActionResult ReplyMessage(MessageReplyVM vm, int messageId)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                       RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var User = _accountService.GetUser(new Guid(value));
                if (vm.Reply.ReplyMessage != null)
                {
                    MessageReplyModel _reply = new MessageReplyModel();
                    _reply.ReplyDateTime = DateTime.Now;
                    _reply.MessageId = messageId;
                    _reply.ReplyMessage = vm.Reply.ReplyMessage;
                    _reply.ReplyFrom = vm.Reply.ReplyFrom;
                    _reply.FromId = User.UserId;
                    _messageReplysService.InsertMsg(_reply);
                    TempData["Msg"] = "Sent successfully";
                    return RedirectToAction("Index", "Message", new { Id = messageId });
                }
                TempData["Msg"] = "Faild to send";
                return RedirectToAction("Index", "Message", new { Id = messageId });
            }
            catch {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Index", "Message", new { Id = messageId });
            }
        }
        
        [HttpPost]
        public ActionResult UserReplyMessage(MessageReplyVM vm, int messageId)
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                       RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var User = _accountService.GetUser(new Guid(value));
                if (vm.Reply.ReplyMessage != null)
                {
                    MessageReplyModel _reply = new MessageReplyModel();
                    _reply.ReplyDateTime = DateTime.Now;
                    _reply.MessageId = messageId;
                    _reply.ReplyMessage = vm.Reply.ReplyMessage;
                    _reply.ReplyFrom = vm.Reply.ReplyFrom;
                    _reply.FromId = User.UserId;
                    _messageReplysService.InsertMsg(_reply);
                    TempData["Msg"] = "Sent successfully";
                    return RedirectToAction("UserMessageList", "Message", new { Id = messageId });
                }
                TempData["Msg"] = "Faild to send";
                return RedirectToAction("UserMessageList", "Message", new { Id = messageId });
            }
            catch {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("UserMessageList", "Message", new { Id = messageId });
            }
        }

        public ActionResult Create()
        {
            try
            {
                var value = _contextAccessor.HttpContext.Session.GetString(
                       RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var User = _accountService.GetUser(new Guid(value));

                MessageReplyVM vm = new MessageReplyVM();
                if (User.UserType == (int)RendERA.Infrastructure.Enum.UserType.Admin)
                {
                    vm.UsersByType = GetUserListItemsByUserType((int)RendERA.Infrastructure.Enum.UserType.Partner);
                }
                else if (User.UserType == (int)RendERA.Infrastructure.Enum.UserType.Partner)
                {
                    vm.UsersByType = GetUserListItemsByUserType((int)RendERA.Infrastructure.Enum.UserType.Admin);
                }
                return View(vm);
            }
            catch {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Index", "Message");
            }
        }


        [HttpPost]
        public ActionResult DeleteMessage(int messageId)
        {
            try{ 
                var value = _contextAccessor.HttpContext.Session.GetString(
                       RendERA.Infrastructure.Enum.SessionKey.LoggedInUserId.ToString());
                if (value == null)
                {
                    RedirectToAction("Login", "Account");
                }
                MessageModel _messageToDelete = _messagesService.GetMsgById(messageId);
                _messagesService.DeleteMsg(messageId);
          
                // also delete the replies related to the message
                var _repliesToDelete = _messageReplysService.GetAllMsgs();
                if (_repliesToDelete != null)
                {
                    foreach (var rep in _repliesToDelete)
                    {
                        _messageReplysService.DeleteMsg(rep.Id);
                    }
                }
                TempData["Msg"] = "Deleted successfully";
                return RedirectToAction("Index", "Message");
            }
            catch
            {
                TempData["Msg"] = "Something went wrong";
                return RedirectToAction("Index", "Message");
            }
        }

        #region Utility
        private IList<SelectListItem> GetUserListItemsByUserType(int userType)
        {
            var elements = _accountService.GetAllUserByUserType(userType);
            var selectList = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.UserId.ToString(),
                    Text = element.user
                });
            }

            return selectList;
        }
        #endregion
    }
}
