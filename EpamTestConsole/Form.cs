using System;
using System.Collections.Generic;

namespace EpamTestConsole
{
    [Serializable]
    public class Form
    {
        public Section FirstLevelSection = null;
        public List<Section> SecondLevelSection = new List<Section>();
        public List<Section> ThirdLevelSection = new List<Section>();


        public void CreateTest(string nameTest)
        {
            Section firstSection = new Section(nameTest);

            FirstLevelSection = firstSection;

            SecondLevelSection = AddSection(FirstLevelSection);

            foreach (Section section in SecondLevelSection)
            {
                foreach (Section sectionLayer3 in AddSection(section))
                    ThirdLevelSection.Add(sectionLayer3);
            }

            AddQuestion(FirstLevelSection);

            foreach (Section s in SecondLevelSection)
            {
                AddQuestion(s);
            }
            foreach (Section s in ThirdLevelSection)
            {
                AddQuestion(s);
            }
        }

        public void AddQuestion(Section section)
        {
            for (int i = 0; ; i++)
            {
                if (i > 0)
                {
                    Console.WriteLine("Чтобы закончить добавление вопросов введите y");
                    if (Console.ReadLine() == "y") break;
                }

                Console.WriteLine($"Добавить в {section.NameSection} добавить вопросы?");
                if (Console.ReadLine() == "y")
                {

                    Console.WriteLine("Введите вопрос:");
                    string question = Console.ReadLine();
                    Console.WriteLine("Введите ответ:");
                    string answer = Console.ReadLine();
                    Console.WriteLine("Добавить варианты ответов?");
                    List<string> answerOptions = null;

                    if (Console.ReadLine() == "y")
                    {
                        Console.WriteLine("Введите n когда добавили достаточно вариантов ответов.");
                        answerOptions = new List<string>();
                        for (; ; )
                        {
                            string tmp = Console.ReadLine();
                            if (tmp == "n")
                            {
                                break;
                            }
                            else
                            {
                                answerOptions.Add(tmp);
                            }
                        }
                    }

                    bool checkAnswer = false;
                    if (answer != null && answer != "")
                        checkAnswer = true;

                    bool options = false;
                    if (answerOptions != null)
                        options = true;

                    bool validateParam = false;
                    if (String.IsNullOrEmpty(question) && String.IsNullOrEmpty(answer))
                    {
                        if(answerOptions!=null)
                        {
                            foreach(string str in answerOptions)
                            {
                                if(String.IsNullOrEmpty(str))
                                {
                                    validateParam = true;
                                }
                            }
                        }                       
                    }
                    if (!validateParam)
                    {
                        section.Questions.Add(CreateQuestion(question, checkAnswer, options, answer, answerOptions));
                    }
                }
            }
        }

        public void WriteSectionToConsole(Section section)
        {
            List<string> answers = new List<string>();

            Console.WriteLine(section.NameSection);   

            foreach (Question question in section.Questions)
            {
                string answer;
                Console.WriteLine("Вопрос: " + question.TextQuestion);
                if (question.Options)
                {
                    int i = 1;
                    Console.WriteLine("Варианты ответа:");
                    foreach (var str in question.AnswerOptions)
                    {
                        Console.WriteLine($" {i})" + str);
                        i++;
                    }
                    Console.WriteLine("Введите нужный вариант ответа:");
                }
                else Console.WriteLine("Введите ответ:");
                answer = Console.ReadLine();
                answers.Add(answer);
            }

        }//проверить ответы

        public List<Section> AddSection(Section section)
        {
            List<Section> sections = new List<Section>();
            Console.WriteLine($"Добавить потемы к {section.NameSection} ?");
            if (Console.ReadLine() == "y")
            {
                Console.WriteLine($"Сколько добавить к {section.NameSection} подтем?");
                int sectionCount = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите название подтем:");
                for (int i = 0; i < sectionCount; i++)
                {
                    sections.Add(new Section(Console.ReadLine(), section));
                }
            }
            return sections;
        }
        public Question CreateQuestion(string _question, bool _checkAnswer, bool _options,
            string _answer = null, List<string> _answerOptions = null)
        {
            Question question = new Question(_question, _checkAnswer, _options, _answer, _answerOptions);
            return question;
        }

    }
}
