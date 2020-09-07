using System;
using System.Collections.Generic;
using System.Linq;

namespace EpamTestConsole
{
    public class Menu
    {
        Management management = new Management();
        delegate void Metod(TreeNode root);

        public void StartConsole()
        {            
            Console.WriteLine("1. " + ConsoleMenuConstant.CreateTest);
            Console.WriteLine("2. " + ConsoleMenuConstant.StartTest);
            Console.WriteLine("3. " + ConsoleMenuConstant.EditTest);

            Console.WriteLine("Введите нужную цифру:");
            switch (Console.ReadLine())
            {
                case "1":
                    CreateTest();
                    WalkTheTree(management.RootNode, AddSection);
                    WalkTheTree(management.RootNode, AddQuestion);
                    Save(management);
                    break;
                case "2":
                    OpenAndRunMethod(true);//start test
                    break;
                case "3":
                    OpenAndRunMethod(false);//edit
                    Save(management);
                    break;
            }
        }

        private void CreateTest()
        {
            Console.WriteLine("Введите тему теста");
            string nameTest = Console.ReadLine();
            management.CreateTest(nameTest);
        }
        public void EditTest()
        {
            Console.WriteLine("Введите тему или подтему, которую нужно редактировать:");
            string nameEdit = Console.ReadLine();
            var node = management.RootNode.Search(management.RootNode, nameEdit);


            string newNameSection;
            string numberQuestionEdit;
            string numberQuestionDelete;
            string numberSectionDelete;

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
                    newNameSection = Console.ReadLine();
                    management.newEditTest(nameEdit, newNameSection);
                    break;
                case "2":
                    Console.WriteLine("Введите номер вопроса:");
                    numberQuestionEdit = Console.ReadLine();
                    EditQuestion(nameEdit, numberQuestionEdit);                    
                    break;
                case "3":
                    AddQuestion(node);
                    break;
                case "4":
                    Console.WriteLine("Введите номер вопроса:");
                    numberQuestionDelete = Console.ReadLine();
                    management.newEditTest(nameEdit, null, null, numberQuestionDelete);
                    break;
                case "5":
                    AddSection(node);
                    break;
                case "6":
                    Console.WriteLine("Введите название темы:");
                    numberSectionDelete = Console.ReadLine();//удалит дочерние подтемы
                    management.newEditTest(nameEdit, null, null, null, numberSectionDelete);
                    break;
                default:
                    Console.WriteLine("Редактирование отменено");
                    break;
            }
        }

        private void EditQuestion(string nameSection, string indexQuestion)
        {
            var node = management.RootNode.Search(management.RootNode, nameSection);

            Question question = node.Section.Questions[Convert.ToInt32(indexQuestion)];

            string textQuestion = "";
            string answer = "";
            string answerOptions = "";
            bool checkAnswer = false;
            bool options = false;

            Console.WriteLine("1. Изменить текст вопроса");
            Console.WriteLine("2. Изменить ответ на вопрос");
            Console.WriteLine("3. Добавить варианты ответов на вопросы");
            Console.WriteLine("4. Удалить варианты ответов на вопросы");
            Console.WriteLine("Введите нужную цифру:");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine($"Введите новый вопрос:");
                    textQuestion = Console.ReadLine();
                    break;
                case "2":
                    Console.WriteLine($"Введите ответ:");
                    answer = Console.ReadLine();
                    break;
                case "3":
                    answerOptions = AddAnswerOtions(
                        node.Section.Questions[Convert.ToInt32(indexQuestion)].TextQuestion
                        );
                    break;
                case "4":
                    answerOptions = "";
                    break;

                default:
                    Console.WriteLine("Редактирование отменено");
                    break;
            }
            

            if (answer != "")
                checkAnswer = true;
            
            if (answerOptions != "")
                options = true;

