using System;
using System.Collections.Generic;

namespace EpamTestConsole
{
    public class Menu
    {
        private Management management = new Management();        
        private readonly CheckedQuestionsRepository repository = new CheckedQuestionsRepository();

        public delegate void Metod(TreeNode root);
        
        public void StartConsole()
        {            
            Console.WriteLine(ConsoleMenuConstant.CreateTest);
            Console.WriteLine(ConsoleMenuConstant.StartTest);
            Console.WriteLine(ConsoleMenuConstant.EditTest);
            Console.WriteLine(ConsoleMenuConstant.ShowHistory);
            
            Console.WriteLine(ConsoleMenuConstant.EnterNamber);
            switch (Console.ReadLine())
            {
                case "1":
                    CreateTest();
                    management.RootTest.WalkTheTree(management.RootTest , AddSection);
                    management.RootTest.WalkTheTree(management.RootTest , AddQuestion);
                    Management.Save(management);
                    break;
                case "2":                    
                    if(!Open())
                    {
                        return;
                    }

                    management.ConsoleTitileTimer.StartTimer();

                    management.RootTest.WalkTheTree(management.RootTest, WriteSectionToConsole);
                    repository.SaveListManagment(management);
                    break;
                case "3":                    
                    if (!Open())
                    {
                        return;
                    }

                    EditTest();
                    Management.Save(management);
                    break;
                case "4":
                    WriteResultsHistoryToConsole();
                    break;
                default:
                    Console.WriteLine(ConsoleMenuConstant.Cancel);
                    break;
            }
        }

        private void CreateTest()
        {
            Console.WriteLine(ConsoleMenuConstant.EnterNameTest);
            string nameTest = Console.ReadLine();
            management.CreateNameTest(nameTest);

            VerificationOptions(management);
        }

        private bool Open()
        {
            Console.WriteLine(ConsoleMenuConstant.EnterFileName);
            string nameFile = Console.ReadLine();
            management = TestRepository.OpenTest(nameFile);

            if (management == null)
            {
                Console.WriteLine(ConsoleMenuConstant.FileNotFound);
                return false;
            }
            return true;
        }

        private void WriteSectionToConsole(TreeNode node)
        {  
            Console.WriteLine(node.Section.NameSection);

            foreach (Question question in node.Section.Questions)
            {
                string userAnswer;
                Console.WriteLine(ConsoleMenuConstant.Question + question.TextQuestion);
                if (question.Options)
                {
                    int i = 0;
                    Console.WriteLine(ConsoleMenuConstant.AnswerOptions);
                    foreach (var str in question.AnswerOptions)
                    {
                        Console.WriteLine($" {i})" + str);
                        i++;
                    }
                    Console.WriteLine(ConsoleMenuConstant.EnterNamber);
                }
                else
                {
                    Console.WriteLine(ConsoleMenuConstant.EnterAnswer);
                }
                

                if (management.ConsoleTitileTimer.Stop)
                {
                    question.UserAnswer = "Время вышло";
                    question.Result = "Время вышло";
                    continue;
                }
                else
                {
                    userAnswer = Console.ReadLine();
                }

                question.UserAnswer = userAnswer;
                question.CheckingAnswer();

                if (management.CheckAfterInput)
                {
                    Console.WriteLine(question.ToString());
                }

            }            
        }
        private void Ex()
        {
            throw new Exception("Время вышло");
        }

