using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PG4500_2017_Exam2;
using Santom;

namespace RobocodeTest{
	[TestClass]
	public class NodeTests{
		[TestMethod]
		public void CreatedAtOriginTest(){
			Node node = new Node(new Vector(0,0), true);
			Assert.IsTrue(node.Grid_I == 0);
			Assert.IsTrue(node.CenterX == ExamSpecs.NodeRadius);
			Assert.IsTrue(node.Grid_J == 0);
			Assert.IsTrue(node.CenterY == ExamSpecs.NodeRadius);
		}

		[TestMethod]
		public void CreatedAtXYTest(){
			Node node = new Node(new Vector(673, 302), true);
			Assert.IsTrue(node.CenterX == 675);
			Assert.IsTrue(node.Grid_J == 13); // is 13
			Assert.IsTrue(node.CenterY == 325);
			Assert.IsTrue(node.Grid_I == 6); // is 6
		}

		[TestMethod]
		public void NodeComparisonTest(){
			List<Node> nodelist = new List<Node>(){
				new Node(new Vector(0,0),false,2,3),	// fcost = 5
				new Node(new Vector(1,0),false,0,5),	// fcost = 5
				new Node(new Vector(2,0),false,1,4),	// fcost = 5
				new Node(new Vector(3,0),false,3,2),	// fcost = 5
				new Node(new Vector(4,0),false,0,1),	// fcost = 1
				new Node(new Vector(5,0),false,0,2),	// fcost = 2
			};

			Assert.IsTrue(nodelist[0].FCost == 5);

			nodelist.Sort();
			Assert.IsTrue(nodelist[0].FCost == 1);
			Assert.IsTrue(nodelist[1].FCost == 2);

			Assert.IsTrue(nodelist[2].FCost == 5);
			Assert.IsTrue(nodelist[2].GCost == 3);

			Assert.IsTrue(nodelist[3].FCost == 5);
			Assert.IsTrue(nodelist[3].GCost == 2);

			Assert.IsTrue(nodelist[4].FCost == 5);
			Assert.IsTrue(nodelist[4].GCost == 1);

			Assert.IsTrue(nodelist[5].FCost == 5);
			Assert.IsTrue(nodelist[5].GCost == 0);
		}

	}
}