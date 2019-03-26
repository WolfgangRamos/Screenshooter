namespace Screenshooter
{
    class Program
    {
        static int Main(string[] args)
        {
            ApplicationScope applicationScope = new ApplicationScope(args);
            ScreenShotMaker screenShooter = ScreenshooterInjector.CreateScreenShotMaker(applicationScope);
            int exitCode = screenShooter.Run();
            return exitCode;
        }
    }
}
