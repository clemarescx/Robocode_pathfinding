using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Robocode;
using Robocode.Util;
using Santom;

namespace PG4500_2017_Exam2{
	// ReSharper disable once InconsistentNaming
	public class Marcle15_Pathfinder : AdvancedRobot{
		private struct Commissioner{
			public double velocity;
			public Vector position;
			public bool found;
			public Vector headingVector;
			public Point leftAvoid;
			public Point rightAvoid;
		}

		private Commissioner _commissioner;

		public Vector PositionVector{ get; set; }
		public Vector VelocityVector{ get; set; }

		Node _currentTargetNode = new Node();
		private Node PositionNode => new Node(PositionVector);
		public long EventTime{ get; set; }
		private Stack<Node> _path = new Stack<Node>();
		private Node _goal;
		private Node[,] _grid;
		private Steering _steering;
		private Pathfinding _pathfinding;
		private const double Factor = 2.1;

		private bool _running = true;
		private CollisionCourse _collision;

		public Marcle15_Pathfinder(){ Console.WriteLine("Constructor..."); }

		void Init(){
			Debug.Assert(_path.Count == 0, "_path is not empty upon init()");
			Debug.Assert(_commissioner.found == false, "_commissioner is already found upon init()");
			_steering = new Steering(this);
			var graph = new Graph(ExamSpecs.Grid);
			_grid = graph.Grid;
			_pathfinding = new Pathfinding(graph);

			AddCustomEvent(
				new Condition(
					"CollisionWithRobot",
					(c) => {
						// looking for time of closest approach
						// c.f. p. 87, Steering Behaviors, Millington & Funge
						var vt = _commissioner.headingVector;
						vt.Normalize();
						vt *= _commissioner.velocity;
						var potentialCollision = new CollisionCourse(PositionVector, _commissioner.position, VelocityVector, vt);
						if(potentialCollision.IsConfirmed())
							_collision = potentialCollision;
						return potentialCollision.IsConfirmed();
					}));
		}

		public override void Run(){
			Init();
			var startPos = _grid[0, 0];
			_steering.SetCourseToNode(startPos);


			var currentTime = EventTime;

			while(_running){
				if(_commissioner.found){
					if(_path.Count == 0 && _commissioner.velocity < 0.0001){
						var newGoal = new Node(_commissioner.position);
						if(newGoal != _goal){ // in case
							_goal = newGoal;
							_path = _pathfinding.AStar(PositionNode, _goal);
						}
					}

					//while we have a goal and we have not reached it
					if(!ReferenceEquals(_goal, null) && !ArrivedAtNode(_goal)){
						if(ArrivedAtNode(_currentTargetNode)){
							// get the next step
							Console.WriteLine("Path nodes remaining: {0}", _path.Count);
							_currentTargetNode = _path.Pop();
						}
						if(EventTime > currentTime){
							_steering.Seek(_currentTargetNode);
							currentTime = EventTime;
						}
					}
				}

				// Scan at all times
				if(Math.Abs(RadarTurnRemaining) < 0.0001){
					SetTurnRadarRightRadians(double.PositiveInfinity);
				}

				try{
					Execute();
				}
				catch(Exception e){
					Console.WriteLine("Execute() failed: {0}", e.Message);
					_running = false;
				}
			}
		}

		private bool ArrivedAtNode(Node goal){ return PositionNode == goal; }


		public override void OnScannedRobot(ScannedRobotEvent evnt){
			// taken from
			// http://old.robowiki.net/robowiki?Radar
			//
			double absBearing = evnt.BearingRadians + HeadingRadians;
			SetTurnRadarRightRadians(Factor * Utils.NormalRelativeAngle(absBearing - RadarHeadingRadians));
			_commissioner.found = true;
			_commissioner.position = MathHelpers.GetGlobalCoords(this, evnt.Distance, evnt.BearingRadians);
			_commissioner.velocity = evnt.Velocity;
			_commissioner.headingVector = new Vector(Math.Sin(evnt.HeadingRadians), Math.Cos(evnt.HeadingRadians));
			var normVec = _commissioner.headingVector.Perp();
			var center = new Vector(_commissioner.position.X, _commissioner.position.Y);
			var head = center + (normVec * ExamSpecs.NodeDiameter);
			var tail = center + (normVec * -ExamSpecs.NodeDiameter);
			_commissioner.leftAvoid = new Point((int)head.X, (int)head.Y);
			_commissioner.rightAvoid = new Point((int)tail.X, (int)tail.Y);
		}


