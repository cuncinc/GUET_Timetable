using GUET.BackEnd.Entity;
using GUET.BackEnd.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace GUET.BackEnd.Service
{
    class WebService
    {
        private static EduSysUtil client;
        private static string currentTerm;

        public WebService() {}

        public WebService(string userId, string password)
        {
            client = new EduSysUtil(userId, password);
        }

        public async Task<bool> Login(string validateCode)
        {
            var state = await client.Login(validateCode);
            currentTerm = await CurrentTerm();
            ApplicationData.Current.LocalSettings.Values["currentTerm"] = currentTerm;
            return state;
        }

        public async Task<Stream> GetValidateCodePicture()
        {
            return await client.GetValidateCodePicture();
        }

        public async Task<string> CurrentTerm()
        {
            var termJson = await client.CurrentTerm();
            var term = termJson.Substring(termJson.IndexOf('"') + 1, 11);
            return term;
        }

        public async Task<PersonInfo> GetPersonInfo()
        {
            PersonInfo personInfo = new PersonInfo();
            var json = await client.PersonInfo();
            JObject jObject = JObject.Parse(json);
            JObject jInfo = (JObject)jObject["data"];
            personInfo.Grade = (string)jInfo["grade"];
            personInfo.Name = (string)jInfo["name"];
            personInfo.Idcard = (string)jInfo["idcard"];
            personInfo.StudentId = (string)jInfo["stid"];

            return personInfo;
        }
        
        public async Task<List<Lesson>> CurrentLessons()
        {
            return await GetLessons(currentTerm);
        }

        public async Task<List<Lesson>> GetLessons(string term)
        {
            var lessones = new List<Lesson>();
            var json = await client.CourseTable(term);

            JObject jObject = JObject.Parse(json);
            JArray jLessones = (JArray)jObject["data"];

            foreach (var jLesson in jLessones)
            {
                Lesson lesson = new Lesson();
                lesson.CourseType = (string)jLesson["tname"];
                lesson.CourseId = (string)jLesson["courseid"];
                lesson.CourseNo = (string)jLesson["courseno"];
                lesson.CourseName = (string)jLesson["cname"];
                lesson.TeacherName = (string)jLesson["name"];
                lesson.StartWeek = (int)jLesson["startweek"];
                lesson.EndWeek = (int)jLesson["endweek"];
                lesson.Classroom = (string)jLesson["croomno"];
                lesson.AttendWeekDay = (int)jLesson["week"];
                lesson.AttendSection = (int)jLesson["seq"];
                if (lesson.AttendSection > 5)
                {
                    lesson.AttendSection = 5;
                }
                lesson.Credit = (double)jLesson["xf"];
                lessones.Add(lesson);
            }
            return lessones;
        }
    
        public async Task<List<Score>> GetScores(string term)
        {
            var scores = new List<Score>();
            var json = await client.Score(term);

            JObject jObject = JObject.Parse(json);
            JArray jScores = (JArray)jObject["data"];

            foreach (var jScore in jScores)
            {
                var score = new Score
                {
                    Term =          (string)jScore["term"],
                    CourseName =    (string)jScore["cname"],
                    ExamScore =     (double)jScore["khcj"],
                    Grade =         (double)jScore["score"],
                    CourseType =    (string)jScore["kctype"],
                    CourseCredit =  (double)jScore["xf"],
                    CourseNo =      (string)jScore["cid"]
                };

                scores.Add(score);
            }
            return scores;
        }
    
        public async Task<double> GetGPA()
        {
            var json = await client.GetGPA();

            JObject jObject = JObject.Parse(json);
            JArray jarray = (JArray)jObject["data"];
            var gpa = (double)jarray[0]["xfj"];
            return gpa;
        }
        
        public async void Logout()
        {
            await client.Logout();
        }
    }
}
