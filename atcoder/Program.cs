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
		var s = new string[3];
		for (var i = 0; i < 3; i++) s[i] = Str;
		var ans = 0;
		for (var i = 0; i < N; i++)
		{
			var c = new HashSet<char>();
			for (var j = 0; j < 3; j++) c.Add(s[j][i]);
			ans += c.Count - 1;
		}
		WriteLine(ans);
	}
}
