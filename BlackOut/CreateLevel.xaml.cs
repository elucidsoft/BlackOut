using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace BlackOut
{
    public partial class CreateLevel : PhoneApplicationPage
    {
        GameData gameData;
        public CreateLevel()
        {
            InitializeComponent();
            gameData = GameData.LoadGameData();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (gameData.levels.Count == 0)
            {
                gameData.levels.Add(new int[5,5]);
            }

            tbxLevel.Text = (gameData.levels.Count).ToString();
            tbCount.Text = (gameData.levels.Count).ToString();
           App.GameManager.Initialize(grid);
           App.GameManager.SetBoard(gameData.levels[Convert.ToInt32(tbxLevel.Text) - 1]);
           App.GameManager.DisplayGrid(false);
        }

        private void appBarBtnAddLevel_Click(object sender, System.EventArgs e)
        {
           App.GameManager.SetBoard(TempLevels.EmptyBoard);
           App.GameManager.ResetBoard();

            int[,] board = TempLevels.EmptyBoard;
            gameData.levels.Add(board);

            tbxLevel.Text = gameData.levels.Count.ToString();
            tbCount.Text = gameData.levels.Count.ToString();
        }

        private void appBarBtnLoadLevel_Click(object sender, System.EventArgs e)
        {
           App.GameManager.ResetBoard();
           App.GameManager.SetBoard(gameData.levels[Convert.ToInt32(tbxLevel.Text) - 1]);
        }

        private void appBarBtnLastLevel_Click(object sender, System.EventArgs e)
        {
            int index = Convert.ToInt32(tbxLevel.Text);
            if (index > 1)
            {
                index -= 1;
                tbxLevel.Text = index.ToString();

               App.GameManager.ResetBoard();
               App.GameManager.SetBoard(gameData.levels[Convert.ToInt32(tbxLevel.Text) - 1]);
            }
        }

        private void appBarBtnNextLevel_Click(object sender, System.EventArgs e)
        {
            int index = Convert.ToInt32(tbxLevel.Text);
            if (index < gameData.levels.Count)
            {
                index += 1;
                tbxLevel.Text = index.ToString();
               App.GameManager.ResetBoard();
               App.GameManager.SetBoard(gameData.levels[Convert.ToInt32(tbxLevel.Text) - 1]);
            }
        }

        private void appBarMnuSaveLevels_Click(object sender, System.EventArgs e)
        {
            GameData.SaveGameData(gameData);
        }

        private void appBarMnuMainMenu_Click(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void appBarMnuClear_Click(object sender, System.EventArgs e)
        {
           App.GameManager.SetBoard(TempLevels.EmptyBoard);
           App.GameManager.ResetBoard();
        }

        private void appBarEditLevel_Click(object sender, System.EventArgs e)
        {
            int[,] board =App.GameManager.GetBoard();
            int index = Convert.ToInt32(tbxLevel.Text);
            gameData.levels[index - 1] = board;
        }
    }
}