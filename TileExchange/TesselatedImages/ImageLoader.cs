using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;

namespace TileExchange.TesselatedImages
{

	/// <summary>
	/// Tesselated image. Combines a standard image (png etc) with a set of fragments.
	/// The fragments represent tiles or puzzle pieces.
	/// </summary>
	public interface ITesselatedImage
	{
		List<IFragment> GetFragments();
	}

	class TesselatedImage : ITesselatedImage
	{

		private Bitmap bitmap;
		private List<IFragment> fragments;
		public TesselatedImage(Bitmap bitmap, List<IFragment> fragments)
		{
			this.bitmap = bitmap;
			this.fragments = fragments;
		}

		public List<IFragment> GetFragments()
		{
			return fragments;
		}
	}

	/// <summary>
	/// Representation of a single puzzle piece.
	/// </summary>
	public interface IFragment
	{
		
		Size GetSize();
	}

	public class Square16Fragment : IFragment
	{
		public Size GetSize()
		{
			return new Size { Width = 16, Height = 16 };
		}
	}

	public class CustomRectangleFragment : IFragment
	{

		private Size size;
		public CustomRectangleFragment(Size size)
		{
			this.size = size;
		}

		public Size GetSize()
		{
			return size;
		}
	}

	/// <summary>
	/// Load an image.
	/// </summary>
	public class TesselatedImageLoader
	{
		private string project_path;
		private string images_path;
		public TesselatedImageLoader()
		{
			project_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			images_path = String.Format("{0}/../../../assets/images/", project_path);
		}

		private Bitmap LoadBitmap(string filename) 
		{
			
			var to_open = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(images_path), filename));
			var loaded = new Bitmap(to_open);
			return loaded;
		}

		public ITesselatedImage LoadFromImagelibrary(string filename, ITesselator tesselation_tactic)
		{
			var bitmap = LoadBitmap(filename);
			var toreturn = new TesselatedImage(bitmap, tesselation_tactic.FragmentImage(bitmap));
			return toreturn;
		}

	}

	/// <summary>
	/// Write out the image to disk.
	/// </summary>
	public class ImageWriter
	{
		private string project_path;
		private string output_path;
		public ImageWriter()
		{
			project_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			output_path = String.Format("{0}/../../../output/", project_path);
		}

		public void WriteBitmap(Bitmap bitmap, string filename)
		{
			var destination = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(output_path), "altered.png"));
			bitmap.Save(destination);
		}

	}

	public interface ITesselator
	{
		List<IFragment> FragmentImage(Bitmap bitmap);
	}

	/// <summary>
	/// Bitmap tesselator.
	/// </summary>
	public class SingleFragmentTesselator : ITesselator
	{
		public List<IFragment> FragmentImage(Bitmap bitmap)
		{
			
			var toreturn = new List<IFragment>();
			toreturn.Add(new CustomRectangleFragment(bitmap.Size));
			return toreturn;
		}
	}
}

