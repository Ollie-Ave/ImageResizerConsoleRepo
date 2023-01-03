using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ImageResizer.UnitTests")]

namespace CommandLineParsingTest
{
    using CommandLine;
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
            Console.WriteLine("Command Line parameters provided were not valid!");
        }


        private static void ReturnImageCallback(CommandLineOptions options)
        {
            try
            {

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

                ImageResizer.ProcessImage(options, path, fileName, extension);
            }
            catch (System.IO.FileNotFoundException ex) // error handling for if the image cannot be found
            {
                Debug.WriteLine("No image found");
                Debug.WriteLine(ex.Message);
            }

        }


    }
}


