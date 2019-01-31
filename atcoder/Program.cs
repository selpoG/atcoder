using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
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
		var a = new ABC();
		var hoge = 0;
		//for (var k = 0; k <= N * (N - 1) / 2; k++)
		for (var k = (N - 5) * (N - 5) / 3; k <= N * N / 3; k++)
		{
			var s = a.createString(N, k);
			if (s != "")
			{
				//var c = CountPair(s);
				//WriteLine($"f({N}, {k}) = {s} => {c}" + (c == k ? "" : " ERROR"));
				hoge++;
			}
		}
		WriteLine(hoge);
	}
	int CountPair(string s)
	{
		var cnt = 0;
		for (var i = 0; i < s.Length; i++) for (var j = i + 1; j < s.Length; j++) if (s[i] < s[j]) cnt++;
		return cnt;
	}
}
class ABC
{
	void createString2(int N, int K, StringBuilder sb)
	{
		var k = N * N / 4;
		if (K == k) { sb.Append('B', N / 2); sb.Append('C', (N + 1) / 2); return; }
		if (K == 0) { sb.Append('C', N); return; }
		var h = N / 2;
		var l = 0;
		for (var i = 0; i < h; i++)
		{
			var k2 = k - (N - h);
			if (k2 < K) { var x = k - K; sb.Append('C', l); sb.Append('B', N - h - x); sb.Append("C"); sb.Append('B', x); sb.Append('C', h - l - 1); return; }
			k = k2; l++;
		}
		throw new Exception();
	}
	public string createString(int N, int K)
	{
		var sb = new StringBuilder();
		createString(N, K, sb);
		return sb.ToString();
	}
	void createString(int N, int K, StringBuilder sb)
	{
		var lim = N * N / 3;
		if (K > lim) return;
		var lim2 = (N - 1) * (N - 1) / 3;
		if (N >= 1 && K <= lim2) { sb.Append("C"); createString(N - 1, K, sb); return; }
		var x = 2 * N / 3;
		sb.Append('A', N - x);
		createString2(x, K - x * (N - x), sb);
	}
}
