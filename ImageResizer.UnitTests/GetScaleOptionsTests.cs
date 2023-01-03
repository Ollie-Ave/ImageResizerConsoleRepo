using CommandLineParsingTest;
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


            ResizeMode mode = Program.GetScaleOptions(commandLineOptions);

            Assert.That(mode, Is.EqualTo(ResizeMode.Pad));

        }

        [Test]
        public void WhenValidModeProvidedExpectProvidedMode()
        {
            CommandLineOptions commandLineOptions = new()
            {
                Mode = "crop"
            };

            ResizeMode mode = Program.GetScaleOptions(commandLineOptions);

            Assert.That(mode, Is.EqualTo(ResizeMode.Crop));

        }

        [Test]
        public void WhenInvalidModeProvidedFallbackToPad()
        {
            CommandLineOptions commandLineOptions = new()
            {
                Mode = "Invalid Mode"
            };


            ResizeMode mode = Program.GetScaleOptions(commandLineOptions);

            Assert.That(mode, Is.EqualTo(ResizeMode.Pad));
        }


    }
}
