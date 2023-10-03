
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Lines
{
	//luong thoi gian
	class TimeThread
	{
		GamePanel panel;
		Timer timer;

		public TimeThread(GamePanel panel)
		{
			this.panel = panel;
			timer = new Timer();
			timer.Interval = 1000;
			timer.Tick += new EventHandler(timer_Tick);
			timer.Start();
		}

		void timer_Tick(object sender, EventArgs e)
		{
			panel.game.seconds++;
			if (panel.game.seconds == 60)
			{
				panel.game.seconds = 0;
				panel.game.minutes++;
				if (panel.game.minutes == 60)
				{
					panel.game.minutes = 0;
					panel.game.hours++;
				}
			}
			panel.repaintHour(panel.game.hours);
			panel.repaintMinutes(panel.game.minutes);
			panel.repaintSeconds(panel.game.seconds);
		}

		public void Stop()
		{
			timer.Stop();
		}
	}

	//luong di chuyen bi
	class MoveThread
	{
		GamePanel panel;
		ArrayList list;
		int count;
		Timer timer;
		Point point;
		int color;

		public MoveThread(GamePanel panel, ArrayList list)
		{
			

			this.panel = panel;
			this.list = list;

			timer = new Timer();
			timer.Interval = Cons.DelayMove;
			timer.Tick += new EventHandler(timer_Tick);

			point = (Point)list[0];
			color = panel.game.matrix[point.X, point.Y];

			panel.game.matrix[point.X, point.Y] = 0;
			panel.pieces[point.X, point.Y].color = 0;

			count = 0;
			panel.isLocking = true;

			timer.Start();
		}

		public void timer_Tick(object Sender, EventArgs e)
		{
			count++;

			if (panel.pieces[point.X, point.Y].color >= 0)
				panel.pieces[point.X, point.Y].Visible = false;

			point = (Point)list[count];
			panel.pieces[point.X, point.Y].Image = Pixel.bitmapPiece[0, color - 1];
			panel.pieces[point.X, point.Y].Visible = true;

			if (count == list.Count - 1)
			{
				timer.Stop();
				panel.game.matrix[point.X, point.Y] = color;
				panel.pieces[point.X, point.Y].color = color;
				ArrayList arr = Algorithms.checkLines(panel.game.matrix, point.X, point.Y);
				if (arr == null)
					panel.appearanceThread = new AppearanceThread(panel, panel.game.matrix);
				else
					panel.destructionThread = new DestructionThread(panel, arr);
			}
		}

		public void Stop()
		{
			timer.Stop();
		}
	}

	//luong bong nhay
	class JumpThread
	{
		Piece piece;

		Timer timer;
		int frame, len;

		public JumpThread(Piece piece)
		{
			this.piece = piece;

			frame = 0;
			len = Cons.BallSelected.Length;

			timer = new Timer();
			timer.Interval = Cons.DelayJump;
			timer.Tick += new EventHandler(timer_Tick);
			timer.Start();
		}

		public void timer_Tick(object Sender, EventArgs e)
		{
			piece.setJumpAt(frame++);
			if (frame == len) frame = 0;
		}

		public void Stop()
		{
			timer.Stop();
			if (piece.color > 0) piece.setNormal();
		}
	}

	//luong bi bien mat
	class DestructionThread
	{
		GamePanel panel;
		Timer timer;
		Point[] p;
		int len, count;

		public DestructionThread(GamePanel panel, ArrayList list)
		{
			//if (panel.mainForm.isSound) LinesMediaPlayer.destroySound.Play();

			this.panel = panel;

			p = new Point[list.Count];
			len = list.Count;
			for (int i = 0; i < list.Count; i++)
				p[i] = (Point)list[i];

			timer = new Timer();
			timer.Interval = Cons.DelayDestruction;
			timer.Tick += new EventHandler(timer_Tick);
			count = 0;
			timer.Start();
		}

		void timer_Tick(object sender, EventArgs e)
		{
			for (int i = 0; i < len; i++)
			{
				panel.pieces[p[i].X, p[i].Y].setDestructionAt(count);
			}
			count++;

			if (count == Cons.BallDestruction.Length)
			{
				timer.Stop();
				for (int i = 0; i < len; i++)
				{
					panel.pieces[p[i].X, p[i].Y].color = 0;
					panel.pieces[p[i].X, p[i].Y].Visible = false;
					panel.game.matrix[p[i].X, p[i].Y] = 0;
				}

				panel.game.scores += p.Length + p.Length - 5;
				panel.repaintYScore(panel.game.scores);
				panel.isLocking = false;
			}
		}

		public void Stop()
		{
			timer.Stop();
		}

	}

	//luong bi xuat hien
	class AppearanceThread
	{
		GamePanel panel;
		Point[] p;
		int len;
		Timer timer;
		int count;

		public AppearanceThread(GamePanel panel, int[,] a)
		{
			this.panel = panel;
			ArrayList list = new ArrayList();
			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					if (a[i, j] < 0)
					{
						list.Add(new Point(i, j));
					}

			p = new Point[list.Count];
			for (int i = 0; i < list.Count; i++)
				p[i] = (Point)list[i];
			len = p.Length;
			timer = new Timer();
			timer.Interval = Cons.DelayAppearence;
			timer.Tick += new EventHandler(timer_Tick);
			count = 0;
			timer.Start();
		}

		void timer_Tick(object sender, EventArgs e)
		{
			for (int i = 0; i < len; i++)
			{
				panel.pieces[p[i].X, p[i].Y].setAppearanceAt(count);
			}
			count++;

			if (count == Cons.BallAppearence.Length)
			{
				this.Stop();
				for (int i = 0; i < len; i++)
				{
					panel.game.matrix[p[i].X, p[i].Y] = -panel.game.matrix[p[i].X, p[i].Y];
					panel.pieces[p[i].X, p[i].Y].color = -panel.pieces[p[i].X, p[i].Y].color;
					panel.pieces[p[i].X, p[i].Y].setNormal();
				}

				ArrayList arr = new ArrayList();
				for (int i = 0; i < len; i++)
				{
					ArrayList a = Algorithms.checkLines(panel.game.matrix, p[i].X, p[i].Y);
					if (a != null)
						arr = Algorithms.merge(arr, a);
				}
				if (arr.Count > 0)
				{
					panel.destructionThread = new DestructionThread(panel, arr);
					Algorithms.addNextColor(panel.game.matrix);
					panel.repaintNextColor(panel.game.matrix);
					panel.repaintPieces(panel.game.matrix);
				}
				else if (Algorithms.countEmpty(panel.game.matrix) < 3)
				{
					panel.setGameOver();
					panel.isLocking = false;
				}
				else
				{
					Algorithms.addNextColor(panel.game.matrix);
					panel.repaintNextColor(panel.game.matrix);
					panel.repaintPieces(panel.game.matrix);
					panel.isLocking = false;
				}
			}
		}

		public void Stop()
		{
			timer.Stop();
		}
	}
}
