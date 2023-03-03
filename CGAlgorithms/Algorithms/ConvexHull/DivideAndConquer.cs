using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public List<Point> Solve(List<Point>points)
        {
            List<Point> outPoints = new List<Point>();
            if (points.Count<3)
            {
                outPoints = points;
                return outPoints;
            }
            if(points.Count==3&&HelperMethods.CheckTurn(new Line(points[0],points[1]),points[2])!=Enums.TurnType.Colinear)
            {
                outPoints = points;
                return outPoints;
            }
            List<Point> Right = new List<Point>();
            List<Point> Left = new List<Point>();
            for (int i=0;i<points.Count/2;i++)
            {
                Right.Add(points[i]);
            }
            for (int i = points.Count / 2; i < points.Count ; i++)
            {
                Left.Add(points[i]);
            }
            Right=Solve(Right);
            Left=Solve(Left);
            double max_Y_R = -100000000000000;
            Point max_R = new Point(2, 3);
            int index_max_R = 0;
            for(int i=0;i<Right.Count;i++)
            {
                if(Right[i].Y>max_Y_R)
                {
                    max_Y_R = Right[i].Y;
                    max_R = Right[i];
                    index_max_R = i;
                }
            }
            double max_Y_L = -100000000000000;
            Point max_L = new Point(2, 3);
            int index_max_L = 0;
            for (int i = 0; i < Left.Count; i++)
            {
                if (Left[i].Y > max_Y_L)
                {
                    max_Y_L = Left[i].Y;
                    max_L = Left[i];
                    index_max_L = i;
                }
            }
            double min_Y_R = 100000000000000;
            Point min_R = new Point(2, 3);
            int index_min_R = 0;
            for (int i = 0; i < Right.Count; i++)
            {
                if (Right[i].Y < min_Y_R)
                {
                    min_Y_R = Right[i].Y;
                    min_R = Right[i];
                    index_min_R = i;
                }
            }
            double min_Y_L = 100000000000000;
            Point min_L = new Point(2, 3);
            int index_min_L = 0;
            for (int i = 0; i < Left.Count; i++)
            {
                if (Left[i].Y < min_Y_L)
                {
                    min_Y_L = Left[i].Y;
                    min_L = Left[i];
                    index_min_L = i;
                }
            }
            
            Line right = new Line(max_R,min_R);
            for (int i = 0; i < Right.Count; i++)
            {
                if (HelperMethods.CheckTurn(right, Right[i]) == Enums.TurnType.Right || HelperMethods.CheckTurn(right, Right[i]) == Enums.TurnType.Colinear|| Right[i].Y > max_L.Y|| Right[i].Y < min_L.Y)
                    if (!outPoints.Contains(Right[i]))
                        outPoints.Add(Right[i]);
                
            }
            
            Line left = new Line(max_L,min_L);
            for (int i = 0; i < Left.Count; i++)
            {
                if (HelperMethods.CheckTurn(left, Left[i]) == Enums.TurnType.Left|| HelperMethods.CheckTurn(left, Left[i]) == Enums.TurnType.Colinear)
                    if (!outPoints.Contains(Left[i]))
                        outPoints.Add(Left[i]);
            }


            for (int i = 0; i < outPoints.Count; i++)
            {
                if (outPoints[i].X < max_L.X && outPoints[i].X > min_L.X)
                    outPoints.RemoveAt(i);
            }
            for (int i=1;i<outPoints.Count-1;i++)
            {
                if (HelperMethods.PointOnSegment(outPoints[i], outPoints[i - 1], outPoints[i + 1]))
                    outPoints.RemoveAt(i);    
            }
            for (int i = 2; i < outPoints.Count; i++)
            {
                if (HelperMethods.PointOnSegment(outPoints[i], outPoints[i - 1], outPoints[i - 2]))
                    outPoints.RemoveAt(i);
            }
            for (int i = 1; i < outPoints.Count - 1; i++)
            {
                if (HelperMethods.PointOnSegment(outPoints[i], outPoints[i - 1], outPoints[outPoints.Count-1]))
                    outPoints.RemoveAt(i);
            }

            return outPoints;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 3)
            {
                outPoints = points;
            }
            else
            {
                points = points.OrderBy(p => p.X).ToList();
                outPoints = Solve(points);   
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}
