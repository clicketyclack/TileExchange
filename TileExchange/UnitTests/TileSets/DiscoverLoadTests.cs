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

namespace TileExchange
{
	[TestFixture]
	public class DiscoverLoadTests
	{
		/// <summary>
		/// Check number of tilesets  in a directory.
		/// </summary>
		[Test]
		public void DiscoverTileSets()
		{
			var tsf = new TileSetFinder();
			Assert.AreEqual(2, tsf.NumberOfTilesets());

			var t1 = tsf.TileSet(0).Tile(0);
			Assert.AreEqual(t1.GetSize().Width, 16);
		}

	}
}