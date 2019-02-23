using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using DijkstraAlgorithm;

public static class DebugLog
{
    public static void Write(object str)
    {
        using (var file = new StreamWriter(@"D:\DebugLog.txt", true))
        {
            file.WriteLine(str);
        }
    }

    public static void Write(List<string> strs)
    {
        using (var file = new StreamWriter(@"D:\DebugLog.txt", true))
        {
            file.WriteLine("*******************************");
            foreach (var str in strs)
                file.WriteLine(str);
        }
    }
}

namespace TGS
{
    public class CheckCoordinates : MonoBehaviour
    {
        TerrainGridSystem tgs;

        // Use this for initialization
        void Start()
        {
            // Исходные данные по сетке
            tgs = TerrainGridSystem.instance;
            List<Cell> myCells = tgs.cells;
            int Nx = tgs.rowCount;
            int Ny = tgs.columnCount;

            // Задать исходную высоту
            Cell zeroCell = myCells[0];
            Vector3 zeroCoord = tgs.CellGetPosition(0);

            // Заполнить весы графа
            int[,] dijk_grid = new int[Nx * Ny, Nx * Ny];
            for (int i = 0; i < Nx; i++)
                for (int j = 0; j < Ny; j++)
                    if (i != j)
                    {
                        Vector3 centerCoord = tgs.CellGetPosition(myCells[j + Ny * i]);

                        if (i != 0)
                        {
                            Vector3 leftCoord = tgs.CellGetPosition(myCells[j + Ny * (i - 1)]);
                            dijk_grid[j + Ny * i, j + Ny * (i - 1)] = (int)Vector3.Distance(centerCoord, leftCoord);
                        }

                        if (i != (Nx - 1))
                        {
                            Vector3 rightCoord = tgs.CellGetPosition(myCells[j + Ny * (i + 1)]);
                            dijk_grid[j + Ny * i, j + Ny * (i + 1)] = (int)Vector3.Distance(centerCoord, rightCoord);
                        }

                        if (j != (Ny - 1))
                        {
                            Vector3 upCoord = tgs.CellGetPosition(myCells[(j + 1) + Ny * i]);
                            dijk_grid[j + Ny * i, (j + 1) + Ny * i] = (int)Vector3.Distance(centerCoord, upCoord);
                        }

                        if (j != 0)
                        {
                            Vector3 downCoord = tgs.CellGetPosition(myCells[(j - 1) + Ny * i]);
                            dijk_grid[j + Ny * i, (j - 1) + Ny * i] = (int)Vector3.Distance(centerCoord, downCoord);
                        }
                    }

            //using (var file = new StreamWriter(@"D:\Dijk_grid.txt", true))
            //{
            //    for (int i = 0; i < Nx * Ny; i++)
            //    {
            //        string str = "";
            //        for (int j = 0; j < Nx * Ny; j++)
            //            str += dijk_grid[i, j].ToString() + "\t";
            //        file.WriteLine(str);
            //    }
            //}


        }

        private void WriteTGSCoordsToFile()
        {
            List<Cell> myCells = tgs.cells;
            int N = tgs.columnCount;
            int M = tgs.rowCount;

            List<string> obj = new List<string>();
            for (int i = 0; i < N; i++)
            {
                string str = "";
                for (int j = 0; j < M; j++)
                {
                    Cell exampleCell = myCells[j + N * i];
                    Vector3 coords = tgs.CellGetPosition(exampleCell);

                    str += coords.y.ToString() + "\t";
                }
                str += "\n";
                obj.Add(str);
            }

            DebugLog.Write(obj);
        }

        private void WriteHeightsToFile()
        {
            int w = tgs.terrain.terrainData.heightmapWidth;
            int h = tgs.terrain.terrainData.heightmapHeight;
            float[,] heights = tgs.terrain.terrainData.GetHeights(0, 0, w, h);

            List<string> obj = new List<string>();
            for (int i = 0; i < w; i++)
            {
                string str = "";
                for (int j = 0; j < h; j++)
                    str += heights[i, j].ToString() + "\t";
                str += "\n";
                obj.Add(str);
            }

            DebugLog.Write(obj);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}