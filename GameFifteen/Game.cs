using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;

namespace GameFifteen
{
    class Game
    {
        int size;
        public int[,] table;
        int space_x, space_y;
        static Random rnd = new Random();

        public Game(int size)
        {
            if (size < 2) size = 2;
            if (size > 5) size = 5;
            this.size = size;
            table = new int[size, size];
        }

        public void start()
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    table[x, y] = coord_to_position(x, y) + 1;

            space_x = size - 1;
            space_y = size - 1;
            table[space_x, space_y] = 0;
        }

        public void Start(int [,] pazzel)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                   table[x,y] = pazzel[y,x];
                    if (table[x, y] == 16 || table[x,y] ==0)
                    {
                        space_x = x;
                        space_y = y;
                    }
                }
            }

            table[space_x, space_y] = 0;

        }

        public void shift(int position)
        {
            int x;
            int y;
            position_to_coord(position, out x, out y);
            if (Math.Abs(space_x - x) + Math.Abs(space_y - y) != 1)
            {
                return;
            }
            table[space_x, space_y] = table[x, y];
            table[x, y] = 0;
            space_x = x;
            space_y = y;
        }

        public void ShiftRandom()
        {
            int step = rnd.Next(0,4);
            int x = space_x;
            int y = space_y;
            switch(step)
            {
                case 0: x--;
                    break;
                case 1: x++;
                    break;
                case 2: y--;
                    break;
                case 3: y++;
                    break;
            }
            shift(coord_to_position(x, y));
        }

        public void CreateText(string fileName)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            sw.Write(table[j, i] + " ");
                        }
                        sw.WriteLine();
                    }
                    
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        


        public int get_number(int position)
        {
            int x;
            int y;
            position_to_coord(position, out x, out y);
            if (x < 0 || x >= size) return 0;
            if (y < 0 || y >= size) return 0;
            return table[x, y];
        }

        private int coord_to_position(int x, int y)
        {
            if (x < 0) x = 0;
            if (x > size - 1) x = size - 1;
            if (y < 0) y = 0;
            if (y > size - 1) y = size * 1;

            return y * size + x;
        }

        private void position_to_coord(int position, out int x, out int y)
        {
            if (position < 0) position = 0;
            if (position > size * size - 1) position = size * size-1;
            x = position % size;
            y = position / size;
        }
    }
}
