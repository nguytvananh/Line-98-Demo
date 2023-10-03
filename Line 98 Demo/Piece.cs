
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Lines
{
	class Piece : PictureBox
	{
		
		public int col, row;
		public int color;

		public Piece(int i, int j) //1 piece = 1 manh file anh ~ 1 vien bi
		{
			this.col = i;
			this.row = j;
			this.Location = Cons.getLocationOfPiece(i, j);
			this.Size = new Size(
				Cons.BallSize.Width, Cons.BallSize.Height);
		}

		public void setNormal()
		{
			this.Image = Pixel.bitmapPiece[0, color - 1];
		}



		public void setJumpAt(int frame)
		{
			this.Image = Pixel.bitmapPiece[Cons.BallSelected[frame] - 1, color - 1];
		}

		public void setAppearanceAt(int frame)
		{
			this.Image = Pixel.bitmapPiece[Cons.BallAppearance[frame] - 1, -color - 1];
		}

		public void setDestructionAt(int frame)
		{
			this.Image = Pixel.bitmapPiece[Cons.BallDestruction[frame] - 1, color - 1];
		}
	}
}
