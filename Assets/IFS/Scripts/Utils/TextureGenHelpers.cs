using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureGenHelpers  
{
	// -----------------------------------------
	public static void DrawRect( Rect _rect, Material _mat ) 
	{
		GL.Begin(GL.QUADS);

			_mat.SetPass(0);
			//GL.Color(_color);

			GL.Vertex3( _rect.x, 			   _rect.y, 				0);
			GL.Vertex3( _rect.x + _rect.width, _rect.y, 				0);
			GL.Vertex3( _rect.x + _rect.width, _rect.y + _rect.height,  0);
			GL.Vertex3( _rect.x, 			   _rect.y + _rect.height,  0);

		GL.End();
	}

    // -----------------------------------------
    public static void DrawCurvedLine(List<Vector3> points, Material _mat)
    {
        GL.Begin(GL.LINES);
        _mat.SetPass(0);
        for (int i = 0; i < points.Count-1; i++)
        {
            var p1 = points[i];
            var p2 = points[i+1];
            GL.Vertex3(p1.x, p1.y, 0);
            GL.Vertex3(p2.x, p2.y, 0);
        }
        GL.End();
    }

    // -----------------------------------------
	public static void DrawCurvedLine(Vector3[] points, float thickness, StepFunctionParams taperStepParams, Material _mat)
    {
        // Generate a list of vertices for a triangle strip
        int n = points.Length - 1;
        Vector3[] vertices = new Vector3[n * 4];
        int vertI = 0;
        for (int i = 0; i < n; i++)
        {
            var lp1 = points[i];
            var lp2 = points[i + 1];
            var direction = (lp2 - lp1).normalized;
            //MathUtils.CircularStepInOut
            var taper = MathUtils.CircularStepInOut(taperStepParams.low0, taperStepParams.high0, taperStepParams.high1, taperStepParams.low1, (float)i / (float)n);
            //taper *= MathUtils.LinearStep(0.1f, 0.12f, taper);
            var right = Vector3.Cross(Vector3.forward, direction) * thickness * 0.5f * taper;
            if (i==0)
            {
                vertices[vertI++] = lp1 + right;
                vertices[vertI++] = lp1 - right;
            }
            vertices[vertI++] = lp2 + right;
            vertices[vertI++] = lp2 - right;
        }

        // Draw a thick line as a triangle strip
        GL.Begin(GL.TRIANGLE_STRIP);
        _mat.SetPass(0);
        for (int i = 0; i < vertices.Length; i++)
        {
            GL.Vertex3(vertices[i].x, vertices[i].y, 0);
        }
        GL.End();
        
    }

    // -----------------------------------------
    public static void DrawCircle( Vector2 _pos, float _radius, Material _mat ) 
	{
		int res = 30;

		_mat.SetPass(0);

		GL.Begin(GL.TRIANGLES);

			for( int i = 0; i < res - 1; i++ )
			{
				float ang 		= MathUtils.Map( (float)i,   0.0f, res-1.0f, 0.0f, Mathf.PI * 2.0f );
				float angNext 	= MathUtils.Map( (float)i+1, 0.0f, res-1.0f, 0.0f, Mathf.PI * 2.0f );

				Vector2 p0 = new Vector2( Mathf.Cos(ang)     * _radius, Mathf.Sin(ang)     * _radius );
				Vector2 p1 = new Vector2( Mathf.Cos(angNext) * _radius, Mathf.Sin(angNext) * _radius );

				GL.Vertex( _pos );
				GL.Vertex( _pos + p0 );
				GL.Vertex( _pos + p1 );
			}

		GL.End();
	}

}
