using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lines
{
	class Score
	{
		
		public static PlayerList readHighScores()
		{
			try
			{
				
				if (!File.Exists(filename)) writeDefaultHighScores();
				Stream s = File.Open(filename, FileMode.Open);
				BinaryFormatter b = new BinaryFormatter();
				PlayerList pl = (PlayerList)b.Deserialize(s);
				s.Close();
				return pl;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public static void writeHighScores(PlayerList pl)
		{
			Stream s = File.Open(filename, FileMode.Create);
			BinaryFormatter b = new BinaryFormatter();
			b.Serialize(s, pl);
			s.Close();
		}
		public static void writeDefaultHighScores()
		{
			Player[] p = new Player[10];
			for (int i = 0; i < 10; i++)
			{
				p[i] = new Player("Player", 0, 0, 1, 0);
			}

			writeHighScores(new PlayerList(p));
		}

		//người chơi tốt nhất
		public static int getMaxScores()
		{
			PlayerList pl = readHighScores();
			return pl.list[0].scores;
		}

		// người chơi tệ nhất
		public static Player getWorstPlayer()
		{
			PlayerList pl = readHighScores();
			return pl.list[9];
		}

		//hàm thêm người chơi vào bảng Best Players
		public static void insert(Player p)
		{
			PlayerList pl = readHighScores();
			int i, j;
			for (i = 0; i < 10; i++)
				if (Player.less(pl.list[i], p)) break;
			for (j = 9; j >= i + 1; j--)
				pl.list[j] = pl.list[j - 1];
			pl.list[i] = p;
			writeHighScores(pl);
		}
		public static string filename = "scores.dat";
	}		

}
