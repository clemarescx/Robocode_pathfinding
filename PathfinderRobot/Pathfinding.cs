using System;
using System.Collections.Generic;
using Santom;

public class Pathfinding{
	private readonly Graph _graph;

	public Pathfinding(Graph graph){ _graph = graph; }


	/// <summary>
	/// Basic implementation of the A* pathfinding algorithm
	/// </summary>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <returns></returns>
	public Stack<Node> AStar(Node start, Node end){
		var open = new List<Node>();
		var closed = new List<Node>();
		open.Add(start);
		Node currentNode = new Node();
		while(open.Count > 0){
			open.Sort();
			currentNode = open[0];

			open.Remove(currentNode);
			closed.Add(currentNode);

			if(currentNode == end){
				break;
			}

			foreach(var neighbour in _graph.GetNeighbours(currentNode)){
				if(neighbour.IsWall || closed.Contains(neighbour)){
					continue;
				}
				var newCost = currentNode.GCost + GetDistance(currentNode, neighbour);
				if(newCost < neighbour.GCost || !open.Contains(neighbour)){
					neighbour.GCost = newCost;
					neighbour.HCost = GetDistance(neighbour, end);
					neighbour.Parent = currentNode;

					if(!open.Contains(neighbour)){
						open.Add(neighbour);
					}
				}
			}
		}

		/*
		 * If the nodes are added to a stack from end to start,
		 * the main method will get them out from start to end (LIFO)
		 * - i.e., no need to reverse the path :)
		 */
		Stack<Node> _path = new Stack<Node>();
		if(currentNode == end)
			while(currentNode != start){
				_path.Push(currentNode);
				currentNode = currentNode.Parent;
			}

		return _path;
	}

	/// <summary>
	/// Euclidean Heuristic to update the HCost of a node 
	/// </summary>
	/// <param name="A"></param>
	/// <param name="B"></param>
	/// <returns></returns>
	double GetDistance(Node A, Node B){
		return Math.Sqrt(
			Math.Pow(A.Grid_I - B.Grid_I, 2) +
			Math.Pow(A.Grid_J - B.Grid_J, 2)
			);
	}
}