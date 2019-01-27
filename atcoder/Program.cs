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
		var t = new Pair[N];
		for (var i = 0; i < N; i++) { var I = GL; t[i] = new Pair(I[0], I[1]); }
		Array.Sort(t, (x, y) => -(x.X + x.Y).CompareTo(y.X + y.Y));
		var ans = 0L;
		for (var i = 0; i < N; i++)
		{
			if (i % 2 == 0) ans += t[i].X;
			else ans -= t[i].Y;
		}
		WriteLine(ans);
	}
}
struct Pair
{
	public readonly long X, Y;
	public Pair(long x, long y) { X = x; Y = y; }
}
