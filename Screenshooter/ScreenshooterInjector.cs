using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screenshooter
{
    static class ScreenshooterInjector
    {
        internal static ScreenShotMaker CreateScreenShotMaker(ApplicationScope applicationScope)
        {
            return new ScreenShotMaker(applicationScope.Args);
        }
    }
}
