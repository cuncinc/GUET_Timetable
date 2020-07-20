using GUET.BackEnd.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace GUET.BackEnd.Service
{
    class HtmlUtils
    {
        static readonly string Domain = "http://bkjw2.guet.edu.cn";
        static readonly string LoginPath = "/student/public/login.asp";   //?username=你的学号&passwd=你的密码&login=登　录";
        static readonly string InfoPath = "/student/Info.asp";
        static readonly string SelectedPath = "/student/Selected.asp";   //?term=2020-2021_1";
        static readonly string CourseTablePath = "/student/coursetable.asp"; //?term=2020-2021_1";
        static readonly string GPAPath = "/student/xuefenji.asp";    //?xn=2018-2019&lwBtnquery=%B2%E9%D1%AF";  //当xn=后直接接&lwB...时，获取入学至今学分绩
        static readonly string ScorePath = "/student/score.asp?lwBtnquery=查询";   // &ckind=

        private static HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = true });

        public static async Task<bool> Login(string studentID, string password)
        {
            var url = Domain + LoginPath + "?login=%B5%C7%A1%A1%C2%BC&username=" + studentID + "&passwd=" + password;
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] responseBodyByte = await response.Content.ReadAsByteArrayAsync();
            var responseBody = ChangeEncoding(responseBodyByte);
            if (responseBody.Contains("用户名或口令错，请重试!!"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static async Task Login()
        {
            string studentID = ApplicationData.Current.LocalSettings.Values["loginStudentID"] as string;
            string password = ApplicationData.Current.LocalSettings.Values["loginPassword"] as string;
            await Login(studentID, password);
        }

        public static async Task<List<string>> GetInfo()
        {
            //学号、姓名、班级、年级、学期
            await Login();
            var url = Domain + InfoPath;
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] responseBodyByte = await response.Content.ReadAsByteArrayAsync();
            var responseBody = ChangeEncoding(responseBodyByte);
            var list = getInfo(responseBody);
            return list;
        }

        static List<string> getInfo(string html)
        {
            //学号、姓名、班级、年级、学期
            var list = new List<string>();
            foreach (Match match in Regex.Matches(html, "<p>.*</p>"))
            {
                var x = match.Value.Trim();
                x = Regex.Replace(x, "</p>", "");
                list.Add(x.Substring(6));
            }
            return list;
        }

        public static async Task<List<SelectedCourse>> GetSeletedCourse(string term)
        {
            await Login();
            var url = Domain + SelectedPath + "?term=" + term;

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] responseBodyByte = await response.Content.ReadAsByteArrayAsync();
            var responseBody = ChangeEncoding(responseBodyByte);
            var list = getSelected(responseBody);
            return list;
        }

        static List<SelectedCourse> getSelected(string html)
        {
            var list = new List<SelectedCourse>();
            html = Regex.Replace(html, "<td align=center>", "");
            foreach (Match match in Regex.Matches(html, "<tr>.*</tr>"))
            {
                var x = match.Value.Trim();
                var strs = x.Split(new string[] { "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                var course = new SelectedCourse(strs[0].Substring(4), strs[1], strs[2], strs[3], strs[4], strs[5], strs[6]);
                list.Add(course);
            }
            return list;
        }

        public static async Task<List<Lesson>> GetCourseTable(string term)
        {
            await Login();
            var url = Domain + CourseTablePath + "?term=" + term;

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] responseBodyByte = await response.Content.ReadAsByteArrayAsync();
            var responseBody = ChangeEncoding(responseBodyByte);
            var list = getCourseTable(responseBody);
            return list;
        }

        static List<Lesson> getCourseTable(string html)
        {
            var list = new List<Lesson>();
            html = Regex.Replace(html, @"<td align='center'>", "");
            int section = 1;
            foreach (Match match in Regex.Matches(html, "</th>.*</td>"))
            {
                var x = match.Value.Trim();
                x = Regex.Replace(x, "</th>", "");
                var strs = x.Split(new string[] { "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                int weekday = 1;
                foreach (var lessonText in strs)
                {
                    if (!lessonText.Equals("&nbsp;"))
                    {
                        string[] lessonOnOneTablet = lessonText.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < lessonOnOneTablet.Length; i += 3)
                        {
                            string courseName = lessonOnOneTablet[i];
                            int startWeek = int.Parse(lessonOnOneTablet[i + 1].Substring(1, lessonOnOneTablet[i + 1].LastIndexOf('-') - lessonOnOneTablet[i + 1].LastIndexOf('(') - 1));
                            int endWeek = int.Parse(lessonOnOneTablet[i + 1].Substring(lessonOnOneTablet[i + 1].LastIndexOf('-') + 1, lessonOnOneTablet[i + 1].LastIndexOf(')') - lessonOnOneTablet[i + 1].LastIndexOf('-') - 1));
                            string classroom = lessonOnOneTablet[i + 1].Substring(lessonOnOneTablet[i + 1].LastIndexOf(')') + 1);
                            string courseNo = lessonOnOneTablet[i + 2].Substring(4);
                            int attendWeekday = weekday;
                            int attendSection = section;
                            var lesson = new Lesson(courseNo, courseName, classroom, startWeek, endWeek, attendWeekday, attendSection);
                            list.Add(lesson);
                        }
                    }
                    ++weekday;
                }
                ++section;
            }
            return list;
        }

        public static async Task<string> GetGPA(string schoolYear)
        {
            await Login();
            var url = Domain + GPAPath + "?lwBtnquery=%B2%E9%D1%AF&xn=" + schoolYear;
            await client.GetAsync(url);     //先访问一次，垃圾系统，第一次访问经常失败
            Thread.Sleep(100);              //第一次访问后，等待一会
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] responseBodyByte = await response.Content.ReadAsByteArrayAsync();
            var responseBody = ChangeEncoding(responseBodyByte);
            return getGPA(responseBody);
        }

        static string getGPA(string html)
        {
            var match = Regex.Match(html, "'>.*</f");
            string gpa = match.Value.Trim().Substring(2, 5);
            return gpa;
        }

        public static async Task<List<Score>> GetScore()
        {
            await Login();
            var url = Domain + ScorePath;

            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            byte[] responseBodyByte = await response.Content.ReadAsByteArrayAsync();
            var responseBody = ChangeEncoding(responseBodyByte);
            List<Score> list = getScore(responseBody);
            return list;
        }

        static List<Score> getScore(string html)
        {
            var list = new List<Score>();
            html = Regex.Replace(html, "<td align=center>", "");
            foreach (Match match in Regex.Matches(html, "<tr>.*</tr>"))
            {
                var x = match.Value.Trim();
                var strs = x.Split(new string[] { "</td>" }, StringSplitOptions.RemoveEmptyEntries);
                var score = new Score(strs[0].Substring(4), strs[1], strs[2], strs[3], Double.Parse(strs[4]), strs[5]);
                list.Add(score);
            }
            return list;
        }

        static string ChangeEncoding(byte[] bs)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  //注册以使用GB2312编码
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            bs = Encoding.Convert(gb2312, utf8, bs);
            return utf8.GetString(bs);
        }
    }
}
