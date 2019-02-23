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
            int Nz = tgs.columnCount;

            // Задать исходную высоту
            Cell zeroCell = myCells[0];
            Vector3 zeroCoord = tgs.CellGetPosition(0);

            using (var file = new StreamWriter(@"D:\Vec_grid.txt", true))
            {
                for (int row = 0; row < Nx; row++)
                {
                    for (int column = 0; column < Nz; column++)
                    {
                        Vector3 centerCoord = tgs.CellGetPosition(myCells[column + Nz * row]);
                        String str = centerCoord.x.ToString() + "\t" + centerCoord.z.ToString() + "\t" + centerCoord.y.ToString();
                        file.WriteLine(str);
                    }
                    file.WriteLine("\n");
                }
            }

            // Заполнить весы графа
            double[,] dijk_grid = new double[Nx * Nz, Nx * Nz];
            for (int row = 0; row < Nx; row++)
                for (int column = 0; column < Nz; column++)
                {
                    int currentIndex = column + Nz * row;
                    Vector3 centerCoord = tgs.CellGetPosition(myCells[currentIndex]);

                    if (row != 0)
                    {
                        int leftIndex = column + Nz * (row - 1);
                        Vector3 leftCoord = tgs.CellGetPosition(myCells[leftIndex]);
                        dijk_grid[currentIndex, leftIndex] = Vector3.Distance(centerCoord, leftCoord);
                    }

                    if (row != (Nx - 1))
                    {
                        int rightIndex = column + Nz * (row + 1);
                        Vector3 rightCoord = tgs.CellGetPosition(myCells[rightIndex]);
                        dijk_grid[currentIndex, rightIndex] = Vector3.Distance(centerCoord, rightCoord);
                    }

                    if (column != (Nz - 1))
                    {
                        int upIndex = (column + 1) + Nz * row;
                        Vector3 upCoord = tgs.CellGetPosition(myCells[upIndex]);
                        dijk_grid[currentIndex, upIndex] = Vector3.Distance(centerCoord, upCoord);
                    }

                    if (column != 0)
                    {
                        int downIndex = (column - 1) + Nz * row;
                        Vector3 downCoord = tgs.CellGetPosition(myCells[downIndex]);
                        dijk_grid[currentIndex, downIndex] = Vector3.Distance(centerCoord, downCoord);
                    }
                        
                    // ----------------

                    if ((row != 0) && (column != 0))
                    {
                        int leftDownIndex = (column - 1) + Nz * (row - 1);
                        Vector3 leftDownCoord = tgs.CellGetPosition(myCells[leftDownIndex]);
                        dijk_grid[currentIndex, leftDownIndex] = Vector3.Distance(centerCoord, leftDownCoord);
                    }

                    if ((row != (Nx - 1)) && (column != 0))
                    {
                        int rightDownIndex = (column - 1) + Nz * (row + 1);
                        Vector3 rightDownCoord = tgs.CellGetPosition(myCells[rightDownIndex]);
                        dijk_grid[currentIndex, rightDownIndex] = Vector3.Distance(centerCoord, rightDownCoord);
                    }

                    if ((row != 0) && (column != (Nz - 1)))
                    {
                        int leftUpIndex = (column + 1) + Nz * (row - 1);
                        Vector3 leftUpCoord = tgs.CellGetPosition(myCells[leftUpIndex]);
                        dijk_grid[currentIndex, leftUpIndex] = Vector3.Distance(centerCoord, leftUpCoord);
                    }

                    if ((row != (Nx - 1)) && (column != (Nz - 1)))
                    {
                        int rightUpIndex = (column + 1) + Nz * (row + 1);
                        Vector3 rightUpCoord = tgs.CellGetPosition(myCells[rightUpIndex]);
                        dijk_grid[currentIndex, rightUpIndex] = Vector3.Distance(centerCoord, rightUpCoord);
                    }
                }

            DijkstraAlgorithm.Dijkstra AlgorithmClass = new DijkstraAlgorithm.Dijkstra(dijk_grid);
            AlgorithmClass.CallDijkstraAlgorithm(0, Nx * Nz - 1);
            double distance = AlgorithmClass.GetMinimumDistance(Nx * Nz - 1);
            List<int> a = AlgorithmClass.GetVerticesPath();

            using (var file = new StreamWriter(@"D:\Dijk_grid.txt", true))
            {
                for (int row = 0; row < Nx * Nz; row++)
                {
                    for (int column = 0; column < Nx * Nz; column++)
                    {
                        String str = dijk_grid[row, column] + "\t";
                        file.Write(str);
                    }
                    file.WriteLine("\n");
                }
            }
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