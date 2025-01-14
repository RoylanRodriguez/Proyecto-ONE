

namespace Logic;

public class MapPrinter
{
    public static void PrintMap(int[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            List<int> ints = new List<int>();
            for (int j = 0; j < map.GetLength(1); j++)
            {
                ints.Add(map[i, j]);
            }

            string rowlog = "";

            foreach (int item in ints)
            {
                if (item == 0)
                {
                    rowlog += "0 ";
                }
                else if (item == 1)
                {
                    rowlog += "1 ";
                }
            }
            Console.WriteLine(rowlog);
        }

    }
}