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
using NUnit.Framework;

namespace TileExchange
{
	/// <summary>
	/// This fixture only verifies that project imports and dependencies work.
	/// Created due to some issues with dependencies not matching my .net target profile.
	/// </summary>
	[TestFixture]

	public class ProjectBasics
	{
		public ProjectBasics()
		{
		}

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

	}
}
