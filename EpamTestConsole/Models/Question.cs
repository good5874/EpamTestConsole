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
        public string UserAnswer { get; set; }
        public string Result { get; set; }
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

        public void CheckingAnswer()
        {
            if (!Options)
            {
                if (!CheckAnswer)
                {
                    Result = "";
                }
                else
                {
                    if (UserAnswer == Answer)
                    {
                        Result = "true";
                    }
                    else
                    {
                        Result = "false";
                    }
                }
            }
            else if (Options)
            {
                if (!CheckAnswer)
                {
                    Result = "";
                }
                else
                {
                    if (UserAnswer == ConsoleMenuConstant.IncorrectInput)
                    {
                        Result = "false";
                    }
                    else
                    {
                        int i = int.Parse(UserAnswer) - 1;

                        if (AnswerOptions[i] == Answer)
                        {
                            Result = "true";
                        }
                        else
                        {
                            Result = "false";
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            string userAnswer = UserAnswer;
            if (Options)
            {
                if (UserAnswer != ConsoleMenuConstant.IncorrectInput
                    && UserAnswer != ConsoleMenuConstant.TimeIsOver
                    && UserAnswer != null)
                {
                    int i = int.Parse(UserAnswer) - 1;
                    userAnswer = AnswerOptions[i];
                }
            }

            return $"\tTextQuestion = {TextQuestion}; UserAnswer = {userAnswer}; Result = {Result}";
        }
    }
}
