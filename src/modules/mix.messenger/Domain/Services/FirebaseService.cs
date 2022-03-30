using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mix.Messenger.Domain.Models;
using Mix.Shared.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Mix.Messenger.Domain.Services
{
    public class FirebaseService
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _environment;
        private FirebaseSettingModel _settings = new FirebaseSettingModel();
        public FirebaseService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            var session = _configuration.GetSection(MixAppSettingsSection.GoogleFirebase);
            session.Bind(_settings);
            var googleCredential = _environment.ContentRootPath;
            var filePath = _settings.Filename;

            googleCredential = Path.Combine(googleCredential, filePath);
            var credential = GoogleCredential.FromFile(googleCredential);

            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential
            });
        }

        public async Task SendToDevice(string registrationToken)
        {
            // This registration token comes from the client FCM SDKs.
            //var registrationToken = "YOUR_REGISTRATION_TOKEN";

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "score", "850" },
                    { "time", "2:45" },
                },
                Token = registrationToken,
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
            /* Sample response
             *  {
                  "name":"projects/myproject-b5ae1/messages/0:1500415314455276%31bd1c9631bd1c96"
                }
             */
        }

        public async Task SendToMultipleDevices(List<string> registrationTokens)
        {
            // Create a list containing up to 500 registration tokens.
            // These registration tokens come from the client FCM SDKs.

            var message = new MulticastMessage()
            {
                Tokens = registrationTokens,
                Data = new Dictionary<string, string>()
                {
                    { "score", "850" },
                    { "time", "2:45" },
                },
            };

            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            // See the BatchResponse reference documentation
            // for the contents of response.
            Console.WriteLine($"{response.SuccessCount} messages were sent successfully");
        }

        public async Task SendAll()
        {
            // These registration tokens come from the client FCM SDKs.
            var registrationTokens = new List<string>()
            {
                "YOUR_REGISTRATION_TOKEN_1",
                // ...
                "YOUR_REGISTRATION_TOKEN_n",
            };
            var message = new MulticastMessage()
            {
                Tokens = registrationTokens,
                Data = new Dictionary<string, string>()
                {
                    { "score", "850" },
                    { "time", "2:45" },
                },
            };

            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            if (response.FailureCount > 0)
            {
                var failedTokens = new List<string>();
                for (var i = 0; i < response.Responses.Count; i++)
                {
                    if (!response.Responses[i].IsSuccess)
                    {
                        // The order of responses corresponds to the order of the registration tokens.
                        failedTokens.Add(registrationTokens[i]);
                    }
                }

                Console.WriteLine($"List of tokens that caused failures: {failedTokens}");
            }
        }

        public async Task SendToTopics(string topic)
        {
            // The topic name can be optionally prefixed with "/topics/".
            // var topic = "highScores";

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "score", "850" },
                    { "time", "2:45" },
                },
                Topic = topic,
            };

            // Send a message to the devices subscribed to the provided topic.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }

        public async Task SendWithCondition(string condition)
        {
            // Define a condition which will send to devices which are subscribed
            // to either the Google stock or the tech industry topics.
            // var condition = "'stock-GOOG' in topics || 'industry-tech' in topics";

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = "$GOOG up 1.43% on the day",
                    Body = "$GOOG gained 11.80 points to close at 835.67, up 1.43% on the day.",
                },
                Condition = condition,
            };

            // Send a message to devices subscribed to the combination of topics
            // specified by the provided condition.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }

        public async Task SendBatchMessages(string registrationToken)
        {
            // Create a list containing up to 500 messages.
            var messages = new List<Message>()
{
                new Message()
                {
                    Notification = new Notification()
                    {
                        Title = "Price drop",
                        Body = "5% off all electronics",
                    },
                    Token = registrationToken,
                },
                new Message()
                {
                    Notification = new Notification()
                    {
                        Title = "Price drop",
                        Body = "2% off all books",
                    },
                    Topic = "readers-club",
                },
            };

            var response = await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
            // See the BatchResponse reference documentation
            // for the contents of response.
            Console.WriteLine($"{response.SuccessCount} messages were sent successfully");
        }
    }
}
