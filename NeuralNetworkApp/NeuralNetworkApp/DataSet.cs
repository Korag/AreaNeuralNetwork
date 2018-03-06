using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace K_Means_Clustering
{
    class DataSet
    {
        private static int t = 0;
        public static int numerator = 0;

        // Pobranie danych z pliku do tablicy
        public static double[][] GetDataSet(string path, int LastColumnInterpreter)
        {
            double[][] DataSet;
            try
            {
                string[] s1 = File.ReadAllLines(path);
                DataSet = new double[s1.Length][];
                for (int i = 0; i < s1.Length; i++)
                {
                    string[] s2 = s1[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    DataSet[i] = new double[s2.Length - LastColumnInterpreter];
                    for (int j = 0; j < s2.Length - LastColumnInterpreter; j++)
                    {
                        DataSet[i][j] = Double.Parse(s2[j], CultureInfo.InvariantCulture);
                    }
                }
                return DataSet;
            }

            catch
            {

            }
            return null;
        }

        // Prezentacja 2 wymiarowej tablicy
        public static void ShowDataSet(double[][] DataSet)
        {
            for (int i = 0; i < DataSet.Length; i++)
            {
                for (int j = 0; j < DataSet[i].Length; j++)
                {
                    Console.Write(DataSet[i][j].ToString("F" + 1) + " ");
                }
                Console.WriteLine("");
            }
        }

        // Prezentacja podzialu na klastry dla danego zbioru danych
        public static void ShowClustered(double[][] DataSet, int[] cluster, int quantityOfClusters)
        {
            for (int k = 0; k < quantityOfClusters; k++)
            {
                Console.WriteLine("-----------" + k + "-----------");
                Console.WriteLine("========================");
                for (int i = 0; i < DataSet.Length; i++)
                {
                    int clusterID = cluster[i];
                    if (clusterID != k) continue;
                    Console.Write(i.ToString().PadLeft(3) + " ");
                    for (int j = 0; j < DataSet[i].Length; ++j)
                    {
                        if (DataSet[i][j] >= 0.0) Console.Write(" ");
                        Console.Write(DataSet[i][j].ToString("F" + 1) + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("========================");
            }
        }

        // Zapis do pliku podzialu na klastry
        public static void SaveClusteredToFile(double[][] DataSet, int[] cluster, int quantityOfClusters, string Path)
        {
            using (StreamWriter sw = new StreamWriter(Path))
            {
                for (int k = 0; k < quantityOfClusters; k++)
                {
                    sw.WriteLine("-----------" + k + "-----------");
                    sw.WriteLine("========================");
                    for (int i = 0; i < DataSet.Length; i++)
                    {
                        int clusterID = cluster[i];
                        if (clusterID != k) continue;
                        sw.Write(i.ToString().PadLeft(3) + " ");
                        for (int j = 0; j < DataSet[i].Length; j++)
                        {
                            if (DataSet[i][j] >= 0.0) sw.Write(" ");
                            sw.Write(DataSet[i][j].ToString("F" + 1) + " ");
                        }
                        sw.WriteLine("");
                    }
                    sw.WriteLine("========================");
                }
            }
        }

        // Zapis kazdego klastra do oddzielnego pliku
        public static void SaveEachClusterToFile(double[][] DataSet,string[] StringSet, int[] cluster, int quantityOfClusters, string Path, int iteration, int higher)
        {
            int s = 0;
            for (int k = 0; k < quantityOfClusters; k++)
            {
                using (StreamWriter sw = new StreamWriter(Path + $"{iteration}-{higher}-{s}.txt"))
                {

                    for (int i = 0; i < DataSet.Length; i++)
                    {
                        int clusterID = cluster[i];
                        if (clusterID != k) continue;


                        for (int j = 0; j < DataSet[i].Length; j++)
                        {
                            if (DataSet[i][j] == 0)
                            {
                                break;
                            }
                            sw.Write(DataSet[i][j].ToString("F" + 1) + " ");

                            if (StringSet != null)
                            {
                                if (j == DataSet[i].Length - 1)
                                {
                                    sw.Write(StringSet[i].ToString() + " ");
                                    continue;
                                }
                            }
                        }
                        sw.WriteLine("");
                    }
                }
                s++;
            }
        }

        // Tworzenie tablicy ostatniego parametru typu string
        public static string[] GetStringVector(string path)
        {
            string[] StringSet;
            try
            {
                string[] s1 = File.ReadAllLines(path);
                StringSet = new string[s1.Length];
                for (int i = 0; i < s1.Length; i++)
                {
                    string[] s2 = s1[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    StringSet[i] = s2[s2.Length-1];
                }
                return StringSet;
            }

            catch
            {

            }
            return null;

        }
            


        // Zapis kazdego klastra do oddzielnego pliku dla iteracji 0 (tzw. bazowej)
        public static void SaveEachClusterToFile_Basic(double[][] DataSet, string[] StringSet, int[] cluster, int quantityOfClusters, string Path)
        {
            int s = 0;
            for (int k = 0; k < quantityOfClusters; k++)
            {
                using (StreamWriter sw = new StreamWriter(Path + $"{0}-{s}.txt"))
                {

                    for (int i = 0; i < DataSet.Length; i++)
                    {
                        int clusterID = cluster[i];
                        if (clusterID != k) continue;


                        for (int j = 0; j < DataSet[i].Length; j++)
                        {

                            if (DataSet[i][j] == 0)
                            {
                                break;
                            }
                           
                            sw.Write(DataSet[i][j].ToString("F" + 1) + " ");

                            if (StringSet != null)
                            {
                                if (j == DataSet[i].Length-1)
                                {
                                    sw.Write(StringSet[i].ToString() + " ");
                                    continue;
                                }
                            }
                        }
                        sw.WriteLine("");
                    }
                }
                s++;
            }
        }

        // Dodanie pojedynczego klastra do tablicy 
        public static double[][] AddOneClusterToMatrix(double[][] DataSet, int[] cluster, int quantityOfClusters)
        {
            double[][] Matrix = new double[DataSet.Length][];

            for (int i = 0; i < DataSet.Length; i++)
            {
                Matrix[i] = new double[DataSet[i].Length];

                int clusterID = cluster[i];
                if (clusterID != numerator) continue;

                for (int j = 0; j < DataSet[i].Length; j++)
                {

                    Matrix[i][j] = DataSet[i][j];
                }
            }

            // Zliczanie niepustych rekordow
            int NewMatrixLength = 0;
            for (int i = 0; i < Matrix.Length; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if(Matrix[i][0]==0 && Matrix[i][1] == 0)
                    {
                       
                    }
                    else
                    {
                        NewMatrixLength++;
                    }
                }
            }

            // Tworzenie tablicy bez pustych rekordow
            int z = 0;
            double[][] MatrixResult = new double[NewMatrixLength][];
            for (int i = 0; i < Matrix.Length; i++)
            {
                for (int j = 0; j < Matrix[i].Length; j++)
                {
                    if (Matrix[i][0] == 0 && Matrix[i][1] == 0)
                    {
                        break;
                    }
                    else
                    {
                        if(j==0)
                        {
                            MatrixResult[z] = new double[Matrix[i].Length];
                        }

                        MatrixResult[z][j] = Matrix[i][j];
                    }
                    if(j==Matrix[i].Length-1)
                    {
                        z++;
                    }
                }
            }


            numerator++;
            return MatrixResult;
        }

        // Prezentacja wektora
        public static void ShowVector(int[] Vector)
        {
            for (int i = 0; i < Vector.Length; i++)
            {
                Console.Write(Vector[i] + " ");
            }
        }

        // Zapis wektora do pliku
        public static void SaveVectorToFile(int[] Vector, string Path)
        {
            using (StreamWriter sw = new StreamWriter(Path))
            {
                for (int i = 0; i < Vector.Length; i++)
                {
                    sw.WriteLine("index: " + i + "\t cluster: " + Vector[i] + " ");
                }
            }
        }

        // Zliczanie plikow w katalogu 
        public static int CountElements(string Path)
        {
            DirectoryInfo d1 = new DirectoryInfo(Path);
            FileInfo[] f1 = d1.GetFiles();
            int count = f1.Length;
            return count;
        }


    }
}
