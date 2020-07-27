using GUET.BackEnd.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Button_login_Click(object sender, RoutedEventArgs e)
        {
            progressRing_login.IsActive = true;

            string studentID = textBox_loginStudentID.Text;
            string password = passwordBox.Password;
            if (await HtmlUtils.Login(studentID, password))  //登录验证
            {
                //账号密码验证成功，注意不要把学号和密码的存储下移，在GetInfo中会用到
                ApplicationData.Current.LocalSettings.Values["loginStudentID"] = studentID;
                ApplicationData.Current.LocalSettings.Values["loginPassword"] = password;
                initData();
                this.Frame.Navigate(typeof(MainPage));  //跳转到主界面
            }
            else
            {
                MessageDialog tipDialog = new MessageDialog("无网络连接或密码错误")
                {
                    Title = "登录失败"
                };
                await tipDialog.ShowAsync();
            }
            progressRing_login.IsActive = false;
            passwordBox.Password = "";
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Button_login_Click(sender, e);
            }
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
            ApplicationData.Current.LocalSettings.Values["currentWeekNum"] = 1;
        }
    }
}
