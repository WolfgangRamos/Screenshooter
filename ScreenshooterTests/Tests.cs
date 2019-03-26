using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screenshooter;

namespace ScreenshooterTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestFullScreenScreenshot()
        {
            CommandLineArguments args = new CommandLineArguments(new string[] { "testshot.png" });
            ScreenShotMaker foo = new ScreenShotMaker(args);
            foo.Run();
        }
    }
}
