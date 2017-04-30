using System;
using NUnit.Framework;

namespace TileExchange
{
	public class ImageEditingTests
	{
		public ImageEditingTests()
		{
		}

		/// <summary>
		/// Run POC method. Will fail if png read/write starts causing issues.
		/// 
		/// If your tests fail here, check that "assets" and "output" paths exist.
		/// </summary>
		[Test]
		public void AlterImagePOC()
		{
			var poc = new MyClass();
			poc.AlterImagePOC();
		}
	}
}