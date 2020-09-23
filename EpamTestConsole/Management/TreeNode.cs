using System;
using System.Collections.Generic;
using System.Threading;

namespace EpamTestConsole
{
    [Serializable]
    public class TreeNode
    {
        public TreeNode(Section section)
        {
            Section = section;
        }
        public Section Section { get; set; }

        public List<TreeNode> ChildNodes { get; private set; }

        public void AddChildNode(TreeNode node)
        {
            if (ChildNodes == null)
            {
                ChildNodes = new List<TreeNode>();
            }
            ChildNodes.Add(node);
        }

        public void Delete(ref TreeNode root, string name)// также стирает все дочерние узлы, если имеются
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);

            if (root.Section.NameSection == name)
            {
                root = null;
            }

            while (queue.Count != 0)
            {
                TreeNode temp = queue.Dequeue();
                if (temp.ChildNodes == null)
                {
                    continue;
                }
                foreach (var node in temp.ChildNodes)
                {
                    if (node.Section.NameSection == name)
                    {
                        temp.ChildNodes.Remove(node);
                        return;
                    }
                    queue.Enqueue(node);
                }
            }
        }

        public TreeNode Search(TreeNode root, string name)
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);
            if (root.Section.NameSection == name)
            {
                return root;
            }
            while (queue.Count != 0)
            {
                TreeNode temp = queue.Dequeue();
                if (temp.ChildNodes == null)
                {
                    continue;
                }
                foreach (var node in temp.ChildNodes)
                {
                    if (node.Section.NameSection == name)
                    {
                        return node;
                    }
                    queue.Enqueue(node);
                }
            }

            return null;
        }

        public static void WalkTheTree(TreeNode root, TreeNodeOperation operation)
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);

            while (queue.Count != 0)
            {
                TreeNode temp = queue.Dequeue();
                operation(temp);

                if (temp.ChildNodes == null)
                {
                    continue;
                }
                foreach (var node in temp.ChildNodes)
                {
                    queue.Enqueue(node);
                }
            }
        }
        public static void WalkTheTree(TreeNode root, TreeNodeOperation WriteSectionToConsole,
            ref Timer timer, ref ManualResetEvent _eventMainMenu, CancellationToken token)
        {

            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);

            while (queue.Count != 0)
            {
                TreeNode temp = queue.Dequeue();
                WriteSectionToConsole(temp);
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine(ConsoleMenuConstant.TimeIsOver);
                    _eventMainMenu.Set();
                    return;
                }

                if (temp.ChildNodes == null)
                {
                    continue;
                }
                foreach (var node in temp.ChildNodes)
                {
                    queue.Enqueue(node);
                }
            }

            timer.Dispose();
            _eventMainMenu.Set();
        }
    }
}
