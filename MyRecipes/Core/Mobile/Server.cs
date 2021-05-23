using ControlzEx.Standard;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Protocol;
using MQTTnet.Server;
using MyRecipes.Core.Events;
using MyRecipes.Core.Mobile.Account;
using MyRecipes.Core.Mobile.Encryption;
using MyRecipes.Core.Mobile.Transfer;
using MyRecipes.Core.Recipes;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tasty.ViewModel;
using Tasty.ViewModel.JsonNet;

namespace MyRecipes.Core.Mobile
{
    public class Server : ViewModelBase
    {
        public event EventHandler<ServerLoggedEventArgs> ServerLogged;
        public event EventHandler<EventArgs> ServerStarting;
        public event EventHandler<EventArgs> StatusChanged;

        // For now use loopback address to listen on all IP addresses for connections
        private static string BROKER_ADDRESS = string.Format("tcp://{0}", "0.0.0.0");

        private MqttFactory factory;
        private IMqttServer mqttServer;
        private bool mIsRunning;
        private JsonObservableCollection<string> mClientIds = new JsonObservableCollection<string>("ClientIds");
        private bool mIsIdle = true;

        public bool IsRunning
        {
            get => mIsRunning;
            private set
            {
                mIsRunning = value;
                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// Usually set to true when starting or stopping the server
        /// </summary>
        public bool IsIdle
        {
            get => mIsIdle;
            private set
            {
                mIsIdle = value;
                InvokePropertyChanged();
            }
        }

        public VeryObservableCollection<string> ClientIds => mClientIds;

        public int ClientCount => ClientIds.Count;

        public Server()
        {
            factory = new MqttFactory();
            mqttServer = factory.CreateMqttServer();
            mClientIds.TriggerAlso("ClientCount");

            mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(UserConnected);
            mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(UserDisconnected);
        }

        private void UserConnected(MqttServerClientConnectedEventArgs e)
        {
            ClientIds.Add(e.ClientId);
            WriteLog("Connection", string.Format("Client connected! Assigned ID: \"{0}\"", e.ClientId));
            OnStatusChanged(EventArgs.Empty);
        }

        private void UserDisconnected(MqttServerClientDisconnectedEventArgs e)
        {
            ClientIds.Remove(e.ClientId);
            WriteLog("Connection", string.Format("Client (ID: \"{0}\") disconnected! Disconnect type: {1}", e.ClientId, e.DisconnectType));
            OnStatusChanged(EventArgs.Empty);
        }

        public async void StartServer(string username, string password)
        {
            if (IsRunning)
            {
                return;
            }

            IsIdle = false;

            OnServerStarting(EventArgs.Empty);

            WriteLog("Server", "Starting...");
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(1883)
                .WithConnectionValidator(c =>
                {
                    if ((!string.IsNullOrWhiteSpace(username) && c.Username != username) || 
                        (!string.IsNullOrWhiteSpace(password) && c.Password != password))
                    {
                        WriteLog("Connection", "Client rejected! Cause: Wrong username or password");
                        c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        return;
                    }

                    c.ReasonCode = MqttConnectReasonCode.Success;
                    
                })
                .Build();

            mqttServer.UseApplicationMessageReceivedHandler(e => ConsumeMessage(e));
            await mqttServer.StartAsync(optionsBuilder);
            IsRunning = mqttServer.IsStarted;
            WriteLog("Server", IsRunning ? "Server is up and running!" : "Unknown error occured during startup!");
            IsIdle = true;
            OnStatusChanged(EventArgs.Empty);
        }

        public async void StopServer()
        {
            if (!IsRunning)
            {
                return;
            }

            IsIdle = false;
            WriteLog("Server", "Shutting down...");
            await mqttServer.StopAsync();
            IsRunning = false;
            ClientIds.Clear();
            WriteLog("Server", "Server has been shut down!");
            IsIdle = true;
            OnStatusChanged(EventArgs.Empty);
        }

        private void ConsumeMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.ClientId))
            {
                return;
            }

            string topic = e.ApplicationMessage.Topic;
            string payload = e.ApplicationMessage.Payload != null ? Encoding.UTF8.GetString(e.ApplicationMessage.Payload) : null;

            WriteLog("Received", string.Format("Topic is \"{0}\" (called by client \"{1}\")", topic, e.ClientId));

