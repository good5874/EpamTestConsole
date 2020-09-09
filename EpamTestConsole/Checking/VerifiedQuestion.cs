using System;
using System.Collections.Generic;
using System.Text;

namespace EpamTestConsole
{
    class VerifiedQuestion
    {
        public string NameSection { get; set; }
        public string TextQuestion { get; set; }
        public string Answer { get; set; }
        public string Result { get; set; }

        public VerifiedQuestion(string _NameSection, string _TextQuestion,string _Answer, string _Result)
        {
            NameSection = _NameSection;
            TextQuestion = _TextQuestion;
            Answer = _Answer;
            Result = _Result;
        }
    }
}
