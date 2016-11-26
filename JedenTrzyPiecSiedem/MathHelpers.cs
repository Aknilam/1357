using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace JedenTrzyPiecSiedem
{
    public class MathHelpers
    {
        public static Point MinPoint(List<Point> points)
        {
            Point min = new Point();
            points.ForEach(p =>
            {
                if (p.X < min.X)
                    min.X = p.X;

                if (p.Y < min.Y)
                    min.Y = p.Y;
            });
            return min;
        }
        public static Point MaxPoint(List<Point> points)
        {
            Point max = new Point();
            points.ForEach(p =>
            {
                if (p.X > max.X)
                    max.X = p.X;

                if (p.Y > max.Y)
                    max.Y = p.Y;
            });
            return max;
        }

        private static List<Point> PreparePoints(IEnumerable<Line> lines)
        {
            return lines.Select(l => new Point(l.X1, l.Y2)).ToList();
        }

        // only for vertical lines
        public static Point MinPoint(IEnumerable<Line> lines)
        {
            return MinPoint(PreparePoints(lines));
        }
        // only for vertical lines
        public static Point MaxPoint(IEnumerable<Line> lines)
        {
            return MaxPoint(PreparePoints(lines));
        }
        public static bool CalcCut(Line onlyX, Line line, int dickness)
        {
            Line newLine1 = new Line();
            double r = dickness / 2;
            newLine1.X1 = newLine1.X2 = onlyX.X1 - r;
            newLine1.Y1 = onlyX.Y1;
            newLine1.Y2 = onlyX.Y2;

            Line newLine2 = new Line();
            newLine2.X1 = newLine2.X2 = onlyX.X1 + r;
            newLine2.Y1 = onlyX.Y1;
            newLine2.Y2 = onlyX.Y2;

            return CalcCut(newLine1, line) || CalcCut(newLine2, line);
        }

        public static bool CalcCut(Line onlyX, Line line)
        {
            double x1 = Math.Min(line.X1, line.X2);
            double x2 = Math.Max(line.X1, line.X2);
            if (!(x1 <= onlyX.X1 && onlyX.X1 <= x2))
            {
                return false;
            }
            double a = (line.Y2 - line.Y1) / (line.X2 - line.X1);
            double b = line.Y1 - a * line.X1;
            double y = a * onlyX.X1 + b;

            return onlyX.Y1 <= y && y <= onlyX.Y2;
        }
    }
}
