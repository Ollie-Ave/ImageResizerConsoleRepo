using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ImageResizer.UnitTests")]

namespace ImageResizer
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;
    using SixLabors.ImageSharp.Processing.Processors.Transforms;
    using System.Diagnostics;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Formats;
    using SixLabors.ImageSharp.Advanced;
    using SixLabors.ImageSharp.Formats.Png;
    using SixLabors.ImageSharp.Formats.Bmp;
    using SixLabors.ImageSharp.Formats.Gif;
    using SixLabors.ImageSharp.Formats.Jpeg;
    using SixLabors.ImageSharp.Formats.Pbm;
    using SixLabors.ImageSharp.Formats.Tiff;
    using SixLabors.ImageSharp.Formats.Tga;
    using SixLabors.ImageSharp.Formats.Webp;

    public static class ImageResizer
    {
        internal static string GetOutputFile(Options options, string outLocation)
        {
            // This function returns the new (full) filepath of the image.

            if (!string.IsNullOrWhiteSpace(options.InputFile))
            {
                // gets the file extension of the inputted image
                string extension = Path.GetExtension(options.InputFile);

                // The name of the inputted image (without extension)
                string fileName = Path.GetFileNameWithoutExtension(options.InputFile);


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

                    options.OutputFile = Path.Combine(outLocation, ouputFilename);
                }

                return options.OutputFile;
            }

            return "Error, inputFile not specified";
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

        public static byte[] ToByteArray(Image image, IImageFormat imageFormat)
        {
            // This function removes dependency on filesystem
            // I.e it allows ImageResizer.ProcessImage not to care about how you got the image (stored as a byte array)
            // All the previously mentioned function cares about is simply that it has the image, not where you got it from

            using (var memoryStream = new MemoryStream())
            {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(imageFormat);
                image.Save(memoryStream, imageEncoder);
                return memoryStream.ToArray();
            }
        }

        public static IImageFormat GetImageFormat(string extension)
        {
            // Converts String representation of extension to imagesharp format

            return extension.ToLower() switch
            {
                ".bmp" => BmpFormat.Instance,
                ".gif" => GifFormat.Instance,
                ".jpeg" => JpegFormat.Instance,
                ".jpg" => JpegFormat.Instance,
                ".jfif" => JpegFormat.Instance,
                ".png" => PngFormat.Instance,
                ".pbm" => PbmFormat.Instance,
                ".tiff" => TiffFormat.Instance,
                ".tga" => TgaFormat.Instance,
                ".webp" => WebpFormat.Instance,
                _ => PngFormat.Instance // FileFormat Unsupported by imageSharp, Fallback to png formatting
            };
        }

        internal static byte[] ProcessImage(Options options, byte[] imageAsBytes)
        {
            IResampler sampler = KnownResamplers.Bicubic;


            // gets the file extension of the inputted image
            string extension = Path.GetExtension(options.InputFile);

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
            byte[] newImageAsBytes = ToByteArray(image, GetImageFormat(extension));


            // Timekeeping
            Debug.WriteLine("Save image: " + stopWatch.ElapsedMilliseconds);
            stopWatch.Stop();


            return newImageAsBytes;
        }

    }
}


