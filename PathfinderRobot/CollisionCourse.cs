using System;
using Santom;

namespace PG4500_2017_Exam2{
	internal class CollisionCourse{
		private readonly Vector _playerPos;
		private readonly Vector _enemyPos;
		private readonly Vector _playerVel;
		private readonly Vector _enemyVel;
		private Vector dp;
		private Vector dv;
		private readonly double _timeClosest;

		public CollisionCourse(Vector playerPos, Vector enemyPos, Vector playerVel, Vector enemyVel){
			_playerPos = playerPos;
			_enemyPos = enemyPos;
			_playerVel = playerVel;
			_enemyVel = enemyVel;
			dp = enemyPos - playerPos;
			dv = enemyVel - playerVel;
			_timeClosest = -(dp.Dot(dv) / dv.LengthSq());
		}

		internal bool IsConfirmed(){ return _timeClosest > 0; }

		public Vector PlayerClosestPos(){ return _playerPos + (_playerVel * _timeClosest); }
		public Vector EnemyClosestPos(){ return _enemyPos + (_enemyVel * _timeClosest); }
	}
}