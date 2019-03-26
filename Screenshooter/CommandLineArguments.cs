using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Options;

namespace Screenshooter
{
    /// <summary>
    /// Commandline arguments that Screenshooter.exe was called with.
    /// </summary>
    internal class CommandLineArguments
    {
        private OptionSet Options;

        private string _SaveFile;
        private string _UpperLeftCoordinateString;
        private string _LowerRightCoordinateString;

        internal bool ShowHelp { get; private set; }
        internal String SaveFile
        {
            get
            {
                return _SaveFile;
            }
            private set
            {
                _SaveFile = EnsureEndsWithPng(value);
            }
        }
        internal Point UpperLeftCoordinate { get; private set; }
        internal Point LowerRightCoordinate { get; private set; }
        internal bool ArgsParsedSuccessful { get; private set; }
        internal String ArgsParseErrorMessage { get; private set; }

        internal CommandLineArguments(string[] args)
        {
            InitializeOptions();
            ParseArgs(args);
        }

        private void InitializeOptions()
        {
            Options = new OptionSet() {
                { "o|out=", "Fully qualified path to save the screenshot to.", v => SaveFile = v },
                { "u|upperleft=", "Upper left coordinate of area to capture, in the form of `x,y'. Defaults to `0,0.'", v => _UpperLeftCoordinateString = v },
                { "l|lowerright=", "Lower right coordinate of area to capture, in the form of `x,y'. Defaults to the lower right corner of the working area, i.e. the area above the task bar.", v => _LowerRightCoordinateString = v },
                { "h|?|help", "Show this message and exit", v => ShowHelp = v != null },
            };
        }

        private Point CoordinateFormString(string coordinateString)
        {
            Point result = Point.Empty;
            if(coordinateString != null)
            {
                try
                {
                    string[] xAndY = coordinateString.Split(',');
                    if(xAndY.Length == 2)
                    {
                        int x = Int32.Parse(xAndY[0]);
                        int y = Int32.Parse(xAndY[1]);
                        result = new Point(x, y);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(String.Format("Count not convert string `{0}' to coordinate.", coordinateString), e);
                }
            }
            return result;
        }

        private void ParseUpperLeftCoordinate(string v)
        {
            throw new NotImplementedException();
        }

        private void ParseArgs(string[] args)
        {
            try
            {
                List<string> unrecognizedArgs = Options.Parse(args);
                UpperLeftCoordinate = CoordinateFormString(_UpperLeftCoordinateString);
                LowerRightCoordinate = CoordinateFormString(_LowerRightCoordinateString);

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
            catch (Exception e)
            {
                ArgsParseErrorMessage = e.Message;
            }
        }

        private static string EnsureEndsWithPng(String fileName)
        {
            String result = fileName;
            bool endsWithPng = fileName.EndsWith(".png", true, System.Globalization.CultureInfo.InvariantCulture);
            if (!endsWithPng)
            {
                result = fileName + ".png";
            }
            return result;
        }

        public void WriteOptionsDescriptions(TextWriter writer)
        {
            Options.WriteOptionDescriptions(Console.Out);
        }
    }
}
