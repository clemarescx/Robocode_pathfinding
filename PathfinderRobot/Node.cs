using System;
using Santom;

public class Node : IComparable<Node>{
	public const int Diameter = ExamSpecs.NodeDiameter;
	public const int Radius = ExamSpecs.NodeRadius;

	//public List<Node> Neighbours = new List<Node>();
	public Node Parent;

	public Vector BattleFieldCoords => new Vector(CenterX, CenterX);
	public int CenterX{ get; }
	public int CenterY{ get; }
	public int Grid_I{ get; }
	public int Grid_J{ get; }
	public double GCost{ get; set; } // the cost so far
	public double HCost{ get; set; } // the heuristic cost to end node

	public double FCost => GCost + HCost;

	public bool IsWall;

	public Node(Vector pos, bool isWall = false, double gcost = 0.0, double hcost = 0.0)
		: this(pos){
		IsWall = isWall;
		GCost = gcost;
		HCost = hcost;
	}

	public Node(Vector p){
		Vector gridNode = BattlefieldToGridCoords(p);
		Grid_I = (int)gridNode.X;
		Grid_J = (int)gridNode.Y;
		CenterY = ((int)gridNode.X * Diameter) + Radius;
		CenterX = ((int)gridNode.Y * Diameter) + Radius;
	}

	public Node(){ }

	public override bool Equals(Object other){
		if(other == null || other.GetType() != GetType())
			return false;

		Node node = (Node)other;
		return (Grid_I == node.Grid_I) && (Grid_J == node.Grid_J);
	}

	public override int GetHashCode(){ return CenterX.GetHashCode() ^ CenterY.GetHashCode(); }

	public static bool operator ==(Node lhs, Node rhs){
		if(ReferenceEquals(lhs, null))
			return ReferenceEquals(rhs, null);
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Node lhs, Node rhs){ return !(lhs == rhs); }

	public int CompareTo(Node other){
		if(ReferenceEquals(this, other))
			return 0;
		if(ReferenceEquals(null, other))
			return 1;

		int compare = FCost.CompareTo(other.FCost);
		if(compare == 0){
			compare = HCost.CompareTo(other.HCost);
		}
		return compare;
	}

	/// <summary>
	/// Convert RoboCode battlefield coordinates to 
	/// its respective node in the Grid[,] 
	/// Got some help from: https://youtu.be/nhiFx28e7JY
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
}