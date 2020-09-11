using System;
using System.Collections.Generic;

namespace EpamTestConsole
{
    public class Menu
    {
        private Management management = new Management();
        private readonly CheckedQuestions checkingQuestions = new CheckedQuestions();
        private readonly CheckedQuestionsRepository repository = new CheckedQuestionsRepository();

        delegate void Metod(TreeNode root);

        public void StartConsole()
        {            
            Console.WriteLine("1. " + ConsoleMenuConstant.CreateTest);
            Console.WriteLine("2. " + ConsoleMenuConstant.StartTest);
            Console.WriteLine("3. " + ConsoleMenuConstant.EditTest);
            Console.WriteLine("4. " + ConsoleMenuConstant.ShowHistory);

            Console.WriteLine(ConsoleMenuConstant.EnterNamber);
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
                    
                    repository.SaveCheckedQuestions(checkingQuestions.VerifiedQuestions);
                    break;
                case "3":
                    OpenAndRunMethod(false);//edit test
                    Save(management);
                    break;
                case "4":
                    WriteResultsHistoryToConsole();
                    break;
                default:
                    Console.WriteLine(ConsoleMenuConstant.Cancele);
                    break;
            }
        }

        private void CreateTest()
        {
            Console.WriteLine("Введите тему теста");
            string nameTest = Console.ReadLine();
            management.CreateNameTest(nameTest);
        }

        private void OpenAndRunMethod(bool StartOrEdit)
        {
            Console.WriteLine(ConsoleMenuConstant.EnterFileName);
            string nameFile = Console.ReadLine();
            management = TestRepository.OpenTest(nameFile);

            if (management == null)
            {
                Console.WriteLine(ConsoleMenuConstant.FileNotFound);
            }
            else
            {
                if (StartOrEdit)
                {
                    WalkTheTree(management.RootNode, WriteSectionToConsole);
                }
                else
                {
                    EditTest();
                }
            }
        }

        private void WriteSectionToConsole(TreeNode node)
        {
            List<string> answers = new List<string>();

            Console.WriteLine(node.Section.NameSection);

            foreach (Question question in node.Section.Questions)
            {
                string answer;
                Console.WriteLine("Вопрос: " + question.TextQuestion);
                if (question.Options)
                {
                    int i = 0;
                    Console.WriteLine("Варианты ответа:");
                    foreach (var str in question.AnswerOptions)
                    {
                        Console.WriteLine($" {i})" + str);
                        i++;
                    }
                    Console.WriteLine(ConsoleMenuConstant.EnterNamber);
                }
                else
                {
                    Console.WriteLine("Введите ответ:");
                }
                answer = Console.ReadLine();
                answers.Add(answer);
            }

            checkingQuestions.CheckingAllQuestions(node.Section, answers);

        }

        public void EditTest()
        {
            Console.WriteLine("Введите тему или подтему, которую нужно редактировать:");
            string nameEdit = Console.ReadLine();
            var node = management.RootNode.Search(management.RootNode, nameEdit);

            if (node == null)
            {
                Console.WriteLine("Тема не найдена");
                return;
            }

            string newNameSection;
            string numberQuestionEdit;
            string numberQuestionDelete;

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
                    management.EditNameSection(nameEdit, newNameSection);
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
                    management.DeleteQuestion(nameEdit, numberQuestionDelete);
                    break;
                case "5":
                    AddSection(node);
                    break;
                case "6":
                    Console.WriteLine("Удаление " + nameEdit);
                    //удалит дочерние подтемы
                    management.DeleteSection(nameEdit);
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

            string textQuestion = question.TextQuestion;
            string answer = question.Answer;
            string answerOptions = "";
            bool checkAnswer = false;
            bool options = false;

            Console.WriteLine("1. Изменить текст вопроса");
            Console.WriteLine("2. Изменить ответ на вопрос");
            Console.WriteLine("3. Добавить варианты ответов на вопросы");
            Console.WriteLine("4. Удалить варианты ответов на вопросы");
            Console.WriteLine(ConsoleMenuConstant.EnterNamber);
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
                    answerOptions = AddAnswerOtions(textQuestion);
                    break;
                case "4":
                    answerOptions = "";
                    break;

                default:
                    Console.WriteLine("Редактирование отменено");
                    return;                    
            }

            if (answer != "")
                checkAnswer = true;

            if (answerOptions != "")
                options = true;

            string _question = indexQuestion + "\n" + textQuestion + "\n" + checkAnswer + "\n" + options + "\n" + answer + "\n" + answerOptions;// доделать
            management.EditQuestion(nameSection, _question);
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
                        Console.WriteLine(ConsoleMenuConstant.EnterY);
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
                Console.WriteLine(ConsoleMenuConstant.EnterN);

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

                string strsectionCount = Console.ReadLine();
                bool isNum = int.TryParse(strsectionCount, out  int intsectionCount);
                if (isNum)
                {
                    Console.WriteLine("Введите название подтем:");
                    for (int i = 1; i < intsectionCount + 1; i++)
                    {
                        Console.WriteLine("Подтема №" + i);
                        string nameSection = Console.ReadLine();
                        text = node.Section.NameSection + "/" + nameSection;
                        management.CreateSection(text);
                    }
                }
                else
                {
                    Console.WriteLine("Ввели не число");
                    Console.WriteLine($"Добавление подтем к {node.Section.NameSection} отменено");
                }
            }
            else
            {
                Console.WriteLine($"Добавление подтем к {node.Section.NameSection} отменено");
            }
        }

        private void WriteResultToConsole()
        {
            foreach (List<CheckedQuestion> list in repository.GetChekedQuestions())
            {
                Console.WriteLine("Название теста: " + list[0].NameSection);
                foreach (CheckedQuestion item in list)
                {
                    Console.WriteLine(item.NameSection + "/" + item.TextQuestion + "/" + item.Answer + "/" + item.Result);
                }
            }
        }

        private void WriteResultToConsole(string nameTest)
        {
            foreach (List<CheckedQuestion> list in repository.GetChekedQuestions(nameTest))
            {
                Console.WriteLine("Название теста: " + list[0].NameSection);
                foreach (CheckedQuestion item in list)
                {
                    Console.WriteLine(item.NameSection + "/" + item.TextQuestion + "/" + item.Answer + "/" + item.Result);
                }
            }
        }

        private void WriteResultsHistoryToConsole()
        {
            Console.WriteLine("1. " + "Получить историю всех пройденых тестов");
            Console.WriteLine("2. " + "Получить историю определенного теста");

            Console.WriteLine(ConsoleMenuConstant.EnterNamber);
            switch (Console.ReadLine())
            {
                case "1":
                    WriteResultToConsole();
                    break;
                case "2":
                    Console.WriteLine(ConsoleMenuConstant.EnterFileName);
                    WriteResultToConsole(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine(ConsoleMenuConstant.Cancele);
                    break;
            }
        }

        private static void Save(Management management)
        {
            if (management == null)
            {
                return;
            }
            Console.WriteLine("Сохранить?");
            if (Console.ReadLine() == Сommands.y.ToString())
            {
                Console.WriteLine(ConsoleMenuConstant.EnterFileName);
                string nameFile = Console.ReadLine();
                TestRepository.SaveTest(management, nameFile);
                Console.WriteLine("Сохранено");
            }
            else
            {
                Console.WriteLine("Сохранение отменено");
            }
        }
    }
}
