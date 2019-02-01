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
	int[,] cnt;
	int Count(int b, int x0, int x1, int y0, int y1)
	{
		if (b == 0) return (x1 - x0) * (y1 - y0) - Count(1, x0, x1, y0, y1);
		return cnt[x1, y1] - cnt[x0, y1] - cnt[x1, y0] + cnt[x0, y0];
	}
	void Solve()
	{
		var N = F;
		var map = new BitArray[N];
		for (var y = 0; y < N; y++)
		{
			map[y] = new BitArray(N);
			var S = Str;
			for (var x = 0; x < N; x += 4)
			{
				var c = S[x / 4];
				var n = char.IsDigit(c) ? c - '0' : c - 'A' + 10;
				for (var i = 0; i < 4; i++) map[y][x + 3 - i] = ((n >> i) & 1) != 0;
			}
		}
		cnt = new int[N + 1, N + 1];
		for (var x = 0; x < N; x++) for (var y = 0; y < N; y++) cnt[x + 1, y + 1] = cnt[x + 1, y] + cnt[x, y + 1] - cnt[x, y] + (map[y][x] ? 1 : 0);
		var used = new BitArray[N];
		for (var y = 0; y < N; y++) used[y] = new BitArray(N);
		var sz = new HashSet<int>();
		for (var x = 0; x < N; x++)
			for (var y = 0; y < N; y++)
				if (!used[y][x])
				{
					var s = 1;
					var b = map[y][x] ? 0 : 1;
					while (x + s < N && y + s < N)
					{
						if (Count(b, x, x + s + 1, y, y + s + 1) == 0) s++;
						else break;
					}
					for (var p = x; p < x + s; p++) for (var q = y; q < y + s; q++) used[q][p] = true;
					sz.Add(s);
				}
		WriteLine(GCD(sz.ToArray()));
	}
	public static int GCD(params int[] ns)
	{
		var ans = ns[0];
		foreach (var x in ns.Skip(1)) ans = GCD(ans, x);
		return ans;
	}
	public static int GCD(int n, int m)
	{
		while (m > 0) { var c = n % m; n = m; m = c; }
		return n;
	}
}
class BitArray
{
	readonly int N;
	readonly sbyte[] bit;
	public BitArray(int n) { N = n; var l = (N + 7) / 8; bit = new sbyte[l]; }
	const sbyte One = 1;
	public bool this[int n] { get { return (bit[n / 8] & (One << (7 - n % 8))) != 0; } set { if (value) bit[n / 8] |= (sbyte)(One << (7 - n % 8)); else bit[n / 8] &= (sbyte)~(One << (7 - n % 8)); } }
}