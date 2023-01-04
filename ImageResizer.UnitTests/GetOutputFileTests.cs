using ImageResizer;
using NUnit.Framework;

namespace ImageResizer.UnitTests
{
    internal class GetOutputFileTests
    {

        // ------------ GET OUTPUT FILE ------------ // 

        [Test]
        public void WhenAllArgumentsPassedToImageExpectAllArgumentsInFilename()
        {
            // CommandLineOptions options, string path, string fileName, string extension

            CommandLineOptions commandLineOptions = new()
            {
                Height = 100,
                Width = 100,
                Scale = "both",
                Mode = "crop",
                InputFile = "C:\\test.png"
            };

            Options options = new(commandLineOptions);

            string outPath = "C:\\";

            string outputFile = ImageResizer.GetOutputFile(options, outPath);

            Assert.That(outputFile, Is.EqualTo("C:\\test---w-100---h-100---m-crop---s-both.png"));
        }

        [Test]
        public void WhenModeIsOmittedExpectOnlyPassedArgumentsInFilename()
        {
            CommandLineOptions commandLineOptions = new()
            {
                Height = 100,
                Width = 100,
                Scale = "both",
                InputFile = "C:\\test.png"
            };

            Options options = new(commandLineOptions);

            string outPath = "C:\\";

            string outputFile = ImageResizer.GetOutputFile(options, outPath);

            Assert.That(outputFile, Is.EqualTo("C:\\test---w-100---h-100---s-both.png"));
        }

        [Test]
        public void WhenScaleIsOmittedExpectOnlyPassedArgumentsInFilename()
        {
            CommandLineOptions commandLineOptions = new()
            {
                Height = 100,
                Width = 100,
                Mode = "crop",
                InputFile = "C:\\test.png"
            };

            Options options = new(commandLineOptions);

            string outPath = "C:\\";

            string outputFile = ImageResizer.GetOutputFile(options, outPath);

            Assert.That(outputFile, Is.EqualTo("C:\\test---w-100---h-100---m-crop.png"));
        }

        [Test]
        public void WhenHeightIsOmittedExpectOnlyPassedArgumentsInFilename()
        {
            CommandLineOptions commandLineOptions = new()
            {
                Width = 100,
                Mode = "crop",
                Scale = "both",
                InputFile = "C:\\test.png"
            };
;

            Options options = new(commandLineOptions);

            string outPath = "C:\\";

            string outputFile = ImageResizer.GetOutputFile(options, outPath);

            Assert.That(outputFile, Is.EqualTo("C:\\test---w-100---m-crop---s-both.png"));
        }

        [Test]
        public void WhenWidthIsOmittedExpectOnlyPassedArgumentsInFilename()
        {
            CommandLineOptions commandLineOptions = new()
            {
                Height = 100,
                Mode = "crop",
                Scale = "both",
                InputFile = "C:\\test.png"
            };


            Options options = new(commandLineOptions);

            string outPath = "C:\\";

            string outputFile = ImageResizer.GetOutputFile(options, outPath);

            Assert.That(outputFile, Is.EqualTo("C:\\test---h-100---m-crop---s-both.png"));
        }


        [Test]
        public void WhenAllArgumentsExceptInputFileOmittedExpectDefaultFilename()
        {
            CommandLineOptions commandLineOptions = new()
            {
                InputFile = "C:\\test.png"
            };

            Options options = new(commandLineOptions);

            string outPath = "C:\\";

            string outputFile = ImageResizer.GetOutputFile(options, outPath);

            Assert.That(outputFile, Is.EqualTo("C:\\test.png"));
        }

        [Test]
        public void WhenNoArgumentsPassedAtAllExpectErrorString()
        {
            CommandLineOptions commandLineOptions = new()
            { 
            };

            Options options = new(commandLineOptions);

            string outPath = "C:\\";

            string outputFile = ImageResizer.GetOutputFile(options, outPath);

            Assert.That(outputFile, Is.EqualTo("Error, inputFile not specified"));
        }

    }
}
