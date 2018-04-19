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
using NUnit.Framework;
using TileExchange.TileSetRepo;
using TileExchange.TileSetTypes;

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
			var tsr = new TileSetRepo.TileSetRepo();
			tsr.Discover();
			var ts_found = (IHueMatchingTileset)tsr.ByName("Pixel Palette")[0];
			Assert.AreEqual(5, ts_found.TilesByHue(0.0f, 0.01f).Count);
		}
	}
}
