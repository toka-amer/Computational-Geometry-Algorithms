using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if(points.Count<=3)
            {
                outPoints = points;
            }
            else
            {
                int min_y_index = 0;
                double min_y = 100000;
                for(int i=0;i<points.Count;i++)
                {
                    if (points[i].Y < min_y)
                    {
                        min_y = points[i].Y;
                        min_y_index = i;
                    }
                }
                outPoints.Add(points[min_y_index]);
                Point next = points[0];
                while (true)
                {
                    Line l = new Line(outPoints[outPoints.Count - 1], next);
                    bool found_right = false;
                    for (int i = 0; i < points.Count; i++)
                    {
                        Enums.TurnType t= HelperMethods.CheckTurn(l, points[i]);
                        if (t == Enums.TurnType.Left|| t == Enums.TurnType.Colinear)
                        {
                            continue;
                        }
                        else if (t == Enums.TurnType.Right)
                        {
                            found_right = true;
                            next = points[i];
                            break;
                        }
                    }
                    if(found_right==false)
                    {
                        if(outPoints.Count<3)
                        {
                            outPoints.Add(next);
                            next = points[0];
                        }
                        else
                        {
                            if (next == outPoints[0] || HelperMethods.PointOnSegment(outPoints[0], outPoints[outPoints.Count - 1], next))
                                break;
                            else
                            {

                                if (HelperMethods.CheckTurn(new Line(outPoints[outPoints.Count - 1], outPoints[outPoints.Count - 2]), next) == Enums.TurnType.Colinear)
                                    outPoints.RemoveAt(outPoints.Count - 1);
                                outPoints.Add(next);
                                next = points[0];
                            }
                        }

                    }  
                }
            }
           
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
