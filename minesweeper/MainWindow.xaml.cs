using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace minesweeper
{
    public partial class MainWindow : Window
    {
        private const int Rows = 12;
        private const int Columns = 12;
        private const int MineCount = 5;

        private Button[,] grid;
        private bool[,] mineGrid;
        private bool gameStarted = false;
        private DispatcherTimer timer;
        private int secondsElapsed = 0;
        private object timerLabel;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }
        private void InitializeGame()
        {
            grid = new Button[Rows, Columns];
            mineGrid = new bool[Rows, Columns];
            timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += Timer_Tick;

            for(int row = 0; row < Rows; row++)
            {
                for(int col = 0; col <Columns; col++)
                {
                    grid[row, col] = new Button
                    {
                        Width = 40,
                        Height = 40,
                        Tag = new Tuple<int, int>(row, col)
                    };
                    grid[row, col].Click += Button_Click;
                    Grid.SetRow(grid[row, col], row);
                    Grid.SetColumn(grid[row, col], col);
                    gameGrid.Children.Add(grid[row, col]);
                }
            }
            GenerateMines();
        }

        private void GenerateMines()
        {
            Random random = new Random();
            for(int i =0; 1 < MineCount;)
            {
                int row = random.Next(0, Rows);
                int col = random.Next(0, Columns);

                if (!mineGrid[row, col])
                {
                    mineGrid[row, col] = true;
                    i++;
                }
            }
        }
        private void Timer_Tick(object sender, RoutedEventArgs e)
        {
            secondsElapsed++;
            timerLabel.Content = $"Time: {secondsElapsed} seconds";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!gameStarted)
            {
                gameStarted = true;
                timer.Start();
            }
            Button button = (Button)sender;
            var position = (Tuple<int, int>)button.Tag;
            int row = position.Item1;
            int col = position.Item1;

            if (mineGrid[row, col])
            {
                GameOver();
            }
            else
            {
                int mineCount = CountAdjacentMines(row, col);
                button.Content = mineCount;
                button.IsEnabled = false;
            }
        }
        private int CountAdjacentMines(int row, int col)
        {
            int mineCount = 0;
            for(int r = row - 1; r <= row + 1; r++)
            {
                for(int c = col - 1; c <= col + 1; c++)
                {
                    if(r >= 0 && r < Rows && c >= 0 && c < Columns && mineGrid[r,c])
                    {
                        mineCount++;
                    }
                }
            }
            return MineCount;
        }
        private void GameOver()
        {
            timer.Stop();
            MessageBox.Show("Game over");
        }
    }
}
