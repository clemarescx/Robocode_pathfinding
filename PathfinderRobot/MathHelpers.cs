using System;
using Santom;
using Robocode;

namespace Santom{
	public static class MathHelpers{
		/// <summary>
		/// To clamp the value val to the range [min, max].</summary>
		/// <remarks>
		/// A copy of the source found in this thread: http://stackoverflow.com/questions/2683442/where-can-i-find-the-clamp-function-in-net </remarks>
		public static T Clamp<T>(this T val, T min, T max) where T :IComparable<T>{
			if(val.CompareTo(min) < 0){
				return min;
			}
			else if(val.CompareTo(max) > 0){
				return max;
			}
			else{
				return val;
			}
		}

		/// <summary>
		/// Return a point relative to the origin of the arena
		/// </summary>
		/// <param name="originRobot"></param>
		/// <param name="distance"></param>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Vector GetGlobalCoords(AdvancedRobot originRobot, double distance, double angle){
			var absoluteBearing = originRobot.HeadingRadians + angle;

			var x = (int)originRobot.X + (distance * Math.Sin(absoluteBearing));
			var y = (int)originRobot.Y + (distance * Math.Cos(absoluteBearing));

			return new Vector(x, y);
		}
		
	}
}