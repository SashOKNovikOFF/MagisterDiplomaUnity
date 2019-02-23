using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace DijkstraAlgorithm
{
    /// <summary>
    ///  Класс, реализующий алгоритм Дейкстры.
    /// </summary>
    public class Dijkstra
    {
        /// <summary>
        /// Граф в виде матрицы M размера N x N, где N - число вершин графа.
        /// Элемент матрицы M(i, j) - вес ребра графа, соединяющего вершины i и j.
        /// M(i, j) характеризует расстояние от вершины i до j. Вес ребра M(i, i) равен нулю.
        /// Расстояние считается всегда положительным.
        /// </summary>
        private int[,] graph;
        /// <summary>
        /// Количество вершин графа.
        /// </summary>
        private int verticesCount;
        /// <summary>
        /// Массив минимальных расстояний от заданной вершины до любой другой вершины.
        /// Представляется в виде [L(0), L(1), ..., L(N)], где L(i) - расстояние от заданной вершины до вершины i.
        /// </summary>
        private int[] minimum_distance;
        /// <summary>
        /// Лист из индексов вершин, по которым можно построить минимальный путь из вершины i до вершины j.
        /// </summary>
        private List<int> VerticesPath;

        /// <summary>
        /// Определить вершину, для которой не рассчитывалось минимальное расстояние до других вершин.
        /// </summary>
        /// <param name="distance">
        /// Массив минимальных расстояний от заданной вершины до любой другой вершины.
        /// </param>
        /// <param name="shortestPathTreeSet">
        /// Массив вершин, по которым уже определялись минимальные расстояния до других вершин.
        /// </param>
        /// <returns>
        /// Индекс искомой вершины.
        /// </returns>
        private int MinimumDistance(int[] distance, bool[] shortestPathTreeSet)
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

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="initGraph">
        /// Исходный граф с весами.
        /// </param>
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
            VerticesPath = new List<int>();
        }

        /// <summary>
        /// Получить минимальное расстояние от заданной вершины до любой другой вершины.
        /// </summary>
        /// <param name="vertice">
        /// Вершина, до которой необходимо рассчитать минимальное расстояние.
        /// </param>
        /// <returns>
        /// Минимальное расстояние L(vertice) от заданной вершины.
        /// </returns>
        public int GetMinimumDistance(int vertice)
        {
            if (vertice < 0 || vertice >= verticesCount)
                throw new System.ArgumentOutOfRangeException("Выбрана вершина вне диапазона имеющихся вершин.");

            return minimum_distance[vertice];
        }

        /// <summary>
        /// Получить лист из индексов вершин, по которым можно построить минимальный путь из вершины i до вершины j.
        /// </summary>
        /// <returns>
        /// Лист из индексов вершин, по которым можно построить минимальный путь из вершины i до вершины j.
        /// </returns>
        public List<int> GetVerticesPath()
        {
            return VerticesPath;
        }

        /// <summary>
        /// Рассчитать минимальные расстояния до всех вершин при помощи алгоритма Дейкстры.
        /// </summary>
        /// <param name="source">
        /// Вершина графа, с которой рассчитывается минимальное расстояние до любой другой.
        /// </param>
        /// <param name="endVertice">
        /// Вершина графа, до которой строится путь с минимальным расстоянием.
        /// </param>
        public void CallDijkstraAlgorithm(int source, int endVertice)
        {
            if (source < 0 || source >= verticesCount || endVertice < 0 || endVertice >= verticesCount)
                throw new System.ArgumentOutOfRangeException("Выбрана вершина вне диапазона имеющихся вершин.");

            int[] distance = new int[verticesCount];
            bool[] shortestPathTreeSet = new bool[verticesCount];

            for (int i = 0; i < verticesCount; ++i)
            {
                distance[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            distance[source] = 0;

            bool pathFlag = true;
            for (int count = 0; count < verticesCount - 1; ++count)
            {
                int u = MinimumDistance(distance, shortestPathTreeSet);

                if ((u != endVertice) && pathFlag)
                    VerticesPath.Add(u);
                else if (pathFlag)
                {
                    VerticesPath.Add(endVertice);
                    pathFlag = false;
                }

                shortestPathTreeSet[u] = true;

                for (int v = 0; v < verticesCount; ++v)
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                        distance[v] = distance[u] + graph[u, v];
            }

            minimum_distance = distance;
        }

    }
}