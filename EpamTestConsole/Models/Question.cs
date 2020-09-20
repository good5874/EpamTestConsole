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
        public string UserAnswer { get; set; }// то что пользователь ввёл в ответ на вопрос, сохраняю в это поле. Если вопросе были варианты ответов то тут будет цифра
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
        // 4 типа вопросов проверет:
        // 1. вопрос где ответ не проверяется, просто запоминает что пользователь ввёл
        // 2. вопрос без ответа, но присутствуют варианты
        // 3. вопрос где сравнивается введённая строка пользователем с ответом на вопрос в тесте
        // 4. вопрос где сравнивается выбранный вариант ответа с ответом на вопрос в тесте
        public void CheckingAnswer() // разбить проверку без лишнего гемороя на 4 метода не получается))
        {
            if(!Options)
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
                    int i = int.Parse(UserAnswer);

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

        public override string ToString()
        {
            string userAnswer = UserAnswer;
            if (Options)
            {
                int i = int.Parse(UserAnswer);
                userAnswer = AnswerOptions[i];
            }           

            return $"TextQuestion = {TextQuestion}; UserAnswer = {userAnswer}; Result = {Result}";
        }
    }
}
