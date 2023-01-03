namespace ImageResizer.UnitTests
{
    using CommandLineParsingTest;
    using NUnit.Framework;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;

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

            Size actual = Program.GetSizeOptions(commandLineOptions, ImageUnderTest);

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

            Size actual = Program.GetSizeOptions(commandLineOptions, ImageUnderTest);

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

            Size actual = Program.GetSizeOptions(commandLineOptions, ImageUnderTest);

            Assert.That(actual.Height, Is.EqualTo(350));
        }

        [Test]
        public void WhenNoWidthProvidedExpectFallbackToDefaultWidth()
        {

            CommandLineOptions commandLineOptions = new()
            {
                Height = 100,
            };

            Size actual = Program.GetSizeOptions(commandLineOptions, ImageUnderTest);

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

            Size actual = Program.GetSizeOptions(commandLineOptions, ImageUnderTest);

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

            Size actual = Program.GetSizeOptions(commandLineOptions, ImageUnderTest);

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

            Size actual = Program.GetSizeOptions(commandLineOptions, ImageUnderTest);

            Size expectedResult = new()
            {
                Width = 200,
                Height = 350,
            };

            Assert.That(actual, Is.EqualTo(expectedResult));
        }



    }
}