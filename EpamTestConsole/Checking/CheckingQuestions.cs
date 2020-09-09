using System;
using System.Collections.Generic;
using System.Text;

namespace EpamTestConsole
{
    class CheckingQuestions
    {
        // 4 типа вопросов проверет:
        // 1. вопрос где ответ не проверяется, просто запоминает что пользователь ввёл
        // 2. вопрос без ответа, но присутствуют варианты
        // 3. вопрос где сравнивается введённая строка пользователем с ответом на вопрос в тесте
        // 4. вопрос где сравнивается выбранный вариант ответа с ответом на вопрос в тесте

        public List<VerifiedQuestion> VerifiedQuestions = new List<VerifiedQuestion>();
        public void CheckingAllQuestions(Section section, List<string> answers)
        {
            for (int i = 0; i < answers.Count; i++)
            {
                if (section.Questions[i].CheckAnswer)
                {
                    if (!section.Questions[i].Options)
                    {
                        VerifiedQuestions.Add
                            (
                            new VerifiedQuestion(section.NameSection, section.Questions[i].TextQuestion, answers[i],
                            CheckingQuestion(section.Questions[i], answers[i]).ToString()
                            ));
                    }
                    else
                    {
                        int answerNumber = Convert.ToInt32(answers[i]);
                        VerifiedQuestions.Add
                            (
                            new VerifiedQuestion(section.NameSection, section.Questions[i].TextQuestion,
                            section.Questions[i].AnswerOptions[answerNumber],
                            CheckingQuestion(section.Questions[i], answerNumber).ToString()                            
                            ));
                    }
                }
                else
                {
                    VerifiedQuestions.Add(new VerifiedQuestion(section.NameSection, section.Questions[i].TextQuestion, answers[i],""));
                }
            }
        }

        private bool CheckingQuestion(Question question, string answer)
        {
            if(question.Answer == answer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CheckingQuestion(Question question, int answerNumber)
        {   
            if (question.AnswerOptions[answerNumber] == question.Answer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
