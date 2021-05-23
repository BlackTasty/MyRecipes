using MyRecipes.Core;
using MyRecipes.Core.Events;
using MyRecipes.Core.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;

namespace MyRecipes.ViewModel
{
    class RemoteConfigurationViewModel : ViewModelBase
    {
        public event EventHandler<EventArgs> LogChanged;

        private const int MAX_MESSAGES = 100;

        private string mUsername;
        private string mPassword;

        private string mIPAddress;
        private VeryObservableCollection<string> mIPAddresses = new VeryObservableCollection<string>("IPAddresses");

        private bool mClearLogOnServerStart = true;
        private Queue<string> mLoggedMessages = new Queue<string>(MAX_MESSAGES);
        protected string mLog;

        public Server Server => App.Server;

        public string Username
        {
            get => mUsername;
            set
            {
                mUsername = value;
                InvokePropertyChanged();
            }
        }

        public string Password
        {
            get => mPassword;
            set
            {
                mPassword = value;
                InvokePropertyChanged();
            }
        }

        public VeryObservableCollection<string> IPAddresses => mIPAddresses;

        public string IPAddress
        {
            get => mIPAddress;
            set
            {
                mIPAddress = value;
                InvokePropertyChanged();
            }
        }

        public string Log
        {
            get => mLog;
        }

        public bool ClearLogOnServerStart
        {
            get => mClearLogOnServerStart;
            set
            {
                mClearLogOnServerStart = value;
                InvokePropertyChanged();
            }
        }

        public int ClientCount => Server.ClientCount;

        public bool IsRunning => Server.IsRunning;

        public bool CredentialInputEnabled => !Server.IsRunning;

        public RemoteConfigurationViewModel()
        {
            Server.ServerLogged += Server_ServerLogged;
            Server.ServerStarting += Server_ServerStarting;
            Server.StatusChanged += Server_StatusChanged;

            RefreshIPAddress();
        }

        public void RefreshIPAddress()
        {
            mIPAddresses.Clear();
            mIPAddresses.AddRange(Utils.GetAllLocalIPAddresses());
            IPAddress = mIPAddresses.LastOrDefault();
            InvokePropertyChanged("IPAddresses");
        }

        public void ClearLog()
        {
            mLog = "";
            mLoggedMessages.Clear();
            InvokePropertyChanged("Log");
        }

        private void Server_StatusChanged(object sender, EventArgs e)
        {
            InvokePropertyChanged("ClientCount");
            InvokePropertyChanged("IsRunning");
            InvokePropertyChanged("CredentialInputEnabled");
        }

        private void Server_ServerStarting(object sender, EventArgs e)
        {
            if (mClearLogOnServerStart)
            {
                ClearLog();
            }
            else
            {
                WriteLog("");
                WriteLog(new string('=', 100));
                WriteLog("");
            }
            InvokePropertyChanged("Log");
        }

        private void Server_ServerLogged(object sender, ServerLoggedEventArgs e)
        {
            WriteLog(string.Format("[{0}] {1}: {2}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff"), e.Tag, e.Message));
        }

        private void WriteLog(string line)
        {
            if (mLoggedMessages.Count > MAX_MESSAGES)
            {
                mLoggedMessages.Dequeue();
            }

            mLoggedMessages.Enqueue(line);

            string log = "";
            foreach (string logLine in mLoggedMessages.ToList())
            {
                log += log != "" ? "\n" + logLine : logLine;
            }

            mLog = log;
            InvokePropertyChanged("Log");
            OnLogChanged(EventArgs.Empty);
        }

        protected virtual void OnLogChanged(EventArgs e)
        {
            LogChanged?.Invoke(this, e);
        }
    }
}
