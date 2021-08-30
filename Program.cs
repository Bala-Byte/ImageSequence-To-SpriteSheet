using System;
using System.IO;
using System.Linq;
using System.Drawing;

namespace SpriteSheetGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			const string outputFileName = @"\output.png";
			Console.WriteLine("Please insert the folder location contaning the images:");

			string imageFolderLocation;
			imageFolderLocation = Console.ReadLine();

			if (imageFolderLocation[0] == '"')
				imageFolderLocation = imageFolderLocation.Trim('"');

			if (Directory.Exists(imageFolderLocation))
            {
				// Supported extensions: BMP GIF EXIF JPG PNG TIFF (Exclude output images)
				string[] supportedExtensions = new[] { "bmp", "gif", "exif", "jpg", "png", "tiff" };
				string[] containedFiles = Directory.EnumerateFiles(imageFolderLocation, "*.*", SearchOption.TopDirectoryOnly).Where(file => supportedExtensions.Any(x => file.ToLower().EndsWith(x)) && !file.EndsWith(outputFileName)).ToArray();

				if (containedFiles.Length == 0)
				{
					Console.WriteLine("No Files Found");
					Environment.Exit(0);
				}

				Bitmap[] imageList = new Bitmap[containedFiles.Length];

				int maxWidth = 0;
				int maxHeight = 0;

				// Reading files
				for (int i = 0; i < imageList.Length; i++)
				{
					Console.WriteLine(containedFiles[i]);
					Bitmap readBitmap = new(containedFiles[i]);
					Console.WriteLine(readBitmap.Width + " : " + readBitmap.Height);

					if (readBitmap.Width > maxWidth)
						maxWidth = readBitmap.Width;

					if (readBitmap.Height > maxHeight)
						maxHeight = readBitmap.Height;

					imageList[i] = readBitmap;
				}

				Console.WriteLine("--maxWidth: " + maxWidth);
				Console.WriteLine("--maxHeight: " + maxHeight);
				Console.WriteLine("--length: " + imageList.Length);

				// Drawing images
				Bitmap outputImage = new(maxWidth * imageList.Length, maxHeight);
				using (Graphics outputGraphics = Graphics.FromImage(outputImage))
				{
					Pen debugPen = new (Color.Red, 2);
					for (int i = 0; i < imageList.Length; i++)
					{
						#if DEBUG
							outputGraphics.DrawLine(debugPen, maxWidth * i, 0, maxWidth * i, maxHeight);
						#endif
						outputGraphics.DrawImage(imageList[i], (maxWidth * i) + (maxWidth - imageList[i].Width) / 2, (maxHeight - imageList[i].Height) / 2);
					}
				}

				// Output
				if (File.Exists(imageFolderLocation + outputFileName))
					File.Delete(imageFolderLocation + outputFileName);

				outputImage.Save(imageFolderLocation + outputFileName);
				Console.WriteLine(imageFolderLocation + outputFileName);
			}
            else
            {
				Console.WriteLine("Incorrect Path");
			}
		}
	}
}
