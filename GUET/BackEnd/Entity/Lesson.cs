using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace GUET.BackEnd.Entity
{
    class Lesson
    {
        public string CourseType { get; set; }  //课程性质，如专业限选、学科基础
        public string CourseId { get; set; }    //课程代码
        public string CourseNo { get; set; }    //课号
        public string CourseName { get; set; }  //课程名称
        public string TeacherName { get; set; } //教师名字
        public int StartWeek { get; set; }      //开始周次
        public int EndWeek { get; set; }        //结束周次
        public string Classroom { get; set; }   //教室
        public int AttendWeekDay { get; set; }  //上课星期几
        public int AttendSection { get; set; }  //上课节次
        public double Credit { get; set; }      //学分

        public Lesson() { }

        public Lesson(string courseNo, string courseName, string classroom, int startWeek, int endWeek, int attendWeekDay, int attendSection)
        {
            if (attendSection>5)
            {
                attendSection = 5;
            }
            CourseNo = courseNo;
            CourseName = courseName;
            Classroom = classroom;
            StartWeek = startWeek;
            EndWeek = endWeek;
            AttendWeekDay = attendWeekDay;
            AttendSection = attendSection;
        }

        public void ShowInConsole()
        {
            Debug.WriteLine($"{CourseNo}\t{CourseName}\t{Classroom}\t{StartWeek}\t{EndWeek}\t{AttendWeekDay}\t{AttendSection}");
        }
    }
}
