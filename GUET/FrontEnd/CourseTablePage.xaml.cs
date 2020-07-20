using GUET.BackEnd.Entity;
using GUET.BackEnd.Model;
using GUET.BackEnd.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class CourseTablePage : Page
    {
        List<string> schoolWeeks = new List<string>();  //数据集，存储“第1周”到“第20周”
        ObservableCollection<LessonTableItem> tableItems = new ObservableCollection<LessonTableItem>(); //数据集，存储gridView的数据，35个
        List<Lesson> lessonTable = new List<Lesson>();    //数据集，存储课表的信息

        public CourseTablePage()
        {
            initData();
            this.InitializeComponent();
            initCourseTableData();
        }

        void initData()
        {
            for (int i = 1; i <= 20; ++i)
            {
                schoolWeeks.Add($"第{i}周");
            }
            for (int i = 0; i < 35; ++i)
            {
                tableItems.Add(new LessonTableItem("#E8E4E1", null));
            }
        }

        private async void initCourseTableData()
        {
            lessonTable = await HtmlUtils.GetCourseTable("2020-2021_1");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < tableItems.Count; ++i)
            {
                tableItems[i] = new LessonTableItem("#E8E4E1", null);
            }

            string str = e.AddedItems[0].ToString();
            int curSchoolWeek = int.Parse(str.Substring(1, str.IndexOf("周") - 1));

            foreach (var lesson in lessonTable)
            {
                if (curSchoolWeek >= lesson.StartWeek && curSchoolWeek <= lesson.EndWeek)
                {
                    int index = (lesson.AttendSection - 1) * 7 + lesson.AttendWeekDay - 1;
                    string text = $"{lesson.CourseName}@{lesson.Classroom}";
                    string color = "#6FE67C";
                    var item = new LessonTableItem(color, text);

                    tableItems[index] = item;
                }
            }
        }
    }
}
