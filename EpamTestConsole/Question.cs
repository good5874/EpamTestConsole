using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpamTestConsole
{
    [Serializable]
    public class Question
    {
        private string textQuestion;
        private bool checkAnswer;
        private bool options;
        private string answer;
        private List<string> answerOptions;

        public string TextQuestion { 
            get { return textQuestion; } 
            set { if (value != null) textQuestion = value; }
        }

        public bool CheckAnswer
        {
            get { return checkAnswer; }
            set { checkAnswer = value; }
        }
        public bool Options
        {
            get { return options; }
            set { options = value; }
        }
        public string Answer
        {
            get { return answer; }
            set { if (CheckAnswer == true && value != null) answer = value; }
        }
        public List<string> AnswerOptions
        {
            get { return answerOptions; }
            set { if(Options == true && value != null) answerOptions = value; }
        }

        public Question(string question, bool checkAnswer, bool options, 
            string answer, List<string> answerOptions)
        {
            TextQuestion = question;
            CheckAnswer = checkAnswer;
            Options = options;

            Answer = answer;
            AnswerOptions = answerOptions;
        }      
    }
}
