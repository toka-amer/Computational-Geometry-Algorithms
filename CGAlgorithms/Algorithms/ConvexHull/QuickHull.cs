using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 4)
            {
                outPoints = points;
                return;
            }
            // getting the max and minimum points
            Point maxX = new Point(-99999, 0);
            Point minX = new Point(99999, 0);
            int maxIndex = -1;
            int minIndex = -1;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X > maxX.X && points[i].Y != maxX.Y)
                {
                    maxX = points[i];
                    maxIndex = i;
                }
                if (points[i].X < minX.X && points[i].Y != minX.Y)
                {
                    minX = points[i];
                    minIndex = i;
                }
            }
            outLines.Add(new Line(minX, maxX));
            outLines.Add(new Line(maxX, minX));
            if (maxIndex > minIndex)
            {
                points.RemoveAt(maxIndex);
                points.RemoveAt(minIndex);
            }
            else
            {
                points.RemoveAt(minIndex);
                points.RemoveAt(maxIndex);
            }
            
            int added = 0; //how many points added in this iteration
            // removing all points inside
            while (true)
            {
                //loop for lines
                for (int i = 0; i < outLines.Count; i++)
                {
                    //getting the most right point of the line
                    double min = 100000000000;
                    int pointIndex = -1;
                    for (int j = 0; j < points.Count; j++)
                    {
                        double distance = HelperMethods.CheckOrientationDistance(outLines[i], points[j]);
                        if (distance < min && distance < 0) //means point on the right of the line
                        {
                            min = distance;
                            pointIndex = j;
                        }
                    }
                    if (min != 100000000000)
                    {
                        added++;
                        Line temp = outLines[i];
                        outLines.RemoveAt(i);
                        outLines.Insert(i, new Line(temp.Start, points[pointIndex]));
                        outLines.Insert(i + 1, new Line(points[pointIndex], temp.End));
                        i++;
                        points.RemoveAt(pointIndex);
                    }
                }
                if (added == 0)
                {
                    break;
                }
                else
                {
                    added = 0;
                }
            }
            outPoints.Add(outLines[0].Start);
            for (int i = 0; i < outLines.Count; i++)
            {
                if (i != outLines.Count - 1)
                {
                    outPoints.Add(outLines[i].End);
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
