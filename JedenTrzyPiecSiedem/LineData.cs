using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;

namespace JedenTrzyPiecSiedem
{
    class LineData
    {
        public static int DefaultHeight = 80;

        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public void Apply(Line line)
        {
            line.X1 = P1.X;
            line.X2 = P2.X;
            line.Y1 = P1.Y;
            line.Y2 = P2.Y;
        }

        public static LineData Prepare(int startX, int startY)
        {
            return new LineData
            {
                P1 = new Point(startX, startY),
                P2 = new Point(startX, startY + DefaultHeight)
            };
        }
    }
}