		public override void OnStatus(StatusEvent e){
			RobotStatus status = e.Status;
			PositionVector = new Vector(status.X, status.Y);
			VelocityVector = new Vector(Math.Sin(status.HeadingRadians), Math.Cos(HeadingRadians)) * status.Velocity;
			EventTime = e.Time;
		}


		public override void OnHitWall(HitWallEvent evnt){
			try{
				Back(Math.Abs(evnt.Bearing) < 90 ? 10 : -10);
			}
			catch(Exception ex){
				Console.WriteLine("Could not move after wall collision: {0}", ex.Message);
			}
		}

		public override void OnHitRobot(HitRobotEvent evnt){
			try{
				Back(Math.Abs(evnt.Bearing) < 90 ? 50 : -50);
			}
			catch(Exception ex){
				Console.WriteLine("Could not move after robot collision: {0}", ex.Message);
			}
		}

		public override void OnCustomEvent(CustomEvent evnt){
			if(evnt.Condition.Name == "CollisionWithRobot"){
				var closestCharPos = _collision.PlayerClosestPos();
				var closestEnemyPos = _collision.EnemyClosestPos();
				if(closestCharPos.Distance(closestEnemyPos) < ExamSpecs.NodeDiameter){
					Console.WriteLine("Potential collision");
					var collisionNode = new Node(closestCharPos);
					//TODO: implement collision avoidance:
					// 1) find node to avoid collisionNode
					//	- must not be outside battlefield
					//	- must not be wall
					//	- must not be on enemy's heading vector
					//	- could be the node touched by the closest of enemy's left and right normal vectors
					// 2) override current trajectory
					// Alternatives:
					//	- save current trajectory, go to temp node, then resume after collision avoided
					//	- go to temp node, then recalculate path to same goal
					PaintNode(collisionNode, Color.DeepPink);
				}
			}
		}

		public override void OnRoundEnded(RoundEndedEvent evnt){ ClearState(); }

		private void ClearState(){
			_running = false;
			_commissioner.found = false;
			_goal = null;
			_path.Clear();
		}


		public override void OnPaint(IGraphics graphics){
			base.OnPaint(graphics);
			PaintEnemyInfo();
			PaintVelocity();
			// RoboCode paint limit : 16 * 4 nodes;
			if(_path != null && _path.Count > 0){
				PaintNode(_currentTargetNode, Color.Red);
				PaintPath(_path);
			}
		}

		private void PaintEnemyInfo(){
			var _x = _commissioner.position.X;
			var _y = _commissioner.position.Y;
			var leftCoords = _commissioner.leftAvoid;
			var rightCoords = _commissioner.rightAvoid;
			var boundingBoxColor = Color.FromArgb(100, Color.LightSkyBlue);
			var rectX = (int)(_x - ExamSpecs.NodeRadius);
			var rectY = (int)(_y - ExamSpecs.NodeRadius);
			var rect = new Rectangle(rectX, rectY, ExamSpecs.NodeDiameter, ExamSpecs.NodeDiameter);
			Graphics.FillEllipse(new SolidBrush(boundingBoxColor), rect);

			var normalsColor = Color.Yellow;
			Graphics.DrawLine(new Pen(normalsColor), leftCoords, rightCoords);
		}

		private void PaintVelocity(){
			var normalsColor = Color.DeepPink;
			var tail = PositionVector + (VelocityVector * 5);
			Graphics.DrawLine(
				new Pen(normalsColor),
				new Point((int)PositionVector.X, (int)PositionVector.Y),
				new Point((int)tail.X, (int)tail.Y));
		}

		private void PaintPath(IEnumerable<Node> path){
			foreach(var node in path){
				PaintNode(node, Color.Green);
			}
		}

		private void PaintNode(Node node, Color color){
			const int radius = ExamSpecs.NodeRadius;
			const int diameter = ExamSpecs.NodeDiameter;

			Color fadedColor = Color.FromArgb(100, color);
			Graphics.FillRectangle(
				new SolidBrush(fadedColor),
				node.CenterX - radius,
				node.CenterY - radius,
				diameter,
				diameter);
		}
	}
}