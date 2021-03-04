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
using System.Threading.Tasks;
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
        WebService webService = new WebService();
        public ScorePage()
        {
            this.InitializeComponent();
            initData();
        }

        private async void initData()
        {
            var list = await webService.GetScores(null);
            //double gpa = CalculateGPA(list);
            double gpa = await GetGPA();
            TextBlock_GPA.Text = $"入学至今学分绩 {gpa.ToString("0.00")}";

            scores.Clear();
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
            else if (e.Column.Tag.ToString() == "ExamScore")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.ExamScore ascending select score);
                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    ScoreGrid.ItemsSource = new ObservableCollection<Score>(from score in scores orderby score.ExamScore descending select score);
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

        private async Task<double> GetGPA()
        {
            return await webService.GetGPA();
        }

        private double CalculateGPA(List<Score> scores)
        {
            double gradeSum = 0.0;
            double creditSum = 0.0;
            foreach (var score in scores)
            {
                if (score.CourseNo.Contains("RZ") || score.CourseNo[0] == 'T') //这里CourseNo是课程代码，不是课号，当时写错了
                {
                    //学分绩不计算任选课和通识课。课程代码以RZ开头的为任选，以T为第一个字母的的是通识课
                    continue;
                }

                if (score.Grade.Equals("优"))
                {
                    gradeSum += 95 * score.CourseCredit;
                }
                else if (score.Grade.Equals("良"))
                {
                    gradeSum += 85 * score.CourseCredit;
                }
                else if (score.Grade.Equals("中"))
                {
                    gradeSum += 75 * score.CourseCredit;
                }
                else if (score.Grade.Equals("及格"))
                {
                    gradeSum += 65 * score.CourseCredit;
                }
                else if (score.Grade.Equals("不及格"))
                {
                    gradeSum += 40 * score.CourseCredit;
                }
                else
                {
                    gradeSum += score.Grade * score.CourseCredit;
                }

                creditSum += score.CourseCredit;
            }
            double gpa = gradeSum / creditSum;
            return gpa;
        }

        private void ScoreGrid_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            //double gpa = CalculateGPA(scores.ToList());
            //double gpa = await GetGPA();
            //TextBlock_GPA.Text = $"入学至今学分绩 {gpa.ToString("0.00")}";
        }

        private void ScoreGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string text = (e.Column.GetCellContent(e.Row) as TextBox).Text;
            double grade;
            if (!double.TryParse(text, out grade) || grade<0 || grade>100)
            {
                e.Cancel = true;
            }
        }
    }
}
