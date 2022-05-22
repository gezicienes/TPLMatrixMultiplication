using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

class Program
{

    static void serial(ulong[,] matrix1, int line1, int column1, ulong[,] matrix2, int line2, int column2)
    {

        //declaration of result matrix
        ulong[,] result = new ulong[column1, line2];

        //multiply the result
        for (int i = 0; i < column1; i++)
        {
            for (int j = 0; j < line2; j++)
            {
                result[i, j] = 0;

                for (int k = 0; k < line1; k++)
                {
                    result[i, j] += matrix1[i, k] * matrix2[k, j];
                }
            }
        }
    }


    static void parallel(ulong[,] matrix1, int line1, int column1, ulong[,] matrix2, int line2, int column2)
    {

        //declaration of result matrix
        ulong[,] result = new ulong[column1, line2];

        ///multiply the result
        Parallel.For(0, column1, i => {
            Parallel.For(0, line2, j => {
                result[i, j] = 0;
                Parallel.For(0, line1, k => {
                    result[i, j] += matrix1[i, k] * matrix2[k, j];
                });
            });
        });
    }
    //1'i 2'ye böl speedup, 12'ye böl efficiency bul.
    static void Main()
    {
        //random and stopwatch variables
        Random rnd = new Random();
        Stopwatch sw = new Stopwatch();

        //test limit
        int n = 1;
        while (n <= 10)
        {
            Console.WriteLine(n + ". PROCESS:\n");

            //declaration of the matrices
            int line1 = 500;
            int column1 = 500;
            ulong[,] matrix1 = new ulong[column1, line1];
            int line2 = 500;
            int column2 = line1;
            ulong[,] matrix2 = new ulong[column2, line2];

            //filling
            for (int y = 0; y < column1; y++)
            {
                for (int x = 0; x < line1; x++)
                {
                    matrix1[y, x] = (ulong)rnd.Next(1,9);
                }
            }
            for (int y = 0; y < column2; y++)
            {
                for (int x = 0; x < line2; x++)
                {
                    matrix2[y, x] = (ulong)rnd.Next(1,9);
                }
            }

            //results
            Console.WriteLine("Matrix1 Column: " + column1 + ", Line: " + line1);
            Console.WriteLine("Matrix2 Column: " + column2 + ", Line: " + line2 + "\n");
            Console.WriteLine("Serial Method results:");
            sw.Start();
            serial(matrix1, line1, column1, matrix2, line2, column2);
            sw.Stop();
            Console.WriteLine("\nThe Serial process takes " + sw.ElapsedMilliseconds + " ms.");
            Console.WriteLine("\n");
            Console.WriteLine("Parallel Method results:");
            sw.Restart();
            parallel(matrix1, line1, column1, matrix2, line2, column2);
            sw.Stop();
            Console.WriteLine("\nThe Parallel process takes " + sw.ElapsedMilliseconds + " ms.");
            Console.WriteLine("\n");

            n++;
        }
    }
}