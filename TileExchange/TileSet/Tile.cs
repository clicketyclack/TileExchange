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

namespace TileExchange.Tile
{


	/// <summary>
	/// A single tile.
	/// </summary>
	public interface ITile
	{
		Size GetSize();
		Color AverageColor();
	}

	public class Tile : ITile
	{
		protected Bitmap image;
		protected Tile()
		{
			
		}

		public Size GetSize()
		{
			return image.Size;
			//return new Size { Width = 16, Height = 16 };
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
					var color = image.GetPixel(x, y);
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

	public class Square16Tile : Tile
	{
		/// <summary>
		/// Initializes a new simple tile of size 16x16.
		/// </summary>
		public Square16Tile()
		{
			image = new Bitmap(16, 16);
		}
	}

	public class BitmapTile : Tile
	{
		/// <summary>
		/// Create a Tile from a bitmap.
		/// </summary>
		/// <param name="bitmap">Bitmap to base tile off.</param>
		public BitmapTile(Bitmap bitmap)
		{
			this.image = bitmap;
		}
	}

	/// <summary>
	/// Generated solid tile.
	/// </summary>
	public class GeneratedSolidTile : Tile
	{

		public GeneratedSolidTile(Size size, Color color)
		{
			image = new Bitmap(size.Width, size.Height);
			for (var x = 0; x < size.Width; x++)
			{
				for (var y = 0; y < size.Width; y++)
				{
					image.SetPixel(x, y, color);
				}
			}
		}
	}
}
