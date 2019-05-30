using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spline
{

	public List<Vector3> points;
	public bool isClosed;

	//----------------------------------------------------------
	public Spline()
	{
		points = new List<Vector3>();
		isClosed = false;
	}

	//----------------------------------------------------------
	public Spline( List<Vector3> _points )
	{
		points = new List<Vector3>();
		isClosed = false;
		AddPoints( _points );
	}

	// ---------------------------------------------------------------
	public void AddPoints( List<Vector3> _points)
	{
		for(var i = 0; i < _points.Count; i++ )
		{
			points.Add( _points[i] );
		}
	}
		
	// ---------------------------------------------------------------
	public void AddPoint(Vector3 _point)
	{
		//Debug.Log( "new Point: " + _point.x + " " + _point.y + " " + _point.z );
		points.Add(_point);
	}

	// ---------------------------------------------------------------
	public Vector3[] ToArray()
	{
		return points.ToArray();
	}

	// ---------------------------------------------------------------
	public void Clear()
	{
		points.Clear();
	}

	// ---------------------------------------------------------------	
	public int Count
	{
		get
		{
			return points.Count;
		}
	}

	// ---------------------------------------------------------------	
	public Vector3 this[int i]
	{
		get
		{
			return points[i];
		}
		set
		{
			points[i] = value;
		}
	}

	// ---------------------------------------------------------------
	public Vector3 lastPoint
	{
		get
		{
			if (points.Count > 0)
			{
				return points[points.Count - 1];
			}
			else
			{
				return Vector3.zero;
			}
		}
	}

    //----------------------------------------------------------
    public void resampleCurrentPointsAsSpline( int _res )
    {
        List<Vector3> newPoints = getInterpolatedSpline( _res );
        points = newPoints;
    }

    //----------------------------------------------------------
    public List<Vector3> getInterpolatedSpline( int _res )
    {
        return getInterpolatedSpline(points, _res);
    }

    //----------------------------------------------------------
    static public List<Vector3> getInterpolatedSpline(List<Vector3> _points, int _res )
	{
		List<Vector3> newPoints = new List<Vector3>();
		for( int i = 0; i < _res; i++ )
		{
			float frac = MathUtils.Map( i, 0, _res-1, 0, 1 );
			newPoints.Add( Spline.getSplinePointAt( frac, _points) );
		}

		return newPoints;
	}


    //----------------------------------------------------------
    public float getPerimeter()
    {
        return getPerimeter(points, isClosed);
    }

    //----------------------------------------------------------
    // Todo: Is this worth having static? The Simplification routines could go into MathUtils, 
    // but then that's a dependency. We're most likely to want to simplify the control points
    // Adding helpers for now.
    //
    static public float getPerimeter(List<Vector3> _points, bool _isClosed)
	{
		float perimeter = 0;
		int lastPosition = _points.Count - 1;
		for (int i = 0; i < lastPosition; i++)
		{
			//perimeter += _points[i].distance(_points[i + 1]);
			perimeter += Vector3.Distance(_points[i], _points[i + 1]);
		}
		if (_isClosed && _points.Count > 1)
		{
			//perimeter += _points[_points.Count - 1].distance(_points[0]);
			perimeter += Vector3.Distance(_points[_points.Count - 1], _points[0]);
		}
		return perimeter;
	}


	//----------------------------------------------------------
	public List<Vector3> getResampledByCount(int _count)
	{
		return getResampledByCount(points, _count, isClosed);
	}

	//----------------------------------------------------------
	static public List<Vector3> getResampledByCount(List<Vector3> _points, int _count, bool _isClosed = false )
	{
		float perimeter = getPerimeter(_points, _isClosed);
		return getResampledBySpacing(_points, perimeter / _count, _isClosed);
	}


	//----------------------------------------------------------
	public List<Vector3> getResampledBySpacing(float _spacing)
	{
		return getResampledBySpacing(points, _spacing, isClosed);
	}

	//----------------------------------------------------------
	static public List<Vector3> getResampledBySpacing(List<Vector3> _points, float spacing, bool _isClosed = false )
	{
		List<Vector3> result = new List<Vector3>();

		// if more properties are added to ofPolyline, we need to copy them here
		//result.setClosed(polyline.isClosed());

		float totalLength = 0;
		int curStep = 0;
		int lastPosition = _points.Count - 1;

		if (_isClosed)
		{
			lastPosition++;
		}

		result.Add(_points[0]);

		for (int i = 1; i < lastPosition; i++)
		{
			bool repeatNext = i == (int)(_points.Count - 1);

			Vector3 cur = _points[i];
			Vector3 next = repeatNext ? _points[0] : _points[i + 1];
			Vector3 diff = next - cur;

			float curSegmentLength = diff.magnitude;
			if (curSegmentLength > 0)
			{
				totalLength += curSegmentLength;

				while (curStep * spacing <= totalLength)
				{
					float curSample = curStep * spacing;
					float curLength = curSample - (totalLength - curSegmentLength);
					float relativeSample = curLength / curSegmentLength;
					//result.addVertex(cur.getInterpolated(next, relativeSample));
					result.Add(Vector3.Lerp(cur, next, relativeSample));
					curStep++;
				}
			}
			else
			{
				curStep++;
			}

		}
		result.Add(_points[_points.Count - 1]);

		return result;
	}

	//----------------------------------------------------------
	public List<Vector3> getSmoothed(int _smoothingSize, float _smoothingShape)
	{
		return getSmoothed(points, _smoothingSize, _smoothingShape, isClosed);
	}

    /// <summary>
    /// SmoothingSize is the size of the smoothing window.
    /// So if smoothingSize is 2, then 2 points from the left, 1 in the center, and 2 on the right (5 total) will be used for smoothing each point.
    /// SmoothingShape describes whether to use a triangular window (0) or box window (1) or something in between (for example, .5).
    /// </summary>
    /// <param name="_points"></param>
    /// <param name="smoothingSize"></param>
    /// <param name="smoothingShape"></param>
    /// <param name="_isClosed"></param>
    /// <returns></returns>
	public static List<Vector3> getSmoothed(List<Vector3> _points, int smoothingSize, float smoothingShape, bool _isClosed)
	{
		int n = _points.Count;
		smoothingSize = Mathf.Clamp(smoothingSize, 0, n);
		smoothingShape = Mathf.Clamp01(smoothingShape);

		// precompute weights and normalization
		float[] weights = new float[smoothingSize];

		// side weights
		for (int i = 1; i < smoothingSize; i++)
		{
			float curWeight = MathUtils.Map(i, 0.0f, smoothingSize, 1.0f, smoothingShape);
			weights[i] = curWeight;
		}

		// make a copy of this polyline
		List<Vector3> result = new List<Vector3>();
		for (int i = 0; i < n; i++) { result.Add(_points[i]); }

		for (int i = 0; i < n; i++)
		{
			float sum = 1; // center weight
			for (int j = 1; j < smoothingSize; j++)
			{
				Vector3 cur = Vector3.zero;
				int leftPosition = i - j;
				int rightPosition = i + j;
				if (leftPosition < 0 && _isClosed)
				{
					leftPosition += n;
				}
				if (leftPosition >= 0)
				{
					cur += _points[leftPosition];
					sum += weights[j];
				}
				if (rightPosition >= n && _isClosed)
				{
					rightPosition -= n;
				}
				if (rightPosition < n)
				{
					cur += _points[rightPosition];
					sum += weights[j];
				}
				result[i] += cur * weights[j];
			}
			result[i] /= sum;
		}

		return result;
	}

	// ---------------------------------------------------------------	
	public Vector3 getSplinePointAt(float _t)
	{
		return getSplinePointAt(_t, points);
	}

	// ---------------------------------------------------------------
	public static Vector3 getSplinePointAt(float _t, List<Vector3> _points)
	{
		if (_points.Count < 4)
		{
			return Vector3.zero;
		}

		if (_t >= 1.0f) { _t = 0.999999f; }
		else if (_t < 0.0000001f) { _t = 0.0000001f; }

		float controlPointAmountMinusEndPoints = (float)(_points.Count - 3);
		float segmentFracLengthMinusControlPoints = 1.0f / controlPointAmountMinusEndPoints;

		int startIndex = (int)Mathf.Floor(_t / segmentFracLengthMinusControlPoints) + 1;

		Vector3 p0 = _points[startIndex - 1];
		Vector3 p1 = _points[startIndex];
		Vector3 p2 = _points[startIndex + 1];
		Vector3 p3 = _points[startIndex + 1];
		if (startIndex + 2 < _points.Count)
		{
			p3 = _points[startIndex + 2];
		}

		_t = Mathf.Repeat(_t, segmentFracLengthMinusControlPoints) / segmentFracLengthMinusControlPoints;
		float t2 = _t * _t;
		float t3 = _t * _t * _t;

		float tmpX = computeCardinalAt(_t, t2, t3, p0.x, p1.x, p2.x, p3.x);
		float tmpY = computeCardinalAt(_t, t2, t3, p0.y, p1.y, p2.y, p3.y);
		float tmpZ = computeCardinalAt(_t, t2, t3, p0.z, p1.z, p2.z, p3.z);

		return new Vector3(tmpX, tmpY, tmpZ);
	}

	// ---------------------------------------------------------------
	public static Vector3 getLinearPointAt(float _t, List<Vector3> _points)
	{
		if (_points.Count < 2)
		{
			return Vector3.zero;
		}

		if (_t >= 1.0f) { _t = 0.999999f; }
		else if (_t < 0.0000001f) { _t = 0.0000001f; }

		float controlPointAmountMinusEndPoints = (float)(_points.Count - 1);
		float segmentFracLengthMinusControlPoints = 1.0f / controlPointAmountMinusEndPoints;

		int startIndex = (int)Mathf.Floor(_t / segmentFracLengthMinusControlPoints);

		Vector3 p0 = _points[startIndex];
		Vector3 p1 = _points[startIndex + 1];

		_t = Mathf.Repeat(_t, segmentFracLengthMinusControlPoints) / segmentFracLengthMinusControlPoints;

		return Vector3.Lerp( p0, p1, _t );

	}

	// ---------------------------------------------------------------	
	public Vector3 getNormal2DForSplinePointAt( float _t, float _lookAheadFraction = 0.01f)
	{
		return getNormal2DForSplinePointAt(_t, points, _lookAheadFraction);
	}

	// ---------------------------------------------------------------
	public static Vector3 getNormal2DForSplinePointAt( float _t, List<Vector3> _points, float _lookAheadFraction = 0.01f)
	{
		Vector3 point;
		Vector3 normal;
		getPointAndNormal2DForSplinePointAt(_t, _points, out point, out normal, _lookAheadFraction);
		return normal;
	}

	// ---------------------------------------------------------------
	public void getPointAndNormal2DForSplinePointAt( float _t, out Vector3 _outPoint, out Vector3 _outNormal, float _lookAheadFraction = 0.01f)
	{
		getPointAndNormal2DForSplinePointAt(_t, points, out _outPoint, out _outNormal, _lookAheadFraction);
	}

	// ---------------------------------------------------------------	
	// The normal will lie on the X and Y plane
	public static void getPointAndNormal2DForSplinePointAt( float _t,
															List<Vector3> _points,
															out Vector3 _outPoint,
															out Vector3 _outNormal,
															float _lookAheadFraction = 0.01f)
	{
		float nextTime = _t + _lookAheadFraction; // the higher this value is, the smoother the normal will animate across the spline
		nextTime = Mathf.Clamp01(nextTime);

		Vector3 tmpPoint1 = getSplinePointAt(_t, _points);
		Vector3 tmpPoint2 = getSplinePointAt(nextTime, _points);

		float diffX = tmpPoint1.x - tmpPoint2.x;
		float diffY = tmpPoint1.y - tmpPoint2.y;

		Vector3 normal = new Vector3(-diffY, diffX, 0.0f);
		normal.Normalize();

		_outPoint = tmpPoint1;
		_outNormal = normal;
	}

	// ---------------------------------------------------------------
	public static float computeCardinalAt(float t, float t2, float t3, float b0, float b1, float b2, float b3)
	{
		return (((-b0 + 3.0f * b1 - 3.0f * b2 + b3) * t3)
				+ ((2.0f * b0 - 5.0f * b1 + 4.0f * b2 - b3) * t2)
				+ ((-b0 + b2) * t)
				+ (2.0f * b1))
				* 0.5f;
	}

	// ---------------------------------------------------------------		
	// Gets the distance between a *spline point* at time _t and one at (_t + _tDiff)  
	public float getSplinePointDistance(float _t, float _tDiff = -0.003f)
	{
		return getSplinePointDistance(_t, _tDiff, points);
	}


	// ---------------------------------------------------------------		
	// Gets the distance between a *spline point* at time _t and one at (_t + _tDiff)  
	public static float getSplinePointDistance(float _t, float _tDiff, List<Vector3> _points)
	{
		Vector3 p1 = getSplinePointAt(_t, _points);
		Vector3 p2 = getSplinePointAt(_t + _tDiff, _points);
		Vector3 delta = p2 - p1;
		return delta.magnitude;
	}

	// ---------------------------------------------------------------	
	// This grabs the length of the current *control point* line segment,
	// nothing to do with the spline we can calculate from these control points
	public float getInterpolatedLineSegmentLength(float _t)
	{
		return getInterpolatedLineSegmentLength(_t, points);
	}

	// ---------------------------------------------------------------	
	// This grabs the length of the current *control point* line segment,
	// nothing to do with the spline we can calculate from these control points
	public static float getInterpolatedLineSegmentLength(float _t, List<Vector3> _points)
	{

		//if( _points.Count < 4 ) {
		if (_points.Count < 5)
		{
			return 0.0f;
		}

		if (_t >= 1.0f) { _t = 0.999999f; }
		else if (_t < 0.0000001f) { _t = 0.0000001f; }

		float controlPointAmountMinusEndPoints = (float)(_points.Count - 4);
		//float controlPointAmountMinusEndPoints = (float)(_points.Count-3);
		float segmentFracLengthMinusControlPoints = 1.0f / controlPointAmountMinusEndPoints;

		int startIndex = (int)Mathf.Floor(_t / segmentFracLengthMinusControlPoints) + 1;

		Vector3 p0 = _points[startIndex - 1];
		Vector3 p1 = _points[startIndex];
		Vector3 p2 = _points[startIndex + 1];
		Vector3 p3 = _points[startIndex + 2];
		Vector3 p4 = _points[startIndex + 3];

		float currSegmentFraction = Mathf.Repeat(_t, segmentFracLengthMinusControlPoints) / segmentFracLengthMinusControlPoints;

		Vector3 prevSegmentDelta = p1 - p0;
		Vector3 currSegmentDelta = p2 - p1;
		Vector3 nextSegmentDelta = p3 - p2;
		Vector3 nextSegmentDelta2 = p4 - p3;

		float prevtSegmentLength = prevSegmentDelta.magnitude;
		float currentSegmentLength = currSegmentDelta.magnitude;
		float nextSegmentLength = nextSegmentDelta.magnitude;
		float nextSegmentLength2 = nextSegmentDelta2.magnitude;

		//return CosineInterpolate( currSegmentFraction, currentSegmentLength, nextSegmentLength );
		return CubicInterpolate(currSegmentFraction, prevtSegmentLength, currentSegmentLength, nextSegmentLength, nextSegmentLength2);
	}

	// ---------------------------------------------------------------		
	public static float CosineInterpolate(float _frac, float _a, float _b)
	{
		float ft = _frac * 3.1415927f;
		float f = (1.0f - Mathf.Cos(ft)) * 0.5f;
		return _a * (1.0f - f) + _b * f;
	}

	// ---------------------------------------------------------------		
	public static float CubicInterpolate(float _frac, float v0, float v1, float v2, float v3)
	{
		float P = (v3 - v2) - (v0 - v1);
		float Q = (v0 - v1) - P;
		float R = v2 - v0;
		float S = v1;
		float frac2 = _frac * _frac;
		float frac3 = _frac * _frac * _frac;
		return (P * frac3) + (Q * frac2) + (R * _frac) + S;
	}

	// ---------------------------------------------------------------	
	public static void getBounds(List<Vector3> _points, out Vector3 _min, out Vector3 _max)
	{
		_min = new Vector3(99999999999.0f, 99999999999.0f, 99999999999.0f);
		_max = new Vector3(-99999999999.0f, -99999999999.0f, -99999999999.0f);
		for (int i = 0; i < _points.Count; i++)
		{
			Vector3 tmp = _points[i];

			if (tmp.x < _min.x) { _min.x = tmp.x; }
			if (tmp.x > _max.x) { _max.x = tmp.x; }

			if (tmp.y < _min.y) { _min.y = tmp.y; }
			if (tmp.y > _max.y) { _max.y = tmp.y; }

			if (tmp.z < _min.z) { _min.z = tmp.z; }
			if (tmp.z > _max.z) { _max.z = tmp.z; }
		}
	}
}