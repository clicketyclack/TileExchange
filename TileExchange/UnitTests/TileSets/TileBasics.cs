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
using NUnit.Framework;
using TileExchange.Tile;

namespace TileExchange.UnitTests
{
	/// <summary>
	/// Verifies basics of tiles: must have a size and an average color.
	/// </summary>
	[TestFixture]	
	public class TileBasics
	{
		
		/// <summary>
		/// Test that ImageProcessor works via a rgba -> hsl conversion.
		/// </summary>
		[Test]
		public void AverageColor()
		{
			var size = new Size { Height = 12, Width = 12 };
			var color = Color.AliceBlue;


			var gst = new GeneratedSolidTile(size, color);

			Assert.AreEqual(color.R, gst.AverageColor().R);

		}
	}
}
