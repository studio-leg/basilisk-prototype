using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// --------------------------------------------------------------------------------------------------------
//
public class CircumferenceShapeUtils 
{

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetCircle( int _res, float _maxRadius, float _angOffset = 0.0f, float maxAngle = 1f )
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, maxAngle);
			float ang = (frac * Mathf.PI * 2.0f) + _angOffset;

			float currRad = _maxRadius;

			Vector3 tmp;
			tmp.x = Mathf.Cos(ang) * currRad;
			tmp.y = Mathf.Sin(ang) * currRad;
			tmp.z = 0.0f;

			p.Add( tmp );
		}

		return p;
	}

    public static List<Vector3> GetShapeFromCircle(int numPoints, int res, float _maxRadius, float _angOffset = 0.0f, float maxAngle = 1f)
    {
        // Generate shape corners / control points
        List<Vector3> controlPoints = new List<Vector3>();
        float meanDistance = maxAngle / (float)(numPoints);
        float maxVariation = 0f;//meanDistance * 0.1f;
        float fraction = _angOffset;
        for (int i = 0; i < numPoints; i++)
        {
            float ang = (fraction * Mathf.PI * 2.0f);
            float currRad = _maxRadius;
            Vector3 tmp;
            tmp.x = Mathf.Cos(ang) * currRad;
            tmp.y = Mathf.Sin(ang) * currRad;
            tmp.z = 0.0f;
            controlPoints.Add(tmp);
            fraction += (meanDistance + UnityEngine.Random.Range(-maxVariation, maxVariation));
        }
        controlPoints.Add(controlPoints[0]);

        // Now add points along lines between control points
        List<Vector3> points = new List<Vector3>();
        int lineRes = res / numPoints;
        float lineStep = 1f / (float)lineRes;
        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            var p1 = controlPoints[i];
            var p2 = controlPoints[i+1];
            var direction = p2 - p1;
            for (int j = 0; j < lineRes; j++)
            {
                float percent = (float)j * lineStep;
                points.Add(p1 + direction * percent);
            }
        }
        return points;
    }

    // --------------------------------------------------------------------------------------------------------
    //
    public static List<Vector3> GetRidgedCircle( int _res, int _numRidges, float _minRadius, float _maxRadius, float _angOffset = 0.0f, float maxAngle = 1f)
	{
		List<Vector3> p = new List<Vector3>();

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, maxAngle);
			float ang = (frac * Mathf.PI * 2.0f) + _angOffset;

			float currRad = MathUtils.Map(Mathf.Cos((frac * (float)_numRidges) * (Mathf.PI * 2.0f)), -1.0f, 1.0f, _minRadius, _maxRadius );

			Vector3 tmp;
			tmp.x = Mathf.Cos(ang) * currRad;
			tmp.y = Mathf.Sin(ang) * currRad;
			tmp.z = 0.0f;

			p.Add( tmp );
		}

		return p;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetSmothstepPulseCircle( int _res, int _numPulses, float _minRadius, float _maxRadius, 
														 float _low0, float _high0, float _high1, float _low1, float _angOffset = 0.0f, float maxAngle = 1f)
	{
		List<Vector3> p = new List<Vector3>();
		float pulseWidth = (1.0f / (float)_numPulses);

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, maxAngle);
			float pulseFrac = (frac % pulseWidth) / pulseWidth;
			float pulse = MathUtils.SmoothStepInOut( _low0, _high0, _high1, _low1, pulseFrac );

			float ang = (frac * Mathf.PI * 2.0f) + _angOffset;

			float currRad = MathUtils.Map( pulse, 0.0f, 1.0f, _minRadius, _maxRadius );

			Vector3 tmp;
			tmp.x = Mathf.Cos(ang) * currRad;
			tmp.y = Mathf.Sin(ang) * currRad;
			tmp.z = 0.0f;

			p.Add( tmp );
		}

		return p;
	}

	// --------------------------------------------------------------------------------------------------------
	//
	public static List<Vector3> GetLinearstepPulseCircle( int _res, int _numPulses, float _minRadius, float _maxRadius, 
														  float _low0, float _high0, float _high1, float _low1, float _angOffset = 0.0f, float maxAngle = 1f)
	{
		List<Vector3> p = new List<Vector3>();
		float pulseWidth = (1.0f / (float)_numPulses);

		for (int i = 0; i < _res; i++)
		{
			float frac = MathUtils.Map(i, 0, _res - 1, 0.0f, maxAngle);
			float pulseFrac = (frac % pulseWidth) / pulseWidth;
			float pulse = MathUtils.LinearStepInOut( _low0, _high0, _high1, _low1, pulseFrac );

			float ang = (frac * Mathf.PI * 2.0f) + _angOffset;

			float currRad = MathUtils.Map( pulse, 0.0f, 1.0f, _minRadius, _maxRadius );

			Vector3 tmp;
			tmp.x = Mathf.Cos(ang) * currRad;
			tmp.y = Mathf.Sin(ang) * currRad;
			tmp.z = 0.0f;

			p.Add( tmp );
		}

		return p;
	}

}