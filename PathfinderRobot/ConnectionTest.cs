namespace PG4500_2017_Exam2{

	using System;
	using NUnit.Framework;

	[TestFixture]
	public class ConnectionTest{

		private Connection _testConnection;

		[SetUp]
		public void Setup(){
			_testConnection = new Connection();
		}

		[TearDown]
		public void TearDown(){
			_testConnection = null;
		}

		[Test]
		public void ConnectionReturnsZero(){
			var cost = _testConnection.GetCost();
			Assert.AreEqual(cost, 0, 0.001);
		}
		
	}
}