            string _question = question + "\n" + checkAnswer + "\n" + options + "\n" + answer + "\n" + answerOptions;
            management.newEditTest(nameSection, null, indexQuestion, null, null, _question);

        }

        private void OpenAndRunMethod(bool StartOrEdit)
        {
            Console.WriteLine(ConsoleMenuConstant.EnterFileName);
            string nameFile = Console.ReadLine();
            management = FormRepository.OpenTest(nameFile);

            if (management == null)
            {
                Console.WriteLine(ConsoleMenuConstant.FileNotFound);
            }
            else
            {
                if (StartOrEdit)
                {
                    management.StartTest();
                }
                else
                {
                    EditTest();
                }
            }
        }              

        private void WalkTheTree(TreeNode root, Metod metod)
        {            
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);

            while (queue.Count != 0)
            {
                TreeNode temp = queue.Dequeue();
                metod(temp);

                if (temp.ChildNodes == null) continue;
                foreach (var node in temp.ChildNodes)
                {
                    queue.Enqueue(node);
                }
            }            
        }

        private void AddQuestion(TreeNode node)
        {
            string _question = "";

            string question;
            string answer = "";            
            string answerOptions = "";
            bool checkAnswer = false;
            bool options = false;

            Console.WriteLine($"Добавить вопросы к {node.Section.NameSection}?");
            if (Console.ReadLine() == Сommands.y.ToString())
            {
                for (int i = 0; ; i++)
                {
                    if (i > 0)
                    {
                        Console.WriteLine("Чтобы закончить добавление вопросов введите <y>");
                        if (Console.ReadLine() == Сommands.y.ToString())
                        {
                            return;
                        }
                    }

                    Console.WriteLine("Введите вопрос:");
                    question = Console.ReadLine();
                    Console.WriteLine("Введите ответ:");
                    answer = Console.ReadLine();                    
                                        
                    answerOptions = AddAnswerOtions(question);

                    checkAnswer = false;
                    if (answer != "")
                        checkAnswer = true;

                    options = false;
                    if (answerOptions != "")
                        options = true;
                  
                    _question += question + "\n" + checkAnswer + "\n" + options + "\n" + answer + "\n" + answerOptions;
                    management.CreateQuestion(node.Section.NameSection, _question);
                }
            }
            else
            {
                Console.WriteLine($"Добавление вопросов к {node.Section.NameSection} отменено");
            }
        }

        private string AddAnswerOtions(string nameQuestion)
        {
            string answerOptions = "";

            Console.WriteLine($"Добавить к <{nameQuestion}> варианты ответов?");
            if (Console.ReadLine() == Сommands.y.ToString())
            {
                Console.WriteLine("Введите <n> когда добавили достаточно вариантов ответов.");

                for (; ; )
                {
                    string tmp = Console.ReadLine();
                    if (tmp == Сommands.n.ToString())
                    {
                        break;
                    }
                    else
                    {
                        answerOptions += tmp + "/";
                    }
                }
                answerOptions = answerOptions.Trim('/');
            }
            return answerOptions;
        }

        private void AddSection(TreeNode node)
        {
            string text = "";
            Console.WriteLine($"Добавить потемы к {node.Section.NameSection} ?");
            if (Console.ReadLine() == Сommands.y.ToString())
            {
                Console.WriteLine($"Сколько подтем добавить к {node.Section.NameSection} подтем?");
                int sectionCount = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите название подтем:");
                for (int i = 1; i < sectionCount + 1; i++)
                {
                    Console.WriteLine("Подтема №" + i);
                    string nameSection = Console.ReadLine();
                    text = node.Section.NameSection + "/" + nameSection;
                    management.CreateSection(text);
                }
            }
            else
            {
                Console.WriteLine($"Добавление подтем к {node.Section.NameSection} отменено");
            }           
        }

        private static void Save(Management management)
        {
            Console.WriteLine("Сохранить?");
            if (Console.ReadLine() == Сommands.y.ToString())
            {
                Console.WriteLine(ConsoleMenuConstant.EnterFileName);
                string nameFile = Console.ReadLine();
                FormRepository.SaveTest(management, nameFile);
                Console.WriteLine("Сохранено");
            }
            else
            {
                Console.WriteLine("Сохранение отменено");
            }
        }
    }
}
