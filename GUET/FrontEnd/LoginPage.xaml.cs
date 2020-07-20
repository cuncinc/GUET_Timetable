using GUET.BackEnd.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            string studentID = textBox_loginStudentID.Text;
            string password = passwordBox.Password;
            progressRing_login.IsActive = true;

            if (await HtmlUtils.Login(studentID, password))  //登录验证
            {
                //账号密码验证成功
                this.Frame.Navigate(typeof(MainPage));  //跳转到主界面
                ApplicationData.Current.LocalSettings.Values["loginStudentID"] = studentID;
                ApplicationData.Current.LocalSettings.Values["loginPassword"] = password;
            }
            else
            {
                MessageDialog tipDialog = new MessageDialog("学号或密码错误，请重新输入")
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
    }
}
