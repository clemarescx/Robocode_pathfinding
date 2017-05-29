using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PG4500_2017_Exam2;
using Santom;

namespace RobocodeTest{
	[TestClass]
	public class GraphTests{
		private readonly int[,] _grid = new int[3, 3]{
			{ 1, 0, 0 },
			{ 1, 1, 0 },
			{ 0, 0, 0 }
		};

		private readonly Graph _graph;
		private readonly int[,] _realGrid = ExamSpecs.Grid;

		public GraphTests(){ _graph = new Graph(_realGrid); }

		[TestMethod]
		public void CreateCorrectAmountOfNodesTest(){
			var endCount = _graph.Grid.Length;
			Assert.IsTrue(endCount == _realGrid.Length);
		}

		[TestMethod]
		public void MaximumCoordinatesTest(){
			var allWithinBounds = true;
			foreach(var node in _graph.Grid){
				var nodeWithinBounds = node.CenterX < 800 && node.CenterX >= 0 &&
				                       node.CenterY < 600 && node.CenterY >= 0;
				if(!nodeWithinBounds){
					allWithinBounds = false;
					break;
				}
			}
			
			Assert.IsTrue(allWithinBounds);
		}

		[TestMethod]
		public void MaximumPaintOffsetTest(){
			var updatedList = new List<Node>();

			foreach(var node in _graph.Grid){
				updatedList.Add(new Node(new Vector(node.CenterX, node.CenterY), false));
			}

			updatedList.ForEach(
				node => {
					Assert.IsTrue(node.CenterX < 800);
					Assert.IsTrue(node.CenterX >= 0);
					Assert.IsTrue(node.CenterY < 600);
					Assert.IsTrue(node.CenterY >= 0);
				});
		}

		[TestMethod]
		public void SetNodesGivesCorrectCoordinates(){
			Graph smallGraph = new Graph(_grid);

			Assert.IsTrue(smallGraph.Grid[2,0].IsWall);
			Assert.IsTrue(smallGraph.Grid[1,0].IsWall);
			Assert.IsTrue(smallGraph.Grid[1,1].IsWall);
			Assert.IsFalse(smallGraph.Grid[0,1].IsWall);
		}

		[TestMethod]
		public void FirstElementCoordinatesTest(){
			var bottomLeft = _graph.Grid[0, 0];
			var topRight = _graph.Grid[11, 15];
			Assert.IsTrue(bottomLeft.CenterX == ExamSpecs.NodeRadius);
			Assert.IsTrue(bottomLeft.CenterY == ExamSpecs.NodeRadius);
			Assert.IsTrue(topRight.CenterX == 775);
			Assert.IsTrue(topRight.CenterY == 575);
		}

		[TestMethod]
		public void WallPositionTest(){
			var node_0_1 = _graph.Grid[11, 0];
			var node_0_2 = _graph.Grid[10, 0];
			var node_0_3 = _graph.Grid[9, 0];
			var node_0_4 = _graph.Grid[8, 0];
			Assert.IsFalse(node_0_1.IsWall);
			Assert.IsFalse(node_0_2.IsWall);
			Assert.IsTrue(node_0_3.IsWall);
			Assert.IsTrue(node_0_4.IsWall);
		}

		[TestMethod]
		public void BfCoordsConversionTest(){
			var topLeft = new Vector(12, 573);
			var bottomLeft = new Vector(12, 2);
			var topRight = new Vector(799, 599);
			var bottomRight = new Vector(750, 4);

			var origin = _graph.BattlefieldToGridCoords(new Vector(0, 0));
			var screen = _graph.BattlefieldToGridCoords(new Vector(800, 600));
			var aberration = _graph.BattlefieldToGridCoords(new Vector(-200, 1000));
			var grid = _graph.Grid;

			var _topLeft = _graph.BattlefieldToGridCoords(topLeft);
			var _bottomLeft = _graph.BattlefieldToGridCoords(bottomLeft);
			var _topright = _graph.BattlefieldToGridCoords(topRight);
			var _bottomright = _graph.BattlefieldToGridCoords(bottomRight);
			Assert.IsTrue(grid[11, 0] == grid[(int)_topLeft.X, (int)_topLeft.Y]);
			Assert.IsTrue(grid[0, 0] == grid[(int)_bottomLeft.X, (int)_bottomLeft.Y]);
			Assert.IsTrue(grid[11, 15] == grid[(int)_topright.X, (int)_topright.Y]);
			Assert.IsTrue(grid[0, 15] == grid[(int)_bottomright.X, (int)_bottomright.Y]);
			Assert.IsTrue(grid[0, 0] == grid[(int)origin.X, (int)origin.Y]);
			Assert.IsTrue(grid[11, 15] == grid[(int)screen.X, (int)screen.Y]);
			Assert.IsTrue(grid[11, 0] == grid[(int)aberration.X, (int)aberration.Y]);
		}

		[TestMethod]
		public void GridBoundsCheckTest(){
			Assert.IsTrue(_graph.IsWithinGrid(0, 0));
			Assert.IsTrue(_graph.IsWithinGrid(2, 3));
			Assert.IsTrue(_graph.IsWithinGrid(-0, -0));
			Assert.IsTrue(_graph.IsWithinGrid(11, 15));

			Assert.IsFalse(_graph.IsWithinGrid(-2, 3));
			Assert.IsFalse(_graph.IsWithinGrid(-2, -3));
			Assert.IsFalse(_graph.IsWithinGrid(2, -3));
			Assert.IsFalse(_graph.IsWithinGrid(12, -3));
			Assert.IsFalse(_graph.IsWithinGrid(12, 3));
			Assert.IsFalse(_graph.IsWithinGrid(12, 16));
			Assert.IsFalse(_graph.IsWithinGrid(-12, 16));
		}

		[TestMethod]
		public void GetNeighboursCountTest(){
			var topLeft = _graph.Grid[11, 0];
			var bottomLeft = _graph.Grid[0, 0];
			var topRight = _graph.Grid[11, 15];
			var bottomRight = _graph.Grid[0, 15];
			List<Node> testNodes = new List<Node>(){
				topLeft,
				bottomLeft,
				topRight,
				bottomRight
			};
			List<Node> neighbours;
			foreach(var corner in testNodes){
				neighbours = _graph.GetNeighbours(corner);
				Assert.IsTrue(neighbours.Count == 3);
			}

			var topEdge = _graph.Grid[11, 1];
			var bottomEdge = _graph.Grid[1, 0];
			var rightEdge = _graph.Grid[10, 15];
			var leftEdge = _graph.Grid[0, 14];
			testNodes.Clear();
			testNodes = new List<Node>(){ topEdge, bottomEdge, rightEdge, leftEdge };
			foreach(var edge in testNodes){
				neighbours = _graph.GetNeighbours(edge);
				Assert.IsTrue(neighbours.Count == 5);
			}

			var center = _graph.Grid[6, 8];
			neighbours = _graph.GetNeighbours(center);
			Assert.IsTrue(neighbours.Count == 8);
		}
	}
}