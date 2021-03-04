using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUET.BackEnd.Entity
{
    class PersonInfo
    {
        public string Grade { get; set; }
        public string Name { get; set; }
        public string Idcard { get; set; }
        public string StudentId { get; set; }


        public PersonInfo(){}

        public PersonInfo(string grade, string name, string idcard, string studentId)
        {
            Grade = grade;
            Name = name;
            Idcard = idcard;
            StudentId = studentId;
        }

        public void Show()
        {
            Debug.WriteLine($"{Name} {StudentId} {Idcard} {Grade}");
        }
    }
}
