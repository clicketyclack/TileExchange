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

namespace TileExchange
{
	public class MyClass
	{
		public MyClass()
		{
			
		}

		/// <summary>
		/// Load an image from tileset, alter it slightly, the write it back.
		/// </summary>
		public void AlterImagePOC()
		{
			var project_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var tileset_path = String.Format("{0}/../../../assets/tilesets/", project_path);
			var output_path = String.Format("{0}/../../../output/", project_path);

			var source_tileset = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(tileset_path), "colors.png"));
			var loaded = new Bitmap(source_tileset);

			loaded.SetPixel(2, 2, Color.Aquamarine);

			var destination = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(output_path), "altered.png"));
			loaded.Save(destination);
		}
	}


}
