using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("ImageResizer.UnitTests")]

namespace ImageResizer
{
    using CommandLine;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Advanced;
    using SixLabors.ImageSharp.Formats;
    using SixLabors.ImageSharp.Formats.Png;
    using SixLabors.ImageSharp.PixelFormats;
    using System.Collections;
    using System.Diagnostics;

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
            Console.WriteLine("Command Line parameters provided were not valid! Try -h");
        }


        private static void ReturnImageCallback(CommandLineOptions commandLineOptions)
        {
            try
            {
                Options options = new(commandLineOptions);  

                // Used if you want the processed imaged to be dumped in the 'bin' folder
                //string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // Used if you want the processed image to be spat out into the same directory as the input
                // Ex: C:\Users\oliver\Downloads
                string path = Path.GetFullPath(options.InputFile).Replace(Path.GetFileName(options.InputFile), string.Empty);

                // The name of the inputted image (without extension)
                string fileName = Path.GetFileNameWithoutExtension(options.InputFile);
                // gets the file extension of the inputted image
                string extension = Path.GetExtension(options.InputFile);

                // those three values are stored seperately so that the parameters passed can be added after the filename but before the extension

                using (Image image = Image.Load(options.InputFile))
                {
                    byte[] imageAsBytes = ToByteArray(image, PngFormat.Instance);

                    ImageResizer.ProcessImage(options, path, fileName, extension, imageAsBytes);
                }

            }
            catch (System.IO.FileNotFoundException ex) // error handling for if the image cannot be found
            {
                Debug.WriteLine("No image found");
                Debug.WriteLine(ex.Message);
            }

        }
        public static byte[] ToByteArray(Image image, IImageFormat imageFormat) 
        {
            // This function removes dependency on filesystem
            // I.e it allows ImageResizer.Process image not to care about how you got the image (stored as a byte array)
            // All the previously mentioned function cares about is simply that it has the image, not where you got it from

            using (var memoryStream = new MemoryStream())
            {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(imageFormat);
                image.Save(memoryStream, imageEncoder);
                return memoryStream.ToArray();
            }
        }

    }
}


