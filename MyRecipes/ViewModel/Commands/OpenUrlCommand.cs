using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.Commands
{
    class OpenUrlCommand : CommandBase
    {
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            if (parameter is string url)
            {
                Process.Start(url);
            }
        }
    }
}
