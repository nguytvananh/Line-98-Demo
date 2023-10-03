
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;
using System.Security.Cryptography.X509Certificates;

namespace Lines
{
	//lop tro choi
    public class Game
    {
        public int[,] matrix;
        public int scores;
        public int hours, minutes, seconds;

        public Game(int[,] matrix, int scores, int hours, int minutes, int seconds)
        {
            this.matrix = matrix;
            this.scores = scores;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
        }

        public static Game getDefault()
        {
            return new Game(Algorithms.generateNewMatrix(), 0, 0, 0, 0);
        }
    }


		[Serializable()]
		//lop danh sach nguoi choi
	class PlayerList
	{
		public Player[] list;

		public PlayerList(Player[] list)
		{
			this.list = list;
		}
	}
	//lop dinh nghia hang so
    class Cons
		
	{
		//doi tuong vien bi
		public static string BALLS = "balls.bmp";//file anh
		public static Size BallSize = new Size(40, 40);
		public static Point UpperLeftBall = new Point(5, 56);
		public static Size BetweenBalls = new Size(5, 5);

		//mau 3 bi sap hien len 
		public static string NEXT = "next.bmp";
		public static Size NextColorSize = new Size(25, 25);
		public static Point[] NextColor = { new Point(164, 9), new Point(193, 9), new Point(222, 9) }; //vi tri hien mau cua 3 bi sap hien len 

		//background
		public static string BKG = "bkg.bmp"; 
		public static Size BKGSize = new Size(410, 461); //kich thuoc nen

		//thoi gian
		public static string TIME = "time.bmp";
		public static Size TimeDigitSize = new Size(7, 13);
        public static Point TimePos = new Point(183, 34);//vi tri thoi gian
		public static int TimeBetween = 2; //khoang cach giua 2 cso tgian
		public static int TimeAndCommaBetween = 1; //khoang cach giua cso tgian & dau :

		//diem cua ban
		public static string YSCORE = "digits.bmp";
		public static Size YScoreDigitSize = new Size(18, 35);
		public static Point YourScorePos = new Point(290, 7);//vi tri ghi diem cua ban o bkg
		public static int YourScoreBetweenX = 3; //khoang cach giua 2 cso diem

		//high score
		public static string HSCORE = "digits.bmp";
		public static Size HScoreDigitSize = new Size(18, 35);
        public static Point HighScorePos = new Point(19, 7);//vi tri ghi high score trong bkg
		public static int HighScoreBetweenX = 3; //khoang cach giua 2 cso diem



		//stt piece cua bi trong file anh (tach o pixel.cs)
		public static int[] BallSelected = { 1, 4, 3, 2, 3, 4, 1, 5, 6, 7, 6, 5 };
		public static int[] BallAppearance = { 22, 21, 20, 19, 18 };
		public static int[] BallDestruction = { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
		public static int BallHint = 21;

		//thoi gian tre cua tung trang thai
		public static int DelayJump = 60;
		public static int DelayAppearance = 60;
		public static int DelayDestruction = 30;
		public static int DelayMove = 20;

		//ham xac dinh vi tri cua 1 manh hinh anh
		public static Point getLocationOfPiece(int col, int row)
		{
			return new Point(
				UpperLeftBall.X + (BallSize.Width + BetweenBalls.Width) * col, // toa do X = bi goc + (bi.rong + k/c ngang giua 2 bi) * so cot
				UpperLeftBall.Y + (BallSize.Height + BetweenBalls.Height) * row); // toa do Y = bi goc + (bi.dai + k/c doc giua 2 bi) * so dong
        }

		//xac dinh toa do cot,hang cua 1 diem p 
		public static Point getColRowIndex(int X, int Y) //nguoc lai cua getLocationOfPiece
		{
			if (X < UpperLeftBall.X) return new Point(-1, -1); 
			if (Y < UpperLeftBall.Y) return new Point(-1, -1);
			return new Point(
				(X - UpperLeftBall.X) / (BallSize.Width + BetweenBalls.Width),
				(Y - UpperLeftBall.Y) / (BallSize.Height + BetweenBalls.Height));
		}
		
		//xac dinh vi tri diem cua ban
		public static Point getLocationOfYScore(int index)
		{
			return new Point(
				YourScorePos.X + (YScoreDigitSize.Width + YourScoreBetweenX) * index,
				YourScorePos.Y);
		}

        //xac dinh vi tri high score
        public static Point getLocationOfHScore(int index)
		{
			return new Point(
				HighScorePos.X + (HScoreDigitSize.Width + HighScoreBetweenX) * index,
				HighScorePos.Y);
		}
	}



}
