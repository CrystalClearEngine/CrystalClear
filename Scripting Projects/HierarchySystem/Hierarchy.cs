using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.HierarchySystem
{
	public struct Hierarchy
	{
		public Hierarchy(string treeCode)
		{
			HierarchyNode = HierarchyNode.BuildHierarchy(treeCode);
		}

		public HierarchyNode HierarchyNode;
	}

	public class HierarchyNode : IEnumerable<HierarchyNode>
	{
		public static HierarchyNode BuildHierarchy(string treeCode)
		{
			string[] lines = treeCode.Split(new[] { Environment.NewLine },
								   StringSplitOptions.RemoveEmptyEntries);

			HierarchyNode result = new HierarchyNode("TreeRoot");
			foreach (var line in lines)
			{
				var trimmedLine = line.Trim();
				var indent = line.Length - trimmedLine.Length;

				var child = new HierarchyNode(trimmedLine);
				List<HierarchyNode> list = new List<HierarchyNode> { result };
				list[indent].Add(child);

				if (indent + 1 < list.Count)
				{
					list[indent + 1] = child;
				}
				else
				{
					list.Add(child);
				}
			}

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			BuildString(sb, this, 0);

			return sb.ToString();
		}

		private static void BuildString(StringBuilder sb, HierarchyNode node, int depth)
		{
			sb.AppendLine(node.Name.PadLeft(node.Name.Length + depth));

			foreach (var child in node)
			{
				BuildString(sb, child, depth + 1);
			}
		}

		private readonly Dictionary<string, HierarchyNode> children =
											new Dictionary<string, HierarchyNode>();

		public readonly string Name;
		public HierarchyNode Parent { get; private set; }

		public HierarchyNode(string name)
		{
			Name = name;
		}

		public HierarchyNode GetChild(string name)
		{
			return children[name];
		}

		public void Add(HierarchyNode item)
		{
			if (item.Parent != null)
			{
				item.Parent.children.Remove(item.Name);
			}

			item.Parent = this;
			children.Add(item.Name, item);
		}

		public IEnumerator<HierarchyNode> GetEnumerator()
		{
			return children.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return children.Count; }
		}
	}
}
