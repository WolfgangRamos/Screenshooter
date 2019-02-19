using Mono.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Screenshooter
{
    class Program
    {
        private static bool ShowHelp;
        private static String SaveFile;
        private static bool ArgsParsedSuccessful;
        private static String ArgsParseErrorMessage;

        private static readonly int SUCCESS_EXIT = 0;
        private static readonly int ERROR_EXIT = 1;

        static int Main(string[] args)
        {
            int exitCode = ERROR_EXIT;
            ParseArgs(args);
            if (ArgsParsedSuccessful)
            {
                try
                {
                    EnsureSaveFileEndsWithPng();
                    MakeAndSaveScreenshot();
                    exitCode = SUCCESS_EXIT;
                }
                catch (Exception e)
                {
                    WriteErrorMessageToConsole(e.Message);
                }
            }
            else
            {
                WriteErrorMessageToConsole(ArgsParseErrorMessage);
            }

            return exitCode;
        }

        private static void EnsureSaveFileEndsWithPng()
        {
            bool saveFileEndsWithPng = SaveFile.EndsWith(".png", true, System.Globalization.CultureInfo.InvariantCulture);
            if (!saveFileEndsWithPng)
            {
                SaveFile = SaveFile + ".png";
            }
        }

        private static void MakeAndSaveScreenshot()
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.WorkingArea.Width,
                                           Screen.PrimaryScreen.WorkingArea.Height,
                                           PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            bmpScreenshot.Save(SaveFile, ImageFormat.Png);
        }

        private static void ParseArgs(string[] args)
        {
            OptionSet options = new OptionSet() {
            { "o|out=", "fully qualified path to save the screenshot to",
              v => SaveFile = v },
            { "h|?|help",  "show this message and exit",
              v => ShowHelp = v != null },
            };

            try
            {
                List<string> unrecognizedArgs = options.Parse(args);
                if (unrecognizedArgs == null || unrecognizedArgs.Count <= 1)
                {
                    if (unrecognizedArgs.Count == 1)
                    {
                        SaveFile = unrecognizedArgs[0];
                    }
                    ArgsParsedSuccessful = true;
                }
                else
                {
                    ArgsParseErrorMessage = String.Format("Unrecognized Arguments `{0}'", String.Join("', `", unrecognizedArgs));
                }
            }
            catch (OptionException e)
            {
                ArgsParseErrorMessage = e.Message;
            }
        }

        private static void WriteErrorMessageToConsole(String errorMessage)
        {
            Console.Write("Screenshooter: ");
            Console.WriteLine(errorMessage);
            Console.WriteLine("Try `Screenshooter --help' for more information.");
        }
    }
}
