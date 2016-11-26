using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JedenTrzyPiecSiedem
{
    class CuttingLine
    {
        Canvas board;
        Point? start;

        Line cuttingLine;

        public CuttingLine(Canvas board)
        {
            this.board = board;
        }
        public void Start(Point start)
        {
            if (!this.start.HasValue)
            {
                this.start = start;
                cuttingLine = new Line();
                cuttingLine.StrokeThickness = 1;
                cuttingLine.Stroke = Brushes.Black;
                cuttingLine.X1 = cuttingLine.X2 = start.X;
                cuttingLine.Y1 = cuttingLine.Y2 = start.Y;

                board.Children.Add(cuttingLine);
            }
        }
        public List<InfoLine> Move(Point nextPoint, List<InfoLine> ils)
        {
            if (start.HasValue)
            {
                cuttingLine.X2 = nextPoint.X;
                cuttingLine.Y2 = nextPoint.Y;

                return BoardDrawingHelpers.DrawCutts(
                    ils,
                    cuttingLine,
                    (line) => line.Stroke = Brushes.Red,
                    (line) => line.Stroke = Brushes.LightSteelBlue
                );
            }
            return new List<InfoLine>();
        }
        public void End(Action onEnd)
        {
            if (start.HasValue)
            {
                start = null;
                board.Children.Remove(cuttingLine);
                onEnd();
            }
        }
    }
}
