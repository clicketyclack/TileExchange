using System;
using ImageProcessor;
using NUnit.Framework;

namespace TileExchange
{
	public class MyClass
	{
		public MyClass()
		{
		}
	}

	/// <summary>
	/// This fixture only verifies that project imports and dependencies work.
	/// Created due to some issues with dependencies not matching my .net target profile.
	/// </summary>
	[TestFixture]
	public class PackageReferenceTests
	{

		/// <summary>
		/// Test that ImageProcessor works via a rgba -> hsl conversion.
		/// </summary>
		[Test]
		public void UseRgbaObjects()
		{
			
			var rgba = ImageProcessor.Imaging.Colors.RgbaColor.FromRgba(222,205,171,255);
			var hsl = ImageProcessor.Imaging.Colors.HslaColor.FromColor(rgba);

			Assert.IsTrue(39.9/360.0 <= hsl.H && hsl.H <= 40.1/360.0);

		}
	}
}
