using GUET.BackEnd.Entity;
using GUET.BackEnd.Model;
using GUET.BackEnd.Service;
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

namespace GUET.FrontEnd
{
    public sealed partial class CourseTablePage : Page
    {
        List<string> schoolWeeks = new List<string>();  //数据集，存储“第1周”到“第20周”
        List<Lesson> lessonTable = new List<Lesson>();    //数据集，存储课表的信息
        string[] colors = { "#2BDAEC", "#FF7A9C", "#EF6A6A", "#B6F45F", "#5AA9ED", "#6FE67C", "#FFCA65", "#FC9065", "#AB73D3", "#ADC190", "#27E7CA", "#55C0FD", "#FCB25B", "#AB73D3" };

        ObservableCollection<LessonTableItem> tableItems = new ObservableCollection<LessonTableItem>(); //数据集，存储gridView的数据，35个
        //键值对，课号是键，GridView上的格子颜色是值。保证课号一致的格子有相同的颜色；课号不一致的格子颜色不同
        Dictionary<string, string> Cno_Color_Dic = new Dictionary<string, string>();
        //ObservableCollection<CourseDetail> courseDetails = new ObservableCollection<CourseDetail>();

        public CourseTablePage()
        {
            this.InitializeComponent();
            initData();
        }

        private async void initData()
        {
            //初始化数据集
            for (int i = 1; i <= 20; ++i)
            {
                schoolWeeks.Add($"第{i}周");
            }

            for (int i = 0; i < 35; ++i)
            {
                tableItems.Add(new LessonTableItem());
            }

            //从网页获取课表数据
            lessonTable = await HtmlUtils.GetCourseTable(4);

            foreach (var lesson in lessonTable)
            {
                string text = lesson.CourseName;
                if (!Cno_Color_Dic.ContainsKey(lesson.CourseNo))//若此课号不在键值里
                {
                    Cno_Color_Dic.Add(lesson.CourseNo, colors[Cno_Color_Dic.Count]);
                }
            }
        }

        private void showCourseTable(int curSchoolWeek)
        {
            foreach (var lesson in lessonTable)
            {
                LessonTableItem item = null;
                int index = (lesson.AttendSection - 1) * 7 + lesson.AttendWeekDay - 1;  //List的index
                string text = lesson.CourseName;
                if (!string.IsNullOrWhiteSpace(lesson.Classroom))
                {
                    text += $" @{lesson.Classroom}";
                }
                item = new LessonTableItem(lesson.CourseNo, text);
                tableItems[index] = item;
            }

            //要渲染2遍，不然无颜色的格子会覆盖掉有颜色的格子
            foreach (var lesson in lessonTable)
            {
                LessonTableItem item = null;
                int index = (lesson.AttendSection - 1) * 7 + lesson.AttendWeekDay - 1;  //List的index
                string text = lesson.CourseName;
                if (!string.IsNullOrWhiteSpace(lesson.Classroom))
                {
                    //如果没有教室信息则不显示
                    text += $" @{lesson.Classroom}";
                }
                if (curSchoolWeek >= lesson.StartWeek && curSchoolWeek <= lesson.EndWeek)
                {
                    string color = Cno_Color_Dic[lesson.CourseNo];
                    item = new LessonTableItem(lesson.CourseNo, color, text);
                    tableItems[index] = item;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedStr = e.AddedItems[0].ToString();
            int curSchoolWeek = int.Parse(selectedStr.Substring(1, selectedStr.IndexOf("周") - 1));
            showCourseTable(curSchoolWeek);
        }

        //private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    courseDetails.Clear();
        //    //弹出本节课详细信息的窗口
        //    var item = e.ClickedItem as LessonTableItem;
        //    if(string.IsNullOrWhiteSpace(item.Text))   //如果格子里没有信息
        //    {
        //        return;
        //    }
        //    FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        //    string courseNo = item.CourseNo;
        //    courseDetails.Add(new CourseDetail(courseNo, courseNo, courseNo, courseNo, courseNo, courseNo));
        //}
    }
}