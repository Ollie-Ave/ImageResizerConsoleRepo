
namespace ImageResizer
{
    public class Options
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public string InputFile { get; set; }

        public string OutputFile { get; set; }

        public string Scale { get; set; }

        public string Mode { get; set; }

        public Options(CommandLineOptions options)
        {
            Width = options.Width;

            Height = options.Height;

            InputFile = options.InputFile;

            OutputFile = options.OutputFile;

            Scale = options.Scale;

            Mode = options.Mode;
        }

        // Alternative ways to parse args go here...
    }
}


