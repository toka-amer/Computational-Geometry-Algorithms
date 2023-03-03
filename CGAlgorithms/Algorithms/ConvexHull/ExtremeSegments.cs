using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;


namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public void orientationTestWithAllPoint(List<Point> points, int indexP, int indexQ, ref int posCounter, ref int negCounter, ref bool flag)
        {
            for (int k = 0; k < points.Count; k++)
            {
                if (k != indexP && k != indexQ && indexQ != indexP)
                {
                    if (!(points[indexP].X == points[indexQ].X && points[indexP].Y == points[indexQ].Y))
                    {
                        flag = true;
                        double det = ((points[indexP].X * (points[indexQ].Y - points[k].Y)) - (points[indexQ].X * (points[indexP].Y - points[k].Y)) + (points[k].X * (points[indexP].Y - points[indexQ].Y)));
                        double area = (1 / 2) * det;
                        if (det > 0)
                        {
                            posCounter++;
                        }
                        else if (det < 0)
                        {
                            negCounter++;
                        }
                    }
                }
            }
        }
        public void removePointsOnSegment(ref List<Point> outpoints)
        {
            List<Point> finalPoints = new List<Point>();
            for (int i = 0; i < outpoints.Count; i++)
            {
                for (int j = 0; j < outpoints.Count; j++)
                {
                    for (int k = 0; k < outpoints.Count; k++)
                    {
                        if (k != j && k != i && i != j && !finalPoints.Contains(outpoints[k]))
                        {
                            if ((HelperMethods.PointOnSegment(outpoints[k], outpoints[i], outpoints[j])))
                            {
                                finalPoints.Add(outpoints[k]);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < finalPoints.Count; i++)
            {
                outpoints.Remove(finalPoints[i]);
            }
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count == 1)
            {
                outPoints.Add(points[0]);
            }
            else if (points.Count == 2)
            {
                outPoints.Add(points[0]);
                outPoints.Add(points[1]);
            }
            else if (points.Count == 3)
            {
                outPoints.Add(points[0]);
                outPoints.Add(points[1]);
                outPoints.Add(points[2]);
            }
            else
            {
                int counter = 0;
                int i = 0;
                int indexOfFirstPoint = 0;
                while (counter < points.Count)
                {
                    int j = 0;
                    while (j < points.Count)
                    {
                        int posCounter = 0;
                        int negCounter = 0;
                        bool flag = false;
                        if ((!outPoints.Contains(points[j])))
                        {
                            orientationTestWithAllPoint(points, i, j, ref posCounter, ref negCounter, ref flag);
                            if ((posCounter == 0 || negCounter == 0) && flag)
                            {
                                if (outPoints.Count >= 2)
                                {
                                    if ((!HelperMethods.PointOnSegment(points[j], outPoints[outPoints.Count - 1], points[i])))
                                    {
                                        outPoints.Add(points[i]);
                                        i = j;
                                        break;
                                    }

                                }
                                else
                                {
                                    outPoints.Add(points[i]);
                                    if (outPoints.Count == 1)
                                    {
                                        indexOfFirstPoint = i;
                                    }
                                    i = j;
                                    break;
                                }
                            }
                        }
                        j++;
                        if (j == points.Count)
                        {
                            if (outPoints.Count != 0)
                            {
                                posCounter = 0;
                                negCounter = 0;
                                flag = false;
                                orientationTestWithAllPoint(points, i, indexOfFirstPoint, ref posCounter, ref negCounter, ref flag);
                                if ((posCounter == 0 || negCounter == 0) && flag)
                                {
                                    outPoints.Add(points[i]);
                                    counter = points.Count;
                                    break;
                                }
                            }
                            else if (i < points.Count - 1)
                            {
                                i++;
                            }
                        }
                    }
                    counter++;
                }
                removePointsOnSegment(ref outPoints);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
