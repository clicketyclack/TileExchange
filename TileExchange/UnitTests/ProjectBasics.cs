﻿/* 
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
using NUnit.Framework;
using System.IO;
using System.Drawing;

using TileExchange.ExchangeEngine;

namespace TileExchange
{
	/// <summary>
	/// This fixture only verifies that project imports and dependencies work.
	/// Created due to some issues with dependencies not matching my .net target profile.
	/// </summary>
	[TestFixture]
	public class ProjectBasics
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
		/// Load an image from tileset, alter it slightly, the write it back.
		/// Will fail if png read/write starts causing issues.
		/// 
		/// If your tests fail here, check that "assets" and "output" paths exist.
		/// </summary>
		[Test]
		public void AlterImagePOC()
		{

			var tileset_path = UserSettings.GetDefaultPath("tileset_path");
			var output_path = UserSettings.GetDefaultPath("output_path");

			var source_tileset = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(tileset_path), "test/colors.png"));
			var loaded = new Bitmap(source_tileset);

			loaded.SetPixel(2, 2, Color.Aquamarine);

			var destination = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(output_path), "altered.png"));
			loaded.Save(destination);
		}
	}
}
