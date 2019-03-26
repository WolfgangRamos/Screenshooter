using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Screenshooter
{
    internal class ScreenShotMaker
    {
        private Point CaptureRegionUpperLeftCorner;
        private Size CaptureRegionSize;

        private CommandLineArguments CommandLineArguments { get; set; }

        internal ScreenShotMaker(CommandLineArguments args)
        {
            CommandLineArguments = args;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Exit code.</returns>
        internal int Run()
        {
            ScreenchooterExitCode exitCode = ScreenchooterExitCode.Error;
            if (CommandLineArguments.ArgsParsedSuccessful)
            {
                exitCode = ShowHelpOrMakeScreenshot();
            }
            else
            {
                WriteErrorMessageToConsole(CommandLineArguments.ArgsParseErrorMessage);
            }
            return (int) exitCode;
        }

        private ScreenchooterExitCode ShowHelpOrMakeScreenshot()
        {
            ScreenchooterExitCode exitCode = ScreenchooterExitCode.Error;
            if (CommandLineArguments.ShowHelp)
            {
                WriteHelpMessageToConsole();
                exitCode = ScreenchooterExitCode.Success;
            }
            else
            {
                try
                {
                    SetCaptureRegionFromCommandLineArguments();
                    MakeAndSaveScreenshot();
                    exitCode = ScreenchooterExitCode.Success;
                }
                catch (Exception e)
                {
                    WriteErrorMessageToConsole(e.Message);
                }
            }

            return exitCode;
        }

        private void SetCaptureRegionFromCommandLineArguments()
        {
            SetCaptureRegionUpperLeftCoordinateFromCommandLineArguments();
            SetCaptureRegionSizeFromCommandLineArguments();
        }

        private void SetCaptureRegionSizeFromCommandLineArguments()
        {
            Point lowerRightCorner = CommandLineArguments.LowerRightCoordinate.IsEmpty ?
                new Point(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height) :
                new Point(Math.Min(CommandLineArguments.LowerRightCoordinate.X, Screen.PrimaryScreen.WorkingArea.Width),
                          Math.Min(CommandLineArguments.LowerRightCoordinate.Y, Screen.PrimaryScreen.WorkingArea.Height));

            CaptureRegionSize = new Size(lowerRightCorner.X - CaptureRegionUpperLeftCorner.X, lowerRightCorner.Y - CaptureRegionUpperLeftCorner.Y);
            if (CaptureRegionSize.Width <= 0 || CaptureRegionSize.Height <= 0)
            {
                throw new Exception("Capture region has negative or zero width or height.");
            }
        }

        private void SetCaptureRegionUpperLeftCoordinateFromCommandLineArguments()
        {
            Point upperLeft = CommandLineArguments.UpperLeftCoordinate.IsEmpty ? new Point(0,0) : CommandLineArguments.UpperLeftCoordinate;
            if (upperLeft.X >= Screen.PrimaryScreen.WorkingArea.Width || upperLeft.Y >= Screen.PrimaryScreen.WorkingArea.Height)
            {
                throw new Exception("Upper left coordinate is outside of screen coordinates.");
            }
            else
            {
                CaptureRegionUpperLeftCorner = upperLeft;
            }
        }

        private void WriteHelpMessageToConsole()
        {
            Console.WriteLine("Usage: Screenshooter [OPTIONS]+ [file]");
            Console.WriteLine("Take a screenshot.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            CommandLineArguments.WriteOptionsDescriptions(Console.Out);
        }

        private void MakeAndSaveScreenshot()
        {
            //Create a new bitmap.
            Bitmap bmpScreenshot = new Bitmap(CaptureRegionSize.Width, CaptureRegionSize.Height, PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(CaptureRegionUpperLeftCorner.X,
                                        CaptureRegionUpperLeftCorner.Y,
                                        0,
                                        0,
                                        CaptureRegionSize,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            bmpScreenshot.Save(CommandLineArguments.SaveFile, ImageFormat.Png);
        }

        private void WriteErrorMessageToConsole(String errorMessage)
        {
            Console.Write("Screenshooter: ");
            Console.WriteLine(errorMessage);
            Console.WriteLine("Try `Screenshooter --help' for more information.");
        }
    }
}
