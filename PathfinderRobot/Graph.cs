using System;
using System.Collections.Generic;
using PG4500_2017_Exam2;
using Santom;

	public class Graph{
		public Node[,] Grid{ get; set; }

		public Graph(int[,] walkMask){
			CreateGrid(walkMask);
		}

		private void CreateGrid(int[,] walkMask){
			Grid = new Node[walkMask.GetLength(0), walkMask.GetLength(1)];
			// the grid from the assignment's description must be flipped upside down
			// since robocode's Y origin is at the bottom left, not the top left
			var heightInversion = walkMask.GetLength(0) - 1;
			for(int i = heightInversion; i >= 0; i--){
				for(int j = 0; j < walkMask.GetLength(1); j++){
					var x = (j * ExamSpecs.NodeDiameter) + ExamSpecs.NodeRadius;
					var y = (i * ExamSpecs.NodeDiameter) + ExamSpecs.NodeRadius;
					Node node = new Node(new Vector(x, y), walkMask[heightInversion - i, j] == 1);
					Grid[i, j] = node;
				}
			}
		}

		public List<Node> GetNeighbours(Node node){
			List<Node> neighbours = new List<Node>();
			for(int i = -1; i <= 1; i++)
			for(int j = -1; j <= 1; j++){
				if(i == 0 && j == 0)
					continue;
				int node_i = node.Grid_I + i;
				int node_j = node.Grid_J + j;
				if(IsWithinGrid(node_i, node_j))
					neighbours.Add(Grid[node_i, node_j]);
			}

			return neighbours;
		}

		/// <summary>
		/// Convert RoboCode battlefield coordinates to 
		/// its respective node in the Grid[,] 
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public Vector BattlefieldToGridCoords(Vector vector){
			var i = Math.Floor(vector.Y / ExamSpecs.NodeDiameter);
			var j = Math.Floor(vector.X / ExamSpecs.NodeDiameter);

			int gridSizeX = (int)Math.Floor((double)(ExamSpecs.FieldWidth / ExamSpecs.NodeDiameter));
			int gridSizeY = (int)Math.Floor((double)(ExamSpecs.FieldHeight / ExamSpecs.NodeDiameter));

			i = i.Clamp(0, gridSizeY - 1);
			j = j.Clamp(0, gridSizeX - 1);
			return new Vector(i, j);
		}

		public bool IsWithinGrid(int i, int j){
			return i >= 0 && i < Grid.GetLength(0) &&
			       j >= 0 && j < Grid.GetLength(1);
		}
	}
