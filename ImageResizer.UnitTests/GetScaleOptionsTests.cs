using ImageResizer;
using NUnit.Framework;
using SixLabors.ImageSharp.Processing;

namespace ImageResizer.UnitTests
{
    internal class GetScaleOptionsTests
    {

        // ------------ GET SCALE OPTIONS ------------ // 

        [Test]
        public void WhenNoModeProvidedFallbackToPad()
        {
            CommandLineOptions commandLineOptions = new()
            {
            };

            Options options = new(commandLineOptions);

            ResizeMode mode = ImageResizer.GetScaleOptions(options);

            Assert.That(mode, Is.EqualTo(ResizeMode.Pad));

        }

        [Test]
        public void WhenValidModeProvidedExpectProvidedMode()
        {
            CommandLineOptions commandLineOptions = new()
            {
                Mode = "crop"
            };

            Options options = new(commandLineOptions);

            ResizeMode mode = ImageResizer.GetScaleOptions(options);

            Assert.That(mode, Is.EqualTo(ResizeMode.Crop));

        }

        [Test]
        public void WhenInvalidModeProvidedFallbackToPad()
        {
            CommandLineOptions commandLineOptions = new()
            {
                Mode = "Invalid Mode"
            };

            Options options = new(commandLineOptions);

            ResizeMode mode = ImageResizer.GetScaleOptions(options);

            Assert.That(mode, Is.EqualTo(ResizeMode.Pad));
        }


    }
}
