using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    { 
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            //  first part:special cases 
            if (points.Count == 0)
                return;
            if(points.Count==1)
            {
                outPoints.Add(points[0]);
                return;
            }
            points.Sort(HelperMethods.sorting);
            int [] next = new int[points.Count];
            int [] previous  = new int[points.Count];
            int i = 1;
            while (i < points.Count && HelperMethods.comparing(points[0], points[i]))
                i += 1;
            if(i==points.Count)
            {
                outPoints.Add(points[0]);
                return;
            }
            else
            {
                next[0] = i;
                previous[0] = i;
                next[i] = 0;
                previous[i] = 0;
            }
            //second part increamental for convexhull
            int index = i;
            for(i = i+1; i<points.Count; i++)
            {
                Point new_point = points[i];
                if (HelperMethods.comparing(new_point, points[index]))
                    continue;
                if(new_point.Y >= points[index].Y)
                {
                    next[i] = next[index];
                    previous[i] = index;
                }
                else
                {
                    next[i] = index;
                    previous[i] = previous[index];
                }
                next[previous[i]] = i;
                while(true)
                {
                    Line seg = new Line(new_point, points[next[i]]);
                    Point next_point = points[next[next[i]]];
                    Enums.TurnType turn = HelperMethods.CheckTurn(seg, next_point); 
                    if (turn != Enums.TurnType.Left)
                    {
                        next[i] = next[next[i]];
                        previous[next[i]] = i;
                        if (turn == Enums.TurnType.Colinear)
                            break;
                    }
                    else
                        break;
                }
                while (true)
                {
                    Line seg = new Line(new_point, points[previous[i]]);
                    Point next_point = points[previous[previous[i]]];
                    Enums.TurnType turn = HelperMethods.CheckTurn(seg, next_point);
                    if (turn != Enums.TurnType.Right)
                    {
                        previous[i] = previous[previous[i]];
                        next[previous[i]] = i;
                        if (turn == Enums.TurnType.Colinear)
                            break;
                    }
                    else break;
                }
                index = i;
            }
            i = 0;
            while(true)
            {
                outPoints.Add(points[i]);
                i = next[i];
                if (i == 0)
                    break;
            }
            return;
        }
               public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
