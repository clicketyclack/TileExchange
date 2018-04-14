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
using TileExchange.TileSet;

namespace TileExchange.UnitTests
{
	/// <summary>
	/// Searches tiles via average color.
	/// </summary>
	[TestFixture]
	public class SearchByColor
	{

		/// <summary>
		/// Verify tileset coloring.
		/// </summary>
		[Test]
		public void TileSetColorFilter()
		{
			var tsf = new TileSetFinder();
			var ts_found = (IHueMatchingTileset)tsf[0];
			for (var tsn = 0; tsn < tsf.NumberOfTilesets(); tsn++)
			{
				ITileSet ts = tsf[tsn];
				if (ts.NumberOfTiles() == 4 * 4)
				{
					ts_found = (IHueMatchingTileset)ts;
				}
			}

			Assert.AreEqual(5, ts_found.TilesByHue(0.0f, 0.01f).Count);
		}
	}
}
