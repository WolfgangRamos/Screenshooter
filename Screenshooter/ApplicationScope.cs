using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screenshooter
{
    class ApplicationScope
    {
        internal readonly CommandLineArguments Args;

        internal ApplicationScope(string[] args)
        {
            Args = new CommandLineArguments(args);
        }
    }
}
