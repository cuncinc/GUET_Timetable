using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUET.BackEnd.Entity
{
    class Score
    {
        public string Term { get; set; }
        public string CourseName { get; set; }
        public string CourseNo { get; set; }
        public double ExamScore { get; set; }
        public double Grade { get; set; }
        public double CourseCredit { get; set; }
        public string CourseType { get; set; }


        public Score() {}

        public Score(string term, string courseName, string courseNo, double examScore, double grade, double courseCredit, string courseType)
        {
            Term = term;
            CourseName = courseName;
            CourseNo = courseNo;
            ExamScore = examScore;
            Grade = grade;
            CourseCredit = courseCredit;
            CourseType = courseType;
        }

        public void Show()
        {
            Debug.WriteLine($"{Term} {CourseNo} {CourseCredit} {Grade}");
        }
    }
}
