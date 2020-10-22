using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using KhatmaBackEnd.Managers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Managers.Classes
{
    public class NotificationManager: INotificationManager
    {
        public static string SendPushNotification(string deviceId, string title="Test", string body="Test", string ApplicationId= "AAAAABjdfcE:APA91bFbFKRUOuKMngEiVGLdsoWCo7g8vGqiiSNkffOqNKQv7zzFqvlGYyZ3bNYT4knL3MoyRNCBl_htUTUoQTIhtI0z-J6l_SsHbnNgkNix5oo6hqVAA1AF0zS72tQi1nZRzHlZJqgM")
        {
            string str;
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body,
                        title,
                        sound = "Enabled"
                    },
               //   data=""
                };
                var json = JsonConvert.SerializeObject(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", ApplicationId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }


        public async Task<string> sendNotificationAsync()
        {
            try
            {


            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile( "quranmajead-firebase-adminsdk-5lwpn-f0f7c0ec2f.json"),
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
            catch (Exception exp)
            {
                return "";
            }

        }
    }
}
