using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("ImageResizer.UnitTests")]

namespace ImageResizer
{
    using CommandLine;
    using SixLabors.ImageSharp;
    using System.Collections;
    using System.Diagnostics;
    using System.Net;

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

                // Temporary output folder
                string path = "C:\\Users\\oliver\\Downloads";

                // Used if you want the processed image to be spat out into the same directory as the input
                // Ex: C:\Users\oliver\Downloads
                //string path = Path.GetFullPath(options.InputFile).Replace(Path.GetFileName(options.InputFile), string.Empty);

                // Img url
                string url = options.InputFile;

                // The name of the inputted image (without extension)
                string fileName = Path.GetFileNameWithoutExtension(url);

                // gets the file extension of the inputted image
                string extension = Path.GetExtension(url);

                // those three values are stored seperately so that the parameters passed can be added after the filename but before the extension

                //using (Image image = Image.Load(options.InputFile))
                //{
                //    IImageFormat format = ImageResizer.GetImageFormat(extension);

                //    byte[] imageAsBytes = ToByteArray(image, format);

                //    byte[] newImageAsBytes = ImageResizer.ProcessImage(options, extension, imageAsBytes);

                //    Image newImage = Image.Load(newImageAsBytes);

                //    newImage.Save(ImageResizer.GetOutputFile(options, path, fileName, extension));
                //}

                using (WebClient webClient = new())
                {
                    // Grab the byte array from requested location
                    byte[] data = webClient.DownloadData(url);

                    // Process image
                    byte[] newImageAsBytes = ImageResizer.ProcessImage(options, extension ,data);

                    // Convert processed byte array into imagesharp format
                    Image newImage = Image.Load(newImageAsBytes);

                    // Save image to hard drive
                    newImage.Save(ImageResizer.GetOutputFile(options, path, fileName, extension));
                } // Dispose


            }
            catch (System.IO.FileNotFoundException ex) // error handling for if the image cannot be found
            {
                Debug.WriteLine("No image found");
                Debug.WriteLine(ex.Message);
            }

        }


    }
}


