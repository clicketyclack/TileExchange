/* 
 * Copyright (C) 2018 Erik Mossberg
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
using System.Drawing;
using NUnit.Framework;
using TileExchange.Fragment;

namespace TileExchange.UnitTests
{
	/// <summary>
	/// Verifies basics of tiles: must have a size and an average color.
	/// </summary>
	[TestFixture]
	public class TileBasics
	{

		/// <summary>
		/// Verify average color calculations.
		/// </summary>
		[Test]
		public void AverageColorSolid()
		{
			var size = new Size { Height = 12, Width = 12 };
			var color = Color.AliceBlue;

			var gst = new ProceduralFragment(size, color);

			Assert.AreEqual(color.R, gst.AverageColor().R);
		}

		/// <summary>
		/// Verify that square16 tiles have basic pixel set support and color calculations.
		/// </summary>
		[Test]
		public void GeneratedBitmapSanity()
		{
			Color misty_rose = ImageProcessor.Imaging.Colors.RgbaColor.FromColor(Color.MistyRose);
			Color red = ImageProcessor.Imaging.Colors.RgbaColor.FromColor(Color.Red);
			BitmapFragment fragment = new ProceduralFragment(new Size { Width = 3, Height = 3 }, misty_rose).ToBitmapFragment();

			var color_pre = fragment.GetPixel(2, 2);
			fragment.SetPixel(2, 2, red);
			var color_post = fragment.GetPixel(2, 2);

			Assert.AreNotEqual(color_pre, color_post);
			Assert.AreEqual(misty_rose, fragment.GetPixel(1, 1));
			Assert.AreEqual(red, fragment.GetPixel(2, 2));
		}

		/// <summary>
		/// Verify that generated bitmaps can correctly calculate average colors.
		/// </summary>
		[Test]
		public void AverageColorGeneratedBitmap()
		{
			BitmapFragment[] fragments = { new Square16Tile().ToBitmapFragment(),
				new ProceduralFragment(new Size { Width = 3, Height = 3}, Color.WhiteSmoke).ToBitmapFragment() };
			foreach (BitmapFragment fragment in fragments)
			{


				var color_pre = fragment.AverageColor();
				fragment.SetPixel(1, 2, Color.Black);
				var color_post = fragment.AverageColor();
				Assert.AreNotEqual(color_pre, color_post);
			}

		}

		/// <summary>
		/// Verify average color for bitmaps with variance.
		/// </summary>
		[Test]
		public void AverageColorBuiltBitmap()
		{

			Bitmap b = new Bitmap(2, 2);
			b.SetPixel(0, 0, ImageProcessor.Imaging.Colors.RgbaColor.FromRgba(0, 0, 0xff, 0xff));
			b.SetPixel(0, 1, ImageProcessor.Imaging.Colors.RgbaColor.FromRgba(0, 0, 0xff, 0xff));
			b.SetPixel(1, 0, ImageProcessor.Imaging.Colors.RgbaColor.FromRgba(0, 0xff, 0xff, 0xff));
			b.SetPixel(1, 1, ImageProcessor.Imaging.Colors.RgbaColor.FromRgba(0, 0xff, 0xff, 0xff));



			var bt = new BitmapFragment(b);
			var average = bt.AverageColor();

			Assert.AreEqual(0, average.R);
			Assert.AreEqual(0x7f, average.G);
			Assert.AreEqual(0xff, average.B);

		}
	}
}
