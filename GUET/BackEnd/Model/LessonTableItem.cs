using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUET.BackEnd.Model
{
    class LessonTableItem
    {
        public string Color { get; set; }
        public string Text { get; set; }

        public LessonTableItem() {}

        public LessonTableItem(string color, string text)
        {
            Color = color;
            Text = text;
        }
    }
}
