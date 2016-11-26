using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace JedenTrzyPiecSiedem
{
    class BoardDrawingHelpers
    {
        public static List<InfoLine> DrawCutts(List<InfoLine> ils, Line cuttingLine, Action<Line> lineToCut, Action<Line> normalLine)
        {
            var cutted = new List<InfoLine>();
            foreach (InfoLine il in ils)
            {
                Line line = il.Line;
                if (MathHelpers.CalcCut(line, cuttingLine, 0))
                {
                    lineToCut(line);
                    cutted.Add(il);
                }
                else
                {
                    normalLine(line);
                }
            }
            cutted = CheckCuttedBySameRow(cutted, normalLine);
            cutted = CheckCuttedByBeingNextTo(cutted, normalLine);
            return cutted;
        }

        private static List<InfoLine> CheckCuttedBySameRow(List<InfoLine> cutted, Action<Line> normalLine)
        {
            if (cutted.Select(il => il.Line.Y1).Distinct().Count() > 1)
            {
                cutted.ForEach(il => normalLine(il.Line));
                return new List<InfoLine>();
            }
            else
            {
                return cutted;
            }
        }
        private static List<InfoLine> CheckCuttedByBeingNextTo(List<InfoLine> cutted, Action<Line> normalLine)
        {
            if (cutted.Count > 1)
            {
                var sorted = cutted.Select(c => c.Col).OrderBy(c => c);
                for (int col = 1; col < sorted.Count(); col++)
                {
                    if ((sorted.ElementAt(col) - sorted.ElementAt(col - 1)) != 1)
                    {
                        return new List<InfoLine>();
                    }
                }
            }
            return cutted;
        }
    }
}