        public void EditTest()
        {     
            Console.WriteLine(ConsoleMenuConstant.EnterNameTestEdit);
            string nameEdit = Console.ReadLine();
            var node = management.RootTest .Search(management.RootTest , nameEdit);

            if (node == null)
            {
                Console.WriteLine(ConsoleMenuConstant.TestNotFound);
                return;
            }

            string newNameSection;
            string numberQuestionEdit;
            string numberQuestionDelete;

            Console.WriteLine(ConsoleMenuConstant.EditNameTest);
            Console.WriteLine(ConsoleMenuConstant.EditQuestion);
            Console.WriteLine(ConsoleMenuConstant.AddQuestions);
            Console.WriteLine(ConsoleMenuConstant.DeleteQuestion);
            Console.WriteLine(ConsoleMenuConstant.AddSection);
            Console.WriteLine(ConsoleMenuConstant.DeleteSection);
            Console.WriteLine(ConsoleMenuConstant.EditVerificationOptions);

            Console.WriteLine(ConsoleMenuConstant.EnterNamber);
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine(ConsoleMenuConstant.EnterNewName);
                    newNameSection = Console.ReadLine();
                    management.EditNameSection(nameEdit, newNameSection);
                    break;
                case "2":
                    Console.WriteLine(ConsoleMenuConstant.EnterNumberQuestion);
                    numberQuestionEdit = Console.ReadLine();
                    EditQuestion(nameEdit, numberQuestionEdit);
                    break;
                case "3":
                    AddQuestion(node);
                    break;
                case "4":
                    Console.WriteLine(ConsoleMenuConstant.EnterNumberQuestion);
                    numberQuestionDelete = Console.ReadLine();
                    management.DeleteQuestion(nameEdit, numberQuestionDelete);
                    break;
                case "5":
                    AddSection(node);
                    break;
                case "6":
                    Console.WriteLine(ConsoleMenuConstant.Delete + nameEdit);
                    //удалит дочерние подтемы
                    management.DeleteSection(nameEdit);
                    break;
                case "7":
                    VerificationOptions(management);
                    break;

                default:
                    Console.WriteLine(ConsoleMenuConstant.Cancel);
                    break;
            }
        }

        private void EditQuestion(string nameSection, string indexQuestion)
        {
            var node = management.RootTest.Search(management.RootTest, nameSection);

            Question question = node.Section.Questions[Convert.ToInt32(indexQuestion)];

            string textQuestion= question.TextQuestion;
            string answer = "";
            List<string> answerOptions = null;
            bool checkAnswer = false;
            bool options = false;

            Console.WriteLine(ConsoleMenuConstant.EditNameQuestion);
            Console.WriteLine(ConsoleMenuConstant.EditAnswerQuestion);
            Console.WriteLine(ConsoleMenuConstant.AddAnswerOption);
            Console.WriteLine(ConsoleMenuConstant.DeleteAnswerOption);

            Console.WriteLine(ConsoleMenuConstant.EnterNamber);
            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine(ConsoleMenuConstant.EnterNewTextQuestion);
                    textQuestion = Console.ReadLine();
                    break;
                case "2":
                    Console.WriteLine(ConsoleMenuConstant.EnterNewAnswerQuestion);
                    answer = Console.ReadLine();
                    break;
                case "3":
                    answerOptions = AddAnswerOptions(textQuestion);
                    break;
                case "4":
                    answerOptions = null;
                    break;
                default:
                    Console.WriteLine(ConsoleMenuConstant.Cancel);
                    return;
            }

            if (answer != "")
            {
                checkAnswer = true;
            }

            if (answerOptions != null)
            {
                options = true;
            }

