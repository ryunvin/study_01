using Microsoft.AspNetCore.SignalR;
using RVCoreBoard.MVC.DataContext;
using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.Hubs
{
    public class CommentHub : Hub
    {
        public async Task BroadcastCommentList()
        {
            await Clients.All.SendAsync("ReceiveCommentList");
        }
    }
}
