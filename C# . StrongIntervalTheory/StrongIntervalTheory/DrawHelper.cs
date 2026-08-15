using MouseLib;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using TraverseHelperLib;

namespace DrawHelperLib
{
	public static class DrawHelper
	{

		public static void DrawArrow(this Graphics gr, Pen pen, PointF p1, PointF p2, float tangLen, float normLen)
		{
			var dif = p2.Minus(p1);
			var difLen = Len(dif);

			gr.DrawLine(pen, p1, p2);
			if (difLen > 3)
			{
				var tang = dif.Mult(1 / difLen);
				var norm = tang.Rot90();

				var p3 = p2.Minus(tang.Mult(tangLen)).Plus(norm.Mult(normLen));
				var p4 = p2.Minus(tang.Mult(tangLen)).Plus(norm.Mult(-normLen));
				gr.DrawLine(pen, p2, p3);
				gr.DrawLine(pen, p2, p4);
			}
		}


		public static PointF Rot90(this PointF p)
		{
			return new PointF(-p.Y, p.X);
		}

		public static PointF Minus(this PointF p1, PointF p2)
		{
			return new PointF(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static PointF Plus(this PointF p1, PointF p2)
		{
			return new PointF(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static PointF Mult(this PointF p, float a)
		{
			return new PointF(p.X * a, p.Y * a);
		}

		public static float Len(this PointF p)
		{
			return (float)Math.Sqrt(p.X * p.X + p.Y * p.Y);
		}

	}
}
