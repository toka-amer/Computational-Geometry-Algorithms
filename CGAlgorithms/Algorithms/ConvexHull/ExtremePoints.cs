using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> P = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                bool flag = false;// lw l2et el point
                for (int j = 0; j < points.Count; j++)
                {
                    for (int k = 0; k < points.Count; k++)
                    {
                        for (int p = 0;p < points.Count; p++)
                        {
                            if (points[i] != points[j] && points[i] != points[k] && points[i] != points[p])
                            {
                                Enums.PointInPolygon chickPoint = HelperMethods.PointInTriangle(points[i], points[j], points[k], points[p]);
                                if (chickPoint == Enums.PointInPolygon.Inside || chickPoint == Enums.PointInPolygon.OnEdge)
                                {
                                    points.Remove(points[i]);
                                    i--;
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                                break;
                        }
                        if (flag)
                            break;
                    }
                    if (flag)
                        break;
                }
            }
            outPoints = points;
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}