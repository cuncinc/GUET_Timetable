using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Windows.Storage;

//https://stackoverflow.com/questions/20005355/how-to-post-data-using-httpclient
//有一个保存cookie的解决方案，尝试一下，看能不能不用每次都登录


namespace GUET.BackEnd.Utils
{
    class EduSysUtil
    {
        static readonly string domain = "http://172.16.13.22";
        static readonly string loginPath = "/Login/SubmitLogin";
        static readonly string logoutPath = "/Login/Logout";
        static readonly string validateCodePath = "/login/GetValidateCode";
        static readonly string personInfoPath = "/Student/GetPerson";
        static readonly string curTermPath = "/Comm/CurTerm";
        static readonly string courseTablePath = "/student/getstutable";
        static readonly string labTablePath = "/student/getlabtable";
        static readonly string scorePath = "/Student/GetStuScore";
        static readonly string labScorePath = "/student/getstulab";
        static readonly string collectPath = "/student/genstuby";
        static readonly string gpaPath = "/student/getbyxw";

        private HttpClient client;
        private string userId;
        private string password;

        public EduSysUtil(string userId, string password)
        {
            this.userId = userId;
            this.password = password;

            // client要保留Cookie，否则不能正常获取数据
            var handler = new HttpClientHandler() { UseCookies = true };
            client = new HttpClient(handler);
            //request的header要加上下列信息才能正常获取数据
            client.DefaultRequestHeaders.Add("Referer", "http://172.16.13.22/Login/MainDesktop");
        }


        public async Task<Stream> GetValidateCodePicture()
        {
            var validateUrl = domain + validateCodePath;
            var response = await client.GetAsync(validateUrl);
            response.EnsureSuccessStatusCode();
            var validateCodeBodyStream = await response.Content.ReadAsStreamAsync();
            //var validateCodeBodyBytes = await response.Content.ReadAsByteArrayAsync();
            //_ = File.WriteAllBytesAsync(@"E:\CC\Desktop\test.txt", validateCodeBodyBytes);
            //Debug.WriteLine(validateCodeBodyBytes);
            return validateCodeBodyStream;
        }

        public async Task<bool> Login(string validateCode)
        {
            // 步骤：
            // 1. 去/login/GetValidateCode获取验证码
            // 2. 解析验证码
            // 3. 去/Login/SubmitLogin使用post登录，body为us=学号&pwd=密码&ck=验证码
            // 4. 解析返回的数据，若验证码不对，则重新登录

            var loginUrl = domain + loginPath;

        //LoginLabel: //goto语句的标签，验证码错误后重新登录
            try
            {

                ////测试，输出到桌面进行验证，稳定后可删去
                //var validateCodeBodyBytes = await response.Content.ReadAsByteArrayAsync();
                //_ = File.WriteAllBytesAsync(@"E:\CC\Desktop\test.jpg", validateCodeBodyBytes);


                //var validateCodeBitmap = SKBitmap.Decode(validateCodeBodyStream);
                //var validateCode = OcrUtil.GetCodeFromStream(validateCodeBodyStream);
                ////这个垃圾库解析出来的字符串会带有回车，要去掉才行
                //if (validateCode.Length > 4)
                //{
                //    validateCode = validateCode.Substring(0, 4);
                //}

                //构造/Login/SubmitLogin的Post表单
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("us", userId),
                    new KeyValuePair<string, string>("pwd", password),
                    new KeyValuePair<string, string>("ck", validateCode)
                });

                var response = await client.PostAsync(loginUrl, formContent);
                response.EnsureSuccessStatusCode();
                var loginBodyText = await response.Content.ReadAsStringAsync();
                if (loginBodyText.Contains("true"))
                    return true;
                else if (loginBodyText.Contains("验证码不正确"))
                    ;
                //goto LoginLabel;    //刷新验证码再登录一次
                else //此处应该把错误信息展示到UI上
                    return false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException! Login WebUtils");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return false;
        }

