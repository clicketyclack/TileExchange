using System;
using NUnit.Framework;
using TileExchange.TesselatedImages;

namespace TileExchange
{
	/// <summary>
	/// Verifies basics of image loading. We should be able to find and open images from the assets directory.
	/// </summary>
	[TestFixture]
	public class LoadingBasics
	{
		public LoadingBasics()
		{
		}
		/// <summary>
		/// Verify that we can load images with single fragments.
		/// </summary>
		[Test]
		[TestCase("blue_building.jpg", 650, 1057 )]
		[TestCase("green_leaf.jpg", 1119, 697)]
		[TestCase("red_blue_transitions.jpg", 448, 448)]
		public void SingleFragmentImages(string imagename, int width, int height)
		{
			
			var til = new TesselatedImageLoader();
			var sft = new SingleFragmentTesselator();

			var loaded_image = til.LoadFromImagelibrary(imagename, sft);
			var fragments = loaded_image.GetFragments();

			Assert.AreEqual(1, fragments.Count);

			var fragment = fragments[0];
			Assert.AreEqual(width, fragment.GetSize().Width);
			Assert.AreEqual(height, fragment.GetSize().Height);

		}
			

	}



}
