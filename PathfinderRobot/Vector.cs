using System;

namespace Santom{
	public struct Vector{
		public double X{ get; set; }
		public double Y{ get; set; }

		public Vector(double x, double y){
			X = x;
			Y = y;
		}

		/// <summary>
		/// Creates a new Vector2D with the normalized values of the input vector.
		/// </summary>
		/// <returns>A new Vector2D, being a normalized version of the original vector.</returns>
		public static Vector Normalize(Vector v){
			Vector vec = new Vector(v.X, v.Y);
			if(!vec.IsZero()){
				double vectorLength = vec.Length();
				vec.X /= vectorLength;
				vec.Y /= vectorLength;
			}

			return vec;
		}

		/// <summary>
		/// Sets x and y to zero.
		/// </summary>
		public void Zero(){
			X = 0.0;
			Y = 0.0;
		}

		/// <summary>
		/// Returns true if both X and Y are zero.
		/// </summary>
		public bool IsZero(){
			return (((X * X) + (Y * Y)).Equals(0.0));
		}

		/// <summary>
		/// Returns the SQUARED length of the vector (thereby avoiding the Sqrt).
		/// </summary>
		public double LengthSq(){
			return ((X * X) + (Y * Y));
		}

		/// <summary>
		/// Returns the length of the vector.
		/// </summary>
		public double Length(){
			return Math.Sqrt(LengthSq());
		}

		/// <summary>
		/// Normalize the vector.
		/// </summary>
		public void Normalize(){
			if(!IsZero()){
				double tmpLength = Length();
				X /= tmpLength;
				Y /= tmpLength;
			}
		}

		/// <summary>
		/// Get the dot product of the vector.
		/// </summary>
		public double Dot(Vector v){
			return ((X * v.X) + (Y * v.Y));
		}

		/// <summary>
		/// Returns positive if v is clockwise of this vector, negative if anticlockwise. 
		/// (Assuming the Y axis is pointing down, X axis to right, like a Window app.)
		/// </summary>
		public int Sign(Vector v){
			if((Y * v.X) > (X * v.Y)){
				// Anticlockwise.
				return -1;
			}
			// Clockwise.
			return 1;
		}

		/// <summary>
		/// Returns the vector that is perpendicular to this one.
		/// </summary>
		public Vector Perp(){
			return new Vector(-Y, X);
		}

		/// <summary>
		/// Adjusts X and Y so that the length of the vector does not exceed max.
		/// </summary>
		public void Truncate(double max){
			if(Length() > max){
				Normalize();
				X *= max;
				Y *= max;
			}
		}

		/// <summary>
		/// Returns the SQUARED distance between this vector and the one passed as a parameter.
		/// </summary>
		public double DistanceSq(Vector v){
			double ySeparation = v.Y - Y;
			double xSeparation = v.X - X;

			return ((ySeparation * ySeparation) + (xSeparation * xSeparation));
		}

		/// <summary>
		/// Returns the distance between this vector and the one passed as a parameter.
		/// </summary>
		public double Distance(Vector v){
			return Math.Sqrt(DistanceSq(v));
		}

		/// <summary>
		/// Given a normalized vector this method reflects the vector it is operating upon. 
		/// (Like the path of a ball bouncing off a wall.)
		/// </summary>
		public void Reflect(Vector norm){
			Vector modifier = 2.0 * Dot(norm) * norm.GetReverse();
			X += modifier.X;
			Y += modifier.Y;
		}

		/// <summary>
		/// Returns the vector that is the reverse of this vector.
		/// </summary>
		public Vector GetReverse(){
			return new Vector(-X, -Y);
		}

		/// <summary>
		/// Comparing two vectors for equal values.
		/// </summary>
		public bool Equals(Vector v){
			// If parameter is null return false:
			if((object)v == null){
				return false;
			}

			// Return true if the fields match:
			return (X.Equals(v.X)) && (Y.Equals(v.Y));
		}

		/// <summary>
		/// Comparing a vector and an object for equal values.
		/// </summary>
		public override bool Equals(Object obj){
			// If parameter is null return false.
			if(obj == null){
				return false;
			}

			// If parameter cannot be cast to Vector2D return false.
			Vector v = (Vector)obj;
			if((Object)v == null){
				return false;
			}

			// Return true if the fields match:
			return (X.Equals(v.X)) && (Y.Equals(v.Y));
		}

		public override int GetHashCode(){ return X.GetHashCode() ^ Y.GetHashCode(); }


		// We want some overloaded operators:
		// ==================================

		public static Vector operator +(Vector lhs, Vector rhs){ return new Vector(lhs.X + rhs.X, lhs.Y + rhs.Y); }

		public static Vector operator -(Vector lhs, Vector rhs){ return new Vector(lhs.X - rhs.X, lhs.Y - rhs.Y); }

		public static Vector operator *(Vector lhs, double rhs){ return new Vector(lhs.X * rhs, lhs.Y * rhs); }

		public static Vector operator *(double lhs, Vector rhs){ return new Vector(lhs * rhs.X, lhs * rhs.Y); }

		public static Vector operator /(Vector lhs, double rhs){ return new Vector(lhs.X / rhs, lhs.Y / rhs); }

		public static bool operator ==(Vector lhs, Vector rhs){
			// If both are null, or both are same instance, return true.
			if(ReferenceEquals(lhs, rhs)){
				return true;
			}
			// (Else) If lhs is null, return false.
			if((object)lhs == null){
				return false;
			}
			// (Else) Pass it on to the Equals method.
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Vector lhs, Vector rhs){ return !(lhs == rhs); }
	}
}