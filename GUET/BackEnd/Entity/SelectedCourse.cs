namespace GUET.BackEnd.Entity
{
    class SelectedCourse
    {
        public string CourseNo { get; set; }    //课号
        public string CourseCode { get; set; }  //课程代码
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string Status { get; set; }      //是否选中，抽签抽中
        public string CheckType { get; set; }   //考察方式
        public string FeeAccount { get; set; }  //学费结算

        public SelectedCourse() { }

        public SelectedCourse(string courseNo, string courseCode, string courseName, string teacherName, string status, string checkType, string feeAccount)
        {
            CourseNo = courseNo;
            CourseCode = courseCode;
            CourseName = courseName;
            TeacherName = teacherName;
            Status = status;
            CheckType = checkType;
            FeeAccount = feeAccount;
        }
    }
}
