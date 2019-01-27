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
	void Solve()
	{
		var I = G;
		V = I[0]; var E = I[1];
		var X = G;
		var es = new Edge[E];
		for (var e = 0; e < E; e++) { I = G; es[e] = new Edge(I[0] - 1, I[1] - 1, I[2], e); }
		Array.Sort(es);
		var uf = new UnionFindTree(V, X);
		foreach (var e in es) uf.Unite(e.From, e.To);
		var ok = new List<Edge>();
		var mark = new bool[E];
		for (var j = 0; j < E; j++)
		{
			var e = es[j];
			var t = j + 1;
			if (uf.GetSize(t, e.From) >= e.Cost) { ok.Add(e); mark[e.Id] = true; }
		}
		ok.Reverse();
		var g = new List<Edge>[V];
		for (var i = 0; i < V; i++) g[i] = new List<Edge>();
		foreach (var e in es) { g[e.From].Add(e); g[e.To].Add(e.Reversed); }
		for (var i = 0; i < V; i++) g[i].Sort((x, y) => y.CompareTo(x));
		var used = new bool[V];
		foreach (var e in ok)
		{
			var st = new Stack<int>();
			st.Push(e.From);
			st.Push(e.To);
			while (st.Count > 0)
			{
				var u = st.Pop();
				used[u] = true;
				var j = g[u].Count - 1;
				while (j >= 0 && g[u][j].Cost <= e.Cost)
				{
					if (!used[g[u][j].To]) st.Push(g[u][j].To);
					mark[g[u][j].Id] = true;
					j--;
				}
				g[u].RemoveRange(j + 1, g[u].Count - j - 1);
			}
		}
		WriteLine(mark.Count(c => !c));
	}
}
struct Edge : IComparable<Edge>
{
	public readonly int From, To, Cost, Id;
	public Edge(int f, int t, int c, int i) { From = f; To = t; Cost = c; Id = i; }
	public int CompareTo(Edge other) => Cost.CompareTo(other.Cost);
	public override string ToString() => $"{From} -> {To}: {Cost} (id: {Id})";
	public Edge Reversed => new Edge(To, From, Cost, Id);
}
struct Pair
{
	public readonly int X;
	public readonly long Y;
	public Pair(int x, long y) { X = x; Y = y; }
}
class UnionFindTree
{
	int now;
	readonly int[] par, rank, time;
	readonly long[] size;
	readonly List<Pair>[] sizeHist;
	public UnionFindTree(int N, int[] X)
	{
		par = new int[N];
		rank = new int[N];
		time = new int[N];
		size = new long[N];
		sizeHist = new List<Pair>[N];
		for (var i = 0; i < N; i++) { par[i] = i; time[i] = int.MaxValue; sizeHist[i] = new List<Pair> { new Pair(0, X[i]) }; size[i] = X[i]; }
	}
	public int Find(int x) => x == par[x] ? x : Find(par[x]);
	public int Find(int t, int x) => time[x] > t ? x : Find(t, par[x]);
	public void Unite(int x, int y)
	{
		now++; x = Find(now, x); y = Find(now, y);
		if (x == y) return;
		if (rank[x] < rank[y]) { var z = x; x = y; y = z; }
		par[y] = x; time[y] = now; size[x] += size[y]; sizeHist[x].Add(new Pair(now, size[x]));
		if (rank[x] == rank[y]) rank[x]++;
	}
	public long GetSize(int t, int x)
	{
		x = Find(t, x);
		var i = FirstBinary(0, sizeHist[x].Count, j => sizeHist[x][j].X > t) - 1;
		return sizeHist[x][i].Y;
	}
	public static int FirstBinary(int min, int max, Predicate<int> pred)
	{
		while (min < max)
		{
			var mid = (min + max) / 2;
			if (pred(mid)) max = mid;
			else min = mid + 1;
		}
		return min;
	}
}
