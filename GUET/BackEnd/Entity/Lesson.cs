using System.Diagnostics;

namespace GUET.BackEnd.Entity
{
    class Lesson
    {
        public string CourseNo { get; set; }    //课号
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public int StartWeek { get; set; }
        public int EndWeek { get; set; }
        public string Classroom { get; set; }
        public int AttendWeekDay { get; set; }   //上课星期
        public int AttendSection { get; set; }   //上课节次

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

        public void Show()
        {
            Debug.WriteLine($"{CourseNo}\t{CourseName}\t{Classroom}\t{StartWeek}\t{EndWeek}\t{AttendWeekDay}\t{AttendSection}");
        }
    }
}
