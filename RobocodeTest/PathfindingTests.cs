using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PG4500_2017_Exam2;
using Santom;

namespace RobocodeTest{
	[TestClass]
	public class PathfindingTests{

		private static readonly int[,] _grid = new int[3, 3]{
			{ 1, 0, 0 },
			{ 1, 1, 0 },
			{ 0, 0, 0 }
		};

		private readonly Graph _graph = new Graph(ExamSpecs.Grid);
		private readonly Graph _smallGraph = new Graph(_grid);

		private readonly Pathfinding _pathfinder;

		public PathfindingTests(){
			_pathfinder = new Pathfinding(_graph);
		}

		[TestMethod]
		public void StraightLightCountTest(){
			Node bottomLeft = _graph.Grid[0, 0];
			Node bottomRight = _graph.Grid[0, 15];
			var path = _pathfinder.AStar(bottomLeft, bottomRight);
			Console.WriteLine("Path count : {0}", path.Count);
			Assert.IsTrue(path.Count == 15);
		}

		[TestMethod]
		public void CircumventWallCountTest(){
			var _smallPF = new Pathfinding(_smallGraph);
			var bottomLeft = _smallGraph.Grid[0, 0];
			var topMiddle = _smallGraph.Grid[2,1];
			var pathAB = _smallPF.AStar(bottomLeft, topMiddle);
			var pathBA = _smallPF.AStar(topMiddle, bottomLeft);
			Assert.IsTrue(pathAB.Count == 3);
			Assert.IsTrue(pathBA.Count == 3);

		}
	}
}