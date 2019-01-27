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
	int V;
	Pair[] es;
	void Solve()
	{
		var I = G;
		V = I[0]; var E = I[1] + V - 1;
		es = new Pair[E];
		for (var e = 0; e < E; e++) { I = G; es[e] = new Pair(I[0] - 1, I[1] - 1); }
		var topo = TopologicalSort(V, es);
		var rev = new Dictionary<int, int>();
		for (var i = 0; i < V; i++) rev[topo[i]] = i;
		var par = new int[V];
		for (var i = 0; i < V; i++) par[i] = -1;
		foreach (var e in es) par[e.Y] = Max(par[e.Y], rev[e.X]);
		for (var i = 0; i < V; i++) WriteLine(par[i] < 0 ? 0 : topo[par[i]] + 1);
	}
	static List<int> TopologicalSort(int V, Pair[] es)
	{
		var topo = new List<int>();
		var s = new Queue<int>();
		var g = new List<int>[V];
		var rg = new int[V];
		for (var i = 0; i < V; i++) g[i] = new List<int>();
		foreach (var e in es) { g[e.X].Add(e.Y); rg[e.Y]++; }
		for (var i = 0; i < V; i++) if (rg[i] == 0) s.Enqueue(i);
		while (s.Count > 0)
		{
			var n = s.Dequeue();
			topo.Add(n);
			foreach (var m in g[n]) if (--rg[m] == 0) s.Enqueue(m);
		}
		return topo.Count == V ? topo : null;
	}
}
struct Pair
{
	public readonly int X, Y;
	public Pair(int x, int y) { X = x; Y = y; }
}
