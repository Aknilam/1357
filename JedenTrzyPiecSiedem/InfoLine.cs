using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace JedenTrzyPiecSiedem
{
    public class InfoLine
    {
        public Line Line { get; private set; }
        public int Row { get; private set; }
        public int Col { get; private set; }
        public InfoLine(int row, int col)
        {
            this.Line = new Line();
            Row = row;
            Col = col;
        }
    }
}
