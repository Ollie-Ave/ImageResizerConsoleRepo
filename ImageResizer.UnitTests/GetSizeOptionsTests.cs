using NUnit.Framework;

namespace ImageResizer.UnitTests
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;

    public class GetSizeOptionsTests
    {
        // ------------ GET SIZE OPTIONS ------------ // 

        private static Image<Rgba32> ImageUnderTest => new(100, 200, new());

        [Test]
        public void WhenNoHeightProvidedExpectFallbackToDefaultHeight()
        {

            CommandLineOptions commandLineOptions = new()
            {
                Width = 100,
            };

            Options options = new(commandLineOptions);

            Size actual = ImageResizer.GetSizeOptions(options, ImageUnderTest);

            Assert.That(actual.Height, Is.EqualTo(200));
        }

        [Test]
        public void WhenHeightProvidedButNoScaleExpectFallbackToDefaultHeight()
        {

            CommandLineOptions commandLineOptions = new()
            {
                Width = 100,
                Height = 350
            };

            Options options = new(commandLineOptions);

            Size actual = ImageResizer.GetSizeOptions(options, ImageUnderTest);

            Assert.That(actual.Height, Is.EqualTo(200));
        }

        [Test]
        public void WhenHeightProvidedAndScaleHorizontalExpectProvidedHeight()
        {

            CommandLineOptions commandLineOptions = new()
            {
                Width = 100,
                Height = 350,
                Scale = "horizontal"
            };

            Options options = new(commandLineOptions);

            Size actual = ImageResizer.GetSizeOptions(options, ImageUnderTest);

            Assert.That(actual.Height, Is.EqualTo(350));
        }

        [Test]
        public void WhenNoWidthProvidedExpectFallbackToDefaultWidth()
        {

            CommandLineOptions commandLineOptions = new()
            {
                Height = 100,
            };

            Options options = new(commandLineOptions);

            Size actual = ImageResizer.GetSizeOptions(options, ImageUnderTest);

            Assert.That(actual.Width, Is.EqualTo(100));
        }

        [Test]
        public void WhenWidthProvidedButNoScaleExpectFallbackToDefaultWidth()
        {

            CommandLineOptions commandLineOptions = new()
            {
                Width = 200,
                Height = 350
            };

            Options options = new(commandLineOptions);

            Size actual = ImageResizer.GetSizeOptions(options, ImageUnderTest);

            Assert.That(actual.Width, Is.EqualTo(100));
        }

        [Test]
        public void WhenWidthProvidedAndScaleVerticalExpectProvidedWidth()
        {

            CommandLineOptions commandLineOptions = new()
            {
                Width = 200,
                Height = 350,
                Scale = "vertical"
            };

            Options options = new(commandLineOptions);

            Size actual = ImageResizer.GetSizeOptions(options, ImageUnderTest);

            Assert.That(actual.Width, Is.EqualTo(200));
        }

        [Test]
        public void WhenBothWidthAndHeightProvidedAndScaleIsBothExpectProvidedValues()
        {
            CommandLineOptions commandLineOptions = new()
            {
                Width = 200,
                Height = 350,
                Scale = "both"
            };

            Options options = new(commandLineOptions);

            Size actual = ImageResizer.GetSizeOptions(options, ImageUnderTest);

            Size expectedResult = new()
            {
                Width = 200,
                Height = 350,
            };

            Assert.That(actual, Is.EqualTo(expectedResult));
        }



    }
}