using NUnit.Framework;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Png;

namespace ImageResizer.UnitTests
{
    internal class ToByteArrayTests
    {
        private static Image<Rgba32> ImageUnderTest => new(100, 200, new());

        [Test]
        public void WhenPassedImageExpectReadableByteArray()
        {
            byte[] byteArray = ImageResizer.ToByteArray(ImageUnderTest, PngFormat.Instance);

            Image newImage = Image.Load(byteArray);

            Assert.That(byteArray[0], Is.EqualTo(137));
        }
    }
}
