using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class RemoteConfigurationDesignViewModel : RemoteConfigurationViewModel
    {
        public RemoteConfigurationDesignViewModel()
        {
            Username = "Foo";

            mLog = string.Format("[{0}] Tag: Message 1", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff"));
            mLog += string.Format("\n[{0}] Tag: Message 2", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff"));
            mLog += string.Format("\n[{0}] Tag: Message 3", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff"));
            mLog += string.Format("\n[{0}] Tag: Message 4", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff"));
            mLog += string.Format("\n[{0}] Tag: Message 5", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff"));
        }
    }
}
