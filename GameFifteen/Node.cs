using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFifteen
{
    class Node
    {
        public List<Node> children = new List<Node>();
        public Node parent;
        public string symbol;
        public int[,] array = new int[4, 4];
        public int x = 0;
        public int y = 0;
        public int col = 4;
        public int F = 0;
        public int H = 0;
        public int G = 0;

        int[,] goalPuzzel = new int[,]
           {
                {1, 2, 3, 4 },
                {5, 6, 7, 8 },
                {9, 10, 11, 12 },
                {13, 14, 15, 16 }
           };



        public Node(int[,] p, string c)
        {
            SetPuzzel(p);
            H = ManhattanDistance(p, goalPuzzel);
            symbol = c;
        }

        public void SetPuzzel(int[,] p)
        {
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (p[i, j] == 0)
                        p[i, j] = 16;
                    this.array[i, j] = p[i, j];
                }
            }
        }




        public void ExpandNode()
        {
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (array[i, j] == 16)
                    {
                        x = i;
                        y = j;
                    }
                }
            }

            MoveToRight(array, x, y);
            MoveToLeft(array, x, y);
            MoveToUp(array, x, y);
            MoveToDown(array, x, y);
        }


        public void MoveToRight(int[,] p, int i, int j)
        {
            if (j < col - 1)
            {
                int[,] array = new int[4, 4];
                CopyPuzzel(array, p);

                int temp = array[i, j + 1];
                array[i, j + 1] = array[i, j];
                array[i, j] = temp;


                string symbol = array[i, j] + " left";
                Node child = new Node(array, symbol);
                children.Add(child);
                child.parent = this;


            }
        }

        public void MoveToLeft(int[,] p, int i, int j)
        {
            if (j > 0)
            {
                int[,] array = new int[4, 4];
                CopyPuzzel(array, p);

                int temp = array[i, j - 1];
                array[i, j - 1] = array[i, j];
                array[i, j] = temp;

                string symbol = array[i, j] + " right";
                Node child = new Node(array, symbol);
                children.Add(child);
                child.parent = this;

            }
        }

        public void MoveToUp(int[,] p, int i, int j)
        {
            if (i > 0)
            {
                int[,] array = new int[4, 4];
                CopyPuzzel(array, p);

                int temp = array[i - 1, j];
                array[i - 1, j] = array[i, j];
                array[i, j] = temp;

                string symbol = array[i, j] + " down";
                Node child = new Node(array, symbol);
                children.Add(child);
                child.parent = this;

            }
        }

        public void MoveToDown(int[,] p, int i, int j)
        {
            if (i < col - 1)
            {
                int[,] array = new int[4, 4];
                CopyPuzzel(array, p);

                int temp = array[i + 1, j];
                array[i + 1, j] = array[i, j];
                array[i, j] = temp;

                //;
                string symbol = array[i, j] + " up";
                Node child = new Node(array, symbol);
                children.Add(child);
                child.parent = this;
            }
        }


        public void PrintPuzzel()
        {
            Console.WriteLine();


            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (array[i, j] == 16)
                    {
                        array[i, j] = 0;
                    }
                    Console.Write(array[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine(symbol);
        }

        public void My_PrintPuzzeltext(bool k, string symbol)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Task2_output.txt", k, System.Text.Encoding.Default))
                {
                    sw.WriteLine(symbol);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool isSamePuzzel(int[,] p)
        {
            bool samePuzzel = true;

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (array[i, j] != p[i, j])
                    {
                        samePuzzel = false;
                    }
                }
            }
            return samePuzzel;
        }



        public void CopyPuzzel(int[,] a, int[,] b)
        {
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    a[i, j] = b[i, j];
                }
            }
        }



        public bool GoalTest()
        {
            bool isGoal = true;
            int m = array[0, 0];

            for (int i = 1; i < col; i++)
            {
                for (int j = 1; j < col; j++)
                {
                    if (m > array[i, j])
                        isGoal = false;
                    m = array[i, j];
                }
            }
            return isGoal;
        }

        public bool GoalState(int[,] a)
        {
            bool isGoalPuzzel = true;
            for (int i = 0; i < col; i++)
            {
                for (int k = 0; k < col; k++)
                {
                    if (goalPuzzel[i, k] != a[i, k])
                    {
                        isGoalPuzzel = false;
                    }
                }
            }
            return isGoalPuzzel;

        }

        public int ManhattanDistance(int[,] current, int[,] goal)
        {
            int manhattanDistance = 0;
            int currentDistance;

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (current[i, j] != goal[i, j])
                    {
                        int i1 = getPositionX(goal, current[i, j]);
                        int j1 = getPositionY(goal, current[i, j]);

                        currentDistance = Math.Abs((i1 - i)) + Math.Abs((j1 - j));

                        manhattanDistance += currentDistance;
                    }
                }
            }

            return manhattanDistance;
        }

        public int getPositionX(int[,] goal, int number)
        {
            int xPosition = 0;

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (goal[i, j] == number)
                        xPosition = i;
                }
            }
            return xPosition;
        }

        public int getPositionY(int[,] goal, int number)
        {
            int yPosition = 0;

            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (goal[i, j] == number)
                        yPosition = j;
                }
            }
            return yPosition;
        }
    }
}
