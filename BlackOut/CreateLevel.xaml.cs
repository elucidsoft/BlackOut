﻿using System;
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
        bool isDirty = false;

        public CreateLevel()
        {
            InitializeComponent();
            gameData = GameData.LoadGameData();
        }

        void GameManager_OnGridBlockClicked(object sender, EventArgs e)
        {
            CheckForSolution(App.GameManager.GetBoard());
            isDirty = true;
        }

        private void CheckForSolution(int[,] board)
        {
            if (App.GameManager.CheckEmptyBoard(board) == 0)
            {
                tbSolution.Foreground = new SolidColorBrush(Colors.LightGray);
                tbSolution.Text = "Empty Board";
            }
            else if (Solver.IsSolvable(board))
            {
                tbSolution.Foreground = new SolidColorBrush(Colors.Green);
                tbSolution.Text = "Solution Found";
            }
            else
            {
                tbSolution.Foreground = new SolidColorBrush(Colors.Red);
                tbSolution.Text = "No Solution";
            }
        }

       

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.GameManager.OnGridBlockClicked += new EventHandler<EventArgs>(GameManager_OnGridBlockClicked);

            if (gameData.levels.Count == 0)
            {
                gameData.levels.Add(new int[5, 5]);
            }

            tbxLevel.Text = (gameData.levels.Count).ToString();
            tbCount.Text = (gameData.levels.Count).ToString();
            App.GameManager.Initialize(grid);
            App.GameManager.SetBoard(gameData.levels[Convert.ToInt32(tbxLevel.Text) - 1]);
            App.GameManager.DisplayGrid(false);
            CheckForSolution(App.GameManager.GetBoard());
        }

        private void appBarBtnAddLevel_Click(object sender, System.EventArgs e)
        {
            App.GameManager.SetBoard(TempLevels.EmptyBoard);
            App.GameManager.ResetBoard(true);

            int[,] board = TempLevels.EmptyBoard;
            gameData.levels.Add(board);

            tbxLevel.Text = gameData.levels.Count.ToString();
            tbCount.Text = gameData.levels.Count.ToString();
            isDirty = true;
        }

        private void appBarBtnLoadLevel_Click(object sender, System.EventArgs e)
        {
            App.GameManager.ResetBoard(true);
            App.GameManager.SetBoard(gameData.levels[Convert.ToInt32(tbxLevel.Text) - 1]);
        }

        private void appBarBtnLastLevel_Click(object sender, System.EventArgs e)
        {
            int index = Convert.ToInt32(tbxLevel.Text);
            if (index > 1)
            {
                index -= 1;
                tbxLevel.Text = index.ToString();
                int[,] board = gameData.levels[Convert.ToInt32(tbxLevel.Text) - 1];
                App.GameManager.SetBoard(board);
                App.GameManager.ResetBoard(true);
                CheckForSolution(board);
            }
        }

        private void appBarBtnNextLevel_Click(object sender, System.EventArgs e)
        {
            int index = Convert.ToInt32(tbxLevel.Text);
            if (index < gameData.levels.Count)
            {
                index += 1;
                tbxLevel.Text = index.ToString();
                
                int[,] board = gameData.levels[Convert.ToInt32(tbxLevel.Text) - 1];
                App.GameManager.SetBoard(board);
                App.GameManager.ResetBoard(true);
                CheckForSolution(board);
            }
        }

        private void appBarMnuSaveLevels_Click(object sender, System.EventArgs e)
        {
            GameData.SaveGameData(gameData);
            isDirty = false;
        }

        private void appBarMnuMainMenu_Click(object sender, System.EventArgs e)
        {
            NavigationService.GoBack();
            
        }

        private void appBarMnuClear_Click(object sender, System.EventArgs e)
        {
            App.GameManager.SetBoard(TempLevels.EmptyBoard);
            App.GameManager.ResetBoard(true);
        }

        private void appBarEditLevel_Click(object sender, System.EventArgs e)
        {
            isDirty = true;
            int[,] board = App.GameManager.GetBoard();
            if(App.GameManager.CheckEmptyBoard(board) == 0)
            {
                MessageBox.Show("You can not save an empty board!");
                return;
            }

            if (Solver.IsSolvable(board))
            {
                int index = Convert.ToInt32(tbxLevel.Text);
                gameData.levels[index - 1] = board;
            }
            else
            {
                MessageBox.Show("Did not save any changes because the current board has no solution!");
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isDirty)
            {
                if (MessageBox.Show("You have unsaved changes, leaving will result in them being lost!", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void grid_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            App.GameManager.OnGridBlockClicked -= new EventHandler<EventArgs>(GameManager_OnGridBlockClicked);
        }
    }
}