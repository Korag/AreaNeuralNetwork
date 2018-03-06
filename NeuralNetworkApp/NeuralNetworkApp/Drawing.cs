using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace K_Means_Clustering
{
    class Drawing
    {
        // Wyszukiwanie najwiekszej wartosci w zbiorze danych dla danego wymiaru
        private static double SearchRange(double[][] BaseMatrix, int Dimension)
        {
            double range = 0;
            double maxValue = BaseMatrix[0][Dimension];

            for (int i = 1; i < BaseMatrix.Length; i++)
            {
                if (BaseMatrix[i][Dimension] > maxValue)
                {
                    maxValue = BaseMatrix[i][Dimension];
                }
            }

            range = maxValue + 2;

            return range;
        }

        // Funkcja do rysowania najbardziej podstawowego wykresu
        public static void DrawClusterGraph(double[][] BaseMatrix, double[][][] ClusteredMatrix, string PathToGraph, int GraphWidth, int GraphHeight, int FirstDimension, int SecondDimension, int iterationsNumber)
        {
            Bitmap b = new Bitmap(GraphWidth, GraphHeight);
            Graphics g = Graphics.FromImage(b);
            Brush br = new SolidBrush(Color.White);

            g.FillRectangle(br, 0, 0, b.Width, b.Height);

            Pen p1 = new Pen(Color.Black, 5);
            g.DrawLine(p1, 0, b.Height / 2, b.Width, b.Height / 2);
            g.DrawLine(p1, b.Width / 2, 0, b.Width / 2, b.Height);

            // Zakresy oraz skala
            double rangeX = Math.Ceiling(SearchRange(BaseMatrix, FirstDimension));
            double rangeY = Math.Ceiling(SearchRange(BaseMatrix, SecondDimension));
            double Unit = 30;

            double scaleX = b.Width / Unit;
            double scaleY = b.Height / Unit;

            double rangePerUnitX = rangeX / Unit;
            double rangePerUnitY = rangeY / Unit;

            Pen p11 = new Pen(Color.Black);
            Pen p2 = new Pen(Color.Red);

            int dx = (int)scaleX;
            int dy = (int)scaleY;

            Font f1 = new Font(FontFamily.GenericSansSerif, 12);
            Brush br2 = new SolidBrush(Color.Blue);
            SolidBrush br3 = new SolidBrush(Color.Red);


            double t1 = rangeX;
            double t2 = rangeY;

            // Rysowanie osi wspolrzednych
            for (int OY = 0; OY < b.Height; OY += dy)
            {
                g.DrawLine(p11, 0, OY, b.Width, OY);

                string s = (t1).ToString("F" + 1);
                g.DrawString(s, f1, br2, b.Width / 2 + 1, OY);
                t1 -= 2 * rangePerUnitX;
            }

            t1 = -rangeX;

            for (int OX = 0; OX < b.Width; OX += dx)
            {
                g.DrawLine(p11, OX, 0, OX, b.Height);

                string s = (t1).ToString("F" + 1);
                g.DrawString(s, f1, br2, OX, (b.Height / 2) - 1);
                t1 += 2 * rangePerUnitX;
            }

            scaleX /= 2 * rangePerUnitX;
            scaleY /= 2 * rangePerUnitX;

            // Rysowanie elips 
            for (int i = 0; i < Clustering.ClustersMade; i++)
            {
                for (int j = 0; j < ClusteredMatrix[i].Length; j++)
                {
                    for (int k = 0; k < ClusteredMatrix[i][j].Length; k++)
                    {
                        float x = (float)(ClusteredMatrix[i][j][FirstDimension]);
                        float y = (float)(ClusteredMatrix[i][j][SecondDimension]);

                        switch (i)
                        {
                            case 0:
                                p2.Color = Color.Red;
                                br3.Color = Color.Red;
                                break;
                            case 1:
                                p2.Color = Color.Blue;
                                br3.Color = Color.Blue;
                                break;
                            case 2:
                                p2.Color = Color.Green;
                                br3.Color = Color.Green;
                                break;
                            case 3:
                                p2.Color = Color.Brown;
                                br3.Color = Color.Brown;
                                break;
                            case 4:
                                p2.Color = Color.Cyan;
                                br3.Color = Color.Cyan;
                                break;
                            case 5:
                                p2.Color = Color.Gold;
                                br3.Color = Color.Gold;
                                break;
                            case 6:
                                p2.Color = Color.Magenta;
                                br3.Color = Color.Magenta;
                                break;
                            case 7:
                                p2.Color = Color.Pink;
                                br3.Color = Color.Pink;
                                break;
                            case 8:
                                p2.Color = Color.Ivory;
                                br3.Color = Color.Ivory;
                                break;
                            case 9:
                                p2.Color = Color.Lime;
                                br3.Color = Color.Lime;
                                break;
                            case 10:
                                p2.Color = Color.Orange;
                                br3.Color = Color.Orange;
                                break;
                            default:
                                p2.Color = Color.Red;
                                break;
                        }


                        g.DrawEllipse(p2, (int)(scaleX * x + b.Width / 2), (int)(b.Height / 2 - scaleY * y), 8, 8);
                        g.FillEllipse(br3, (int)(scaleX * x + b.Width / 2), (int)(b.Height / 2 - scaleY * y), 8, 8);
                    }
                }
            }

            int r = DataSet.CountElements(@"..\..\..\");
            r++;
            b.Save(PathToGraph + r + ".png");
        }

        // Funkcja do rysowania wykresu z uwzglednieniem dziedziczenia(kolor figury) po nadrzednej iteracji
        public static void DrawClusterGraph2(double[][] BaseMatrix, double[][][] ClusteredMatrix, string PathToGraph, int GraphWidth, int GraphHeight, int FirstDimension, int SecondDimension, int iterationsNumber)
        {
            Bitmap b = new Bitmap(GraphWidth, GraphHeight);
            Graphics g = Graphics.FromImage(b);
            Brush br = new SolidBrush(Color.White);

            g.FillRectangle(br, 0, 0, b.Width, b.Height);

            Pen p1 = new Pen(Color.Black, 5);
            g.DrawLine(p1, 0, b.Height / 2, b.Width, b.Height / 2);
            g.DrawLine(p1, b.Width / 2, 0, b.Width / 2, b.Height);

            double rangeX = Math.Ceiling(SearchRange(BaseMatrix, FirstDimension));
            double rangeY = Math.Ceiling(SearchRange(BaseMatrix, SecondDimension));
            double Unit = 30;

            double scaleX = b.Width / Unit;
            double scaleY = b.Height / Unit;

            double rangePerUnitX = rangeX / Unit;
            double rangePerUnitY = rangeY / Unit;

            Pen p11 = new Pen(Color.Black);
            Pen p2 = new Pen(Color.Red);

            int dx = (int)scaleX;
            int dy = (int)scaleY;

            Font f1 = new Font(FontFamily.GenericSansSerif, 12);
            Brush br2 = new SolidBrush(Color.Blue);
            SolidBrush br3 = new SolidBrush(Color.Red);
           

            double t1 = rangeX;
            double t2 = rangeY;

            for (int OY = 0; OY < b.Height; OY += dy ) 
            {
                g.DrawLine(p11, 0, OY, b.Width, OY);

                string s = (t1).ToString("F" + 1);
                g.DrawString(s, f1, br2, b.Width/2 + 1, OY);
                t1 -= 2*rangePerUnitX;
            }

            t1 = -rangeX;

            for (int OX = 0; OX < b.Width; OX += dx )
            {
                g.DrawLine(p11, OX, 0, OX, b.Height);

                string s = (t1).ToString("F" + 1);
                g.DrawString(s, f1, br2, OX, (b.Height / 2) - 1);
                t1 += 2*rangePerUnitX;
            }

            scaleX /= 2 * rangePerUnitX;
            scaleY /= 2 * rangePerUnitX;

            for (int i = 0; i < Clustering.ClustersBasicQuantity; i++)
            {
                for (int j = 0; j < ClusteredMatrix[i].Length; j++)
                {
                    for (int k = 0; k < ClusteredMatrix[i][j].Length; k++)
                    {

                        float x = (float)(ClusteredMatrix[i][j][FirstDimension]);
                        float y = (float)(ClusteredMatrix[i][j][SecondDimension]);

                        switch (i)
                        {
                            case 0:
                                p2.Color = Color.Red;
                                br3.Color = Color.Red;
                                break;
                            case 1:
                                p2.Color = Color.Blue;
                                br3.Color = Color.Blue;
                                break;
                            case 2:
                                p2.Color = Color.Green;
                                br3.Color = Color.Green;
                                break;
                            case 3:
                                p2.Color = Color.Brown;
                                br3.Color = Color.Brown;
                                break;
                            case 4:
                                p2.Color = Color.Cyan;
                                br3.Color = Color.Cyan;
                                break;
                            case 5:
                                p2.Color = Color.Gold;
                                br3.Color = Color.Gold;
                                break;
                            case 6:
                                p2.Color = Color.Magenta;
                                br3.Color = Color.Magenta;
                                break;
                            case 7:
                                p2.Color = Color.Pink;
                                br3.Color = Color.Pink;
                                break;
                            case 8:
                                p2.Color = Color.Ivory;
                                br3.Color = Color.Ivory;
                                break;
                            case 9:
                                p2.Color = Color.Lime;
                                br3.Color = Color.Lime;
                                break;
                            case 10:
                                p2.Color = Color.Orange;
                                br3.Color = Color.Orange;
                                break;
                            default:
                                p2.Color = Color.Red;
                                break;
                        }

                        g.DrawEllipse(p2, (int)(scaleX * x + b.Width / 2), (int)(b.Height / 2 - scaleY * y), 8, 8);
                        g.FillEllipse(br3, (int)(scaleX * x + b.Width / 2), (int)(b.Height / 2 - scaleY * y), 8, 8);
                    }
                }
            }

            // Ilosc iteracji
            for (int z = 0; z < iterationsNumber; z++)
            {
                // Ilosc klastrow z nadrzednej iteracji (za 1 razem jest to liczba klastrow z tzw Basic Iteracji)
                for (int m = 0; m < Clustering.ClustersBasicQuantity+Clustering.ClustersInIterations[z]; m++)
                {
                    // Ilosc klastrow stworzonych dla kazdego klastra z iteracji wyzszej
                    for (int i = Clustering.ClustersBasicQuantity + Clustering.ClustersInIterations[z] + Clustering.ClustersInIterations[z+1] * m; i < Clustering.ClustersBasicQuantity + Clustering.ClustersInIterations[z + 1] + Clustering.ClustersInIterations[z+1] * m; i++)
                    {
                        for (int j = 0; j < ClusteredMatrix[i].Length; j++)
                        {
                            for (int k = 0; k < ClusteredMatrix[i][j].Length; k++)
                            {

                                float x = (float)(ClusteredMatrix[i][j][FirstDimension]);
                                float y = (float)(ClusteredMatrix[i][j][SecondDimension]);

                                br3.Color = Color.White;
                                g.FillEllipse(br3, (int)(scaleX * x + b.Width / 2), (int)(b.Height / 2 - scaleY * y), 8, 8);

                                switch (m)
                                {
                                    case 0:
                                        p2.Color = Color.Red;
                                        br3.Color = Color.Red;
                                        break;
                                    case 1:
                                        p2.Color = Color.Blue;
                                        br3.Color = Color.Blue;
                                        break;
                                    case 2:
                                        p2.Color = Color.Green;
                                        br3.Color = Color.Green;
                                        break;
                                    case 3:
                                        p2.Color = Color.Brown;
                                        br3.Color = Color.Brown;
                                        break;
                                    case 4:
                                        p2.Color = Color.Cyan;
                                        br3.Color = Color.Cyan;
                                        break;
                                    case 5:
                                        p2.Color = Color.Gold;
                                        br3.Color = Color.Gold;
                                        break;
                                    case 6:
                                        p2.Color = Color.Magenta;
                                        br3.Color = Color.Magenta;
                                        break;
                                    case 7:
                                        p2.Color = Color.Pink;
                                        br3.Color = Color.Pink;
                                        break;
                                    case 8:
                                        p2.Color = Color.Ivory;
                                        br3.Color = Color.Ivory;
                                        break;
                                    case 9:
                                        p2.Color = Color.Lime;
                                        br3.Color = Color.Lime;
                                        break;
                                    case 10:
                                        p2.Color = Color.Orange;
                                        br3.Color = Color.Orange;
                                        break;
                                    default:
                                        p2.Color = Color.Red;
                                        break;
                                }

                            
                                if (i == Clustering.ClustersBasicQuantity + Clustering.ClustersInIterations[z] + 2 * m)
                                {
                                    g.DrawRectangle(p2, (int)(scaleX * x + b.Width / 2), (int)(b.Height / 2 - scaleY * y), 8, 8);
                                    g.FillRectangle(br3,(int)(scaleX * x + b.Width / 2),(int)(b.Height / 2 - scaleY * y), 8, 8);
                                }
                                else
                                {
                                    g.DrawEllipse(p2, (int)(scaleX * x + b.Width / 2), (int)(b.Height / 2 - scaleY * y), 8, 8);
                                    g.FillEllipse(br3, (int)(scaleX * x + b.Width / 2), (int)(b.Height / 2 - scaleY * y), 8, 8);
                                }
                            }
                        }
                    }
                }
            }
            int r = DataSet.CountElements(@"..\..\..\");
            r++;
            b.Save(PathToGraph + r + ".png");
        }

        // Funkcja rysowania z nieco innym przedstawieniem dziedziczenia po klastrach z iteracji nadrzednej
        public static void DrawClusterGraph3(double[][] BaseMatrix, double[][][] ClusteredMatrix, string PathToGraph, int GraphWidth, int GraphHeight, int FirstDimension, int SecondDimension, int iterationsNumber)
        {
            Bitmap b = new Bitmap(GraphWidth, GraphHeight);
            Graphics g = Graphics.FromImage(b);
            Brush br = new SolidBrush(Color.White);

            g.FillRectangle(br, 0, 0, b.Width, b.Height);

            Pen p1 = new Pen(Color.Black, 5);
            g.DrawLine(p1, 0, b.Height / 2, b.Width, b.Height / 2);
            g.DrawLine(p1, b.Width / 2, 0, b.Width / 2, b.Height);

            double rangeX = Math.Ceiling(SearchRange(BaseMatrix, FirstDimension));
            double rangeY = Math.Ceiling(SearchRange(BaseMatrix, SecondDimension));
            double Unit = 30;

            double scaleX = b.Width / Unit;
            double scaleY = b.Height / Unit;

            double rangePerUnitX = rangeX / Unit;
            double rangePerUnitY = rangeY / Unit;

            Pen p11 = new Pen(Color.Black);
            Pen p2 = new Pen(Color.Red);

            int dx = (int)scaleX;
            int dy = (int)scaleY;

            Font f1 = new Font(FontFamily.GenericSansSerif, 12);
            Brush br2 = new SolidBrush(Color.Blue);
            SolidBrush br3 = new SolidBrush(Color.Red);


            double t1 = rangeX;
            double t2 = rangeY;

            for (int OY = 0; OY < b.Height; OY += dy)
            {
                g.DrawLine(p11, 0, OY, b.Width, OY);

                string s = (t1).ToString("F" + 1);
                g.DrawString(s, f1, br2, b.Width / 2 + 1, OY);
                t1 -= 2 * rangePerUnitX;
            }

            t1 = -rangeX;

            for (int OX = 0; OX < b.Width; OX += dx)
            {
                g.DrawLine(p11, OX, 0, OX, b.Height);

                string s = (t1).ToString("F" + 1);
                g.DrawString(s, f1, br2, OX, (b.Height / 2) - 1);
                t1 += 2 * rangePerUnitX;
            }

            scaleX /= 2 * rangePerUnitX;
            scaleY /= 2 * rangePerUnitX;


            for (int i = 0; i < Clustering.ClustersBasicQuantity; i++)
            {
                for (int j = 0; j < ClusteredMatrix[i].Length; j++)
                {
                    for (int k = 0; k < ClusteredMatrix[i][j].Length; k++)
                    {

                        float x = (float)(ClusteredMatrix[i][j][FirstDimension]);
                        float y = (float)(ClusteredMatrix[i][j][SecondDimension]);

                        switch (i)
                        {
                            case 0:
                                p2.Color = Color.Red;
                                break;
                            case 1:
                                p2.Color = Color.Blue;
                                break;
                            case 2:
                                p2.Color = Color.Green;
                                break;
                            case 3:
                                p2.Color = Color.Brown;
                                break;
                            case 4:
                                p2.Color = Color.Cyan;
                                break;
                            case 5:
                                p2.Color = Color.Gold;
                                break;
                            case 6:
                                p2.Color = Color.Magenta;
                                break;
                            case 7:
                                p2.Color = Color.Pink;
                                break;
                            case 8:
                                p2.Color = Color.Ivory;
                                break;
                            case 9:
                                p2.Color = Color.Lime;
                                break;
                            case 10:
                                p2.Color = Color.Orange;
                                break;
                            default:
                                p2.Color = Color.Red;
                                break;
                        }

                        g.DrawEllipse(p2, (int)(scaleX * x + b.Width / 2), (int)(b.Height / 2 - scaleY * y), 8, 8);
                    }
                }
            }

            for (int z = 0; z < iterationsNumber; z++)
            {
                for (int m = 0; m < Clustering.ClustersBasicQuantity + Clustering.ClustersInIterations[z]; m++)
                {
                    for (int i = Clustering.ClustersBasicQuantity + Clustering.ClustersInIterations[z] + Clustering.ClustersInIterations[z+1] * m; i < Clustering.ClustersBasicQuantity + Clustering.ClustersInIterations[z + 1] + Clustering.ClustersInIterations[z+1] * m; i++)
                    {
                        for (int j = 0; j < ClusteredMatrix[i].Length; j++)
                        {
                            for (int k = 0; k < ClusteredMatrix[i][j].Length; k++)
                            {

                                float x = (float)(ClusteredMatrix[i][j][FirstDimension]);
                                float y = (float)(ClusteredMatrix[i][j][SecondDimension]);

                                if (i == Clustering.ClustersBasicQuantity + Clustering.ClustersInIterations[z] + 2 * m)
                                {
                                    p2.Color = Color.Orange;
                                }
                                if (i == (Clustering.ClustersBasicQuantity + Clustering.ClustersInIterations[z] + 2 * m)+1)
                                {
                                    p2.Color = Color.Lime;
                                }
                                if (i == (Clustering.ClustersBasicQuantity + Clustering.ClustersInIterations[z] + 2 * m) + 2)
                                {
                                    p2.Color = Color.Ivory;
                                }
                                if (i == (Clustering.ClustersBasicQuantity + Clustering.ClustersInIterations[z] + 2 * m) + 3)
                                {
                                    p2.Color = Color.Pink;
                                }
                            
                                g.DrawEllipse(p2, (int)(scaleX * x + b.Width / 2), (int)(b.Height / 2 - scaleY * y), 8-(z+2), 8-(z+2));

                            }
                        }
                    }
                }
            }
            int r = DataSet.CountElements(@"..\..\..\");
            r++;
            b.Save(PathToGraph + r + ".png");
        }
    }
}
