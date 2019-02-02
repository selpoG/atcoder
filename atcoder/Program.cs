using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static System.Math;
class Z { static void Main() => new K(); }
class K
{
	int F => int.Parse(Str);
	long FL => long.Parse(Str);
	int[] G => Strs.Select(int.Parse).ToArray();
	long[] GL => Strs.Select(long.Parse).ToArray();
	string Str => ReadLine();
	string[] Strs => Str.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
	const int MOD = 1000000007;
	public K()
	{
		SetOut(new StreamWriter(OpenStandardOutput()) { AutoFlush = false });
		Solve();
		Out.Flush();
	}
	void Solve()
	{
		var N = F;
		var offers = new (int a, int b, int k)[N];
		for (var i = 0; i < N; i++) { var I = G; offers[i] = (I[0], I[1], I[2]); }
		var mat = new long[N, N];
		for (var i = 0; i < N; i++) for (var j = 0; j < N; j++) mat[i, j] = -Max(0, offers[i].a - (long)offers[i].b * Min(offers[i].k, j));
		var v = Hungarian(mat);
		var ans = 0L;
		for (var i = 0; i < N; i++) ans += mat[i, v[i]];
		WriteLine(-ans);
	}
	static T[] ConstantArray<T>(int n, T val) { var a = new T[n]; for (var i = 0; i < n; i++) a[i] = val; return a; }
	const long Inf = 4011686018427387913L;
	// n <= m, n 人 を m 個の仕事に重複なく割り当てる
	// 人 i を仕事 j に割り当てたときの損失が a[i][j]
	// 最小の損失を与える割当を返す
	int[] Hungarian(long[,] a)
	{
		int n = a.GetLength(0), m = a.GetLength(1);
		var toright = ConstantArray(n, -1);
		var toleft = ConstantArray(m, -1);
		var ofsleft = ConstantArray(n, 0L);
		var ofsright = ConstantArray(m, 0L);
		for (int r = 0; r < n; r++)
		{
			var left = ConstantArray(n, false);
			var right = ConstantArray(m, false);
			var trace = ConstantArray(m, -1);
			var ptr = ConstantArray(m, r);
			left[r] = true;
			while (true)
			{
				var d = Inf;
				for (int j = 0; j < m; j++) if (!right[j]) d = Min(d, a[ptr[j], j] + ofsleft[ptr[j]] + ofsright[j]);
				for (int i = 0; i < n; i++) if (left[i]) ofsleft[i] -= d;
				for (int j = 0; j < m; j++) if (right[j]) ofsright[j] += d;
				int b = -1;
				for (int j = 0; j < m; j++) if (!right[j] && a[ptr[j], j] + ofsleft[ptr[j]] + ofsright[j] == 0) b = j;
				trace[b] = ptr[b];
				int c = toleft[b];
				if (c < 0)
				{
					while (b >= 0)
					{
						int q = trace[b];
						int z = toright[q];
						toleft[b] = q;
						toright[q] = b;
						b = z;
					}
					break;
				}
				right[b] = left[c] = true;
				for (int j = 0; j < m; j++) if (a[c, j] + ofsleft[c] < a[ptr[j], j] + ofsleft[ptr[j]]) ptr[j] = c;
			}
		}
		return toright;
	}
}