            if (topic == "categories") // Mobile device requests category list
            {
                SendMessage(topic, JsonConvert.SerializeObject(App.AvailableCategories), e.ClientId);
            }
            else if (topic == "ingredients") // Mobile device requests ingredient list
            {
                SendMessage(topic, JsonConvert.SerializeObject(App.AvailableIngredients), e.ClientId);
            }
            else if (topic == "recipes")
            {
                if (string.IsNullOrEmpty(payload)) // Mobile device requests recipe list
                {
                    SendMessage(topic + "/clear", null, e.ClientId);
                    //List<RecipeTransfer> recipeTransfers = new List<RecipeTransfer>();

                    foreach (Recipe recipe in App.AvailableRecipes)
                    {
                        RecipeTransfer recipeTransfer = new RecipeTransfer(recipe);
                        SendMessage(topic, JsonConvert.SerializeObject(recipeTransfer), e.ClientId);
                        /*if (!string.IsNullOrWhiteSpace(recipe.RecipeImage.FilePath))
                        {
                            SendRecipeImage(topic, recipe, e.ClientId);
                        }*/
                    }
                }
                else // Mobile device requests specific recipe (payload is recipe GUID)
                {
                    Recipe requested = App.AvailableRecipes.FirstOrDefault(x => x.Guid == payload);

                    if (requested != null)
                    {
                        SendMessage(topic + "/requested", JsonConvert.SerializeObject(requested), e.ClientId);
                    }
                }
            }
            else if (topic.StartsWith("recipes/img"))
            {
                Recipe recipe = App.AvailableRecipes.FirstOrDefault(x => x.Guid == payload);

                if (!string.IsNullOrWhiteSpace(recipe.RecipeImage.FilePath))
                {
                    SendRecipeImage(topic, recipe, e.ClientId);
                }
            }
            else if (topic.StartsWith("recipes/upload/")) // Mobile device is uploading an image for a recipe
            {
                string recipeGuid = topic.Replace("/recipes/upload/", "");
                Recipe recipe = App.AvailableRecipes.FirstOrDefault(x => x.Guid == recipeGuid);

                if (recipe != null)
                {
                    //TODO: Set uploaded image as recipe image and confirm upload to uploading device
                    string filePath = Path.Combine(App.BasePath, "uploads");
                    Directory.CreateDirectory(filePath);
                    filePath = Path.Combine(filePath, recipe.Name + ".jpg");

                    File.WriteAllBytes(filePath, e.ApplicationMessage.Payload);
                    recipe.RecipeImage.FilePath = filePath;
                }
            }
            else if (topic == "season")
            {
                var seasonFruits = App.AvailableIngredients.Where(x => x.IngredientCategory == IngredientCategory.Fruits &&
                             x.Seasons?.Count > 0).OrderBy(x => x.Name);
                SendMessage(topic + "/fruits", JsonConvert.SerializeObject(seasonFruits), e.ClientId);
                var seasonVegetables = App.AvailableIngredients.Where(x => x.IngredientCategory == IngredientCategory.Vegetables &&
                             x.Seasons?.Count > 0).OrderBy(x => x.Name);
                SendMessage(topic + "/vegetables", JsonConvert.SerializeObject(seasonVegetables), e.ClientId);
                var seasonNuts = App.AvailableIngredients.Where(x => x.IngredientCategory == IngredientCategory.Nuts &&
                             x.Seasons?.Count > 0).OrderBy(x => x.Name);
                SendMessage(topic + "/nuts", JsonConvert.SerializeObject(seasonNuts), e.ClientId);
            }
        }

        private async void SendMessage(string topic, string payload, string clientId)
        {
            topic = clientId + "/" + topic;
            var message = new MqttApplicationMessageBuilder()
                .WithContentType("application/json")
                .WithPayloadFormatIndicator(MqttPayloadFormatIndicator.CharacterData)
                .WithTopic(topic)
                .WithResponseTopic(topic)
                .WithPayload(payload)
                .Build();

            await mqttServer.PublishAsync(message, CancellationToken.None);
            WriteLog("Sent", string.Format("Published message to topic \"{0}\"", topic));
        }

        private async void SendRecipeImage(string topic, Recipe recipe, string clientId)
        {
            topic = clientId + "/" + topic + "/" + recipe.Guid;
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithResponseTopic(topic)
                .WithPayload(File.ReadAllBytes(recipe.RecipeImage.FilePath))
                .Build();

            await mqttServer.PublishAsync(message, CancellationToken.None);
            WriteLog("Sent", string.Format("Published message to topic \"{0}\"", topic));
        }

        private void WriteLog(string tag, string message)
        {
            OnServerLogged(new ServerLoggedEventArgs(tag, message));
        }

        protected virtual void OnServerLogged(ServerLoggedEventArgs e)
        {
            ServerLogged?.Invoke(this, e);
        }

        protected virtual void OnServerStarting(EventArgs e)
        {
            ServerStarting?.Invoke(this, e);
        }

        protected virtual void OnStatusChanged(EventArgs e)
        {
            StatusChanged?.Invoke(this, e);
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
