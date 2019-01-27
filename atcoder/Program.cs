using System.IO;
using System.Linq;
using static System.Console;
using System.Collections.Generic;
using System.Text;
struct Pair
{
	public readonly int A, B;
	public Pair(int a, int b) { A = a; B = b; }
}
class K
{
	static int[] G => ReadLine().Split().Select(int.Parse).ToArray();
	static void Main()
	{
		SetOut(new StreamWriter(OpenStandardOutput()) { AutoFlush = false });
		var I = G;
		int N = I[0], M = I[1];
		var es = new List<Pair>(M);
		for (var e = 0; e < M; e++) { I = G; es.Add(new Pair(I[0] - 1, I[1] - 1)); }
		var fuf = new FastUnionFindTree(N);
		foreach (var e in es) fuf.UniteCategory(e.A, e.B);
		var par = new int[N];
		var repId = new Dictionary<int, int>();
		for (var u = 0; u < N; u++) if (u == (par[u] = fuf.GetRootOf(u))) repId[u] = repId.Count;
		fuf = null;
		var R = repId.Count;
		var id = new Dictionary<int, int>[R];
		var es2 = new List<int>[R];
		var qs2 = new HashSet<int>[R];
		for (var r = 0; r < R; r++) { id[r] = new Dictionary<int, int>(); es2[r] = new List<int>(); qs2[r] = new HashSet<int>(); }
		for (var u = 0; u < N; u++)
		{
			var r = repId[par[u]];
			id[r][u] = id[r].Count;
		}
		for (var e = 0; e < M; e++) es2[repId[par[es[e].A]]].Add(e);
		var Q = G[0];
		var ans = new int[Q];
		var qs = new Pair[Q];
		for (var i = 0; i < Q; i++)
		{
			I = G; ans[i] = -1;
			var q = qs[i] = new Pair(I[0] - 1, I[1] - 1);
			var r = repId[par[q.A]];
			if (par[q.A] == par[q.B]) { qs2[r].Add(i); qs[i] = new Pair(id[r][q.A], id[r][q.B]); }
		}
		repId.Clear();
		for (var r = 0; r < R; r++)
		{
			if (qs2[r].Count == 0) continue;
			if (qs2[r].Count <= 8 || id[r].Count <= 10)
			{
				fuf = new FastUnionFindTree(id[r].Count);
				foreach (var i in es2[r])
				{
					int a = fuf.GetRootOf(id[r][es[i].A]), b = fuf.GetRootOf(id[r][es[i].B]);
					if (a != b)
					{
						fuf.UniteCategory(a, b);
						foreach (var q in qs2[r]) if (ans[q] < 0 && fuf.IsSameCategory(qs[q].A, qs[q].B)) ans[q] = i + 1;
					}
				}
				qs2[r].Clear();
				continue;
			}
			fuf = new FastUnionFindTree(id[r].Count);
			var con = new List<UnionFindTree>(id[r].Count) { UnionFindTree.Create(id[r].Count) };
			var ids = new List<int> { 0 };
			foreach (var e in es2[r])
			{
				var last = con[con.Count - 1];
				int a = fuf.GetRootOf(id[r][es[e].A]), b = fuf.GetRootOf(id[r][es[e].B]);
				if (a != b)
				{
					ids.Add(e + 1);
					fuf.UniteCategory(a, b);
					con.Add(last.Unite(a, b));
				}
			}
			fuf = null; es2[r].Clear();
			foreach (var q in qs2[r])
			{
				int left = 0, right = con.Count;
				while (left < right)
				{
					var i = (left + right) / 2;
					int a = con[i][qs[q].A], b = con[i][qs[q].B];
					if (a == b) right = i;
					else left = i + 1;
				}
				ans[q] = ids[left];
			}
			con.Clear(); ids.Clear(); qs2[r].Clear();
		}
		var sb = new StringBuilder();
		for (var i = 0; i < Q; i++) sb.AppendLine(ans[i].ToString());
		Write(sb);
		Out.Flush();
	}
}
struct UnionFindTree
{
	public int this[int x]
	{
		get
		{
			int y;
			while ((y = par.At(x)) != x) x = y;
			return x;
		}
	}
	readonly Tree par, rank;
	UnionFindTree(Tree p, Tree r) { par = p; rank = r; }
	public UnionFindTree Unite(int x, int y)
	{
		int rx = rank.At(x), ry = rank.At(y);
		if (rx < ry) return new UnionFindTree(par.Set(x, y), rank);
		return new UnionFindTree(par.Set(y, x), rx == ry ? rank.Set(x, rx + 1) : rank);
	}
	static Dictionary<int, UnionFindTree> memo = new Dictionary<int, UnionFindTree>();
	public static UnionFindTree Create(int N)
	{
		if (!memo.ContainsKey(N))
		{
			var a = new int[N];
			var r = Tree.Create(a, 0, N);
			for (var i = 1; i < N; i++) a[i] = i;
			var p = Tree.Create(a, 0, N);
			memo[N] = new UnionFindTree(p, r);
		}
		return memo[N];
	}
}
class FastUnionFindTree
{
	readonly int[] rank, par;
	public FastUnionFindTree(int N)
	{
		par = new int[N];
		rank = new int[N];
		for (var i = 1; i < N; i++) par[i] = i;
	}
	public int GetRootOf(int x) => par[x] == x ? x : par[x] = GetRootOf(par[x]);
	public bool UniteCategory(int x, int y)
	{
		if ((x = GetRootOf(x)) == (y = GetRootOf(y))) return false;
		if (rank[x] < rank[y]) par[x] = y;
		else
		{
			par[y] = x;
			if (rank[x] == rank[y]) rank[x]++;
		}
		return true;
	}
	public bool IsSameCategory(int x, int y) => GetRootOf(x) == GetRootOf(y);
}
class Tree
{
	readonly Tree L, R;
	readonly int I, X;
	public Tree(Tree l, int i, int x, Tree r) { L = l; X = x; I = i; R = r; }
	public int At(int i)
	{
		var t = this;
		while (true)
			if (t.I > i) t = t.L;
			else if (t.I < i) t = t.R;
			else return t.X;
	}
	public Tree Set(int i, int x)
	{
		if (I > i) return new Tree(L.Set(i, x), I, X, R);
		if (I < i) return new Tree(L, I, X, R.Set(i, x));
		return new Tree(L, i, x, R);
	}
	public static Tree Create(int[] list, int l, int r)
	{
		if (r - l == 1) return new Tree(null, l, list[l], null);
		if (r - l == 2) return new Tree(new Tree(null, l, list[l], null), l + 1, list[l + 1], null);
		if (r - l == 3) return new Tree(new Tree(null, l, list[l], null), l + 1, list[l + 1], new Tree(null, l + 2, list[l + 2], null));
		var n = (l + r) / 2;
		return new Tree(Create(list, l, n), n, list[n], Create(list, n + 1, r));
	}
}
