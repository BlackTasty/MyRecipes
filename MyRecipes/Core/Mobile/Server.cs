using ControlzEx.Standard;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Protocol;
using MQTTnet.Server;
using MyRecipes.Core.Mobile.Account;
using MyRecipes.Core.Mobile.Encryption;
using MyRecipes.Core.Recipes;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRecipes.Core.Mobile
{
    class Server : ViewModelBase
    {
        private const string BROKER_ADDRESS = "localhost:1883";

        private MqttFactory factory;
        private IMqttServer mqttServer;
        
        public Server()
        {
            factory = new MqttFactory();
            mqttServer = factory.CreateMqttServer();
        }

        public async void StartServer(User user)
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(1883)
                .WithConnectionValidator(c =>
                {
                    if (c.Username != user.Username || c.Password != user.Password)
                    {
                        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        return;
                    }

                    c.ReasonCode = MqttConnectReasonCode.Success;
                })
                .Build();

            mqttServer.UseApplicationMessageReceivedHandler(e => ConsumeMessage(e));
            await mqttServer.StartAsync(optionsBuilder);
        }

        private void ConsumeMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            string topic = e.ApplicationMessage.Topic;
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

            if (topic == "categories") // Mobile device requests category list
            {
                SendMessage(topic, JsonConvert.SerializeObject(App.AvailableCategories));
            }
            else if (topic == "ingredients") // Mobile device requests ingredient list
            {
                SendMessage(topic, JsonConvert.SerializeObject(App.AvailableIngredients));
            }
            else if (topic == "recipes")
            {
                if (string.IsNullOrEmpty(payload)) // Mobile device requests recipe list
                {
                    SendMessage(topic, JsonConvert.SerializeObject(App.AvailableRecipes));
                }
                else // Mobile device requests specific recipe (payload is recipe GUID)
                {
                    Recipe requested = App.AvailableRecipes.FirstOrDefault(x => x.Guid == payload);

                    if (requested != null)
                    {
                        SendMessage(topic + "/requested", JsonConvert.SerializeObject(requested));
                    }
                }
            }
            else if (topic.StartsWith("recipes/")) // Mobile device is uploading an image for a recipe
            {
                string recipeGuid = topic.Replace("recipes/", "");
                Recipe recipe = App.AvailableRecipes.FirstOrDefault(x => x.Guid == recipeGuid);

                if (recipe != null)
                {
                    //TODO: Set uploaded image as recipe image and confirm upload to uploading device
                }
            }
            else if (topic == "season")
            {
                var seasonFruits = App.AvailableIngredients.Where(x => x.IngredientCategory == IngredientCategory.Fruits &&
                             x.Seasons?.Count > 0).OrderBy(x => x.Name);
                SendMessage(topic + "/fruits", JsonConvert.SerializeObject(seasonFruits));
                var seasonVegetables = App.AvailableIngredients.Where(x => x.IngredientCategory == IngredientCategory.Vegetables &&
                             x.Seasons?.Count > 0).OrderBy(x => x.Name);
                SendMessage(topic + "/vegetables", JsonConvert.SerializeObject(seasonVegetables));
                var seasonNuts = App.AvailableIngredients.Where(x => x.IngredientCategory == IngredientCategory.Nuts &&
                             x.Seasons?.Count > 0).OrderBy(x => x.Name);
                SendMessage(topic + "/nuts", JsonConvert.SerializeObject(seasonNuts));
            }
        }

        private async void SendMessage(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

            await mqttServer.PublishAsync(message, CancellationToken.None);
        }

        /*public bool Login(User user)
        {
            return Login(user.Email, user.Password);
        }

        public async bool Login(string username, string passwordEncrypted)
        {
            string clientId = Hasher.EncryptString(username, "MidnightBagel");

            var options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithCredentials(username, Encoding.UTF8.GetBytes(passwordEncrypted))
                .WithCleanSession()
                .WithWebSocketServer(string.Format("{0}/{1}/{2}", BROKER_ADDRESS, clientId, "login"))
                .Build();

            await mqttServer.ConnectAsync(options, CancellationToken.None);

            return true;
        }

        /*public async bool Login(string email, string passwordEncrypted)
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString() + "-" + Hasher.EncryptString(email, passwordEncrypted))
                .WithCredentials(email, Encoding.UTF8.GetBytes(passwordEncrypted))
                .WithCleanSession()
                .WithWebSocketServer(BROKER_ADDRESS + "/login")
                .Build();

            await mqttClient.ConnectAsync(options, CancellationToken.None);

            return true;
        }*/
    }
}
