using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public List<TreeNode> ChildNodes { get; set; }        

        public void AddChildNode(TreeNode node)
        {
            if (ChildNodes == null)
            {
                ChildNodes = new List<TreeNode>();
            }
            ChildNodes.Add(node);           
        }        
        
        public void Delete(ref TreeNode root,string name)// также стирает все дочерние узлы, если имеются
        {  
            Queue<TreeNode> q= new Queue<TreeNode>(); 
            q.Enqueue(root); 

            if(root.Section.NameSection == name)
            {
                root = null;                
            }

            while (q.Count!=0) 
            {
                TreeNode temp = q.Dequeue();
                if (temp.ChildNodes == null) continue;
                foreach (var node in temp.ChildNodes)
                {
                    if (node.Section.NameSection == name)
                    {                        
                         temp.ChildNodes.Remove(node);
                        return;
                    }
                    q.Enqueue(node);
                }   
            }           
        }
        public TreeNode Search(TreeNode root, string name)
        {
            Queue<TreeNode> q = new Queue<TreeNode>();
            q.Enqueue(root);
            if (root.Section.NameSection == name)
            {
                return root;
            }
            while (q.Count != 0)
            {
                TreeNode temp = q.Dequeue();
                if (temp.ChildNodes == null) continue;
                foreach (var node in temp.ChildNodes)
                {
                    if (node.Section.NameSection == name)
                    {                        
                        return node;
                    }
                    q.Enqueue(node);
                }
            }

            return null;
        }
    }
}
