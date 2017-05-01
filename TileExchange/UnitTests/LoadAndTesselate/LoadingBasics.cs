/* 
 * Copyright (C) 2017 Erik Mossberg
 *
 * This file is part of TileExchanger.
 *
 * TileExchanger is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * TileExchanger is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 * 
 */
using System;
using System.Linq;
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
		[TestCase("blue_building.jpg", 650, 1057)]
		[TestCase("green_leaf.jpg", 1119, 697)]
		[TestCase("red_blue_transitions.jpg", 448, 448)]
		public void SingleFragmentImages(string imagename, int width, int height)
		{

			var til = new TesselatedImageLoader();
			var sft = new SingleFragmentTesselator();

			var loaded_image = til.LoadFromImagelibrary(imagename, sft);
			var fragments = loaded_image.GetImageFragments();

			Assert.AreEqual(1, fragments.Count);

			var fragment = fragments[0];
			Assert.AreEqual(width, fragment.GetReplacementFragment().GetSize().Width);
			Assert.AreEqual(height, fragment.GetReplacementFragment().GetSize().Height);

		}

		/// <summary>
		/// Verify that a image can be evenly divisible into 16x16 fragments.
		/// </summa>
		[Test]
		public void BasicFragmentation()
		{
			var til = new TesselatedImageLoader();
			var sft = new Basic16Tesselator();
			var loaded_image = til.LoadFromImagelibrary("red_blue_transitions.jpg", sft);
			var fragments = loaded_image.GetImageFragments();

			Assert.AreEqual(28 * 28, fragments.Count);
			Assert.AreNotEqual(fragments[0].GetPosition(), fragments[1].GetPosition());

			var bottom_corner =
				from fragment in fragments
				where fragment.GetPosition().X > 175 && fragment.GetPosition().Y > 275
				select fragment.GetPosition();

			// [288, 304, 320, 336, 352, 368, 384, 400, 416, 432]  <- 10
			// [176, 192, 208, 224, 240, 256, 272, 288, 304, 320, 336, 352, 368, 384, 400, 416, 432] <- 17
			Assert.AreEqual(10 * 17, bottom_corner.Count());

		}
	}
}
