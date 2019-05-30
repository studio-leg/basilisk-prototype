using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatrixUtils 
{

	// -------------------------------------------------------------------------------------------------------------------
	// Given a list of points, return a list of matrices that represents the chain, 
	// trying to keep the orientations as consistent as possible
	//
	public static List<Matrix4x4> getLookAtTransforms( List<Vector3> _pointList, Vector3 _initialUp )
	{
		List<Matrix4x4> transforms = new List<Matrix4x4>();

		Vector3 xAxis = Vector3.right;
		Vector3 yAxis = _initialUp;
		Vector3 zAxis = Vector3.forward;

		for( int i = 0; i < _pointList.Count-1; i++ )
		{
			Vector3 p0 = _pointList[ i ];
			Vector3 p1 = _pointList[ i + 1 ];

			if( i == 0 )
			{
				zAxis = -(p1 - p0).normalized; // grab our forward
				yAxis = _initialUp;
				xAxis = Vector3.Cross( yAxis, zAxis ); // calculate side
			}
			else
			{
				zAxis = -(p1 - p0).normalized; // forward
				xAxis = Vector3.Cross( yAxis, zAxis ); // guess side based on previous segment's Y
				yAxis = Vector3.Cross( zAxis, xAxis ); // recalculate Y now that we have forward and sideways
			}

			xAxis = xAxis.normalized;
			zAxis = zAxis.normalized;

			//_transforms.push_back( ofMatrix4x4::newLookAtMatrix( p0, p0 + zAxis, yAxis) );

			transforms.Add( Matrix4x4.TRS(p0, Quaternion.LookRotation(zAxis, yAxis), Vector3.one) );
		}

		return transforms;
	}

	// -------------------------------------------------------------------------------------------------------------------
	public static List<Matrix4x4> getLookAtTransforms( List<Vector3> _pointList )
	{
		return getLookAtTransforms( _pointList, Vector3.up );
	}

	// -------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Extract translation from transform matrix.
	/// </summary>
	/// <param name="matrix">Transform matrix. This parameter is passed by reference
	/// to improve performance; no changes will be made to it.</param>
	/// <returns>
	/// Translation offset.
	/// </returns>
	public static Vector3 ExtractTranslationFromMatrix(ref Matrix4x4 matrix) {
		Vector3 translate;
		translate.x = matrix.m03;
		translate.y = matrix.m13;
		translate.z = matrix.m23;
		return translate;
	}

	// -------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Extract rotation quaternion from transform matrix.
	/// </summary>
	/// <param name="matrix">Transform matrix. This parameter is passed by reference
	/// to improve performance; no changes will be made to it.</param>
	/// <returns>
	/// Quaternion representation of rotation transform.
	/// </returns>
	public static Quaternion ExtractRotationFromMatrix(ref Matrix4x4 matrix) {
		Vector3 forward;
		forward.x = matrix.m02;
		forward.y = matrix.m12;
		forward.z = matrix.m22;

		Vector3 upwards;
		upwards.x = matrix.m01;
		upwards.y = matrix.m11;
		upwards.z = matrix.m21;

		return Quaternion.LookRotation(forward, upwards);
	}

	// -------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Extract scale from transform matrix.
	/// </summary>
	/// <param name="matrix">Transform matrix. This parameter is passed by reference
	/// to improve performance; no changes will be made to it.</param>
	/// <returns>
	/// Scale vector.
	/// </returns>
	public static Vector3 ExtractScaleFromMatrix(ref Matrix4x4 matrix) {
		Vector3 scale;
		scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
		scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
		scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
		return scale;
	}

	// -------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Extract position, rotation and scale from TRS matrix.
	/// </summary>
	/// <param name="matrix">Transform matrix. This parameter is passed by reference
	/// to improve performance; no changes will be made to it.</param>
	/// <param name="localPosition">Output position.</param>
	/// <param name="localRotation">Output rotation.</param>
	/// <param name="localScale">Output scale.</param>
	public static void DecomposeMatrix(ref Matrix4x4 matrix, out Vector3 localPosition, out Quaternion localRotation, out Vector3 localScale) {
		localPosition = ExtractTranslationFromMatrix(ref matrix);
		localRotation = ExtractRotationFromMatrix(ref matrix);
		localScale = ExtractScaleFromMatrix(ref matrix);
	}

	// -------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Set transform component from TRS matrix.
	/// </summary>
	/// <param name="transform">Transform component.</param>
	/// <param name="matrix">Transform matrix. This parameter is passed by reference
	/// to improve performance; no changes will be made to it.</param>
	public static void SetLocalTransformFromMatrix(Transform transform, ref Matrix4x4 matrix) {
		transform.localPosition = ExtractTranslationFromMatrix(ref matrix);
		transform.localRotation = ExtractRotationFromMatrix(ref matrix);
		transform.localScale = ExtractScaleFromMatrix(ref matrix);
	}	

	// -------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Set transform component from TRS matrix.
	/// </summary>
	/// <param name="transform">Transform component.</param>
	/// <param name="matrix">Transform matrix. This parameter is passed by reference
	/// to improve performance; no changes will be made to it.</param>
	public static void SetGlobalTransformFromMatrix(Transform transform, ref Matrix4x4 matrix) {
		transform.position = ExtractTranslationFromMatrix(ref matrix);
		transform.rotation = ExtractRotationFromMatrix(ref matrix);
		transform.localScale = ExtractScaleFromMatrix(ref matrix);
	}	
}