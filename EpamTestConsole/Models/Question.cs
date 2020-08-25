using System;
using System.Collections.Generic;

namespace EpamTestConsole
{
    [Serializable]
    public class Question
    {
        public string TextQuestion { get; set; }
        public bool CheckAnswer { get; set; }
        public bool Options { get; set; }
        public string Answer { get; set; }
        public List<string> AnswerOptions { get; set; }

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
