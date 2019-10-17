using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameFifteen
{
    public partial class Form1 : Form
    {
        Game game;
        int repeat = 0;
        List<Node> solution;

        static int[,] InitialState = new int[,]
        {
            {1, 2, 3, 4 },
            {5, 6, 7, 8 },
            {9, 10, 11, 12 },
            {13, 14, 15, 0 }
        };

        static int[,] ErrorState = new int[,]
       {
            {1, 2, 3, 4 },
            {5, 6, 7, 8 },
            {9, 10, 11, 12 },
            {13, 15, 14, 0 }
       };

        public Form1()
        {
            InitializeComponent();

            game = new Game(4);
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialog1.InitialDirectory = @Environment.CurrentDirectory;
        }

     

        private Button button(int position)
        {
            switch (position)
            {
                case 0: return button0;
                case 1: return button1;
                case 2: return button2;
                case 3: return button3;
                case 4: return button4;
                case 5: return button5;
                case 6: return button6;
                case 7: return button7;
                case 8: return button8;
                case 9: return button9;
                case 10: return button10;
                case 12: return button12;
                case 11: return button11;
                case 13: return button13;
                case 14: return button14;
                case 15: return button15;
                default: return null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            start_game();
        }

        private void startGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start_game();
        }

        private void task2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;


                solution = Solver(fileName);
                bool isRewrite = false;
                for (int i = 0; i < solution.Count; i++)
                {
                    string symbol = solution[i].symbol;
                    PrintInTextFile(isRewrite, symbol);
                    isRewrite = true;
                }
            }
        }

        private void task3ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;

                solution = Solver(fileName);
                repeat = 0;
                timer1.Interval = Convert.ToInt32(textBox1.Text);
                timer1.Enabled = true;
            }

        }


        private void start_game()
        {
            game.Start(InitialState);
            refresh();

        }

        private void refresh()
        {

            for (int position = 0; position < 16; position++)
            {
                int nr = game.get_number(position);
                button(position).Text = nr.ToString();
                button(position).Visible = (nr > 0);
            }
        }

        private void createTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                game.CreateText(fileName);
            }
        }


        public void showTable(int[,] Arr)
        {
            int position = 0;
            for (int k = 0; k < 4; k++)
            {
                for (int L = 0; L < 4; L++)
                {
                    button(position).Text = Arr[k, L].ToString();
                    button(position).Visible = Arr[k, L] !=16;
                    position++;
                }
            }
        }


        private void Show(int k)
        {

            if (k == 0)
            {
                refresh();
                return;
            }
            else
            {
                game.Start(solution[k].array);
                refresh();
            }
        }


        private static List<Node> Solver(string fileName)
        {
            int[,] MyExample = GetArray(fileName);
            Node root;

            if (SamePazzel(MyExample,ErrorState))
            {
                root = new Node(InitialState, null);
                MessageBox.Show("This puzzle is unsolvable");
            }
            else
            {
                root = new Node(MyExample, null);
            }

            Search ui = new Search();
            List<Node> solution = ui.BreadthFirstSearch(root);

            solution.Reverse();
            return solution;
        }

        private static int[,] GetArray(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            int[,] Example = new int[lines.Length, lines[0].Split(' ').Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < temp.Length; j++)
                    Example[i, j] = Convert.ToInt32(temp[j]);
            }

            return Example;
        }

        private static bool SamePazzel(int[,] a, int[,] b)
        {
            bool samePuzzel = true;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (a[i, j] != b[i, j])
                        samePuzzel = false;
                }
            }
            return samePuzzel;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Show(repeat);
            if (repeat == solution.Count - 1 || (solution.Count == 0))
            {
                timer1.Enabled = false;
            }
            repeat++;
        }

      
        private static void PrintInTextFile(bool isRewrite, string symbol)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Task2_output.txt", isRewrite, Encoding.Default))
                {
                    sw.WriteLine(symbol);

                }

            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            int position = Convert.ToInt32(((Button)sender).Tag);
            game.shift(position);
            refresh();

        }

        private void shuffelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int m = 0; m < 50; m++)
            {
                game.ShiftRandom();
            }
            refresh();
        }
    }
}
