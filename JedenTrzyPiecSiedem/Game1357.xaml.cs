using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace JedenTrzyPiecSiedem
{
    public partial class Game1357 : Window
    {
        public static RoutedCommand SaveCommand = new RoutedUICommand("Options", "OptionsCommand", typeof(Game1357), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.S, ModifierKeys.Control)
        }));
        public static RoutedCommand AppendCommand = new RoutedUICommand("Options", "OptionsCommand", typeof(Game1357), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.D, ModifierKeys.Control)
        }));
        public static RoutedCommand ResetCommand = new RoutedUICommand("Options", "OptionsCommand", typeof(Game1357), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.N, ModifierKeys.Control)
        }));
        public static RoutedCommand TestCommand = new RoutedUICommand("Options", "OptionsCommand", typeof(Game1357), new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.T, ModifierKeys.Control)
        }));

        Board board;
        Engine e;

        public Game1357()
        {
            InitializeComponent();
            who.Items.Add("computer");
            who.Items.Add("user");

            board = new Board(canvas, () =>
            {
                board.ApplyMove(e.FindBestMove());
                board.CheckFinish(true);
            }, (autoMadeLastMovement) =>
            {
                if (autoMadeLastMovement)
                {
                    MessageBox.Show("Komputer przegrał");
                }
                else
                {
                    MessageBox.Show("Użytkownik przegrał");
                }
            });

            e = new Engine(board.infoLines);
        }

        private void DrawGame(int levels)
        {
            board.DrawGame(levels, (width, height) =>
            {
                window.Width = width;
                window.Height = height + (double.IsNaN(buttons.Height) ? 50 : buttons.Height);
            });
        }
        
        private void MenuItem_Click_Save(object sender, RoutedEventArgs e)
        {
            Serialize.Write(board.Data);
        }

        private void MenuItem_Click_Append(object sender, RoutedEventArgs e)
        {
            Serialize.Append(board.Data);
        }

        private void MenuItem_Click_Reset(object sender, RoutedEventArgs e)
        {
            DrawGame(4);
        }
        private void MenuItem_Click_Test(object sender, RoutedEventArgs rea)
        {
            Console.WriteLine(string.Join(", ", e.Possibilities.Select(p => p.Count)));

            board.ApplyMove(e.FindBestMove());
            Console.WriteLine(string.Join(", ", e.Possibilities.Select(p => p.Count)));
        }

        private void start_Click(object sender, RoutedEventArgs rea)
        {
            if (who.SelectedItem != null) {
                if ((string)who.SelectedItem == "computer")
                {
                    DrawGame(4);
                    turn.Content = who.SelectedItem;
                    board.ApplyMove(e.FindBestMove());
                }
                else if ((string)who.SelectedItem == "user")
                {
                    DrawGame(4);
                    turn.Content = who.SelectedItem;
                }
            }
        }

        private void cofnij_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
