using Microsoft.AspNet.SignalR;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Threading;

using static Capstone.Account_Manager_ManageAccount;
using System.Threading.Tasks;

namespace Capstone
{

    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        //public static void SendNotification()
        //{
        //    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        //    context.Clients.All.updateNotifications();
        //}

        //private static NpgsqlConnection _conn;
        //private static Thread _notificationListenerThread;

        //public static void StartListeningToDatabase()
        //{
        //    // Start a thread to listen for database changes
        //    _notificationListenerThread = new Thread(ListenForDatabaseChanges);
        //    _notificationListenerThread.Start();
        //}

        //private static void ListenForDatabaseChanges()
        //{
        //    // Establish a connection to the database
        //    _conn = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=123456;Database=trashtrack");
        //    _conn.Open();

        //    // Set up the LISTEN command to receive notifications
        //    using (var cmd = new NpgsqlCommand("LISTEN new_notification;", _conn))
        //    {
        //        cmd.ExecuteNonQuery();
        //    }

        //    // Start listening for notifications
        //    while (true)
        //    {
        //        _conn.Wait();  // Wait for notification from PostgreSQL

        //        // When notified, call the method to send the notification to clients
        //        NotifyClients("A new notification has been added/updated in the database.");
        //    }
        //}

        //public static void NotifyClients(string message)
        //{
        //    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        //    context.Clients.All.receiveNotification(message); // Notify all connected clients
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    // Clean up when a client disconnects
        //    if (_notificationListenerThread != null && _notificationListenerThread.IsAlive)
        //    {
        //        _notificationListenerThread.Abort();
        //        _conn.Close();
        //    }
        //    return base.OnDisconnected(stopCalled);
        //}



    }
}