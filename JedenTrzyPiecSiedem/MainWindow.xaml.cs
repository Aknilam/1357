using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        Board board;

        public Game1357()
        {
            InitializeComponent();

            board = new Board(canvas);

            DrawGame(4);
        }

        private void DrawGame(int levels)
        {
            board.DrawGame(levels, (width, height) =>
            {
                window.Width = width;
                window.Height = height;
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
    }
}
