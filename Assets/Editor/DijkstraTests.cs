using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

// Пользовательские библиотеки
using DijkstraAlgorithm;

public class DijkstraTests
{
    /// <summary>
    /// Проверить, что расстояние от 1-ой вершины до неё самой определяется корректно.
    /// </summary>
    [Test]
    public void CheckOneVertice()
    {
        int[,] graph = { { 0 } };
        DijkstraAlgorithm.Dijkstra AlgorithmClass = new DijkstraAlgorithm.Dijkstra(graph);
        AlgorithmClass.CallDijkstraAlgorithm(0, 0);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(0), 0);
        Assert.AreEqual(AlgorithmClass.GetVerticesPath().Count, 0);
    }

    /// <summary>
    /// Проверить, что корректно обрабатываются отрицательные весы графа.
    /// </summary>
    [Test]
    public void CheckNegativeDistance()
    {
        int[,] graph = { { 0, -1 }, { -1, 0 } };
        Assert.Throws<System.ArgumentException>(() => new DijkstraAlgorithm.Dijkstra(graph));
    }

    /// <summary>
    /// Проверить, что корректно обрабатываются ненулевые диагональные элементы графа.
    /// </summary>
    [Test]
    public void CheckDiagonalElements()
    {
        int[,] graph = { { 0, -1 }, { -1, -1 } };
        Assert.Throws<System.ArgumentException>(() => new DijkstraAlgorithm.Dijkstra(graph));
    }

    /// <summary>
    /// Проверить, что корректно обрабатываются неквадратные матрицы-графы.
    /// </summary>
    [Test]
    public void CheckSquareGraph()
    {
        int[,] graph = { { 0, -1 } };
        Assert.Throws<System.ArgumentException>(() => new DijkstraAlgorithm.Dijkstra(graph));
    }

    /// <summary>
    /// Проверить, что корректно обрабатываются неправильно выбранные вершины.
    /// </summary>
    [Test]
    public void CheckOutOfRangeVertices()
    {
        int[,] graph = { { 0, 1 },
                         { 1, 0 } };
        DijkstraAlgorithm.Dijkstra AlgorithmClass = new DijkstraAlgorithm.Dijkstra(graph);

        Assert.Throws<System.ArgumentOutOfRangeException>(() => AlgorithmClass.GetMinimumDistance(-1));
        Assert.Throws<System.ArgumentOutOfRangeException>(() => AlgorithmClass.GetMinimumDistance(2));

        Assert.Throws<System.ArgumentOutOfRangeException>(() => AlgorithmClass.CallDijkstraAlgorithm(-1, 0));
        Assert.Throws<System.ArgumentOutOfRangeException>(() => AlgorithmClass.CallDijkstraAlgorithm(2, 0));
        Assert.Throws<System.ArgumentOutOfRangeException>(() => AlgorithmClass.CallDijkstraAlgorithm(0, -1));
        Assert.Throws<System.ArgumentOutOfRangeException>(() => AlgorithmClass.CallDijkstraAlgorithm(0, 2));
    }

    /// <summary>
    /// Проверить работу алгоритма Дейкстры на примере из Википедии.
    /// </summary>
    [Test]
    public void TestWikipediaExample()
    {
        int[,] graph = { { 0,  7,  9,  0,  0, 14 },
                         { 7,  0,  10, 15, 0, 0 },
                         { 9,  10, 0,  11, 0, 2 },
                         { 0,  15, 11, 0,  6, 0 },
                         { 0,  0,  0,  6,  0, 9 },
                         { 14, 0,  2,  0,  9, 0 } };
        DijkstraAlgorithm.Dijkstra AlgorithmClass = new DijkstraAlgorithm.Dijkstra(graph);
        AlgorithmClass.CallDijkstraAlgorithm(0, 5);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(0), 0);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(1), 7);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(2), 9);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(3), 20);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(4), 20);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(5), 11);

        List<int> VerticesPath = new List<int> { 0, 1, 2, 5 };
        Assert.AreEqual(AlgorithmClass.GetVerticesPath(), VerticesPath);
    }
}
