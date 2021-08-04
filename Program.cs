using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace SpriteSheetGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Please insert the folder location contaning the images:");

			// C:\Users\Balázs\Documents\C#\ImageSequence-To-SpriteSheet\imageFolder
			string imageFolderLocation;
			imageFolderLocation = Console.ReadLine();


			if (Directory.Exists(imageFolderLocation))
			{
				// TODO: Arrays be faster?
				List<Bitmap> imageList = new();

				int maxWidth = 0;
				int maxHeight = 0;

				// TODO: Supported extensions: BMP GIF JPEG PNG TIFF
				string[] containedFiles = Directory.GetFiles(imageFolderLocation, "*.png");
				if (containedFiles.Length == 0)
                {
					Console.WriteLine("No Files Found");
					Environment.Exit(0);
				}

				foreach (string fileLocation in containedFiles)
				{
					Console.WriteLine(fileLocation);
					Bitmap readBitmap = new(fileLocation);
					Console.WriteLine(readBitmap.Width + " : " + readBitmap.Height);

					if (readBitmap.Width > maxWidth)
						maxWidth = readBitmap.Width;

					if (readBitmap.Height > maxHeight)
						maxHeight = readBitmap.Height;

					imageList.Add(readBitmap);
				}

				Console.WriteLine("--maxWidth: " + maxWidth);
				Console.WriteLine("--maxHeight: " + maxHeight);
				Console.WriteLine("--length: " + imageList.Count);

				// TODO: Center images
				Bitmap outputImage = new(maxWidth * containedFiles.Length, maxHeight);
				using (Graphics outputGraphics = Graphics.FromImage(outputImage))
                {
					Pen debugPen = new (Color.Red, 2);
					for (int i = 0; i < imageList.Count; i++)
					{
						#if DEBUG
							outputGraphics.DrawLine(debugPen, maxWidth * i, 0, maxWidth * i, maxHeight);
						#endif
						outputGraphics.DrawImage(imageList[i], maxWidth * i, 0);
					}
				}

				// Output
				string outputFileName = @"\output.png";
				if (File.Exists(imageFolderLocation + outputFileName))
				{
					Random rand = new();
					outputFileName = @"\output" + rand.Next() + ".png";
				}

				outputImage.Save(imageFolderLocation + outputFileName);
				Console.WriteLine(imageFolderLocation + outputFileName);
			} else
			{
				Console.WriteLine("Incorrect Path");
			}
		}
	}
}
