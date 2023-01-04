using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ImageResizer.UnitTests")]

namespace ImageResizer
{
    using CommandLine.Text;
    using CommandLine;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;
    using SixLabors.ImageSharp.Processing.Processors.Transforms;
    using System.Diagnostics;
    using SixLabors.ImageSharp.PixelFormats;
    using System.Collections;

    public static class ImageResizer
    {
        internal static string GetOutputFile(Options options, string path, string fileName, string extension)
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

        internal static ResizeMode GetScaleOptions(Options options)
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

        internal static Size GetSizeOptions(Options options, Image image)
        {
            // Returns a width and height for the new image based on Width, Height, Scale properties

            bool isScaleBoth = options.Scale == "both" && (options.Height != 0 || options.Width != 0);

            bool isHorizontal = options.Scale == "horizontal" && options.Height != 0;
            bool isVertical = options.Scale == "vertical" && options.Width != 0;

            int height = isScaleBoth || isHorizontal ? options.Height : image.Height;
            int width = isScaleBoth || isVertical ? options.Width : image.Width;

            return new(width, height);
        }


        internal static void ProcessImage(Options options, string path, string fileName, string extension, byte[] imageAsBytes)
        {
            IResampler sampler = KnownResamplers.Bicubic;

            var image = Image.Load<Rgba32>(imageAsBytes);

            // Here stopwatch has the purpose of tracking the timings for the image manipulations
            Stopwatch stopWatch = new();
            stopWatch.Start();

            ResizeOptions resizeOptions = new()
            {
                Size = GetSizeOptions(options, image),
                Mode = GetScaleOptions(options),
                Sampler = sampler
            };

            // Timekeeping
            Debug.WriteLine("Getting mutate config: " + stopWatch.ElapsedMilliseconds);
            stopWatch.Restart();


            // apply configuration defined above
            image.Mutate(x => x.Resize(resizeOptions));

            // Timekeeping
            Debug.WriteLine("Getting mutatation: " + stopWatch.ElapsedMilliseconds);
            stopWatch.Restart();

            // Creates a new image in memory
            Image newImage = image;

            // Save new image to hard drive
            newImage.Save(GetOutputFile(options, path, fileName, extension));

            // Timekeeping
            Debug.WriteLine("Save image: " + stopWatch.ElapsedMilliseconds);
            stopWatch.Stop();
        }
    }
}


