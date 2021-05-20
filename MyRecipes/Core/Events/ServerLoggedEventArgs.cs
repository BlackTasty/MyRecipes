using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Events
{
    public class ServerLoggedEventArgs : EventArgs
    {
        private string tag;
        private string message;

        public string Tag => tag;

        public string Message => message;

        public ServerLoggedEventArgs(string tag, string message)
        {
            this.tag = tag;
            this.message = message;
        }
    }
}
