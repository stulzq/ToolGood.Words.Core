using System.Collections.Generic;

namespace ToolGood.Words.Core.internals
{
	public abstract class BaseSearch
	{
		protected TrieNode[] _first = new TrieNode[char.MaxValue + 1];
		protected TrieNode _root = new TrieNode();


		public virtual void SetKeywords(ICollection<string> _keywords)
		{
			var first = new TrieNode[char.MaxValue + 1];
			var root = new TrieNode();

			foreach (var p in _keywords)
			{
				if (string.IsNullOrEmpty(p)) continue;

				//var nd = root;
				var nd = _first[p[0]];
				if (nd == null)
				{
					nd = root.Add(p[0]);
					first[p[0]] = nd;
				}
				for (var i = 1; i < p.Length; i++) nd = nd.Add(p[i]);
				nd.SetResults(p);
			}
			_first = first; // root.ToArray();

			var links = new Dictionary<TrieNode, TrieNode>();
			foreach (var item in root.m_values) TryLinks(item.Value, null, links);

			foreach (var item in links) item.Key.Merge(item.Value);

			_root = root;
		}

		private void TryLinks(TrieNode node, TrieNode node2, Dictionary<TrieNode, TrieNode> links)
		{
			foreach (var item in node.m_values)
			{
				TrieNode tn = null;
				if (node2 == null)
				{
					tn = _first[item.Key];
					if (tn != null) links[item.Value] = tn;
				}
				else
				{
					if (node2.TryGetValue(item.Key, out tn)) links[item.Value] = tn;
				}
				TryLinks(item.Value, tn, links);
			}
		}
	}
}