            question = new Question(textQuestion, checkAnswer, options, answer, answerOptions);            
            management.EditQuestion(nameSection, question, indexQuestion);
        }        

        private void AddQuestion(TreeNode node)
        {
            Console.WriteLine(ConsoleMenuConstant.AddQuestionTo + $"<{node.Section.NameSection}>?");
            if (Console.ReadLine() == ConsoleСommand.y.ToString())
            {    
                string textQuestion;
                string answer = "";
                List<string> answerOptions = null;
                bool checkAnswer = false;
                bool options = false;

                for (int i = 0; ; i++)
                {
                    if (i > 0)
                    {
                        Console.WriteLine(ConsoleMenuConstant.EnterY);
                        if (Console.ReadLine() == ConsoleСommand.y.ToString())
                        {
                            return;
                        }
                    }

                    Console.WriteLine(ConsoleMenuConstant.EnterQuestion);
                    textQuestion = Console.ReadLine();
                    Console.WriteLine(ConsoleMenuConstant.EnterAnswer);
                    answer = Console.ReadLine();

                    answerOptions = AddAnswerOptions(textQuestion);

                    if (answer != "")
                    {
                        checkAnswer = true;
                    }

                    if (answerOptions != null)
                    {
                        options = true;
                    }

                    var question = new Question(textQuestion, checkAnswer, options, answer, answerOptions);
                    management.CreateQuestion(node.Section.NameSection, question);
                }
            }
            else
            {
                Console.WriteLine(ConsoleMenuConstant.Cancel);
            }
        }

        private List<string> AddAnswerOptions(string nameQuestion)
        {
            List<string> answerOptions = null;

            Console.WriteLine(ConsoleMenuConstant.AddAnswerOptionTo + $"<{nameQuestion}>?");
            if (Console.ReadLine() == ConsoleСommand.y.ToString())
            {
                Console.WriteLine(ConsoleMenuConstant.EnterN);

                answerOptions = new List<string>();

                for (; ; )
                {
                    string tmp = Console.ReadLine();
                    if (tmp == ConsoleСommand.n.ToString())
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

        private void AddSection(TreeNode node)
        {            
            Console.WriteLine(ConsoleMenuConstant.AddSectionTo + $"<{node.Section.NameSection}>?");
            if (Console.ReadLine() == ConsoleСommand.y.ToString())
            {
                Console.WriteLine(ConsoleMenuConstant.HowManySectionsToAdd + $"<{node.Section.NameSection}>?");

                string strsectionCount = Console.ReadLine();
                bool isNum = int.TryParse(strsectionCount, out  int intsectionCount);
                if (isNum)
                {
                    Console.WriteLine(ConsoleMenuConstant.EnterNameSections);
                    for (int i = 1; i < intsectionCount + 1; i++)
                    {
                        Console.WriteLine(ConsoleMenuConstant.Sections + i);
                        string nameSection = Console.ReadLine();                       
                        management.CreateSection(node.Section.NameSection, new Section(nameSection));
                    }
                }
                else
                {
                    Console.WriteLine(ConsoleMenuConstant.NotNamber);
                    Console.WriteLine(ConsoleMenuConstant.Cancel);
                }
            }
            else
            {
                Console.WriteLine(ConsoleMenuConstant.Cancel);
            }
        }

        private void WriteResultToConsole()
        {
            foreach (Management management in repository.GetListManagements())
            {
                Console.WriteLine(ConsoleMenuConstant.Sections + management.RootTest.Section.NameSection);
                management.RootTest.WalkTheTree(management.RootTest, WriteResult);               
            }
        }

        private void WriteResult(TreeNode test)
        {
            foreach (Question question in test.Section.Questions)
            {
                Console.WriteLine(question.ToString());
            }
        }

        private void WriteResultToConsole(string nameTest)
        {
            foreach (Management management in repository.GetListManagements(nameTest))
            {
                Console.WriteLine(ConsoleMenuConstant.Sections + management.RootTest.Section.NameSection);
                management.RootTest.WalkTheTree(management.RootTest, WriteResult);
            }
        }

        private void WriteResultsHistoryToConsole()
        {
            Console.WriteLine(ConsoleMenuConstant.GetHistory);
            Console.WriteLine(ConsoleMenuConstant.GetTestHistory);

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
                    Console.WriteLine(ConsoleMenuConstant.Cancel);
                    break;
            }
        }      

        private void VerificationOptions(Management management)
        {
            Console.WriteLine(ConsoleMenuConstant.CheckAnswerAfterInput);
            if (Console.ReadLine() == ConsoleСommand.y.ToString())
            {
                management.CheckAfterInput = true;
            }
            else
            {
                management.CheckAfterInput = false;
            }
        }
    }
}
