using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUET.BackEnd.Model
{
    class LessonTableItem
    {
        public string CourseNo { get; set; }
        public string Color { get; set; }
        public string Text { get; set; }

        public LessonTableItem()
        {
            Color = "#E8E4E1";
            Text = null;
            CourseNo = null;
        }

        public LessonTableItem(string courseNo, string text)
        {
            Color = "#E8E4E1";
            CourseNo = courseNo;
            Text = text;
        }

        public LessonTableItem(string courseNo, string color, string text)
        {
            CourseNo = courseNo;
            Color = color;
            Text = text;
        }
    }
}
