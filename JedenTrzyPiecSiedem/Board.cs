using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
 
namespace JedenTrzyPiecSiedem
{
    public class Board
    {
        Canvas canvas;

        Line background = new Line();
        CuttingLine cuttingLine;
        List<InfoLine> cutted = new List<InfoLine>();

        public InfoLines infoLines;
        private Action onMove;
        private Action<bool> onFinish;

        public Board(Canvas canvas, Action onMove, Action<bool> onFinish)
        {
            this.canvas = canvas;
            this.onMove = onMove;
            this.onFinish = onFinish;
            canvas.VerticalAlignment = VerticalAlignment.Center;
            canvas.HorizontalAlignment = HorizontalAlignment.Center;
            
            canvas.MouseDown += Board_MouseDown;
            canvas.MouseMove += Board_MouseMove;
            canvas.MouseUp += Board_MouseUp;
            
            background.Stroke = Brushes.White;
            canvas.Children.Add(background);
            
            cuttingLine = new CuttingLine(canvas);

            infoLines = new InfoLines(this);
        }

        private void Board_MouseUp(object sender, MouseButtonEventArgs e)
        {
            cuttingLine.End(() =>
            {
                RemoveCutted(cutted);

                if (cutted.Any())
                {
                    CheckFinish(false);
                    if (infoLines.Data.Any())
                    {
                        onMove();
                    }
                }

                cutted = new List<InfoLine>();
            });
        }
        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            cutted = cuttingLine.Move(e.GetPosition(canvas), infoLines.Data);
        }
        private void Board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cuttingLine.Start(e.GetPosition(canvas));
        }
        private void RemoveCutted(List<InfoLine> toCut)
        {
            toCut.ForEach(c =>
            {
                canvas.Children.Remove(c.Line);
                infoLines.Data.Remove(c);
            });
        }

        public void ApplyMove(List<InfoLine> toRemove)
        {
            if (toRemove != null)
                RemoveCutted(toRemove);
        }

        public void CheckFinish(bool autoMadeLastMovement)
        {
            if (!infoLines.Data.Any())
            {
                onFinish(autoMadeLastMovement);
            }
        }

        private void UpdateBackground(Point min, Point max)
        {
            background.X1 = background.X2 = (max.X - min.X) / 2;
            background.Y1 = min.Y;
            background.Y2 = max.Y;
            background.StrokeThickness = max.X - min.X + InfoLines.PaddingX * 2;
        }

        private IEnumerable<Line> OnlyLines
        {
            get
            {
                return infoLines.Data.Select(il => il.Line);
            }
        }

        public List<InfoLine> Data
        {
            get
            {
                return infoLines.Data;
            }
        }

        public void DrawGame(int levels, Action<double, double> onDraw = null)
        {
            infoLines.Clear(il => canvas.Children.Remove(il.Line));

            infoLines.DrawRows(levels);

            Offset(onDraw);
        }

        private void Offset(Action<double, double> onDraw = null)
        {
            Point min = MathHelpers.MinPoint(infoLines.Points);
            Point max = MathHelpers.MaxPoint(infoLines.Points);

            foreach (Line l in OnlyLines)
            {
                l.X1 = l.X2 = l.X1 - min.X;
                l.Y1 = l.Y1 - min.Y;
                l.Y2 = l.Y2 - min.Y;
            }

            min = MathHelpers.MinPoint(OnlyLines);
            max = MathHelpers.MaxPoint(OnlyLines);

            canvas.Height = max.Y - min.Y;
            canvas.Width = max.X - min.X;

            this.UpdateBackground(min, max);

            onDraw?.Invoke(max.X - min.X + InfoLines.PaddingX * 2, max.Y - min.Y + InfoLines.PaddingX * 2);
        }

        public void AddLine(Line line)
        {
            canvas.Children.Add(line);
        }
     }
 }

