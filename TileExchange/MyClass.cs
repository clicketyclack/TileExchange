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
