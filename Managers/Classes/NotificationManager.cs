using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using KhatmaBackEnd.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Managers.Classes
{
    public class NotificationManager: INotificationManager
    {
        public async Task<string> sendNotificationAsync()
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "quranmajead-firebase-adminsdk-5lwpn-f0f7c0ec2f.json")),
            });
            Console.WriteLine(defaultApp.Name); // "[DEFAULT]"
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    ["FirstName"] = "John",
                    ["LastName"] = "Doe"
                },
                Notification = new Notification
                {
                    Title = "Message Title",
                    Body = "Message Body",
                },
               
                //Token = "d3aLewjvTNw:APA91bE94LuGCqCSInwVaPuL1RoqWokeSLtwauyK-r0EmkPNeZmGavSG6ZgYQ4GRjp0NgOI1p-OAKORiNPHZe2IQWz5v1c3mwRE5s5WTv6_Pbhh58rY0yGEMQdDNEtPPZ_kJmqN5CaIc",
                Topic = "news"
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
            return result;

        }
    }
}
