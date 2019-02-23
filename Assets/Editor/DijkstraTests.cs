using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

// Пользовательские библиотеки
using DijkstraAlgorithm;

public class DijkstraTests
{
    [Test]
    public void CheckOneVertice()
    {
        int[,] graph = { { 0 } };
        DijkstraAlgorithm.Dijkstra AlgorithmClass = new DijkstraAlgorithm.Dijkstra(graph);
        AlgorithmClass.CallDijkstraAlgorithm(0);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(0), 0);
    }

    [Test]
    public void CheckNegativeDistance()
    {
        int[,] graph = { { 0, -1 }, { -1, 0 } };
        Assert.Throws<System.ArgumentException>(() => new DijkstraAlgorithm.Dijkstra(graph));
    }

    [Test]
    public void CheckDiagonalElements()
    {
        int[,] graph = { { 0, -1 }, { -1, -1 } };
        Assert.Throws<System.ArgumentException>(() => new DijkstraAlgorithm.Dijkstra(graph));
    }

    [Test]
    public void CheckSquareGraph()
    {
        int[,] graph = { { 0, -1 } };
        Assert.Throws<System.ArgumentException>(() => new DijkstraAlgorithm.Dijkstra(graph));
    }

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
        AlgorithmClass.CallDijkstraAlgorithm(0);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(0), 0);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(1), 7);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(2), 9);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(3), 20);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(4), 20);
        Assert.AreEqual(AlgorithmClass.GetMinimumDistance(5), 11);
    }
}
