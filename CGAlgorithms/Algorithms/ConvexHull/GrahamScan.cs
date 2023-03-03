using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public List<KeyValuePair<Point, double>> SortByAngle(List<Point> points,int firstIndex)
        {
            Dictionary<Point, double> sortedPoints = new Dictionary<Point, double>();
            double angle;
            for (int i = 0; i < points.Count; i++)
            {
                if(i != firstIndex)
                {
                    if(points[i].Y == points[firstIndex].Y)
                    {
                        sortedPoints.Add(points[i], 1.571);
                    }
                    else if(points[i].X == points[firstIndex].X)
                    {
                        sortedPoints.Add(points[i], 0);
                    }
                    else
                    {
                        angle = Math.Atan((points[i].X - points[firstIndex].X) / (points[i].Y - points[firstIndex].Y));
                        if (angle < 0)
                        {
                            angle = -1 * angle;
                            angle = 3.14 - angle;
                        }
                        sortedPoints.Add(points[i], angle);
                    }
                }
            }
            List<KeyValuePair<Point, double>> x = sortedPoints.ToList();
            x.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            for(int i = 0; i< x.Count; i++)
            {
                if(i!= x.Count - 1)
                {
                    if(x[i].Value == x[i + 1].Value)
                    {
                        if(HelperMethods.PointOnSegment(x[i].Key,points[firstIndex],x[i+1].Key))
                        {
                            x.RemoveAt(i);
                        }
                        else if (HelperMethods.PointOnSegment(x[i+1].Key, points[firstIndex], x[i].Key))
                        {
                            x.RemoveAt(i+1);
                        }
                    }
                }
            }
            return x;
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
                double min = 100000000;
                int minIndex = -1;
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].X < min)
                    {
                        min = points[i].X;
                        minIndex = i;
                    }
                }
                List<KeyValuePair<Point, double>> sortedPoints = new List<KeyValuePair<Point, double>>();
                sortedPoints = SortByAngle(points, minIndex);
                outPoints.Add(points[minIndex]);
                for(int j = 0; j < sortedPoints.Count; j++)
                {
                    if (j == 0)
                    {
                        outPoints.Add(sortedPoints[j].Key);
                        continue;
                    }
                    Point x = outPoints.ElementAt(outPoints.Count -  2);
                    Point y = outPoints.ElementAt(outPoints.Count - 1);
                    Line l = new Line(x, y);
                    Enums.TurnType turnType = HelperMethods.CheckTurn(l, sortedPoints[j].Key);
                    if (turnType == Enums.TurnType.Right)
                    {
                        if(!outPoints.Contains(sortedPoints[j].Key))
                        {
                            outPoints.Add(sortedPoints[j].Key);
                        }
                    }
                    else
                    {
                        outPoints.RemoveAt(outPoints.Count - 1);
                        int counter = 0;
                        int len = outPoints.Count;
                        while (counter < len && outPoints.Count >= 2)
                        {
                            Point testX = outPoints.ElementAt(outPoints.Count - 2);
                            Point testY = outPoints.ElementAt(outPoints.Count - 1);
                            Line testL = new Line(testX, testY);
                            Enums.TurnType turnTypeTest = HelperMethods.CheckTurn(testL, sortedPoints[j].Key);
                            if (turnTypeTest == Enums.TurnType.Right)
                            {
                                outPoints.Add(sortedPoints[j].Key);
                                break;
                            }
                            else
                            {
                                outPoints.RemoveAt(outPoints.Count - 1);
                            }
                            counter++;
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
