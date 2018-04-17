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

namespace TileExchange.UnitTests.TileSets
{
	public class RepoSerialization
	{
		public RepoSerialization()
		{
		}


		/// <summary>
		/// Verify that TileSetRepo can be constructed and serialized.
		/// </summary>
		[Test]
		public void RepoFromScratch()
		{
			var repo = new TileSetRepo.TileSetRepo();

			var repo_string = repo.Serialize();

			var repo2 = TileSetRepo.TileSetRepo.DeSerialize(repo_string);


		}


	}
}
