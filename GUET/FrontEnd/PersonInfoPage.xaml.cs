using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace GUET.FrontEnd
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PersonInfoPage : Page
    {
        List<string> schoolWeeks = new List<string>();
        List<string> terms = new List<string>();

        public PersonInfoPage()
        {
            this.InitializeComponent();
            initData();
        }

        private void initData()
        {
            string studentName = ApplicationData.Current.LocalSettings.Values["studentName"] as string;
            string grade = ApplicationData.Current.LocalSettings.Values["grade"] as string;
            TextBlock_Name.Text = studentName;
            TextBlock_grade.Text = grade;

            //for (int i = 1; i <= 20; ++i)
            //{
            //    schoolWeeks.Add($"第{i}周");
            //}

            //for (int i = 0; i < 12; ++i)
            //{
            //    string term = $"{int.Parse(grade) + ((i - 1) / 2)}-{int.Parse(grade) + ((i - 1) / 2) + 1}_{((i + 1) % 2) + 1}";
            //    terms.Add(term);
            //}
        }

        private async void HyperlinkButton_Click_Logout(object sender, RoutedEventArgs e)
        {
            //删除所有暂存的数据
            await ApplicationData.Current.ClearAsync();
            //ApplicationData.Current.LocalSettings.Values.Remove("loginStudentID");
            //ApplicationData.Current.LocalSettings.Values.Remove("loginPassword");
            //ApplicationData.Current.LocalSettings.Values.Remove("grade");
            //ApplicationData.Current.LocalSettings.Values.Remove("studentName");
            //ApplicationData.Current.LocalSettings.Values.Remove("currentTermNum");

            (Window.Current.Content as Frame).Navigate(typeof(LoginPage));  //跳转到登录界面
        }
    }
}