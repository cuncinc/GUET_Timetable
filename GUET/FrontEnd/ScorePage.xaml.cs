using GUET.BackEnd.Entity;
using GUET.BackEnd.Service;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace GUET.FrontEnd
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ScorePage : Page
    {
        ObservableCollection<Score> scores = new ObservableCollection<Score>();
        public ScorePage()
        {
            this.InitializeComponent();
            initData();
        }

        private async void initData()
        {
            double gpa = await HtmlUtils.CalculateGPA();
            TextBlock_GPA.Text = $"入学至今的学分绩为 {gpa.ToString("0.0000")}";
            var list = await HtmlUtils.GetScore();
            foreach(var item in list)
            {
                scores.Add(item);
            }
        }

        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            //排序
            if (e.Column.Tag.ToString() == "Grade")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.Grade ascending select score);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.Grade descending select score);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
            }
            else if (e.Column.Tag.ToString() == "Term")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.Term ascending select score);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.Term descending select score);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
            }
            else if (e.Column.Tag.ToString() == "CourseName")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.CourseName ascending select score);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.CourseName descending select score);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
            }
            else if (e.Column.Tag.ToString() == "CourseType")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.CourseType ascending select score);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.CourseType descending select score);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
            }
            else if (e.Column.Tag.ToString() == "CourseCredit")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.CourseCredit ascending select score);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.CourseCredit descending select score);
                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
            }
            // add code to handle sorting by other columns as required

            // Remove sorting indicators from other columns
            foreach (var dgColumn in ScoreGrid.Columns)
            {
                if (dgColumn.Tag.ToString() != e.Column.Tag.ToString())
                {
                    dgColumn.SortDirection = null;
                }
            }
        }
    }
}
