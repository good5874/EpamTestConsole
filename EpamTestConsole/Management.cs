using System;
using System.Collections.Generic;
using System.Linq;

namespace EpamTestConsole
{
    [Serializable]
    public class Management
    {
        TreeNode RootNode { get; set; } = new TreeNode(new Section("root"));

        public void CreateTest(string nameTest)
        {
            RootNode.Section.NameSection = nameTest;
            AddSection(RootNode);
            AddQuestion(RootNode);           
        }
        public void EditTest()
        {

            Console.WriteLine("Введите тему или подтему, которую нужно редактировать:");
            string name = Console.ReadLine();
            TreeNode node =RootNode.Search(RootNode, name);
            Console.WriteLine("1. Изменить название темы");
            Console.WriteLine("2. Изменить вопрос");
            Console.WriteLine("3. Добавить вопросы");
            Console.WriteLine("4. Удалить вопрос");
            Console.WriteLine("5. Добавить подтему");
            Console.WriteLine("6. Удалить подтему");
            Console.WriteLine("Введите нужную цифру:");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Введите новое название:");
                    node.Section.NameSection = Console.ReadLine();
                    break;
                case "2":
                    Console.WriteLine("Введите номер вопроса:");
                    EditQuestion(node.Section, Console.ReadLine());
                    break;
                case "3":
                    AddQuestion(node,true);
                    break;
                case "4":
                    Console.WriteLine("Введите номер вопроса:");
                    DeleteQuestion(node.Section, Console.ReadLine());
                    break;
                case "5":
                    AddSection(node);
                    break;
                case "6":
                    Console.WriteLine("Введите название темы:");
                    node.Delete(ref node, Console.ReadLine());//удалит дочерние подтемы
                    break;
                default:
                    Console.WriteLine("Редактирование отменено");
                    break;
            }            
        }
        public void StartTest()
        {  
            Queue<TreeNode> q = new Queue<TreeNode>();
            q.Enqueue(RootNode);

            while (q.Count != 0)
            {
                TreeNode temp = q.Dequeue();
                WriteSectionToConsole(temp.Section);
                if (temp.ChildNodes == null) continue;
                foreach (var node in temp.ChildNodes)
                {                    
                    q.Enqueue(node);
                }
            }            
        }
        private void AddSection(TreeNode root)
        {
            Queue<TreeNode> q = new Queue<TreeNode>();
            q.Enqueue(root);

            while (q.Count != 0)
            {
                TreeNode temp = q.Dequeue();

                Console.WriteLine($"Добавить потемы к {temp.Section.NameSection} ?");
                if (Console.ReadLine() == Сommands.y.ToString())
                {
                    Console.WriteLine($"Сколько подтем добавить к {temp.Section.NameSection} подтем?");
                    int sectionCount = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Введите название подтем:");
                    for (int i = 0; i < sectionCount; i++)
                    {
                        string nameSection = Console.ReadLine();
                        temp.AddChildNode(new TreeNode(new Section(nameSection)));
                    }
                }
                else
                {
                    Console.WriteLine($"Добавление подтем к {temp.Section.NameSection} отменено");
                }

                if (temp.ChildNodes == null) continue;
                foreach (var node in temp.ChildNodes)
                {                   
                    q.Enqueue(node);
                }
            }           
        }
        private void AddQuestion(TreeNode root, bool edit = false)
        {
            Queue<TreeNode> q = new Queue<TreeNode>();
            q.Enqueue(root);
            int i = 0;

            List<Question> tmpQuestions=null;
            while (q.Count != 0)
            {
                if(edit)
                {
                    if (i == 0)
                    {
                        tmpQuestions = root.Section.Questions;
                    }
                    if (i>0)
                    {
                        root.Section.Questions = root.Section.Questions.Concat(tmpQuestions).ToList();
                        return;
                    }
                    i++;
                }
                
                TreeNode temp = q.Dequeue();

                Console.WriteLine($"Добавить вопросы к {temp.Section.NameSection}?");
                if (Console.ReadLine() == Сommands.y.ToString())
                {
                    temp.Section.Questions = CreateQuestions(temp.Section);
                }
                else
                {
                    Console.WriteLine($"Добавление вопросов к {temp.Section.NameSection} отменено");
                }

                if (temp.ChildNodes == null) continue;
                foreach (var node in temp.ChildNodes)
                {
                    q.Enqueue(node);
                }
            }
        }
        private List<Question> CreateQuestions(Section section)
        {
            List<Question> Questions = new List<Question>();

            string question;
            string answer;
            List<string> answerOptions= null;
            bool checkAnswer = false;
            bool options = false;

            for (int i = 0; ; i++)
            {
                if (i > 0)
                {
                    Console.WriteLine("Чтобы закончить добавление вопросов введите y");
                    if (Console.ReadLine() == Сommands.y.ToString())
                    {
                        return Questions;
                    }
                }

                Console.WriteLine("Введите вопрос:");
                question = Console.ReadLine();
                Console.WriteLine("Введите ответ:");
                answer = Console.ReadLine();  

                checkAnswer = false;
                if (answer != null && answer != "")
                    checkAnswer = true;

                options = false;
                if (answerOptions != null)
                    options = true;
                Questions.Add(new Question(question, checkAnswer, options, answer, answerOptions));
                answerOptions = AddAnswerOption(Questions.Last());
            }
        }
        private void EditQuestion(Section section, string indexQuestion)
        {
            Question question = section.Questions[Convert.ToInt32(indexQuestion)];
            Console.WriteLine($"Изменить вопрос <{question.TextQuestion}>?");
            if (Console.ReadLine() == Сommands.y.ToString())
            {
                Console.WriteLine($"Введите <y> чтобы изменить вопрос");
                if (Console.ReadLine() == Сommands.y.ToString())
                {
                    Console.WriteLine($"Введите вопрос:");
                    question.TextQuestion = Console.ReadLine();
                }
                Console.WriteLine($"Введите <y> чтобы изменить ответ");
                if (Console.ReadLine() == Сommands.y.ToString())
                {
                    Console.WriteLine($"Введите ответ:");
                    question.Answer = Console.ReadLine();

                    if (question.Answer != null)
                    {
                        question.Options = true;
                    }
                    else
                    {
                        question.Options = false;
                    }
                }
                Console.WriteLine($"Добавить или изменить нажмите <y>, удалить варианты ответов введите <del>");
                if (Console.ReadLine() == Сommands.y.ToString())
                {
                    question.AnswerOptions = AddAnswerOption(question);
                    
                    if (question.Answer != null && question.Answer != "")
                        question.CheckAnswer = true;                   
                    
                }
                else if (Console.ReadLine() == Сommands.del.ToString())
                {
                    question.AnswerOptions = null;
                    question.Options = false;
                }

            }
            section.Questions[Convert.ToInt32(indexQuestion)] = question;
        }
        private List<string> AddAnswerOption(Question question)
        {
            List<string> answerOptions = null;

            Console.WriteLine($"Добавить к <{question.TextQuestion}> варианты ответов?");

            if (Console.ReadLine() == Сommands.y.ToString())
            {
                answerOptions = new List<string>();
                Console.WriteLine("Введите n когда добавили достаточно вариантов ответов.");
                answerOptions = new List<string>();
                for (; ; )
                {
                    string tmp = Console.ReadLine();
                    if (tmp == Сommands.n.ToString())
                    {
                        break;
                    }
                    else
                    {
                        answerOptions.Add(tmp);
                    }
                }
            }
            return answerOptions;
        }
        private void DeleteQuestion(Section section, string indexQuestion)
        {
            int i = Convert.ToInt32(indexQuestion);
            Console.WriteLine($"Вопрос удалён <{section.Questions[i].TextQuestion}>");
            section.Questions.RemoveAt(i);
        }
        private void WriteSectionToConsole(Section section)
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

    }
}
