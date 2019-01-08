//
// System.Drawing.Drawing2D.GraphicsPath.cs
//
// Authors:
//
//   Miguel de Icaza (miguel@ximian.com)
//   Duncan Mak (duncan@ximian.com)
//   Jordi Mas i Hernandez (jordi@ximian.com)
//   Ravindra (rkumar@novell.com)
//   Sebastien Pouliot  <sebastien@ximian.com>
//
// Copyright (C) 2004,2006-2007 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	public sealed class GraphicsPath : MarshalByRefObject, ICloneable, IDisposable
	{
		// 1/4 is the FlatnessDefault as defined in GdiPlusEnums.h
		private const float FlatnessDefault = 1.0f / 4.0f;

		internal IntPtr nativePath = IntPtr.Zero;

		GraphicsPath (IntPtr ptr)
		{
			nativePath = ptr;
		}

		public GraphicsPath ()
		{
                        Status status = GDIPlus.GdipCreatePath (FillMode.Alternate, out nativePath);
                        GDIPlus.CheckStatus (status);
		}

		public GraphicsPath (FillMode fillMode)
		{
                        Status status = GDIPlus.GdipCreatePath (fillMode, out nativePath);
                        GDIPlus.CheckStatus (status);
		}

		public GraphicsPath (Point_[] pts, byte[] types)
			: this (pts, types, FillMode.Alternate)
		{
		}

		public GraphicsPath (PointF_[] pts, byte[] types)
			: this (pts, types, FillMode.Alternate)
		{
		}

		public GraphicsPath (Point_[] pts, byte[] types, FillMode fillMode)
		{
			if (pts == null)
				throw new ArgumentNullException ("pts");
			if (pts.Length != types.Length)
				throw new ArgumentException ("Invalid parameter passed. Number of points and types must be same.");

			Status status = GDIPlus.GdipCreatePath2I (pts, types, pts.Length, fillMode, out nativePath);
			GDIPlus.CheckStatus (status);
		}

		public GraphicsPath (PointF_[] pts, byte[] types, FillMode fillMode)
		{
			if (pts == null)
				throw new ArgumentNullException ("pts");
			if (pts.Length != types.Length)
				throw new ArgumentException ("Invalid parameter passed. Number of points and types must be same.");

			Status status = GDIPlus.GdipCreatePath2 (pts, types, pts.Length, fillMode, out nativePath);
			GDIPlus.CheckStatus (status);
		}
	
                public object Clone ()
                {
                        IntPtr clone;

                        Status status = GDIPlus.GdipClonePath (nativePath, out clone);
                        GDIPlus.CheckStatus (status);                      	

                        return new GraphicsPath (clone);
                }

                public void Dispose ()
                {
                        Dispose (true);
                        System.GC.SuppressFinalize (this);
                }

                ~GraphicsPath ()
                {
                        Dispose (false);
                }
                
		void Dispose (bool disposing)
		{
			Status status;
			if (nativePath != IntPtr.Zero) {
				status = GDIPlus.GdipDeletePath (nativePath);
				GDIPlus.CheckStatus (status);

				nativePath = IntPtr.Zero;
			}
		}

		public FillMode FillMode {
			get {
				FillMode mode;
				Status status = GDIPlus.GdipGetPathFillMode (nativePath, out mode);
				GDIPlus.CheckStatus (status);

				return mode;
			}
			set {
				if ((value < FillMode.Alternate) || (value > FillMode.Winding))
					throw new InvalidEnumArgumentException ("FillMode", (int)value, typeof (FillMode));

				Status status = GDIPlus.GdipSetPathFillMode (nativePath, value);
				GDIPlus.CheckStatus (status);
			}
		}

		public PathData PathData {
			get {
				int count;
				Status status = GDIPlus.GdipGetPointCount (nativePath, out count);
				GDIPlus.CheckStatus (status);

				PointF_ [] points = new PointF_ [count];
				byte [] types = new byte [count];

				// status would fail if we ask points or types with a 0 count
				// anyway that would only mean two unrequired unmanaged calls
				if (count > 0) {
					status = GDIPlus.GdipGetPathPoints (nativePath, points, count);
					GDIPlus.CheckStatus (status);

					status = GDIPlus.GdipGetPathTypes (nativePath, types, count);
					GDIPlus.CheckStatus (status);
				}

				PathData pdata = new PathData ();
				pdata.Points = points;
				pdata.Types = types;
				return pdata;
			}
		}

		public PointF_ [] PathPoints {
			get {
				int count;
				Status status = GDIPlus.GdipGetPointCount (nativePath, out count);
				GDIPlus.CheckStatus (status);
				if (count == 0)
					throw new ArgumentException ("PathPoints");

				PointF_ [] points = new PointF_ [count];
				status = GDIPlus.GdipGetPathPoints (nativePath, points, count); 
				GDIPlus.CheckStatus (status);		      	

				return points;
			}
		}

		public byte [] PathTypes {
			get {
				int count;
				Status status = GDIPlus.GdipGetPointCount (nativePath, out count);
				GDIPlus.CheckStatus (status);
				if (count == 0)
					throw new ArgumentException ("PathTypes");

				byte [] types = new byte [count];
				status = GDIPlus.GdipGetPathTypes (nativePath, types, count);
				GDIPlus.CheckStatus (status);

				return types;
			}
		}

		public int PointCount {
			get {
				int count;
				Status status = GDIPlus.GdipGetPointCount (nativePath, out count);
				GDIPlus.CheckStatus (status);

				return count;
			}
		}

		internal IntPtr NativeObject {
			get {
				return nativePath;
			}
			set {
				nativePath = value;
			}
		}

                //
                // AddArc
                //
                public void AddArc (Rectangle_ rect, float start_angle, float sweep_angle)
                {
                        Status status = GDIPlus.GdipAddPathArcI (nativePath, rect.X, rect.Y, rect.Width, rect.Height, start_angle, sweep_angle);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddArc (RectangleF_ rect, float start_angle, float sweep_angle)
                {
                        Status status = GDIPlus.GdipAddPathArc (nativePath, rect.X, rect.Y, rect.Width, rect.Height, start_angle, sweep_angle);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddArc (int x, int y, int width, int height, float start_angle, float sweep_angle)
                {
                        Status status = GDIPlus.GdipAddPathArcI (nativePath, x, y, width, height, start_angle, sweep_angle);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddArc (float x, float y, float width, float height, float start_angle, float sweep_angle)
                {
                        Status status = GDIPlus.GdipAddPathArc (nativePath, x, y, width, height, start_angle, sweep_angle);
                        GDIPlus.CheckStatus (status);                      	
                }

                //
                // AddBezier
                //
                public void AddBezier (Point_ pt1, Point_ pt2, Point_ pt3, Point_ pt4)
                {
                        Status status = GDIPlus.GdipAddPathBezierI (nativePath, pt1.X, pt1.Y,
                                        pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
                                        
			GDIPlus.CheckStatus (status);                      		                                      
                }

                public void AddBezier (PointF_ pt1, PointF_ pt2, PointF_ pt3, PointF_ pt4)
                {
                        Status status = GDIPlus.GdipAddPathBezier (nativePath, pt1.X, pt1.Y,
                                        pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
                                        
			GDIPlus.CheckStatus (status);                      	                                       
                }

                public void AddBezier (int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
                {
                        Status status = GDIPlus.GdipAddPathBezierI (nativePath, x1, y1, x2, y2, x3, y3, x4, y4);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddBezier (float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
                {
                        Status status = GDIPlus.GdipAddPathBezier (nativePath, x1, y1, x2, y2, x3, y3, x4, y4);
                        GDIPlus.CheckStatus (status);                      	
                }

                //
                // AddBeziers
                //
#if NET_2_0
                public void AddBeziers (params Point_ [] pts)
#else
                public void AddBeziers (Point [] pts)
#endif
                {
			if (pts == null)
				throw new ArgumentNullException ("pts");
                        Status status = GDIPlus.GdipAddPathBeziersI (nativePath, pts, pts.Length);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddBeziers (PointF_ [] pts)
                {
			if (pts == null)
				throw new ArgumentNullException ("pts");
                        Status status = GDIPlus.GdipAddPathBeziers (nativePath, pts, pts.Length);
                        GDIPlus.CheckStatus (status);                      	
                }

                //
                // AddEllipse
                //
                public void AddEllipse (RectangleF_ r)
                {
                        Status status = GDIPlus.GdipAddPathEllipse (nativePath, r.X, r.Y, r.Width, r.Height);
                        GDIPlus.CheckStatus (status);                      	
                }
                
                public void AddEllipse (float x, float y, float width, float height)
                {
                        Status status = GDIPlus.GdipAddPathEllipse (nativePath, x, y, width, height);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddEllipse (Rectangle_ r)
                {
                        Status status = GDIPlus.GdipAddPathEllipseI (nativePath, r.X, r.Y, r.Width, r.Height);
                        GDIPlus.CheckStatus (status);                      	
                }
                
                public void AddEllipse (int x, int y, int width, int height)
                {
                        Status status = GDIPlus.GdipAddPathEllipseI (nativePath, x, y, width, height);
                        GDIPlus.CheckStatus (status);                      	
                }
                

                //
                // AddLine
                //
                public void AddLine (Point_ a, Point_ b)
                {
                        Status status = GDIPlus.GdipAddPathLineI (nativePath, a.X, a.Y, b.X, b.Y);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddLine (PointF_ a, PointF_ b)
                {
                        Status status = GDIPlus.GdipAddPathLine (nativePath, a.X, a.Y, b.X,
                                        b.Y);
                                        
			GDIPlus.CheckStatus (status);                      	                                       
                }

                public void AddLine (int x1, int y1, int x2, int y2)
                {
                        Status status = GDIPlus.GdipAddPathLineI (nativePath, x1, y1, x2, y2);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddLine (float x1, float y1, float x2, float y2)
                {
                        Status status = GDIPlus.GdipAddPathLine (nativePath, x1, y1, x2,
                                        y2);                
                                        
			GDIPlus.CheckStatus (status);                      	                                       
                }

                //
                // AddLines
                //
		public void AddLines (Point_[] points)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			if (points.Length == 0)
				throw new ArgumentException ("points");

			Status status = GDIPlus.GdipAddPathLine2I (nativePath, points, points.Length);
			GDIPlus.CheckStatus (status);                      	
		}

		public void AddLines (PointF_[] points)
		{
			if (points == null)
				throw new ArgumentNullException ("points");
			if (points.Length == 0)
				throw new ArgumentException ("points");

			Status status = GDIPlus.GdipAddPathLine2 (nativePath, points, points.Length);
			GDIPlus.CheckStatus (status);                      	
		}
        
                //
                // AddPie
                //
                public void AddPie (Rectangle_ rect, float startAngle, float sweepAngle)
                {
                        Status status = GDIPlus.GdipAddPathPie (
                                nativePath, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddPie (int x, int y, int width, int height, float startAngle, float sweepAngle)
                {
                        Status status = GDIPlus.GdipAddPathPieI (nativePath, x, y, width, height, startAngle, sweepAngle);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddPie (float x, float y, float width, float height, float startAngle, float sweepAngle)
                {
                        Status status = GDIPlus.GdipAddPathPie (nativePath, x, y, width, height, startAngle, sweepAngle);                
                        GDIPlus.CheckStatus (status);                      	
                }

                //
                // AddPolygon
                //
                public void AddPolygon (Point_ [] points)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathPolygonI (nativePath, points, points.Length);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddPolygon (PointF_ [] points)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathPolygon (nativePath, points, points.Length);
                        GDIPlus.CheckStatus (status);                      	
                }

                //
                // AddRectangle
                //
                public void AddRectangle (Rectangle_ rect)
                {
                        Status status = GDIPlus.GdipAddPathRectangleI (nativePath, rect.X, rect.Y, rect.Width, rect.Height);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddRectangle (RectangleF_ rect)
                {
                        Status status = GDIPlus.GdipAddPathRectangle (nativePath, rect.X, rect.Y, rect.Width, rect.Height);
                        GDIPlus.CheckStatus (status);                      	
                }

                //
                // AddRectangles
                //
                public void AddRectangles (Rectangle_ [] rects)
                {
			if (rects == null)
				throw new ArgumentNullException ("rects");
			if (rects.Length == 0)
				throw new ArgumentException ("rects");

                        Status status = GDIPlus.GdipAddPathRectanglesI (nativePath, rects, rects.Length);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddRectangles (RectangleF_ [] rects)
                {
			if (rects == null)
				throw new ArgumentNullException ("rects");
			if (rects.Length == 0)
				throw new ArgumentException ("rects");

                        Status status = GDIPlus.GdipAddPathRectangles (nativePath, rects, rects.Length);
                        GDIPlus.CheckStatus (status);                      	
                }

                //
                // AddPath
                //
                public void AddPath (GraphicsPath addingPath, bool connect)
                {
			if (addingPath == null)
				throw new ArgumentNullException ("addingPath");

                        Status status = GDIPlus.GdipAddPathPath (nativePath, addingPath.nativePath, connect);
                        GDIPlus.CheckStatus (status);                      	
                }

                public PointF_ GetLastPoint ()
                {
                        PointF_ pt;
                        Status status = GDIPlus.GdipGetPathLastPoint (nativePath, out pt);
                        GDIPlus.CheckStatus (status);                      	

                        return pt;
                }

                //
                // AddClosedCurve
                //
                public void AddClosedCurve (Point_ [] points)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathClosedCurveI (nativePath, points, points.Length);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddClosedCurve (PointF_ [] points)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathClosedCurve (nativePath, points, points.Length);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddClosedCurve (Point_ [] points, float tension)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathClosedCurve2I (nativePath, points, points.Length, tension);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddClosedCurve (PointF_ [] points, float tension)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathClosedCurve2 (nativePath, points, points.Length, tension);
                        GDIPlus.CheckStatus (status);                      	
                }

                //
                // AddCurve
                //
                public void AddCurve (Point_ [] points)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathCurveI (nativePath, points, points.Length);
                        GDIPlus.CheckStatus (status);                      	
                }
                
                public void AddCurve (PointF_ [] points)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathCurve (nativePath, points, points.Length);
                        GDIPlus.CheckStatus (status);                      	
                }
                
                public void AddCurve (Point_ [] points, float tension)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathCurve2I (nativePath, points, points.Length, tension);
                        GDIPlus.CheckStatus (status);                      	
                }
                
                public void AddCurve (PointF_ [] points, float tension)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathCurve2 (nativePath, points, points.Length, tension);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void AddCurve (Point_ [] points, int offset, int numberOfSegments, float tension)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathCurve3I (nativePath, points, points.Length,
                                        offset, numberOfSegments, tension);
                                        
			GDIPlus.CheckStatus (status);                      	                                       
                }
                
                public void AddCurve (PointF_ [] points, int offset, int numberOfSegments, float tension)
                {
			if (points == null)
				throw new ArgumentNullException ("points");

                        Status status = GDIPlus.GdipAddPathCurve3 (nativePath, points, points.Length,
                                        offset, numberOfSegments, tension);
                                        
			GDIPlus.CheckStatus (status);                      	                                       
                }
                        
                public void Reset ()
                {
                        Status status = GDIPlus.GdipResetPath (nativePath);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void Reverse ()
                {
                        Status status = GDIPlus.GdipReversePath (nativePath);
                        GDIPlus.CheckStatus (status);                      	
                }

                public void Transform (Matrix matrix)
                {
			if (matrix == null)
				throw new ArgumentNullException ("matrix");

                        Status status = GDIPlus.GdipTransformPath (nativePath, matrix.nativeMatrix);
                        GDIPlus.CheckStatus (status);                      	
                }
                
		[MonoTODO ("The StringFormat parameter is ignored when using libgdiplus.")]
		public void AddString (string s, FontFamily family, int style, float emSize, Point_ origin, StringFormat format)
		{
			Rectangle_ layout = new Rectangle_ ();
			layout.X = origin.X;
			layout.Y = origin.Y;
			AddString (s, family, style, emSize, layout, format);
		}

		[MonoTODO ("The StringFormat parameter is ignored when using libgdiplus.")]
		public void AddString (string s, FontFamily family, int style, float emSize, PointF_ origin, StringFormat format)
  		{
			RectangleF_ layout = new RectangleF_ ();
			layout.X = origin.X;
			layout.Y = origin.Y;
			AddString (s, family, style, emSize, layout, format);
                }

		[MonoTODO ("The layoutRect and StringFormat parameters are ignored when using libgdiplus.")]
		public void AddString (string s, FontFamily family, int style, float emSize, Rectangle_ layoutRect, StringFormat format)
		{
			if (family == null)
				throw new ArgumentException ("family");

			IntPtr sformat = (format == null) ? IntPtr.Zero : format.NativeObject;
			// note: the NullReferenceException on s.Length is the expected (MS) exception
			Status status = GDIPlus.GdipAddPathStringI (nativePath, s, s.Length, family.NativeObject, style, emSize, ref layoutRect, sformat);
			GDIPlus.CheckStatus (status);
		}

		[MonoTODO ("The layoutRect and StringFormat parameters are ignored when using libgdiplus.")]
  		public void AddString (string s, FontFamily family, int style, float emSize, RectangleF_ layoutRect, StringFormat format)
		{
			if (family == null)
				throw new ArgumentException ("family");

			IntPtr sformat = (format == null) ? IntPtr.Zero : format.NativeObject;
			// note: the NullReferenceException on s.Length is the expected (MS) exception
			Status status = GDIPlus.GdipAddPathString (nativePath, s, s.Length, family.NativeObject, style, emSize, ref layoutRect, sformat);
			GDIPlus.CheckStatus (status);
		}

		public void ClearMarkers()               
		{
                	Status s = GDIPlus.GdipClearPathMarkers (nativePath);

                        GDIPlus.CheckStatus (s);
                }
                
		public void CloseAllFigures()
		{
                	Status s = GDIPlus.GdipClosePathFigures (nativePath);

                        GDIPlus.CheckStatus (s);
                }  	
                
		public void CloseFigure()
		{
                	Status s = GDIPlus.GdipClosePathFigure (nativePath);

                        GDIPlus.CheckStatus (s);
                } 

                public void Flatten ()
                {
                	Flatten (null, FlatnessDefault); 
                }  	
  
		public void Flatten (Matrix matrix)
		{
                	Flatten (matrix, FlatnessDefault);
                }
		
		public void Flatten (Matrix matrix, float flatness)
		{
                        IntPtr m = (matrix == null) ? IntPtr.Zero : matrix.nativeMatrix;
                	Status status = GDIPlus.GdipFlattenPath (nativePath, m, flatness);

                        GDIPlus.CheckStatus (status);
                }  		
                
                public RectangleF_ GetBounds ()
                {
                	return GetBounds (null, null);
                }  		

                public RectangleF_ GetBounds (Matrix matrix)
                {
                	return GetBounds (matrix, null);
                }

                public RectangleF_ GetBounds (Matrix matrix, Pen pen)
                {
                        RectangleF_ retval;
                        IntPtr m = (matrix == null) ? IntPtr.Zero : matrix.nativeMatrix;
                        IntPtr p = (pen == null) ? IntPtr.Zero : pen.nativeObject;
                        
                        Status s = GDIPlus.GdipGetPathWorldBounds (nativePath, out retval, m, p);

                        GDIPlus.CheckStatus (s);

                        return retval;
                }

		public bool IsOutlineVisible (Point_ point, Pen pen)
		{
                        return IsOutlineVisible (point.X, point.Y, pen, null);
                }  		
		
		public bool IsOutlineVisible (PointF_ point, Pen pen)
		{
                	return IsOutlineVisible (point.X, point.Y, pen, null);
                } 
		
		public bool IsOutlineVisible (int x, int y, Pen pen)
		{
                        return IsOutlineVisible (x, y, pen, null);
                }

		public bool IsOutlineVisible (float x, float y, Pen pen)
		{
                	return IsOutlineVisible (x, y, pen, null);
                }  		
		
		public bool IsOutlineVisible (Point_ pt, Pen pen, Graphics graphics)
		{
                	return IsOutlineVisible (pt.X, pt.Y, pen, graphics);
                }  		
		
		public bool IsOutlineVisible (PointF_ pt, Pen pen, Graphics graphics)
		{
                	return IsOutlineVisible (pt.X, pt.Y, pen, graphics);
                }  		
				
		public bool IsOutlineVisible (int x, int y, Pen pen, Graphics graphics)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

                        bool result;
                        IntPtr g = (graphics == null) ? IntPtr.Zero : graphics.nativeObject;
                        
                	Status s = GDIPlus.GdipIsOutlineVisiblePathPointI (nativePath, x, y, pen.nativeObject, g, out result);
                        GDIPlus.CheckStatus (s);

                        return result;
                }  		

		public bool IsOutlineVisible (float x, float y, Pen pen, Graphics graphics)
		{
			if (pen == null)
				throw new ArgumentNullException ("pen");

                        bool result;
                        IntPtr g = (graphics == null) ? IntPtr.Zero : graphics.nativeObject;
                        
                	Status s = GDIPlus.GdipIsOutlineVisiblePathPoint (nativePath, x, y, pen.nativeObject, g, out result);
                        GDIPlus.CheckStatus (s);

                        return result;
                }  		
                
                public bool IsVisible (Point_ point)
                {
                	return IsVisible (point.X, point.Y, null);
                }  		
                
                public bool IsVisible (PointF_ point)
                {
                	return IsVisible (point.X, point.Y, null);
                }  		
                
                public bool IsVisible (int x, int y)
                {
                	return IsVisible (x, y, null);
                }

                public bool IsVisible (float x, float y)
                {
                	return IsVisible (x, y, null);
                }  		                
                
                public bool IsVisible (Point_ pt, Graphics graphics)
                {
                	return IsVisible (pt.X, pt.Y, graphics);
                }  		
                
                public bool IsVisible (PointF_ pt, Graphics graphics)
                {
                	return IsVisible (pt.X, pt.Y, graphics);
                }  		
                                
                public bool IsVisible (int x, int y, Graphics graphics)
                {
                        bool retval;

                	IntPtr g = (graphics == null) ? IntPtr.Zero : graphics.nativeObject;

                        Status s = GDIPlus.GdipIsVisiblePathPointI (nativePath, x, y, g, out retval);

                        GDIPlus.CheckStatus (s);

                        return retval;
                }  		
                
                public bool IsVisible (float x, float y, Graphics graphics)
                {
                        bool retval;

                	IntPtr g = (graphics == null) ? IntPtr.Zero : graphics.nativeObject;

                        Status s = GDIPlus.GdipIsVisiblePathPoint (nativePath, x, y, g, out retval);

                        GDIPlus.CheckStatus (s);

                        return retval;
                }  		
                
                public void SetMarkers ()
                {
                	Status s = GDIPlus.GdipSetPathMarker (nativePath);

                        GDIPlus.CheckStatus (s);
                }
                
                public void StartFigure()
                {
                	Status s = GDIPlus.GdipStartPathFigure (nativePath);

                        GDIPlus.CheckStatus (s);
                }  		
                
		[MonoTODO ("GdipWarpPath isn't implemented in libgdiplus")]
                public void Warp (PointF_[] destPoints, RectangleF_ srcRect)
                {
                	Warp (destPoints, srcRect, null, WarpMode.Perspective, FlatnessDefault);
                }  		

		[MonoTODO ("GdipWarpPath isn't implemented in libgdiplus")]
		public void Warp (PointF_[] destPoints, RectangleF_ srcRect, Matrix matrix)
		{
                	Warp (destPoints, srcRect, matrix, WarpMode.Perspective, FlatnessDefault);
                }  		

		[MonoTODO ("GdipWarpPath isn't implemented in libgdiplus")]
		public void Warp (PointF_[] destPoints, RectangleF_ srcRect, Matrix matrix, WarpMode warpMode)
		{
                	Warp (destPoints, srcRect, matrix, warpMode, FlatnessDefault);
                }  		

		[MonoTODO ("GdipWarpPath isn't implemented in libgdiplus")]
		public void Warp (PointF_[] destPoints, RectangleF_ srcRect, Matrix matrix,  WarpMode warpMode, float flatness)
		{
			if (destPoints == null)
				throw new ArgumentNullException ("destPoints");

                	IntPtr m = (matrix == null) ? IntPtr.Zero : matrix.nativeMatrix;

                        Status s = GDIPlus.GdipWarpPath (nativePath, m, destPoints, destPoints.Length,
                                        srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, warpMode, flatness);

                        GDIPlus.CheckStatus (s);
                }
                
		[MonoTODO ("GdipWidenPath isn't implemented in libgdiplus")]
                public void Widen (Pen pen)
		{
                	Widen (pen, null, FlatnessDefault);
                }  		
                
		[MonoTODO ("GdipWidenPath isn't implemented in libgdiplus")]
		public void Widen (Pen pen, Matrix matrix)
		{	
                	Widen (pen, matrix, FlatnessDefault);
                }  		
                
		[MonoTODO ("GdipWidenPath isn't implemented in libgdiplus")]
		public void Widen (Pen pen, Matrix matrix, float flatness)
                {
			if (pen == null)
				throw new ArgumentNullException ("pen");
#if NET_2_0
			if (PointCount == 0)
				return;
#endif
                	IntPtr m = (matrix == null) ? IntPtr.Zero : matrix.nativeMatrix;

			Status s = GDIPlus.GdipWidenPath (nativePath, pen.nativeObject, m, flatness);
			GDIPlus.CheckStatus (s);
                } 
        }
}
