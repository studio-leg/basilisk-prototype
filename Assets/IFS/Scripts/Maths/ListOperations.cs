using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// --------------------------------------------------------------------------------------------------------
//
public class ListOperations 
{

    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // These operations currently make the pretty huge assumption that the incoming points are equally spaced //
    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    /// <summary>
    /// Lerps between two sets of points, assumes that each list is the same length!
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static List<Vector3> LerpPoints(List<Vector3> from, List<Vector3> to, float amount)
    {
        var points = new List<Vector3>();
        for (int i = 0; i < from.Count; i++)
        {
            points.Add(Vector3.Lerp(from[i], to[i], amount));
        }
        return points;
    }

    // --------------------------------------------------------------------------------------------------------
    //
    public static List<Vector3> add( List<Vector3> _list, Vector3 _offset )
	{
		List < Vector3 > tmpOut = new List<Vector3>();

        for ( int i = 0; i < _list.Count; i++ )
		{
			tmpOut.Add( _list[i] + _offset );
		}

		return tmpOut;
    }

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> add(  List<Vector3> _list1,  List<Vector3> _list2 )
	{
		List<Vector3> tmpOut = new List<Vector3>();
		if ( _list1.Count != _list2.Count )
		{
			return tmpOut ;
		}

		for( int i = 0; i < _list1.Count; i++ )
		{
			tmpOut.Add( _list1[i] + _list2[i] );
		}

		return tmpOut;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> multiply( List<Vector3> _list, Vector3 _scale )
	{
		List<Vector3> tmpOut = new List<Vector3>();

		for ( int i = 0; i < _list.Count; i++ )
		{
			tmpOut.Add(Vector3.Scale(_list[i], _scale) );
		}
		return tmpOut;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> multiply(  List<Vector3> _list1,  List<Vector3> _list2, bool _useX = true, bool _useY = true, bool _useZ = true )
	{
		List<Vector3> tmpOut = new List<Vector3>();

		if ( _list1.Count != _list2.Count )
		{
			return tmpOut;
		}

		for( int i = 0; i < _list1.Count; i++ )
		{
			Vector3 tmp = _list2[i];

			if (!_useX) tmp.x = 1f;
			if (!_useY) tmp.y = 1f;
			if (!_useZ) tmp.z = 1f;
			//Debug.Log(tmp + "	" + _useX + "	" + _useY + "	" + _useZ);

			tmpOut.Add( Vector3.Scale( _list1[i], tmp) );
		}

		return tmpOut;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> mix( List<Vector3> _list1, List<Vector3> _list2, float _amount )
	{
		List<Vector3> tmpOut = new List<Vector3>();
		if ( _list1.Count != _list2.Count )
		{
			return tmpOut;
		}

		for ( int i = 0; i < _list1.Count; i++ )
		{
			tmpOut.Add( Vector3.Lerp( _list1[i], _list2[i], _amount ) );
		}

		return tmpOut;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static float getTipAsNormalizedCoordinate( List<Vector3> _list )
	{
		float length = -9999.0f;
		float highest = -9999.0f;
		float highestX = 0f;
		for (int i = 0; i < _list.Count; i++)
		{
			Vector3 p = _list[i];

			//Debug.Log(p);

			length = Mathf.Max(length, p.x);

			if (p.y >= highest )
			{
				highest = p.y;
				highestX = p.x;
            }
		}

		return highestX / length;
	}

	// --------------------------------------------------------------------------------------------------------
	// Get the point that equals the input _x together with the Y and Z that lie on the line _points make up
	//
	public static Vector3 getPointAtClosestX( float _x, List<Vector3> _points )
	{
		if ( _points.Count == 0 ) { return Vector3.zero; }
		if ( _points.Count == 1 ) { return _points[0]; }

		for( int i = 0; i < _points.Count; i++ )
		{
			if( _points[i].x > _x && i > 0)
			{
				Vector3 p0 = _points[i-1];
				Vector3 p1 = _points[i  ];
				float frac = MathUtils.Map( _x, p0.x, p1.x, 0f, 1f );
				//Debug.Log( "_x " + _x + "	p0.x " + p0.x + "	p1.x " + p1.x + "	frac " + frac);
				return Vector3.Lerp(p0, p1, frac); // !! Returning here as soon as point is found, be careful with any code below this line
			}
		}

		// We have at least two points if we get here
		// TODO: what should we return here?
		return Vector3.Lerp(_points[0], _points[1], 0.5f );
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> multiplyAlongX( List<Vector3> _inputPoints, List<Vector3> _controlPoints, bool _useX = true, bool _useY = true, bool _useZ = true )
	{
		List<Vector3> tmpOut = new List<Vector3>();

		//Debug.Log("multiplyAlongX " + _inputPoints.Count + " " + _controlPoints.Count );

		for (int i = 0; i < _inputPoints.Count; i++)
		{
			Vector3 controlPoint = getPointAtClosestX( _inputPoints[i].x, _controlPoints );

			if (!_useX) controlPoint.x = 1f;
			if (!_useY) controlPoint.y = 1f;
			if (!_useZ) controlPoint.z = 1f;

			tmpOut.Add(Vector3.Scale( _inputPoints[i], controlPoint ));
		}

		return tmpOut;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static void DebugDraw(ref List<Vector3> _points, Color _col )
	{
		DebugDraw( ref _points, _col, Vector3.zero );
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static void DebugDraw( ref List<Vector3> _points, Color _col, Vector3 _offset, bool _drawTicks = true )
	{
		if (_points.Count > 1)
		{float tickSize = 0.05f;
			
			for (int i = 0; i < _points.Count - 1; i++)
			{
				Vector3 p0 = _points[i    ] + _offset;
				Vector3 p1 = _points[i + 1] + _offset;
				Debug.DrawLine( p0, p1, _col);

				if( _drawTicks )
				{
					Debug.DrawLine( p0 - new Vector3(0f,tickSize,0f), p0 + new Vector3(0f,tickSize, 0f), _col);
					Debug.DrawLine( p0 - new Vector3(0f,0f,tickSize), p0 + new Vector3(0f,0f,tickSize), _col);
				}
			}
		}
	}
}
