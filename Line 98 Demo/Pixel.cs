using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Lines
{
	class Pixel
	{

		//tach pixel cua bi tu file anh
		public static Bitmap[,] bitmapPiece = clip(Cons.BALLS, 22, 7); //22 status 
		public static Bitmap[,] clip(Bitmap bigImage, int cols, int rows) //chia file anh theo so cot, so hang
		{
			Bitmap[,] images = new Bitmap[cols, rows];

			int pieceWidth = bigImage.Width / cols; //do rong manh = do rong file anh/ so cot
			int pieceHeight = bigImage.Height / rows;

			for (int i = 0; i < cols; i++)
				for (int j = 0; j < rows; j++)
				{
					images[i, j] = bigImage.Clone(
						new Rectangle(i * pieceWidth, j * pieceHeight, pieceWidth, pieceHeight),
						System.Drawing.Imaging.PixelFormat.DontCare);
				}
			return images;
		}


		public static Bitmap[,] clip(string fileName, int cols, int rows)
		{
			return Pixel.clip(new Bitmap(fileName), cols, rows);
		}


		//tach pixel chu so tu file anh
		public static Bitmap[] bitmapYScoreDigit = clip(Cons.YSCORE, 10); //10 chu so tu 0-9
		public static Bitmap[] bitmapHScoreDigit = clip(Cons.HSCORE, 10);
		public static Bitmap[] bitmapNextColor = clip(Cons.NEXT, 7); // 7 mau bi
		public static Bitmap[] bitmapTimeDigit = clip(Cons.TIME, 10);

		public static Bitmap[] clip(Bitmap bigImage, int cols)
		{
			Bitmap[] images = new Bitmap[cols];

			int pieceWidth = bigImage.Width / cols;
			int pieceHeight = bigImage.Height;

			for (int i = 0; i < cols; i++)
			{
				images[i] = bigImage.Clone(
					new Rectangle(i * pieceWidth, 0, pieceWidth, pieceHeight),
					System.Drawing.Imaging.PixelFormat.DontCare);
			}
			return images;
		}

		public static Bitmap[] clip(string fileName, int cols)
		{
			return Pixel.clip(new Bitmap(fileName), cols);
		}		
		

	}
}
