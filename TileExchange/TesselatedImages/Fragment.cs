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
using System.Drawing;

namespace TileExchange.Fragment
{


	/// <summary>
	/// A single Fragment, representation of a single puzzle piece.
	/// </summary>
	public interface IFragment
	{
		Size GetSize();
		Color AverageColor();
		Color GetPixel(int x, int y);
		Bitmap AsBitmap();
	}

	public class ProceduralFragment : IFragment
	{
		private Size size;
		private Color color;
		public ProceduralFragment(Size size, Color color)
		{
			this.size = size;
			this.color = color;
		}

		public Color AverageColor()
		{
			return color;
		}

		public Size GetSize()
		{
			return size;
		}

		public Color GetPixel(int x, int y)
		{
			return color;
		}

		public Bitmap AsBitmap()
		{
			return ToBitmapFragment().AsBitmap();
		}

		public BitmapFragment ToBitmapFragment()
		{
			var width = GetSize().Width;
			var height = GetSize().Height;
			Bitmap bitmap = new Bitmap(size.Width, size.Height);
			for (var x = 0; x < size.Width; x++)
			{
				for (var y = 0; y < size.Width; y++)
				{
					bitmap.SetPixel(x, y, GetPixel(x, y));
				}
			}

			return new BitmapFragment(bitmap);
		}
	}



	public class BitmapFragment : IFragment
	{
		protected Bitmap image;

		/// <summary>
		/// Create a Tile from a bitmap.
		/// </summary>
		/// <param name="bitmap">Bitmap to base tile off.</param>
		public BitmapFragment(Bitmap bitmap)
		{
			var size = new Rectangle(new Point(0, 0), bitmap.Size);
			this.image = bitmap.Clone(size, bitmap.PixelFormat);
		}

		public Size GetSize()
		{
			return image.Size;
		}

		/// <summary>
		/// Sets the pixel.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="color">Color.</param>
		public void SetPixel(int x, int y, Color color)
		{
			this.image.SetPixel(x, y, color);
		}

		public Color GetPixel(int x, int y)
		{
			return this.image.GetPixel(x, y);
		}

		public Bitmap AsBitmap()
		{
			return image;
		}


		/// <summary>
		/// Calculate average 
		/// </summary>
		/// <returns>A new color with RGB channels matching the average of the tile.</returns>
		public Color AverageColor()
		{
			int r = 0;
			int g = 0;
			int b = 0;

			var size = GetSize();
			int pixcount = size.Width * size.Height;

			for (var x = 0; x < size.Width; x++)
			{
				for (var y = 0; y < size.Width; y++)
				{
					var color = GetPixel(x, y);
					r += color.R;
					g += color.G;
					b += color.B;

				}
			}

			byte br = (Byte)(r / pixcount);
			byte bg = (Byte)(g / pixcount);
			byte bb = (Byte)(b / pixcount);

			return ImageProcessor.Imaging.Colors.RgbaColor.FromRgba(br, bg, bb, 0xff);
		}
	}


	/// <summary>
	/// Hardcoded 16x16 image fragment.
	/// </summary>
	public class Square16Tile : CustomRectangleFragment
	{
		/// <summary>
		/// Initializes a new simple tile of size 16x16.
		/// </summary>
		public Square16Tile() : base(new Size { Width = 16, Height = 16 })
		{
		}
	}


	/// <summary>
	/// Image fragment which can be custom dimensions.
	/// </summary
	public class CustomRectangleFragment : ProceduralFragment
	{
		public CustomRectangleFragment(Size size) : base(size, Color.AliceBlue)
		{
		}
	}
}
