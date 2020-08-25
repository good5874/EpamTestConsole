using System;
using System.Collections.Generic;

namespace EpamTestConsole
{
    [Serializable]
    public class Question
    {
        private string textQuestion;
        private string answer;
        private List<string> answerOptions;

        public string TextQuestion
        {
            get { return textQuestion; }
            set { if (value != "") textQuestion = value; }
        }

        public bool CheckAnswer { get; set; }
        public bool Options { get; set; }

        public string Answer
        {
            get { return answer; }
            set { if (CheckAnswer == true && value != "") answer = value; }
        }
        public List<string> AnswerOptions
        {
            get { return answerOptions; }
            set { if (Options == true) answerOptions = value; }
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
