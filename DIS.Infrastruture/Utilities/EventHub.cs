using DIS.Infrastructure.Utilities;
using DIS.Web.ViewModels;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.SignalR;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Infrastructure.Utilities
{
    public class EventHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Response data = new Response();
            data.message = "connection success!";
            await Clients.Client(Context.ConnectionId).SendAsync("Connect", data);
            // await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
            //await  Clients.All.SendAsync("Connect", data);
        }
        public class Response
        {
            public string? message { get; set; }
        }
        public async Task SendMessage(SignalREvent data)
        {
            await Clients.All.SendAsync("MessageReceived", data);
        }
        public async Task<bool> SendInboxMessage(string message, MessageViewModel data)
        {
            await Clients.All.SendAsync(message, data);
            return true;
        }
        public async Task OnSendMessage(string message, int from, int to, int project_id)
        {
            SignalREvent data = new SignalREvent();
            //data.message = message;
            //data.from_id = from;
            //data.to_id = to;
            //data.project_id = project_id;
            //await Clients.All.SendAsync("MessageReceived", data);
        }
        public async Task Dispatch(SignalREvent e)
        {
            await Clients.All.SendAsync("_Dispatch", e);
        }
    }
}
