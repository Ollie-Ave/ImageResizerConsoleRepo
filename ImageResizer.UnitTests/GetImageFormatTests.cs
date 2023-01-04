using NUnit.Framework;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace ImageResizer.UnitTests
{
    internal class GetImageFormatTests
    {
        [Test]
        public void WhenFileFormatInvalidExpectFallbackToPngFormatInstance()
        {
            string extension = ".dwjiao";

            IImageFormat format = ImageResizer.GetImageFormat(extension);

            Assert.That(format, Is.EqualTo(PngFormat.Instance));
        }

        [Test]
        public void WhenFileFormatValidExpectProvidedFormatInstance()
        {
            string extension = ".jpg";

            IImageFormat format = ImageResizer.GetImageFormat(extension);

            Assert.That(format, Is.EqualTo(JpegFormat.Instance));
        }

    }
}
