using UnityEngine;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class GLDebug : MonoBehaviour
{
	private struct Line
	{
		public Vector3 start;
		public Vector3 end;
		public Color color;
		public float startTime;
		public float duration;
		
		public Line (Vector3 start, Vector3 end, Color color, float startTime, float duration)
		{
			this.start = start;
			this.end = end;
			this.color = color;
			this.startTime = startTime;
			this.duration = duration;
		}
		
		public bool DurationElapsed (bool drawLine)
		{
			if (drawLine)
			{
				GL.Color (color);
				GL.Vertex (start);
				GL.Vertex (end);
			}
			return Time.time - startTime >= duration;
		}
	}

    private struct Quad
    {
        public Vector3 topLeft;
        public float width;
        public float height;
        public Color color;
        public float startTime;
        public float duration;

        public Quad(Vector3 topLeft, float width, float height, Color color, float startTime, float duration)
        {
            this.topLeft = topLeft;
            this.width = width;
            this.height = height;
            this.color = color;
            this.startTime = startTime;
            this.duration = duration;
        }

        public bool DurationElapsed(bool drawQuad)
        {
            if (drawQuad)
            {
                GL.Color(color);
                GL.Vertex3(topLeft.x, 0, topLeft.z);
                GL.Vertex3(topLeft.x + width, 0, topLeft.z);
                GL.Vertex3(topLeft.x + width, 0, topLeft.z + height);
                GL.Vertex3(topLeft.x, 0, topLeft.z + height);
            }
            return Time.time - startTime >= duration;
        }
    }

    private static GLDebug instance;
	private static Material matZOn;
	private static Material matZOff;
	
	public KeyCode toggleKey;
	public bool displayLines = true;
	public bool displayQuads = true;
#if UNITY_EDITOR
    public bool displayGizmos = true;
#endif
	//public ScreenRect rect = new ScreenRect (0, 0, 150, 20);
	
	private List<Line> linesZOn;
	private List<Line> linesZOff;
    private List<Quad> quadsZOn;
    private List<Quad> quadsZOff;
    private float milliseconds;
	
	void Awake ()
	{
		if (instance)
		{
			DestroyImmediate (this);
			return;
		}
		instance = this;
		SetMaterial ();
		linesZOn = new List<Line> ();
		linesZOff = new List<Line> ();
        quadsZOn = new List<Quad>();
        quadsZOff = new List<Quad>();
    }
	
	void SetMaterial ()
	{
        matZOn = new Material(Shader.Find("Unlit/GLDebugShader_On"))
        {
            hideFlags = HideFlags.HideAndDontSave
        };

        matZOff = new Material(Shader.Find("Unlit/GLDebugShader_Off"))
        {
            hideFlags = HideFlags.HideAndDontSave
        };
    }
	
	void Update ()
	{
		if (Input.GetKeyDown (toggleKey))
			displayLines = !displayLines;
		
		if (!displayLines)
		{
			Stopwatch timer = Stopwatch.StartNew ();
			
			linesZOn = linesZOn.Where (l => !l.DurationElapsed (false)).ToList ();
			linesZOff = linesZOff.Where (l => !l.DurationElapsed (false)).ToList ();
			
			timer.Stop ();
			milliseconds = timer.Elapsed.Ticks / 10000f;
		}
	}
	
	/*void OnGUI ()
	{
		GUI.Label (rect, "GLDebug : " + milliseconds.ToString ("f") + " ms");
	}*/
	
#if UNITY_EDITOR
	void OnDrawGizmos ()
	{
		if (!displayGizmos || !Application.isPlaying)
			return;
		for (int i = 0; i < linesZOn.Count; i++)
		{
			Gizmos.color = linesZOn[i].color;
			Gizmos.DrawLine (linesZOn[i].start, linesZOn[i].end);
		}
		for (int i = 0; i < linesZOff.Count; i++)
		{
			Gizmos.color = linesZOff[i].color;
			Gizmos.DrawLine (linesZOff[i].start, linesZOff[i].end);
		}
	}
#endif
	
	void OnPostRender ()
	{
		if (!displayLines && !displayQuads) return;
		
		Stopwatch timer = Stopwatch.StartNew ();

        if (displayLines)
        {
            matZOn.SetPass(0);
            GL.Begin(GL.LINES);
            linesZOn = linesZOn.Where(l => !l.DurationElapsed(true)).ToList();
            GL.End();

            matZOff.SetPass(0);
            GL.Begin(GL.LINES);
            linesZOff = linesZOff.Where(l => !l.DurationElapsed(true)).ToList();
            GL.End();
        }

        if (displayQuads)
        {
            matZOn.SetPass(0);
            GL.Begin(GL.QUADS);
            quadsZOn = quadsZOn.Where(q => !q.DurationElapsed(true)).ToList();
            GL.End();

            matZOff.SetPass(0);
            GL.Begin(GL.QUADS);
            quadsZOff = quadsZOff.Where(q => !q.DurationElapsed(true)).ToList();
            GL.End();
        }

        timer.Stop ();
		milliseconds = timer.Elapsed.Ticks / 10000f;
	}

	private static void DrawLine (Vector3 start, Vector3 end, Color color, float duration = 0, bool depthTest = false)
	{
		if (duration == 0 && !instance.displayLines)
			return;
		if (start == end)
			return;
		if (depthTest)
			instance.linesZOn.Add (new Line (start, end, color, Time.time, duration));
		else
			instance.linesZOff.Add (new Line (start, end, color, Time.time, duration));
	}

    private static void DrawQuad(Vector3 topLeft, float width, float height, Color color, float duration = 0, bool depthTest = false)
    {
        if (duration == 0 && !instance.displayQuads)
            return;
        if (width == 0 || height == 0)
            return;
        if (depthTest)
            instance.quadsZOn.Add(new Quad(topLeft, width, height, color, Time.time, duration));
        else
            instance.quadsZOff.Add(new Quad(topLeft, width, height, color, Time.time, duration));
    }

    /// <summary>
    /// Draw a quad starting at the top left corner, with a certain width and height, with color for a duration of time and with or without depth testing.
    /// If duration is 0 then the quad is rendered 1 frame.
    /// </summary>
    /// <param name="topLeft">Point in world space representing the top left corner of the quad.</param>
    /// <param name="width">Width of the quad.</param>
    /// <param name="height">Height of the quad.</param>
    /// <param name="color">Color of the quad.</param>
    /// <param name="duration">How long the quad should be visible for.</param>
    /// <param name="depthTest">Should the quad be obscured by objects closer to the camera ?</param>
    public static void DrawQuad(Vector3 topLeft, float width, float height, Color? color = null, float duration = 0, bool depthTest = false)
    {
        DrawQuad(topLeft, width, height, color ?? Color.white, duration, depthTest);
    }

    /// <summary>
    /// Draw a line from start to end with color for a duration of time and with or without depth testing.
    /// If duration is 0 then the line is rendered 1 frame.
    /// </summary>
    /// <param name="start">Point in world space where the line should start.</param>
    /// <param name="end">Point in world space where the line should end.</param>
    /// <param name="color">Color of the line.</param>
    /// <param name="duration">How long the line should be visible for.</param>
    /// <param name="depthTest">Should the line be obscured by objects closer to the camera ?</param>
    public static void DrawLine (Vector3 start, Vector3 end, Color? color = null, float duration = 0, bool depthTest = false)
	{
		DrawLine (start, end, color ?? Color.white, duration, depthTest);
	}

	/// <summary>
	/// Draw a line from start to start + dir with color for a duration of time and with or without depth testing.
	/// If duration is 0 then the ray is rendered 1 frame.
	/// </summary>
	/// <param name="start">Point in world space where the ray should start.</param>
	/// <param name="dir">Direction and length of the ray.</param>
	/// <param name="color">Color of the ray.</param>
	/// <param name="duration">How long the ray should be visible for.</param>
	/// <param name="depthTest">Should the ray be obscured by objects closer to the camera ?</param>
	public static void DrawRay (Vector3 start, Vector3 dir, Color? color = null, float duration = 0, bool depthTest = false)
	{
		if (dir == Vector3.zero)
			return;
		DrawLine (start, start + dir, color, duration, depthTest);
	}
	
	/// <summary>
	/// Draw an arrow from start to end with color for a duration of time and with or without depth testing.
	/// If duration is 0 then the arrow is rendered 1 frame.
	/// </summary>
	/// <param name="start">Point in world space where the arrow should start.</param>
	/// <param name="end">Point in world space where the arrow should end.</param>
	/// <param name="arrowHeadLength">Length of the 2 lines of the head.</param>
	/// <param name="arrowHeadAngle">Angle between the main line and each of the 2 smaller lines of the head.</param>
	/// <param name="color">Color of the arrow.</param>
	/// <param name="duration">How long the arrow should be visible for.</param>
	/// <param name="depthTest">Should the arrow be obscured by objects closer to the camera ?</param>
	public static void DrawLineArrow (Vector3 start, Vector3 end, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20, Color? color = null, float duration = 0, bool depthTest = false)
	{
		DrawArrow (start, end - start, arrowHeadLength, arrowHeadAngle, color, duration, depthTest);
	}
	
	/// <summary>
	/// Draw an arrow from start to start + dir with color for a duration of time and with or without depth testing.
	/// If duration is 0 then the arrow is rendered 1 frame.
	/// </summary>
	/// <param name="start">Point in world space where the arrow should start.</param>
	/// <param name="dir">Direction and length of the arrow.</param>
	/// <param name="arrowHeadLength">Length of the 2 lines of the head.</param>
	/// <param name="arrowHeadAngle">Angle between the main line and each of the 2 smaller lines of the head.</param>
	/// <param name="color">Color of the arrow.</param>
	/// <param name="duration">How long the arrow should be visible for.</param>
	/// <param name="depthTest">Should the arrow be obscured by objects closer to the camera ?</param>
	public static void DrawArrow (Vector3 start, Vector3 dir, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20, Color? color = null, float duration = 0, bool depthTest = false)
	{
		if (dir == Vector3.zero)
			return;
		DrawRay (start, dir, color, duration, depthTest);
		Vector3 right = Quaternion.LookRotation (dir) * Quaternion.Euler (0, 180 + arrowHeadAngle, 0) * Vector3.forward;
		Vector3 left  = Quaternion.LookRotation (dir) * Quaternion.Euler (0, 180 - arrowHeadAngle, 0) * Vector3.forward;
		DrawRay (start + dir, right * arrowHeadLength, color, duration, depthTest);
		DrawRay (start + dir, left  * arrowHeadLength, color, duration, depthTest);
	}
	
	/// <summary>
	/// Draw a square with color for a duration of time and with or without depth testing.
	/// If duration is 0 then the square is renderer 1 frame.
	/// </summary>
	/// <param name="pos">Center of the square in world space.</param>
	/// <param name="rot">Rotation of the square in euler angles in world space.</param>
	/// <param name="scale">Size of the square.</param>
	/// <param name="color">Color of the square.</param>
	/// <param name="duration">How long the square should be visible for.</param>
	/// <param name="depthTest">Should the square be obscured by objects closer to the camera ?</param>
	public static void DrawSquare (Vector3 pos, Vector3? rot = null, Vector3? scale = null, Color? color = null, float duration = 0, bool depthTest = false)
	{
		DrawSquare (Matrix4x4.TRS (pos, Quaternion.Euler (rot ?? Vector3.zero), scale ?? Vector3.one), color, duration, depthTest);
	}
	/// <summary>
	/// Draw a square with color for a duration of time and with or without depth testing.
	/// If duration is 0 then the square is renderer 1 frame.
	/// </summary>
	/// <param name="pos">Center of the square in world space.</param>
	/// <param name="rot">Rotation of the square in world space.</param>
	/// <param name="scale">Size of the square.</param>
	/// <param name="color">Color of the square.</param>
	/// <param name="duration">How long the square should be visible for.</param>
	/// <param name="depthTest">Should the square be obscured by objects closer to the camera ?</param>
	public static void DrawSquare (Vector3 pos, Quaternion? rot = null, Vector3? scale = null, Color? color = null, float duration = 0, bool depthTest = false)
	{
		DrawSquare (Matrix4x4.TRS (pos, rot ?? Quaternion.identity, scale ?? Vector3.one), color, duration, depthTest);
	}
	/// <summary>
	/// Draw a square with color for a duration of time and with or without depth testing.
	/// If duration is 0 then the square is renderer 1 frame.
	/// </summary>
	/// <param name="matrix">Transformation matrix which represent the square transform.</param>
	/// <param name="color">Color of the square.</param>
	/// <param name="duration">How long the square should be visible for.</param>
	/// <param name="depthTest">Should the square be obscured by objects closer to the camera ?</param>
	public static void DrawSquare (Matrix4x4 matrix, Color? color = null, float duration = 0, bool depthTest = false)
	{
		Vector3
			p_1	= matrix.MultiplyPoint3x4 (new Vector3 ( .5f, 0,  .5f)),
			p_2	= matrix.MultiplyPoint3x4 (new Vector3 ( .5f, 0, -.5f)),
			p_3	= matrix.MultiplyPoint3x4 (new Vector3 (-.5f, 0, -.5f)),
			p_4	= matrix.MultiplyPoint3x4 (new Vector3 (-.5f, 0,  .5f));
		
		DrawLine (p_1, p_2, color, duration, depthTest);
		DrawLine (p_2, p_3, color, duration, depthTest);
		DrawLine (p_3, p_4, color, duration, depthTest);
		DrawLine (p_4, p_1, color, duration, depthTest);
	}
	
	/// <summary>
	/// Draw a cube with color for a duration of time and with or without depth testing.
	/// If duration is 0 then the square is renderer 1 frame.
	/// </summary>
	/// <param name="pos">Center of the cube in world space.</param>
	/// <param name="rot">Rotation of the cube in euler angles in world space.</param>
	/// <param name="scale">Size of the cube.</param>
	/// <param name="color">Color of the cube.</param>
	/// <param name="duration">How long the cube should be visible for.</param>
	/// <param name="depthTest">Should the cube be obscured by objects closer to the camera ?</param>
	public static void DrawCube (Vector3 pos, Vector3? rot = null, Vector3? scale = null, Color? color = null, float duration = 0, bool depthTest = false)
	{
		DrawCube (Matrix4x4.TRS (pos, Quaternion.Euler (rot ?? Vector3.zero), scale ?? Vector3.one), color, duration, depthTest);
	}
	/// <summary>
	/// Draw a cube with color for a duration of time and with or without depth testing.
	/// If duration is 0 then the square is renderer 1 frame.
	/// </summary>
	/// <param name="pos">Center of the cube in world space.</param>
	/// <param name="rot">Rotation of the cube in world space.</param>
	/// <param name="scale">Size of the cube.</param>
	/// <param name="color">Color of the cube.</param>
	/// <param name="duration">How long the cube should be visible for.</param>
	/// <param name="depthTest">Should the cube be obscured by objects closer to the camera ?</param>
	public static void DrawCube (Vector3 pos, Quaternion? rot = null, Vector3? scale = null, Color? color = null, float duration = 0, bool depthTest = false)
	{
		DrawCube (Matrix4x4.TRS (pos, rot ?? Quaternion.identity, scale ?? Vector3.one), color, duration, depthTest);
	}
	/// <summary>
	/// Draw a cube with color for a duration of time and with or without depth testing.
	/// If duration is 0 then the square is renderer 1 frame.
	/// </summary>
	/// <param name="matrix">Transformation matrix which represent the cube transform.</param>
	/// <param name="color">Color of the cube.</param>
	/// <param name="duration">How long the cube should be visible for.</param>
	/// <param name="depthTest">Should the cube be obscured by objects closer to the camera ?</param>
	public static void DrawCube (Matrix4x4 matrix, Color? color = null, float duration = 0, bool depthTest = false)
	{
		Vector3
			down_1	= matrix.MultiplyPoint3x4 (new Vector3 ( .5f, -.5f,  .5f)),
			down_2	= matrix.MultiplyPoint3x4 (new Vector3 ( .5f, -.5f, -.5f)),
			down_3	= matrix.MultiplyPoint3x4 (new Vector3 (-.5f, -.5f, -.5f)),
			down_4	= matrix.MultiplyPoint3x4 (new Vector3 (-.5f, -.5f,  .5f)),
			up_1	= matrix.MultiplyPoint3x4 (new Vector3 ( .5f,  .5f,  .5f)),
			up_2	= matrix.MultiplyPoint3x4 (new Vector3 ( .5f,  .5f, -.5f)),
			up_3	= matrix.MultiplyPoint3x4 (new Vector3 (-.5f,  .5f, -.5f)),
			up_4	= matrix.MultiplyPoint3x4 (new Vector3 (-.5f,  .5f,  .5f));
		
		DrawLine (down_1, down_2, color, duration, depthTest);
		DrawLine (down_2, down_3, color, duration, depthTest);
		DrawLine (down_3, down_4, color, duration, depthTest);
		DrawLine (down_4, down_1, color, duration, depthTest);
		
		DrawLine (down_1, up_1, color, duration, depthTest);
		DrawLine (down_2, up_2, color, duration, depthTest);
		DrawLine (down_3, up_3, color, duration, depthTest);
		DrawLine (down_4, up_4, color, duration, depthTest);
		
		DrawLine (up_1, up_2, color, duration, depthTest);
		DrawLine (up_2, up_3, color, duration, depthTest);
		DrawLine (up_3, up_4, color, duration, depthTest);
		DrawLine (up_4, up_1, color, duration, depthTest);
	}

    /// <summary>
	/// Draw a circle with color for a duration of time and with or without depth testing.
	/// If duration is 0 then the circle is renderer 1 frame.
	/// </summary>
	/// <param name="center">Center position of the circle.</param>
	/// <param name="radius">Radius of the circle.</param>
	/// <param name="color">Color of the cube.</param>
	/// <param name="duration">How long the cube should be visible for.</param>
	/// <param name="depthTest">Should the cube be obscured by objects closer to the camera ?</param>
	public static void DrawCircle2d(Vector3 center, float radius, Color? color = null, float duration = 0, bool depthTest = false)
    {
        int numSegments = 15;
        float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
        float theta = 0f;

        // first point
        float x = radius * Mathf.Cos(theta);
        float z = radius * Mathf.Sin(theta);
        Vector3 previousPoint = center + new Vector3(x, 0, z);
        theta += deltaTheta;

        for (int i = 0; i < numSegments + 1; i++)
        {
            x = radius * Mathf.Cos(theta);
            z = radius * Mathf.Sin(theta);
            Vector3 currentPoint = center + new Vector3(x, 0, z);

            DrawLine(previousPoint, currentPoint, color, duration, depthTest);

            theta += deltaTheta;
            previousPoint = currentPoint;
        }
    }
}