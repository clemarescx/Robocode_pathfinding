using System;
using System.Collections.Generic;
using PG4500_2017_Exam2;
using Robocode.Util;
using Santom;

public class Steering{
	// the robot starts clipping through walls with a tolerance higher than 30 degrees
	private const double ANGLE_TO_TARGET_TOLERANCE = Math.PI / 6;

	private readonly Marcle15_Pathfinder _r;
	public Steering(Marcle15_Pathfinder playerRobot){ _r = playerRobot; }

	/// <summary>
	/// Move the robot towards a target node centre.
	/// The robot moves forward only if it is turned 
	/// towards the target within ANGLE_TO_TARGET_TOLERANCE radians 
	/// </summary>
	/// <param name="target"></param>
	public void Seek(Node target){
		var coords = NodeToPlayerCoords(target);
		double distanceToTarget = GetDistancetoPosition(coords);
		double angleToTarget = GetAngleToPosition(coords);

		_r.SetTurnRightRadians(angleToTarget);

		// only move forward if we're in the right direction
		if(Math.Abs(_r.TurnRemainingRadians) < ANGLE_TO_TARGET_TOLERANCE)
			_r.SetAhead(distanceToTarget);
	}

	public void SetCourseToNode(Node node){
		var dst = NodeToPlayerCoords(node);
		SetCourseToPos(dst);
	}

	public Vector NodeToPlayerCoords(Node node){
		return new Vector(_r.PositionVector.X - node.CenterX, _r.PositionVector.Y - node.CenterY);
	}


	private void SetCourseToPos(Vector dst){ SetCourseToPos(dst.X, dst.Y); }

	public void SetCourseToPos(double x, double y){
		var distance = GetDistancetoPosition(x, y);
		var angle = GetAngleToPosition(x, y);
		try{
			_r.TurnRightRadians(angle);
			_r.Ahead(distance);
		}
		catch(Exception e){
			Console.WriteLine("could not continue advancing");
			Console.WriteLine("msg: {0}", e.Message);
		}
	}


	private double GetAngleToPosition(Vector targetPos){ return GetAngleToPosition(targetPos.X, targetPos.Y); }

	private double GetAngleToPosition(double x, double y){
		return Utils.NormalRelativeAngle(Math.Atan2(-x, -y) - _r.HeadingRadians);
	}

	private double GetDistancetoPosition(Vector position){ return GetDistancetoPosition(position.X, position.Y); }

	private double GetDistancetoPosition(double distX, double distY){
		return Math.Sqrt((distX * distX) + (distY * distY));
	}
}