using System;
using NUnit.Framework;
using System.IO;
using System.Drawing;
using System.Reflection;

namespace TileExchange
{
	public class MyClass
	{
		public MyClass()
		{
			
		}

		/// <summary>
		/// Load an image from tileset, alter it slightly, the write it back.
		/// </summary>
		public void AlterImagePOC()
		{
			var project_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var tileset_path = String.Format("{0}/../../../assets/tilesets/", project_path);
			var output_path = String.Format("{0}/../../../output/", project_path);

			var source_tileset = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(tileset_path), "colors.png"));
			var loaded = new Bitmap(source_tileset);

			loaded.SetPixel(2, 2, Color.Aquamarine);

			var destination = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(output_path), "altered.png"));
			loaded.Save(destination);
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

			var rgba = ImageProcessor.Imaging.Colors.RgbaColor.FromRgba(222, 205, 171, 255);
			var hsl = ImageProcessor.Imaging.Colors.HslaColor.FromColor(rgba);

			Assert.IsTrue(39.9 / 360.0 <= hsl.H && hsl.H <= 40.1 / 360.0);



		}
		/// <summary>
		/// Run POC method. Will fail if png read/write starts causing issues.
		/// 
		/// If your tests fail here, check that "assets" and "output" paths exist.
		/// </summary>
		[Test]
		public void AlterImagePOC()
		{
			var poc = new MyClass();
			poc.AlterImagePOC();
		}
	}
}
