using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// --------------------------------------------------------------------------------------------------------
//
public class LengthShapeUtils 
{

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetHeights(int _res, float _length = 10f, float _height = 1f )
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, 1.0f);
			p.Add(new Vector3(frac * _length, _height, 0f));
		}

		return p;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetLine(int _res, float _length = 10f, float _startHeight = 0f, float _endHeight = 1f )
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, 1.0f);
			p.Add(new Vector3(frac * _length, MathUtils.Map( frac, 0, 1, _startHeight, _endHeight) , 0f));
		}

		return p;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetSmoothStepInOut( int _res, float _length = 10f, float _height = 1f, 
													float _low0 = 0f, float _high0 = 0.25f, float _high1 = 0.75f, float _low1 = 1f )
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, 1.0f );

			float x = frac * _length;
			float y = MathUtils.SmoothStepInOut( _low0, _high0, _high1, _low1, frac ) * _height;

			p.Add( new Vector3( x, y, 0f ) );
		}

		return p;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetLinearStepInOut(int _res, float _length = 10f, float _height = 1f,
													float _low0 = 0f, float _high0 = 0.25f, float _high1 = 0.75f, float _low1 = 1f)
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, 1.0f);

			float x = frac * _length;
			float y = MathUtils.LinearStepInOut(_low0, _high0, _high1, _low1, frac) * _height;

			p.Add(new Vector3(x, y, 0f));
		}

		return p;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetLinearStep( int _res, float _length = 10f, float _height = 1f,
											   float _low0 = 0f, float _high0 = 1.0f )
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, 1.0f);

			float x = frac * _length;
			float y = MathUtils.LinearStep(_low0, _high0, frac) * _height;

			p.Add(new Vector3(x, y, 0f));
		}

		return p;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetNoise( int _res, float _length = 10f, float _height = 1f, float _frequency = 1.0f, float _offset = 0.0f )
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, 1.0f);

			float x = frac * _length;
			float y = Mathf.PerlinNoise( _offset + (x * _frequency), 0f) * _height;

			p.Add(new Vector3(x, y, 0f));
		}

		return p;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetCircularStepInOut( int _res, float _length = 10f, float _height = 1f, 
													  float _low0 = 0f, float _high0 = 0.25f, float _high1 = 0.75f, float _low1 = 1f )
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, 1.0f );
            frac = MathUtils.Smoothstep(0.0f, 0.5f, frac);
            float x = frac * _length;
			float y = MathUtils.CircularStepInOut( _low0, _high0, _high1, _low1, frac ) * _height;

			p.Add( new Vector3( x, y, 0f ) );
		}

		return p;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetEquallySpacedSplinePoints(int _res, List<Vector3> _controlPoints )
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			p.Add( Spline.getSplinePointAt( MathUtils.Map(i, 0, _res - 1, 0.0f, 1.0f), _controlPoints) );
		}
		// I'm getting one point too many for some reason
		return Spline.getResampledByCount( p, p.Count-1 );
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetEquallySpacedLinearPoints(int _res, List<Vector3> _controlPoints )
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			p.Add( Spline.getLinearPointAt( MathUtils.Map(i, 0, _res - 1, 0.0f, 1.0f), _controlPoints) );
		}
			
		// I'm getting one point too many for some reason
		return Spline.getResampledByCount( p, p.Count-1 );
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetLinearStepPoints(int _res, float _length, float _low0, float _high0 )
	{
		List<Vector3> p = new List<Vector3>();
		for( int i = 0; i < _res; i++ )
		{
			float tmpX = MathUtils.Map( i, 0, _res-1, 0f, _length );
			float tmpY = MathUtils.LinearStep( _low0 * _length, _high0 * _length, tmpX );
			p.Add( new Vector3( tmpX, tmpY, 0f ));
		}
		return p;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetLinearStepOutPoints(int _res, float _length, float _high1, float _low1 )
	{
		List<Vector3> p = new List<Vector3>();
		for( int i = 0; i < _res; i++ )
		{
			float tmpX = MathUtils.Map( i, 0, _res-1, 0f, _length );
			float tmpY = 1f - MathUtils.LinearStep( _high1 * _length, _low1 * _length, tmpX );
			p.Add( new Vector3( tmpX, tmpY, 0f ));
		}
		return p;
	}
}