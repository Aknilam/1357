using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace JedenTrzyPiecSiedem
{
    class InfoLines
    {
        public static int PaddingX = 40;
        public static int PaddingY = 20;

        public List<InfoLine> Data { get; private set; } = new List<InfoLine>();
        public List<Point> Points { get; private set; } = new List<Point>();

        Board board;
        public InfoLines(Board board)
        {
            this.board = board;
        }

        public void Clear(Action<InfoLine> onRemove)
        {
            Data.ForEach(onRemove);
            Data = new List<InfoLine>();
        }

        public void DrawRows(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                DrawRow(2 * (i + 1) - 1, i, -levels / 2 * (LineData.DefaultHeight + PaddingY), i);
            }
        }
        // count jest nieparzyste
        private void DrawRow(int count, int position, int offsetY, int row)
        {
            int col = 0;
            for (int i = -(count / 2); i <= count / 2; i++)
            {
                InfoLine il = new InfoLine(row, col++);
                LineData ld = LineData.Prepare(PaddingX * i, position * LineData.DefaultHeight + (position - 1) * PaddingY + offsetY);
                DrawLine(il, ld);
                Data.Add(il);
            }
        }

        private void DrawLine(InfoLine il, LineData ld)
        {
            Points.Add(ld.P1);

            var line = il.Line;
            line.Stroke = Brushes.LightSteelBlue;
            ld.Apply(line);
            line.StrokeThickness = 6;
            board.AddLine(line);
        }
    }
}