        public async Task<bool> Logout()
        {
            //要清除client的Cookie，不然会影响到下一个用户

            var logoutUrl = domain + logoutPath;

            try
            {
                var response = await client.PostAsync(logoutUrl, null);
                //response.EnsureSuccessStatusCode();   //状态码是302
                client.Dispose();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException! Logout WebUtils");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return true;
        }

        public async Task<string> PersonInfo()
        {
            var personInfoUrl = domain + personInfoPath;
            try
            {
                var response = await client.PostAsync(personInfoUrl, null);
                response.EnsureSuccessStatusCode();
                var personInfoJson = await response.Content.ReadAsStringAsync();
                return personInfoJson;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException! PersonInfo WebUtils");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }

        public async Task<string> CurrentTerm()
        {
            var curTermUrl = domain + curTermPath;

            try
            {
                var response = await client.GetAsync(curTermUrl);
                response.EnsureSuccessStatusCode();
                var curTerm = await response.Content.ReadAsStringAsync();
                return curTerm;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException! CurTerm WebUtils");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }

        public async Task<string> CourseTable(string term)
        {
            var builder = new UriBuilder(domain + courseTablePath);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["term"] = term;
            builder.Query = query.ToString();
            var courseTableUrl = builder.ToString();

            try
            {
                var response = await client.GetAsync(courseTableUrl);
                response.EnsureSuccessStatusCode();
                var courseTableJson = await response.Content.ReadAsStringAsync();
                return courseTableJson;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException! CourseTable WebUtils");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }

        public async Task<string> LabTable(string term)
        {
            var builder = new UriBuilder(domain + labTablePath);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["term"] = term;
            builder.Query = query.ToString();
            var labTableUrl = builder.ToString();
            try
            {
                var response = await client.GetAsync(labTableUrl);
                response.EnsureSuccessStatusCode();
                var labTableJson = await response.Content.ReadAsStringAsync();
                return labTableJson;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException! LabTable WebUtils");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }

        public async Task<string> Score(string term)
        {
            var builder = new UriBuilder(domain + scorePath);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["term"] = term;
            builder.Query = query.ToString();
            var scoreUrl = builder.ToString();
            try
            {
                var response = await client.GetAsync(scoreUrl);
                response.EnsureSuccessStatusCode();
                var scoreJson = await response.Content.ReadAsStringAsync();
                return scoreJson;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException! Score WebUtils");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }

        public async Task<string> LabScore(string term)
        {
            var builder = new UriBuilder(domain + labScorePath);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["term"] = term;
            builder.Query = query.ToString();
            var labScoreUrl = builder.ToString();
            try
            {
                var response = await client.GetAsync(labScoreUrl);
                response.EnsureSuccessStatusCode();
                var labScoreJson = await response.Content.ReadAsStringAsync();
                return labScoreJson;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException! Score WebUtils");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }
        
        public async Task<string> GetGPA()
        {
            //var term = ApplicationData.Current.LocalSettings.Values["currentTerm"];
            //var collectUrl = domain + collectPath + "/" + term;

            //try
            //{
            //    //构造/Login/SubmitLogin的Post表单
            //    var formContent = new FormUrlEncodedContent(new[]
            //    {
            //        new KeyValuePair<string, string>("ctype", "byyqxf"),
            //        new KeyValuePair<string, string>("stid", password),
            //        new KeyValuePair<string, string>("grade", validateCode),
            //        new KeyValuePair<string, string>("spno", validateCode)
            //    });

            //    var response = await client.PostAsync(loginUrl, formContent);
            //    response.EnsureSuccessStatusCode();
            //    var loginBodyText = await response.Content.ReadAsStringAsync();
            //    if (loginBodyText.Contains("true"))
            //        return true;
            //    else if (loginBodyText.Contains("验证码不正确"))
            //        ;
            //    //goto LoginLabel;    //刷新验证码再登录一次
            //    else //此处应该把错误信息展示到UI上
            //        return false;
            //}
            //catch (HttpRequestException e)
            //{
            //    Console.WriteLine("\nException! Login WebUtils");
            //    Console.WriteLine("Message :{0} ", e.Message);
            //}
            //return false;



            var gpaUrl = domain + gpaPath; // + "&" + "_dc=" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            try
            {
                var response = await client.GetAsync(gpaUrl);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException! CurTerm WebUtils");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }
    }
}
