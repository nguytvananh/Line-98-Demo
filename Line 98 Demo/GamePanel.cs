
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace Lines
{
	//lop cac hanh dong xay ra cua 1 panel game
	class GamePanel : Panel
	{

		public Game game;
		bool isSelected;
		public bool isLocking;
		int colSelected, rowSelected;

		public MainForm mainForm;
		public GamePanel(MainForm mainForm)
		{
			this.mainForm = mainForm;

			this.Location = new Point(0, 24);
			this.Size = new Size(Cons.BKGSize.Width, Cons.BKGSize.Height);

			buildPieces();
			buildYScore();
			buildHScore();
			buildNextColor();
			
			buildHour();
			buildMinutes();
			buildSeconds();
			
			buildBackground();

			newGame();
		}

		public void newGame()
		{
			applyGameShape(Game.getDefault());
		}

		public void applyGameShape(Game gameShape)
		{
			this.game = gameShape;
			isSelected = false;
			isLocking = false;
			stopAllThread();
			timeThread = new TimeThread(this);

			int[,] matrix = gameShape.matrix;

			//cac ham cap nhat lai hinh anh
			repaintPieces(matrix);
			repaintNextColor(matrix);
			repaintYScore(gameShape.scores);
			repaintHScore(Score.getMaxScores());
			repaintHour(gameShape.hours); 
			repaintMinutes(gameShape.minutes);
			repaintSeconds(gameShape.seconds);
		}

		private void stopAllThread()
		{
			if (jumpThread != null) jumpThread.Stop();
			if (moveThread != null) moveThread.Stop();
			if (appearanceThread != null) appearanceThread.Stop();
			if (destructionThread != null) destructionThread.Stop();
			if (timeThread != null) timeThread.Stop();
		}

		private void buildBackground()
		{
			background.Location = new Point(0, 0);
			background.Size = new Size(Cons.BKGSize.Width, Cons.BKGSize.Height);
			background.Image = new Bitmap(Cons.BKG);
			background.MouseClick += new MouseEventHandler(background_MouseClick);

			this.Controls.Add(background);
		}

		private void buildPieces()
		{
			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
				{
					pieces[i, j] = new Piece(i, j);
					pieces[i, j].Click += new System.EventHandler(this.piece_Click);
					this.Controls.Add(pieces[i, j]);
				}
		}

		public void repaintPieces(int[,] matrix) //matrix la mau bi
		{
			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					if (matrix[i, j] == 0)
					{
						pieces[i, j].Visible = false;
						pieces[i, j].color = 0;
					}
					else if (matrix[i, j] > 0)
					{
						pieces[i, j].Visible = true;
						pieces[i, j].color = matrix[i, j];
						pieces[i, j].setNormal();
					}
					else
					{
						pieces[i, j].Visible = true;
						pieces[i, j].color = matrix[i, j];
						
					}
		}

		private void buildNextColor()
		{
			for (int i = 0; i < 3; i++)
			{
				nextColor[i] = new PictureBox();
				nextColor[i].Location = Cons.NextColor[i];
				nextColor[i].Size = Cons.NextColorSize;
				this.Controls.Add(nextColor[i]);
			}
		}

		public void repaintNextColor(int[,] matrix)
		{
			int count = 0;
			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					if (matrix[i, j] < 0)
					{
						nextColor[count++].Image = Pixel.bitmapNextColor[-matrix[i, j] - 1];
					}
		}

		private void buildYScore()
		{
			for (int i = 0; i < 5; i++)
			{
				YScore[i] = new PictureBox();
				YScore[i].Location = Cons.getLocationOfYScore(i);
				YScore[i].Size = Cons.YScoreDigitSize;
				this.Controls.Add(YScore[i]);
			}
		}

		public void repaintYScore(int score)
		{
			int[] digit = Algorithms.splitDigit(score, 5);
			for (int i = 0; i < 5; i++)
				YScore[i].Image = Pixel.bitmapYScoreDigit[digit[i]];
		}

		private void buildHScore()
		{
			for (int i = 0; i < 5; i++)
			{
				HScore[i] = new PictureBox();
				HScore[i].Location = Cons.getLocationOfHScore(i);
				HScore[i].Size = Cons.HScoreDigitSize;
				this.Controls.Add(HScore[i]);
			}
		}

		public void repaintHScore(int score)
		{
			int[] digit = Algorithms.splitDigit(score, 5);
			for (int i = 0; i < 5; i++)
				HScore[i].Image = Pixel.bitmapHScoreDigit[digit[i]];
		}

		private void buildHour()
		{
			hours.Location = Cons.TimePos;
			hours.Size = Cons.TimeDigitSize;
			this.Controls.Add(hours);
		}

		public void repaintHour(int v)
		{
			hours.Image = Pixel.bitmapTimeDigit[v];
		}

		private void buildMinutes()
		{
			minutes1.Location = new Point(
				hours.Location.X + hours.Size.Width + 1 + 2 * Cons.TimeAndCommaBetween,
				Cons.TimePos.Y);
			minutes1.Size = Cons.TimeDigitSize;

			minutes2.Location = new Point(
				minutes1.Location.X + minutes1.Size.Width + Cons.TimeBetween,
				Cons.TimePos.Y);
			minutes2.Size = Cons.TimeDigitSize;

			this.Controls.Add(minutes1);
			this.Controls.Add(minutes2);
		}

		public void repaintMinutes(int v)
		{
			int[] digit = Algorithms.splitDigit(v, 2);
			minutes1.Image = Pixel.bitmapTimeDigit[digit[0]];
			minutes2.Image = Pixel.bitmapTimeDigit[digit[1]];
		}

		private void buildSeconds()
		{
			seconds1.Location = new Point(
				minutes2.Location.X + minutes2.Size.Width + 1 + 2 * Cons.TimeAndCommaBetween,
				Cons.TimePos.Y);
			seconds1.Size = Cons.TimeDigitSize;

			seconds2.Location = new Point(
				seconds1.Location.X + seconds1.Size.Width + Cons.TimeBetween,
				Cons.TimePos.Y);
			seconds2.Size = Cons.TimeDigitSize;

			this.Controls.Add(seconds1);
			this.Controls.Add(seconds2);
		}

		public void repaintSeconds(int v)
		{
			int[] digit = Algorithms.splitDigit(v, 2);
			seconds1.Image = Pixel.bitmapTimeDigit[digit[0]];
			seconds2.Image = Pixel.bitmapTimeDigit[digit[1]];
		}

		private void piece_Click(object sender, EventArgs e)
		{
			if (isLocking) return;
			Piece piece = sender as Piece;
			if (!isSelected && piece.color < 0) return;

			if (!isSelected)
			{
				newJump(piece);
				isSelected = true;
			}
			else if (piece.color < 0)
				moveTo(piece.col, piece.row);
			else
				newJump(piece);
		}

		private void background_MouseClick(object sender, MouseEventArgs e)
		{
			if (isLocking) return;
			Point p = Cons.getColRowIndex(e.X, e.Y);
			if (p.X == -1) return;

			if (game.matrix[p.X, p.Y] != 0)
			{
				piece_Click(pieces[p.X, p.Y], null);
			}
			else
			{
				if (!isSelected) return;
				moveTo(p.X, p.Y);
			}
		}

		private void newJump(Piece piece)
		{
			colSelected = piece.col;
			rowSelected = piece.row;
			if (jumpThread != null) jumpThread.Stop();
			jumpThread = new JumpThread(piece);
		}

		public void moveTo(int newCol, int newRow)
		{
			ArrayList list = Algorithms.havePath(game.matrix, colSelected, rowSelected, newCol, newRow);
			if (list != null)
			{
				jumpThread.Stop();
				moveThread = new MoveThread(this, list);
				isSelected = false;
			}
		}

		public void setGameOver()
		{
			if (jumpThread != null) jumpThread.Stop();
			if (timeThread != null) timeThread.Stop();
			Player newPlayer = new Player("", game.scores, game.hours, game.minutes, game.seconds);
			Player worstPlayer = Score.getWorstPlayer();
			if (Player.less(newPlayer, worstPlayer))
			{
				new HighScoresForm().ShowDialog(mainForm);
			}
			else
			{
				new FrmInputName(game).ShowDialog(mainForm);
			}
			newGame();
		}

        public JumpThread jumpThread;
        public MoveThread moveThread;
        public AppearanceThread appearanceThread;
        public DestructionThread destructionThread;
        public TimeThread timeThread;

        public Piece[,] pieces = new Piece[9, 9];
        public PictureBox background = new PictureBox();
        public PictureBox[] nextColor = new PictureBox[3];
        public PictureBox[] YScore = new PictureBox[5];
        public PictureBox[] HScore = new PictureBox[5];
        public PictureBox hours = new PictureBox();
        public PictureBox minutes1 = new PictureBox();
        public PictureBox minutes2 = new PictureBox();
        public PictureBox seconds1 = new PictureBox();
        public PictureBox seconds2 = new PictureBox();
    }
}
