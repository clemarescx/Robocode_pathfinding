using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PG4500_2017_Exam2;
using Santom;

namespace RobocodeTest{
	[TestClass]
	public class SteeringTests{
		private readonly Steering _steering;
		private readonly Graph _graph;
		private readonly int[,] _realGrid = ExamSpecs.Grid;

		public SteeringTests(){
			_graph = new Graph(_realGrid);
			_steering = new Steering(new Marcle15_Pathfinder());
		}
	}
}