using GUET.BackEnd.Entity;
using GUET.BackEnd.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUET.BackEnd.Service
{
    class WebService
    {
        private static EduSysUtil client;

        public WebService(string userId, string password)
        {
            client = new EduSysUtil(userId, password);
        }

        public async Task<bool> Login()
        {
            return await client.Login();
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
    }
}
