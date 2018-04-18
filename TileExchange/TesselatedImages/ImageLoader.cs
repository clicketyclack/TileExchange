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
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;

using TileExchange.Fragment;
using TileExchange.ExchangeEngine;

namespace TileExchange.TesselatedImages
{


	/// <summary>
	/// An Image fragment, which is sub-section of a larger image.
	/// </summary>
	public interface IImageFragment
	{
		/// <summary>
		/// Gets the original fragment.
		/// </summary>
		/// <returns>The fragment.</returns>
		IFragment GetOriginalFragment();

		/// <summary>
		/// Gets the position of the fragment in the picture.
		/// </summary>
		/// <returns>The position.</returns>
		Point GetPosition();

		/// <summary>
		/// Set the replacement fragment.
		/// </summary>
		void SetReplacementFragment(IFragment fragment);

		/// <summary>
		/// Gets the replacement fragment.
		/// </summary>
		/// <returns>The replacement fragment.</returns>
		IFragment GetReplacementFragment();
	}

	public class ImageFragment : IImageFragment
	{
		private IFragment replacement;
		private IFragment original_fragment;
		private Point position;

		public ImageFragment(IFragment original_fragment, Point position)
		{
			this.original_fragment = original_fragment;
			this.position = position;
			this.replacement = original_fragment;
		}

		public IFragment GetOriginalFragment()
		{
			return original_fragment;
		}

		/// <summary>
		/// Gets the position of the fragment in the picture.
		/// </summary>
		/// <returns>The position.</returns>
		public Point GetPosition()
		{
			return position;
		}

		/// <summary>
		/// Set the replacement fragment.
		/// </summary>
		public void SetReplacementFragment(IFragment fragment)
		{
			replacement = fragment;
		}

		/// <summary>
		/// Gets the replacement fragment.
		/// </summary>
		/// <returns>The replacement fragment.</returns>
		public IFragment GetReplacementFragment()
		{
			return replacement;
		}
		                     
	}


	/// <summary>
	/// Tesselated image. Combines a standard image (png etc) with a set of fragments.
	/// The fragments represent tiles or puzzle pieces.
	/// </summary>
	public interface ITesselatedImage
	{
		List<IImageFragment> GetImageFragments();
		Bitmap AssembleFragments();
		Bitmap OriginalImage();
	}

	class TesselatedImage : ITesselatedImage
	{

		private Bitmap bitmap;
		private List<IImageFragment> fragments;
		public TesselatedImage(Bitmap bitmap, List<IImageFragment> fragments)
		{
			this.bitmap = bitmap;
			this.fragments = fragments;
		}

		public List<IImageFragment> GetImageFragments()
		{
			return fragments;
		}

		/// <summary>
		/// Returns the original image.
		/// </summary>
		/// <returns>The original image.</returns>
		public Bitmap OriginalImage()
		{
			return bitmap;
		}

		public Bitmap AssembleFragments() {
			var width = 0;
			var height = 0;
			foreach (var fragment in fragments)
			{
				var width_required = fragment.GetPosition().X + fragment.GetReplacementFragment().GetSize().Width;
				if (width < width_required)
				{
					width = width_required;
				}
				var height_required = fragment.GetPosition().Y + fragment.GetReplacementFragment().GetSize().Height;
				if (height<height_required)
				{
					height = height_required;
				}
			}

			var toreturn = new Bitmap(width, height);


			Graphics g = Graphics.FromImage(toreturn);
			foreach (var fragment in fragments)
			{
				var replacement = fragment.GetReplacementFragment();

				var pos_x = fragment.GetPosition().X;
				var pos_y = fragment.GetPosition().Y;

				var draw_width = replacement.GetSize().Width;
				var draw_height = replacement.GetSize().Height;

				Rectangle area = new Rectangle(0, 0, draw_width, draw_height);
				g.DrawImage(replacement.AsBitmap(), pos_x, pos_y, area, GraphicsUnit.Pixel);

			}
			g.Dispose();

			return toreturn;
		}
	}

	/// <summary>
	/// Load an image.
	/// </summary>
	public class TesselatedImageLoader
	{
		
		public TesselatedImageLoader()
		{
		}

		private Bitmap LoadBitmap(string filename)
		{

			var images_path = UserSettings.GetDefaultPath("images_path");
			var to_open = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(images_path), filename));
			var loaded = new Bitmap(to_open);
			return loaded;
		}

		public ITesselatedImage LoadFromImagelibrary(string filename, ITesselator tesselation_tactic)
		{
			var bitmap = LoadBitmap(filename);
			var toreturn = new TesselatedImage(bitmap, tesselation_tactic.FragmentImage(bitmap));
			return toreturn;
		}
	}

	/// <summary>
	/// Write out the image to disk.
	/// </summary>
	public class ImageWriter
	{
		public ImageWriter()
		{			
		}

		/// <summary>
		/// Writes a bitmap to a filename.
		/// </summary>
		/// <param name="bitmap">Bitmap.</param>
		/// <param name="filename">Filename.</param>
		public void WriteBitmap(Bitmap bitmap, string filename)
		{
			var output_path = UserSettings.GetDefaultPath("output_path");
			var file_abspath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(output_path), filename));

			output_path = System.IO.Path.GetDirectoryName(file_abspath);
			if (!System.IO.Directory.Exists(output_path)) {
				var classname = this.GetType().Name;
				var msg = String.Format("{0} trying to write file {1} but directory {2} does not exist or is not a directory.", classname, filename, output_path);
				throw new IOException(msg);
			}

			bitmap.Save(file_abspath);
		}

	}

	public interface ITesselator
	{
		List<IImageFragment> FragmentImage(Bitmap bitmap);
	}

	/// <summary>
	/// Bitmap tesselator.
	/// </summary>
	public class SingleFragmentTesselator : ITesselator
	{
		public List<IImageFragment> FragmentImage(Bitmap bitmap)
		{

			var toreturn = new List<IImageFragment>();

			var fragment = new BitmapFragment(bitmap);
			var single_fragment = new ImageFragment(fragment, new Point { X = 0, Y = 0 });

			toreturn.Add(single_fragment);
			return toreturn;
		}
	}

	/// <summary>
	/// Tesselator that turns an image into a set of 16x16 fragments.
	/// </summary>
	public class Basic16Tesselator : ITesselator
	{
		public List<IImageFragment> FragmentImage(Bitmap bitmap)
		{
			var toreturn = new List<IImageFragment>();
			var xtilecount = Math.Floor(bitmap.Size.Width / 16.0);
			var ytilecount = Math.Floor(bitmap.Size.Height / 16.0);

			for (var xtilenr = 0; xtilenr < xtilecount; xtilenr++)
			{
				for (var ytilenr = 0; ytilenr < ytilecount; ytilenr++)
				{

					var position = new Point(xtilenr * 16, ytilenr * 16);
					var size = new Size(new Point(16, 16));
					var sub_bitmap = bitmap.Clone(new Rectangle(position, size), bitmap.PixelFormat);

					var fragment = new BitmapFragment(sub_bitmap);
					var single_fragment = new ImageFragment(fragment, position);

					toreturn.Add(single_fragment);
				}
			}

			return toreturn;
		}
	}
}

