using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFifteen
{
    class Search
    {
        public Search()
        { }

        public List<Node> BreadthFirstSearch(Node root)
        {
            List<Node> PathToSolution = new List<Node>();
            List<Node> OpenList = new List<Node>();
            List<Node> ClosedList = new List<Node>();
            List<Node> Node = new List<Node>();
            int cycle = 9;

            OpenList.Add(root);
            bool goalFound = false;


            if (root.H == 0)
            {
                goalFound = true;
            }

            while (!goalFound)
            {
                int hMin = 100;

                Node currentNode;

                while (OpenList.Count != 0)
                {
                    currentNode = OpenList[0];
                    ClosedList.Add(currentNode);
                    OpenList.RemoveAt(0);
                    currentNode.ExpandNode();

                    for (int j = 0; j < currentNode.children.Count; j++)
                    {
                        Node.Add(currentNode.children[j]);
                    }
                }

                if (cycle == 0)
                {
                    for (int i = 0; i < Node.Count; i++)
                    {
                        if (Node[i].H < hMin)
                        {
                            hMin = Node[i].H;
                        }
                    }

                    for (int i = 0; i < Node.Count; i++)
                    {
                        if (Node[i].H == hMin)
                        {
                            Node currentChild = Node[i];
                            if (currentChild.GoalState(currentChild.array))
                            {
                                Console.WriteLine("Goal found");
                                goalFound = true;

                                PathTrace(PathToSolution, currentChild);
                            }
                            if (!Contains(OpenList, currentChild) && !Contains(ClosedList, currentChild))
                            {
                                OpenList.Add(currentChild);
                            }
                        }
                    }
                    Node.Clear();
                }
                else
                {
                    for (int i = 0; i < Node.Count; i++)
                    {
                        Node currentChild = Node[i];
                        if (currentChild.GoalState(currentChild.array))
                        {
                            Console.WriteLine("Goal found");
                            goalFound = true;
                            PathTrace(PathToSolution, currentChild);
                        }
                        if (!Contains(OpenList, currentChild) && !Contains(ClosedList, currentChild))
                        {
                            OpenList.Add(currentChild);
                        }
                    }
                    Node.Clear();
                }


                if (cycle == 0)
                {
                    cycle = 9;
                }
                else
                    cycle = cycle - 1;
            }
            return PathToSolution;
        }

        public void PathTrace(List<Node> path, Node n)
        {
            Console.WriteLine("Tracing path ..");
            Node current = n;

            path.Add(current);

            while (current.parent != null)
            {
                current = current.parent;
                path.Add(current);
            }
        }

        public static bool Contains(List<Node> list, Node c)
        {
            bool contains = false;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].isSamePuzzel(c.array))
                {
                    contains = true;
                }
            }

            return contains;
        }
    }
}
