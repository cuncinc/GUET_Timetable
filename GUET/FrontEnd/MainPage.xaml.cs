using GUET.BackEnd.Entity;
using GUET.BackEnd.Model;
using GUET.BackEnd.Service;
using GUET.FrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace GUET
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            //mainPageFrame.Navigate(typeof(CourseTablePage)); 
            initData();
        }

        private async void initData()
        {
            //获取信息
            var infos = await HtmlUtils.GetInfo();
            string studentName = infos[1];     //姓名
            string grade = infos[3];    //年级，如2020
            string term = infos[4];     //学期，如2020-2021_1表示大一上，2020-2021_2表示大一下
            int currentTermNum = 2 * (int.Parse(term.Substring(0, 4)) - int.Parse(grade)) + int.Parse(term.Substring(10));

            //暂存信息
            ApplicationData.Current.LocalSettings.Values["grade"] = grade;
            ApplicationData.Current.LocalSettings.Values["studentName"] = studentName;
            ApplicationData.Current.LocalSettings.Values["currentTermNum"] = currentTermNum;
        }

        private void NavigationViewItem_Tapped_CourseTable(object sender, TappedRoutedEventArgs e)
        {
            mainPageFrame.Navigate(typeof(CourseTablePage));    //tap到标签
        }

        private void NavigationViewItem_Tapped_More(object sender, TappedRoutedEventArgs e)
        {
            mainPageFrame.Navigate(typeof(PersonInfoPage));
        }

        private void NavigationViewItem_Tapped_Score(object sender, TappedRoutedEventArgs e)
        {
            mainPageFrame.Navigate(typeof(ScorePage));
        }
    }
}
