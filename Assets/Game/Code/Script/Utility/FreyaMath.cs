// by Freya Holmér (https://github.com/FreyaHolmer/Mathfs)

using System.Runtime.CompilerServices;
using UnityEngine;

namespace Freya
{
	/// <summary>Various extensions for floats, vectors and colors</summary>
	public static class MathfsExtensions
	{
		const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

		#region Vector rotation and angles

		/// <summary>Returns the angle of this vector, in radians</summary>
		/// <param name="v">The vector to get the angle of. It does not have to be normalized</param>
		/// <seealso cref="Mathfs.DirToAng"/>
		[MethodImpl(INLINE)] public static float Angle(this Vector2 v) => Mathf.Atan2(v.y, v.x);

		/// <summary>Rotates the vector 90 degrees clockwise (negative Z axis rotation)</summary>
		[MethodImpl(INLINE)] public static Vector2 Rotate90CW(this Vector2 v) => new Vector2(v.y, -v.x);

		/// <summary>Rotates the vector 90 degrees counter-clockwise (positive Z axis rotation)</summary>
		[MethodImpl(INLINE)] public static Vector2 Rotate90CCW(this Vector2 v) => new Vector2(-v.y, v.x);

		/// <summary>Rotates the vector around <c>pivot</c> with the given angle (in radians)</summary>
		/// <param name="v">The vector to rotate</param>
		/// <param name="pivot">The point to rotate around</param>
		/// <param name="angRad">The angle to rotate by, in radians</param>
		[MethodImpl(INLINE)] public static Vector2 RotateAround(this Vector2 v, Vector2 pivot, float angRad) => Rotate(v - pivot, angRad) + pivot;

		/// <summary>Rotates the vector around <c>(0,0)</c> with the given angle (in radians)</summary>
		/// <param name="v">The vector to rotate</param>
		/// <param name="angRad">The angle to rotate by, in radians</param>
		public static Vector2 Rotate(this Vector2 v, float angRad)
		{
			float ca = Mathf.Cos(angRad);
			float sa = Mathf.Sin(angRad);
			return new Vector2(ca * v.x - sa * v.y, sa * v.x + ca * v.y);
		}

		#endregion

		#region Swizzling

		/// <summary>Returns X and Y as a Vector2, equivalent to <c>new Vector2(v.x,v.y)</c></summary>
		[MethodImpl(INLINE)] public static Vector2 XY(this Vector2 v) => new Vector2(v.y, v.x);
		[MethodImpl(INLINE)] public static Vector2Int XZ(this Vector3Int v) => new Vector2Int(v.x, v.z);
		[MethodImpl(INLINE)] public static Vector3 X0Y(this Vector2 v) => new Vector3(v.x, 0.0f, v.y);
		[MethodImpl(INLINE)] public static Vector3 X0Y(this Vector2Int v) => new Vector3(v.x, 0.0f, v.y);
		/// <summary>Returns Y and X as a Vector2, equivalent to <c>new Vector2(v.y,v.x)</c></summary>
		[MethodImpl(INLINE)] public static Vector2 YX(this Vector2 v) => new Vector2(v.y, v.x);

		/// <summary>Returns X and Z as a Vector2, equivalent to <c>new Vector2(v.x,v.z)</c></summary>
		[MethodImpl(INLINE)] public static Vector2 XZ(this Vector3 v) => new Vector2(v.x, v.z);

		/// <summary>Returns this vector as a Vector3, slotting X into X, and Y into Z, and the input value y into Y.
		/// Equivalent to <c>new Vector3(v.x,y,v.y)</c></summary>
		[MethodImpl(INLINE)] public static Vector3 XZtoXYZ(this Vector2 v, float y = 0) => new Vector3(v.x, y, v.y);

		/// <summary>Returns this vector as a Vector3, slotting X into X, and Y into Y, and the input value z into Z.
		/// Equivalent to <c>new Vector3(v.x,v.y,z)</c></summary>
		[MethodImpl(INLINE)] public static Vector3 XYtoXYZ(this Vector2 v, float z = 0) => new Vector3(v.x, v.y, z);

		/// <summary>Sets X to 0</summary>
		[MethodImpl(INLINE)] public static Vector2 FlattenX(this Vector2 v) => new Vector2(0f, v.y);

		/// <summary>Sets Y to 0</summary>
		[MethodImpl(INLINE)] public static Vector2 FlattenY(this Vector2 v) => new Vector2(v.x, 0f);

		/// <summary>Sets X to 0</summary>
		[MethodImpl(INLINE)] public static Vector3 FlattenX(this Vector3 v) => new Vector3(0f, v.y, v.z);

		/// <summary>Sets Y to 0</summary>
		[MethodImpl(INLINE)] public static Vector3 FlattenY(this Vector3 v) => new Vector3(v.x, 0f, v.z);

		/// <summary>Sets Z to 0</summary>
		[MethodImpl(INLINE)] public static Vector3 FlattenZ(this Vector3 v) => new Vector3(v.x, v.y, 0f);

		#endregion

		#region Vector directions & magnitudes

		/// <summary>Returns a vector with the same direction, but with the given magnitude.
		/// Equivalent to <c>v.normalized*mag</c></summary>
		[MethodImpl(INLINE)] public static Vector2 WithMagnitude(this Vector2 v, float mag) => v.normalized * mag;

		/// <summary>Returns a vector with the same direction, but with the given magnitude.
		/// Equivalent to <c>v.normalized*mag</c></summary>
		[MethodImpl(INLINE)] public static Vector3 WithMagnitude(this Vector3 v, float mag) => v.normalized * mag;

