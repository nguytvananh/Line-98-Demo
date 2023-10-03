using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;

namespace Lines
{
	class Algorithms
	{
		
		//ma tran 9*9 o  
		public static int[,] generateNewMatrix()
		{
			Random random = new Random();
			int[,] a = new int[9, 9];

			int i, j, remain, count;

			for (i = 0; i < 9; i++)
				for (j = 0; j < 9; j++) a[i, j] = 0;

			count = 81;
			bool stop;
			do
			{
				remain = random.Next(count--) + 1;
				stop = false;
				for (i = 0; i < 9; i++)
				{
					if (stop) break;
					for (j = 0; j < 9; j++)
						if (a[i, j] == 0)
						{
							remain--;
							if (remain == 0)
							{
								a[i, j] = random.Next(7) + 1;
								stop = true;
								break;
							}
						}
				}
			} while (count > 76); //9*9 - 5

            addNextColor(a);

			return a;
		}

        //hàm tạo màu ngẫu nhiên cho viên bi sắp hiện lên
        public static void addNextColor(int[,] a)
		{
			Random random = new Random();
			int count = countEmpty(a);
			int tmp, i, j, remain;
			bool stop;

			for (tmp = 0; tmp < 3; tmp++)
			{
				remain = random.Next(count--) + 1;
				stop = false;
				for (i = 0; i < 9; i++)
				{
					if (stop) break;
					for (j = 0; j < 9; j++)
						if (a[i, j] == 0)
						{
							remain--;
							if (remain == 0)
							{
								a[i, j] = -(random.Next(7) + 1);
								stop = true;
								break;
							}
						}
				}
			}
		}

        //hàm đếm số ô trống chưa có bi
        public static int countEmpty(int[,] a)
		{
			int i, j, count = 0;
			for (i = 0; i < 9; i++)
				for (j = 0; j < 9; j++)
					if (a[i, j] <= 0) count++;
			return count;
		}

        //hàm tìm đường đi có thể có của bi
        public static ArrayList havePath(int[,] a, int i1, int j1, int i2, int j2)
		{
			int[,] previ = new int[9, 9]; //mảng các ô 'cha' i-1 của i ~ các ô đi trước của i
            int[,] prevj = new int[9, 9];
			
			int[] queuei = new int[81]; //2 hàng đợi để loang
			int[] queuej = new int[81];
			
			int fist = 0, last = 0, x, y;

			for (x = 0; x < 9; x++)
				for (y = 0; y < 9; y++) previ[x, y] = -1; //ô cha theo i của [x,y] = -1

            queuei[0] = i2;
			queuej[0] = j2;
			previ[i2, j2] = -2;

			while (fist <= last)
			{
				x = queuei[fist];
				y = queuej[fist];
				fist++;
				if (y > 0)
				{
					if (x == i1 & y - 1 == j1)
					{
						previ[i1, j1] = x;
						prevj[i1, j1] = y;
						return buildPath(previ, prevj, i1, j1);
					}
					if (previ[x, y - 1] == -1) //nếu ô cha của [x,y-1] = -1 ~ chưa có
                        if (a[x, y - 1] <= 0) //nếu phần tử của mảng a chưa/sắp hiện lên
                        {
							last++;
							queuei[last] = x; //cho ô [x,y-1] vào hàng đợi
                            queuej[last] = y - 1;
							previ[x, y - 1] = x; //ô cha của [x,y-1] là [x,y]
                            prevj[x, y - 1] = y;
						}
				}
				if (y < 8)
				{
					if (x == i1 & y + 1 == j1)
					{
						previ[i1, j1] = x;
						prevj[i1, j1] = y;
						return buildPath(previ, prevj, i1, j1);
					}
					if (previ[x, y + 1] == -1)
						if (a[x, y + 1] <= 0)
						{
							last++;
							queuei[last] = x;
							queuej[last] = y + 1;
							previ[x, y + 1] = x;
							prevj[x, y + 1] = y;
						}
				}
				if (x > 0)
				{
					if (x - 1 == i1 & y == j1)
					{
						previ[i1, j1] = x;
						prevj[i1, j1] = y;
						return buildPath(previ, prevj, i1, j1);
					}
					if (previ[x - 1, y] == -1)
						if (a[x - 1, y] <= 0)
						{
							last++;
							queuei[last] = x - 1;
							queuej[last] = y;
							previ[x - 1, y] = x; //ô cha của [x-1,y] là [x,y]
                            prevj[x - 1, y] = y;
						}
				}
				if (x < 8)
				{
					if (x + 1 == i1 & y == j1)
					{
						previ[i1, j1] = x;
						prevj[i1, j1] = y;
						return buildPath(previ, prevj, i1, j1);
					}
					if (previ[x + 1, y] == -1)
						if (a[x + 1, y] <= 0)
						{
							last++;
							queuei[last] = x + 1;
							queuej[last] = y;
							previ[x + 1, y] = x;
							prevj[x + 1, y] = y;
						}
				}

			}
			return null;
		}

        //hàm vẽ đường đi của bi 
        public static ArrayList buildPath(int[,] previ, int[,] prevj, int i1, int j1)
		{
			ArrayList arr = new ArrayList();
			int k;
			while (true)
			{
				arr.Add(new Point(i1, j1));
				k = i1;
				i1 = previ[i1, j1];
				if (i1 == -2) break; //nếu ô đích là [i1,j1] thì break
                j1 = prevj[k, j1];
			}
			return arr;
		}

        //hàm kiểm tra các viên bi ăn điểm
        public static ArrayList checkLines(int[,] a, int iCenter, int jCenter)
		{
			ArrayList list = new ArrayList();

			int[] u = { 0, 1, 1, 1 };
			int[] v = { 1, 0, -1, 1 };
			int i, j, k;
			for (int t = 0; t < 4; t++)
			{
				k = 0;
				i = iCenter;
				j = jCenter;
				while (true)
				{
					i += u[t];
					j += v[t];
					if (!isIncluded(i, j))
						break;
					if (a[i, j] != a[iCenter, jCenter])
						break;
					k++;
				}
				i = iCenter;
				j = jCenter;
				while (true)
				{
					i -= u[t];
					j -= v[t];
					if (!isIncluded(i, j))
						break;
					if (a[i, j] != a[iCenter, jCenter])
						break;
					k++;
				}
				k++;
				if (k >= 5)
					while (k-- > 0)
					{
						i += u[t];
						j += v[t];
						if (i != iCenter || j != jCenter)
							list.Add(new Point(i, j));
					}
			}
			if (list.Count > 0) list.Add(new Point(iCenter, jCenter));
			else list = null;
			return list;
		}

		public static ArrayList merge(ArrayList l1, ArrayList l2)
		{
			foreach (Point p in l2)
				if (!checkExist(l1, p))
					l1.Add(p);
			return l1;
		}

		public static bool checkExist(ArrayList l, Point p)
		{
			foreach (Point pp in l)
				if (pp.X == p.X && pp.Y == p.Y)
					return true;
			return false;
		}

        //hàm kiểm tra ô [i,j] nằm trong bảng
        public static bool isIncluded(int i, int j)
		{
			return (i >= 0 && i < 9 && j >= 0 && j < 9);
		}
		
		//hàm tách chữ số
		public static int[] splitDigit(int n, int length)
		{
			int[] digit = new int[length];
			while (n > 0)
			{
				if (length == 0) break;
				digit[--length] = n % 10;
				n = n / 10;
			}
			while (length > 0) digit[--length] = 0;
			return digit;
		}
	}
}
