using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace DijkstraAlgorithm
{
    public class Dijkstra
    {
        private int[,] graph;
        private int verticesCount;
        private int[] minimum_distance;

        public Dijkstra(int[,] initGraph)
        {
            verticesCount = initGraph.GetLength(0);

            for (int vertice = 0; vertice < verticesCount; vertice++)
                if (initGraph[vertice, vertice] != 0)
                    throw new System.ArgumentException("Диагональные вершины графа должны быть равны нулю.");

            for (int row = 0; row < verticesCount; row++)
                for (int column = 0; column < verticesCount; column++)
                    if (row != column && initGraph[row, column] < 0)
                        throw new System.ArgumentException("Весы графа должны быть больше нуля.");

            if (initGraph.GetLength(0) != initGraph.GetLength(1))
                throw new System.ArgumentException("Матрица с весами графа должна быть квадратной.");

            graph = initGraph;
            minimum_distance = new int[verticesCount];
        }
        public int GetMinimumDistance(int vertice)
        {
            return minimum_distance[vertice];
        }
        private int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
        {
            int min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < verticesCount; ++v)
            {
                if (shortestPathTreeSet[v] == false && distance[v] <= min)
                {
                    min = distance[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }
        public void CallDijkstraAlgorithm(int source)
        {
            int[] distance = new int[verticesCount];
            bool[] shortestPathTreeSet = new bool[verticesCount];

            for (int i = 0; i < verticesCount; ++i)
            {
                distance[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            distance[source] = 0;

            for (int count = 0; count < verticesCount - 1; ++count)
            {
                int u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
                shortestPathTreeSet[u] = true;

                for (int v = 0; v < verticesCount; ++v)
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                        distance[v] = distance[u] + graph[u, v];
            }

            minimum_distance = distance;
        }

    }
}