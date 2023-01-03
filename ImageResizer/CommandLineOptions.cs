
namespace ImageResizer
{
    using CommandLine;

    public sealed class CommandLineOptions
    {
        [Option('w', "width", Required = false, HelpText = "Width of the Image")]
        public int Width { get; set; }
        [Option('h', "height", Required = false, HelpText = "Height of the image")]
        public int Height { get; set; }
        [Option('i', "inputFile", Required = true, HelpText = "The name of the file that you want to be edited")]
        public string InputFile { get; set; }

        [Option('o', "outputFile", Required = false, HelpText = "The name of the outputted file")]
        public string OutputFile { get; set; }

        [Option('s', "scale", Required = false, HelpText = "Should the image scale horizontally, vertically or both? defaults to none")]
        public string Scale { get; set; }

        [Option('m', "mode", Required = false, HelpText = "How do you want the image to react to changes in aspect ratio, defaults to stretch.")]
        public string Mode { get; set; }
    }
}


