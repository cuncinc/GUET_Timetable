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
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace GUET.FrontEnd
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        WebService webService;
        string studentID;
        string password;

        public LoginPage()
        {
            this.InitializeComponent();
            if (ApplicationData.Current.LocalSettings.Values["loginStudentID"] != null)
            {
                textBox_loginStudentID.Text = ApplicationData.Current.LocalSettings.Values["loginStudentID"].ToString();
                passwordBox.Password = ApplicationData.Current.LocalSettings.Values["loginPassword"].ToString();
            }
        }

        private async void Button_getCode_Click(object sender, RoutedEventArgs e)
        {
            studentID = textBox_loginStudentID.Text;
            password = passwordBox.Password;
            webService = new WebService(studentID, password);
            var stream = await webService.GetValidateCodePicture();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.DecodePixelWidth = 80; //match the target Image.Width, not shown
            await bitmapImage.SetSourceAsync(stream.AsRandomAccessStream());
            image_validateCode.Source = bitmapImage;
        }

        private async void Button_login_Click(object sender, RoutedEventArgs e)
        {
            progressRing_login.IsActive = true;
            
            string validateCode = textBox_loginValidateCode.Text;
            if (await webService.Login(validateCode))  //登录验证
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
            //passwordBox.Password = "";
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Button_getCode_Click(sender, e);
            }
        }

        private async void initData()
        {
            ////获取信息
            //var infos = await HtmlUtils.GetInfo();
            var info = await webService.GetPersonInfo();
            string studentName = info.Name;     //姓名
            string grade = info.Grade;    //年级，如2020
            string term = await webService.CurrentTerm();     //学期，如2020-2021_1表示大一上，2020-2021_2表示大一下
            Debug.WriteLine(term);
            int currentTermNum = 2 * (int.Parse(term.Substring(0, 4)) - int.Parse(grade)) + int.Parse(term.Substring(10));

            //暂存信息
            ApplicationData.Current.LocalSettings.Values["grade"] = grade;
            ApplicationData.Current.LocalSettings.Values["studentName"] = studentName;
            ApplicationData.Current.LocalSettings.Values["currentTermNum"] = currentTermNum;
            ApplicationData.Current.LocalSettings.Values["currentWeekNum"] = 1;
        }

        private void textBox_loginValidateCode_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Button_login_Click(sender, e);
            }
        }
    }
}
