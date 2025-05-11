using System;
using System.Data.Common;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Reflection.Metadata.BlobBuilder;

namespace MinesweeperWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
     

    public partial class MainWindow : Window
    {
        private int gridSize = 10;      // grid size
        private int nbMines = 10;       // number of mines
        private int nbCellsChecked = 0; // number of cells that have been checked (opened)
        private int[,] matrix;          // matrix preserving grid values (see below)

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }
        
        public void InitializeGame()
        {

            matrix = new int[gridSize, gridSize];
            nbCellsChecked = 0;
            GRDGame.Children.Clear();
            GRDGame.ColumnDefinitions.Clear();
            GRDGame.RowDefinitions.Clear();


            for (int i = 0; i < gridSize; i++)
            {
                GRDGame.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                GRDGame.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            }
            for (int r = 0; r < gridSize; r++) {
                for (int c = 0; c < gridSize; c++)
                {
                    Border b = new Border(); 
                    b.BorderThickness = new Thickness(1);
                    b.BorderBrush = new SolidColorBrush(Colors.White);
                    b.SetValue(Grid.RowProperty, r);
                    b.SetValue(Grid.ColumnProperty, c);
                    GRDGame.Children.Add(b);
                    Grid grid2 = new Grid();
                    b.Child = grid2;
                    Button button = new Button();        
                    button.Click += Button_Click;
                    grid2.Children.Add(button);
                }
            }
           }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Border b = (Border)VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(clickedButton));
            int column = Grid.GetColumn(b) + 1;
            int row = Grid.GetRow(b) + 1;
            MessageBox.Show(" Coordinate : (" + row + "," + column + ")");
        }

        private UIElement GetUIElementFromPosition(Grid g, int col, int row)
        {
            return g.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);
        }

        public Boolean CheckCell(int column, int row)
        {
            Button button = (Button)GetUIElementFromPosition(GRDGame, column, row);
            if (button.Visibility == Visibility.Visible)
            {
                button.Visibility = Visibility.Hidden;
                nbCellsChecked++;
                if (matrix[column, row] == -1)
                {
                    MessageBox.Show("Perdu !");
                    resetGame();
                    return true;
                }
                else
                {
                    if (nbCellsChecked == Math.Pow(gridSize, 2))
                    {
                        MessageBox.Show("Gagné !");
                        resetGame();
                        return true;
                    }
                    else
                    {
                        if (matrix[column,row] == 0)
                        {
                            for (int i = Math.Max(0,column - 1); i < Math.Min(gridSize - 1 , column+1); i++)
                            {
                                for(int j = Math.Max(0, row - 1); i < Math.Min(gridSize - 1, row + 1); j++){
                                    bool resltat = CheckCell(i, j);
                                    if (resltat) { return true; }
                                }
                            }
                        }
                    }
                        
                    
                }
            }
            return false;
        }

        public void resetGame()
        {
            GRDGame.Children.Clear();
            GRDGame.ColumnDefinitions.Clear();
            GRDGame.RowDefinitions.Clear();
        }

        }
    }
