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
        public string Grade { get; set; }
        public double CourseCredit { get; set; }
        public string CourseType { get; set; }

        public Score() { }

        public Score(string term, string courseName, string courseNo, string grade, double courseCredit, string courseType)
        {
            Term = term;
            CourseName = courseName;
            CourseNo = courseNo;
            Grade = grade;
            CourseCredit = courseCredit;
            CourseType = courseType;
        }

        public void Show()
        {
            Debug.WriteLine($"{Term}\t{CourseName}\t{CourseNo}\t{Grade}\t{CourseCredit}\t{CourseType}");
        }
    }
}
