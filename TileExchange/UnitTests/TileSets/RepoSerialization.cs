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
using NUnit.Framework;

using TileExchange.TileSetTypes;

namespace TileExchange.UnitTests.TileSets
{
	public class RepoSerialization
	{

		/// <summary>
		/// Verify that TileSetRepo can be constructed and serialized.
		/// </summary>
		[Test]
		public void RepoFromScratch()
		{
			var repo = new TileSetRepo.TileSetRepo();
			var repo_string_0ts = repo.Serialize();

			var tsr1 = ProceduralHSVTileSet.Default();
			tsr1.packname = "unique1";
			var tsr2 = ProceduralHSVTileSet.Default();
			tsr2.packname = "unique2";

			repo.AddTileSet(tsr1);
			repo.AddTileSet(tsr2);

			var repo_string_2ts = repo.Serialize();

			var repo0 = TileSetRepo.TileSetRepo.DeSerialize(repo_string_0ts);
			var repo2 = TileSetRepo.TileSetRepo.DeSerialize(repo_string_2ts);

			// System.Console.WriteLine(String.Format("Repo0 serialization {0}", repo_string_0ts));
			// System.Console.WriteLine(String.Format("Repo2 serialization {0}", repo_string_2ts));

			Assert.AreEqual(0, repo0.NumberOfTilesets());
			Assert.AreEqual(2, repo2.NumberOfTilesets());
			StringAssert.Contains("unique1", repo_string_2ts);
		}
	}
}
