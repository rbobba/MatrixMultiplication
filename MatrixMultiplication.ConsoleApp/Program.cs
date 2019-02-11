using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMultiplication.ConsoleApp
{
    class Program
    {
        /// <summary>
        /// Entry point for the application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            try
            {
                //Stopwatch stopwatch = Stopwatch.StartNew();
                var result = GetMatrixResult<object>().GetAwaiter().GetResult();
                //stopwatch.Stop();
                Console.WriteLine(result);
                //Console.WriteLine(stopwatch.ElapsedMilliseconds + " milli Seconds");
                Console.ReadLine();
            }
            catch (ArgumentException aex)
            {
                Console.WriteLine($"Caught ArgumentException: {aex.Message}");
            }
        }

        public static async Task<object> GetMatrixResult<T>()
        {
            APIClient client = new APIClient();
            var matrixSizeUri = "api/numbers/init/1000";
            var matrixSize = await client.GetAsync<MatrixSize>(matrixSizeUri);
            var size = matrixSize.Value;

            var A = CreateArray<int>(size, size);
            var B = CreateArray<int>(size, size);
            var C = CreateArray<int>(size, size);

            string[] dsNames = new string[] { "A", "B" };
            var rowsA = new int[size][];
            var rowsB = new int[size][];


            var matrixData = GetMatrixData(client, size, dsNames);
            rowsA = (int[][])matrixData["A"];
            rowsB = (int[][])matrixData["B"];

            A = PopulateMatrixData(A, rowsA);
            B = PopulateMatrixData(B, rowsB);

            C = MultiplyMatrix(A, B, C, size);

            var concatenatedHashInput = MD5Hash(string.Concat(C.SelectMany(a => a).ToArray()));

            return await client.PostAsync<object>("api/numbers/validate", concatenatedHashInput);

        }

        private static int[][] PopulateMatrixData(int[][] dataset, int[][] rows)
        {

            for (int i = 0; i < rows.GetLength(0); i++)
            {
                for (int j = 0; j < rows.GetLength(0); j++)
                {
                    dataset[i] = rows[i];
                    dataset[j] = rows[j];
                }
            }

            return dataset;
        }

        private static Hashtable GetMatrixData(APIClient client, int size, string[] dataset)
        {
            Hashtable tb = new Hashtable();
            Parallel.ForEach(dataset, ds =>
            {
                var rows = Enumerable.Range(0, size)
                     .AsParallel()
                     .AsOrdered()
                     .Select(e => client.GetAsync<MatrixData>("api/numbers/" + ds + "/row/" + e).Result.Value)
                     .ToArray();
                tb.Add(ds, rows);
            });
            return tb;
        }
        private static T[][] CreateArray<T>(int rows, int cols)
        {
            T[][] array = new T[rows][];
            for (int i = 0; i < array.GetLength(0); i++)
                array[i] = new T[cols];

            return array;
        }

        /// <summary>
        /// This method will multiply matrix A, B and returns C
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="size"></param>
        /// <returns>C</returns>
        private static int[][] MultiplyMatrix(int[][] A, int[][] B, int[][] C, int size)
        {
            var source = Enumerable.Range(0, size);
            var pquery = from num in source.AsParallel()
                         select num;
            pquery.ForAll((i) =>
            {
                int[] iRowA = A[i];
                int[] iRowC = C[i];
                for (int k = 0; k < size; k++)
                {
                    int[] kRowB = B[k];
                    int ikA = iRowA[k];
                    for (int j = 0; j < size; j++)
                    {
                        iRowC[j] += ikA * kRowB[j];
                    }
                }
            });
            return C;
        }
        private static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString());
            }
            return hash.ToString();
        }

    }

}
