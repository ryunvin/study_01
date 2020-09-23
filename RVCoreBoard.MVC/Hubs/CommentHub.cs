using Microsoft.AspNetCore.SignalR;
using RVCoreBoard.MVC.Attributes;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RVCoreBoard.MVC.Models.User;

namespace RVCoreBoard.MVC.Hubs
{
    public class CommentHub : Hub
    {
        [CustomAuthorize(RoleEnum = UserLevel.Junior | UserLevel.Senior | UserLevel.Manager | UserLevel.Admin)]
        public async Task BroadcastCommentList(List<Comment> commentList)
        {
            await Clients.All.SendAsync("ReceiveCommentList", commentList);
        }
    }
}
