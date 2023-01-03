using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("ImageResizer.UnitTests")]

namespace CommandLineParsingTest
{
    using CommandLine;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;
    using SixLabors.ImageSharp.Processing.Processors.Transforms;
    using System.Collections;
    using System.Diagnostics;

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

    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(opts => ArgsAccepted(opts))
                .WithNotParsed((errs) => HandleParseError(errs));
        }

        private static void ArgsAccepted(CommandLineOptions options)
        {
            ReturnImageCallback(options);
        }


        private static void HandleParseError(IEnumerable errors)
        {
            Console.WriteLine("Command Line parameters provided were not valid!");
        }


        private static void ReturnImageCallback(CommandLineOptions options)
        {
            IResampler Sampler = KnownResamplers.Bicubic;

            try
            {

                // Used if you want the processed imaged to be dumped in the 'bin' folder
                //string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // Used if you want the processed image to be spat out into the same directory as the input
                string path = Path.GetFullPath(options.InputFile).Replace(Path.GetFileName(options.InputFile), string.Empty); // Ex: C:\Users\oliver\Downloads
                string fileName = Path.GetFileNameWithoutExtension(options.InputFile); // The name of the inputted image (without extension)
                string extension = Path.GetExtension(options.InputFile); // gets the file extension of the inputted image

                // those three values are stored seperately so that the parameters passed can be added after the filename but before the extension

                using (Image image = Image.Load(options.InputFile)) // Load the image into memory
                {
                    // Here stopwatch has the purpose of tracking the timings for the image manipulations
                    Stopwatch stopWatch = new();
                    stopWatch.Start();

                    ResizeOptions resizeOptions = new()
                    {
                        Size = GetSizeOptions(options, image),
                        Mode = GetScaleOptions(options),
                        Sampler = Sampler 
                    };

                    Debug.WriteLine("Getting mutate config: " + stopWatch.ElapsedMilliseconds); // Timekeeping
                    stopWatch.Restart();


                    // apply configuration defined above
                    image.Mutate(x => x.Resize(resizeOptions));

                    Debug.WriteLine("Getting mutatation: " + stopWatch.ElapsedMilliseconds); // Timekeeping
                    stopWatch.Restart();

                    // Creates a new image in memory
                    Image newImage = image;

                    // Save new image to hard drive
                    newImage.Save(GetOutputFile(options, path, fileName, extension));

                    Debug.WriteLine("Save image: " + stopWatch.ElapsedMilliseconds); // Timekeeping
                    stopWatch.Stop();
                }
            }
            catch (System.IO.FileNotFoundException ex) // error handling for if the image cannot be found
            {
                Debug.WriteLine("No image found");
                Debug.WriteLine(ex.Message);
            }
        }

        internal static string GetOutputFile(CommandLineOptions options, string path, string fileName, string extension)
        {
            // This function returns the new filename of the image.

            if (string.IsNullOrWhiteSpace(options.OutputFile))
            {
                // For each of these statements it is simply checking if the option has been passed in
                // if it has it will save a string to be appended onto the filename later
                // if it does not it will simply save an empty string 

                string widthSuffix = options.Width > 0 ? $"---w-{options.Width}" : string.Empty; 
                string heightSuffix = options.Height > 0 ? $"---h-{options.Height}" : string.Empty;
                string modeSuffix = !string.IsNullOrWhiteSpace(options.Mode) ? $"---m-{options.Mode}" : string.Empty;
                string scaleSuffix = !string.IsNullOrWhiteSpace(options.Scale) ? $"---s-{options.Scale}" : string.Empty;

                // Combine the predefined suffixes, original filename and extension
                string ouputFilename = $"{fileName}{widthSuffix}{heightSuffix}{modeSuffix}{scaleSuffix}{extension}";

                options.OutputFile = Path.Combine(path, ouputFilename);
            }

            return options.OutputFile;
        }

        internal static ResizeMode GetScaleOptions(CommandLineOptions options)
        {
            // Simple function that converts the string inputted into the ResizeMode format
            // ResizeMode is defined by imagesharp

            return options.Mode?.ToLower() switch
            {
                "crop" => ResizeMode.Crop,
                "stretch" => ResizeMode.Stretch,
                "pad" => ResizeMode.Pad,
                _ => ResizeMode.Pad,
            };
        }

        internal static Size GetSizeOptions(CommandLineOptions options, Image image)
        {
            // Returns a width and height for the new image based on Width, Height, Scale properties

            bool isScaleBoth = options.Scale == "both" && (options.Height != 0 || options.Width != 0);

            bool isHorizontal = options.Scale == "horizontal" && options.Height != 0;
            bool isVertical = options.Scale == "vertical" && options.Width != 0;
   
            int height = isScaleBoth || isHorizontal ? options.Height : image.Height;
            int width = isScaleBoth || isVertical ? options.Width : image.Width;

            return new(width, height);
        }
    }
}