		/// <summary>Returns the vector going from one position to another, also known as the displacement.
		/// Equivalent to <c>target-v</c></summary>
		[MethodImpl(INLINE)] public static Vector2 To(this Vector2 v, Vector2 target) => target - v;

		/// <summary>Returns the vector going from one position to another, also known as the displacement.
		/// Equivalent to <c>target-v</c></summary>
		[MethodImpl(INLINE)] public static Vector3 To(this Vector3 v, Vector3 target) => target - v;

		/// <summary>Returns the normalized direction from this vector to the target.
		/// Equivalent to <c>(target-v).normalized</c> or <c>v.To(target).normalized</c></summary>
		[MethodImpl(INLINE)] public static Vector2 DirTo(this Vector2 v, Vector2 target) => (target - v).normalized;

		/// <summary>Returns the normalized direction from this vector to the target.
		/// Equivalent to <c>(target-v).normalized</c> or <c>v.To(target).normalized</c></summary>
		[MethodImpl(INLINE)] public static Vector3 DirTo(this Vector3 v, Vector3 target) => (target - v).normalized;

		#endregion

		#region Color manipulation

		/// <summary>Returns the same color, but with the specified alpha value</summary>
		/// <param name="c">The source color</param>
		/// <param name="a">The new alpha value</param>
		[MethodImpl(INLINE)] public static Color WithAlpha(this Color c, float a) => new Color(c.r, c.g, c.b, a);

		/// <summary>Returns the same color and alpha, but with RGB multiplied by the given value</summary>
		/// <param name="c">The source color</param>
		/// <param name="m">The multiplier for the RGB channels</param>
		[MethodImpl(INLINE)] public static Color MultiplyRGB(this Color c, float m) => new Color(c.r * m, c.g * m, c.b * m, c.a);

		/// <summary>Returns the same color and alpha, but with the RGB values multiplief by another color</summary>
		/// <param name="c">The source color</param>
		/// <param name="m">The color to multiply RGB by</param>
		[MethodImpl(INLINE)] public static Color MultiplyRGB(this Color c, Color m) => new Color(c.r * m.r, c.g * m.g, c.b * m.b, c.a);

		/// <summary>Returns the same color, but with the alpha channel multiplied by the given value</summary>
		/// <param name="c">The source color</param>
		/// <param name="m">The multiplier for the alpha</param>
		[MethodImpl(INLINE)] public static Color MultiplyA(this Color c, float m) => new Color(c.r, c.g, c.b, c.a * m);

		#endregion

		#region Rect

		/// <summary>Expands the rectangle to encapsulate the point <c>p</c></summary>
		/// <param name="r">The rectangle to expand</param>
		/// <param name="p">The point to encapsulate</param>
		public static Rect Encapsulate(this Rect r, Vector2 p)
		{
			r.xMax = Mathf.Max(r.xMax, p.x);
			r.xMin = Mathf.Min(r.xMin, p.x);
			r.yMax = Mathf.Max(r.yMax, p.y);
			r.yMin = Mathf.Min(r.yMin, p.y);
			return r;
		}

		#endregion

		#region Simple float and int operations

		/// <summary>Returns true if v is between or equal to <c>min</c> &amp; <c>max</c></summary>
		/// <seealso cref="Between(float,float,float)"/>
		[MethodImpl(INLINE)] public static bool Within(this float v, float min, float max) => v >= min && v <= max;

		/// <summary>Returns true if v is between or equal to <c>min</c> &amp; <c>max</c></summary>
		/// <seealso cref="Between(int,int,int)"/>
		[MethodImpl(INLINE)] public static bool Within(this int v, int min, int max) => v >= min && v <= max;

		/// <summary>Returns true if v is between, but not equal to, <c>min</c> &amp; <c>max</c></summary>
		/// <seealso cref="Within(float,float,float)"/>
		[MethodImpl(INLINE)] public static bool Between(this float v, float min, float max) => v > min && v < max;

		/// <summary>Returns true if v is between, but not equal to, <c>min</c> &amp; <c>max</c></summary>
		/// <seealso cref="Within(int,int,int)"/>
		[MethodImpl(INLINE)] public static bool Between(this int v, int min, int max) => v > min && v < max;

		/// <summary>Clamps the value to be at least <c>min</c></summary>
		[MethodImpl(INLINE)] public static float AtLeast(this float v, float min) => v < min ? min : v;

		/// <summary>Clamps the value to be at least <c>min</c></summary>
		[MethodImpl(INLINE)] public static int AtLeast(this int v, int min) => v < min ? min : v;

		/// <summary>Clamps the value to be at most <c>max</c></summary>
		[MethodImpl(INLINE)] public static float AtMost(this float v, float max) => v > max ? max : v;

		/// <summary>Clamps the value to be at most <c>max</c></summary>
		[MethodImpl(INLINE)] public static int AtMost(this int v, int max) => v > max ? max : v;

		/// <summary>Squares the value. Equivalent to <c>v*v</c></summary>
		[MethodImpl(INLINE)] public static float Square(this float v) => v * v;

		/// <summary>Cubes the value. Equivalent to <c>v*v*v</c></summary>
		[MethodImpl(INLINE)] public static float Cube(this float v) => v * v * v;

		/// <summary>Squares the value. Equivalent to <c>v*v</c></summary>
		[MethodImpl(INLINE)] public static int Square(this int v) => v * v;


		#endregion


	}

}
