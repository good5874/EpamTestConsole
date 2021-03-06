﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace EpamTestConsole
{
    public class Menu
    {
        private Management management;
        public CheckedQuestionsRepository repository;
        private ManualResetEvent _event;

        public Menu()
        {
            management = new Management();
            repository = new CheckedQuestionsRepository();
            Console.Title = "";
        }

        public void StartConsole()
        {
            Console.WriteLine(ConsoleMenuConstant.CreateTest);
            Console.WriteLine(ConsoleMenuConstant.StartTest);
            Console.WriteLine(ConsoleMenuConstant.EditTest);
            Console.WriteLine(ConsoleMenuConstant.ShowHistory);

            Console.WriteLine(ConsoleMenuConstant.EnterNamber);

            switch (Console.ReadLine())
            {
                case MainMenuConstant.CreateTest:
                    CreateTest();
                    TreeNode.WalkTheTree(management.RootTest, AddSection);
                    TreeNode.WalkTheTree(management.RootTest, AddQuestion);
                    Management.Save(management);
                    break;
                case MainMenuConstant.StartTest:
                    if (!Open())
                    {
                        break;
                    }
                    if (management.ConsoleTitileTimer != null)
                    {
                        _event = new ManualResetEvent(false);
                        management.ConsoleTitileTimer.StartTimer(management.RootTest, WriteSectionToConsole, _event);
                        _event.WaitOne();
                        TimeIsOver(management.RootTest);
                    }
                    else
                    {
                        TreeNode.WalkTheTree(management.RootTest, WriteSectionToConsole);
                    }
                    repository.SaveListManagment(management);
                    break;
                case MainMenuConstant.EditTest:
                    if (!Open())
                    {
                        break;
                    }
                    EditTest();
                    Management.Save(management);
                    break;
                case MainMenuConstant.ShowHistory:
                    WriteResultsHistoryToConsole();
                    break;
                default:
                    Console.WriteLine(ConsoleMenuConstant.Cancel);
                    break;
            }

            Console.WriteLine(ConsoleMenuConstant.Continue);
            Console.ReadKey();
            ReturnMainMenu();
        }

        private void CreateTest()
        {
            Console.WriteLine(ConsoleMenuConstant.EnterNameTest);
            string nameTest = Console.ReadLine();
            management.CreateNameTest(nameTest);

            VerifyOptions(management);
            AddTimer();
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

        private void WriteSectionToConsole(TreeNode test)
        {
            Console.WriteLine(ConsoleMenuConstant.Section + test.Section.NameSection);

            foreach (Question question in test.Section.Questions)
            {
                string userAnswer;
                Console.WriteLine(ConsoleMenuConstant.Question + question.TextQuestion);
                if (question.Options)
                {
                    int i = 1;
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

                if (question.Options)
                {
                    string temp = Console.ReadLine();
                    bool isNum = int.TryParse(temp, out int number);
                    if (question.AnswerOptions.Count + 1 > number && number > 0
                        && isNum)
                    {
                        userAnswer = number.ToString();
                    }
                    else
                    {
                        userAnswer = ConsoleMenuConstant.IncorrectInput;
                    }
                }
                else
                {
                    userAnswer = Console.ReadLine();
                }

                if (management.ConsoleTitileTimer != null &&
                    management.ConsoleTitileTimer.token.IsCancellationRequested)
                {
                    return;
                }
                question.UserAnswer = userAnswer;
                question.CheckingAnswer();

                if (management.CheckAfterInput)
                {
                    Console.WriteLine(question.ToString());
                }
            }
            Console.WriteLine(test.Section.ToString());
        }

        public void EditTest()
        {
            TreeNode.WalkTheTree(management.RootTest, WriteSectionName);

            Console.WriteLine(ConsoleMenuConstant.EnterNameTestEdit);
            string nameEdit = Console.ReadLine();
            var node = management.RootTest.Search(management.RootTest, nameEdit);

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
            Console.WriteLine(ConsoleMenuConstant.EditTimer);

            Console.WriteLine(ConsoleMenuConstant.EnterNamber);

            switch (Console.ReadLine())
            {
                case EditMetuConstant.EditNameTest:
                    Console.WriteLine(ConsoleMenuConstant.EnterNewName);
                    newNameSection = Console.ReadLine();
                    management.EditNameSection(nameEdit, newNameSection);
                    break;
                case EditMetuConstant.EditQuestion:
                    Console.WriteLine(ConsoleMenuConstant.EnterNumberQuestion);
                    numberQuestionEdit = Console.ReadLine();
                    if (!IsNotNumber(numberQuestionEdit, nameEdit))
                    {
                        EditQuestion(nameEdit, numberQuestionEdit);
                    }
                    break;
                case EditMetuConstant.AddQuestions:
                    AddQuestion(node);
                    break;
                case EditMetuConstant.DeleteQuestion:
                    Console.WriteLine(ConsoleMenuConstant.EnterNumberQuestion);
                    numberQuestionDelete = Console.ReadLine();
                    if (!IsNotNumber(numberQuestionDelete, nameEdit))
                    {
                        management.DeleteQuestion(nameEdit, numberQuestionDelete);
                    }
                    break;
                case EditMetuConstant.AddSection:
                    AddSection(node);
                    break;
                case EditMetuConstant.DeleteSection:
                    Console.WriteLine(ConsoleMenuConstant.Delete + nameEdit);
                    management.DeleteSection(nameEdit);
                    break;
                case EditMetuConstant.EditVerificationOptions:
                    VerifyOptions(management);
                    break;
                case EditMetuConstant.EditTimer:
                    AddTimer();
                    break;

                default:
                    Console.WriteLine(ConsoleMenuConstant.Cancel);
                    break;
            }
        }

        private void EditQuestion(string nameSection, string indexQuestion)
        {
            var node = management.RootTest.Search(management.RootTest, nameSection);

            Question question = node.Section.Questions[Convert.ToInt32(indexQuestion) - 1];

            string textQuestion = question.TextQuestion;
            string answer = question.Answer;
            List<string> answerOptions = question.AnswerOptions;
            bool checkAnswer = question.CheckAnswer;
            bool options = question.Options;

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

        private void AddQuestion(TreeNode test)
        {
            Console.WriteLine(ConsoleMenuConstant.AddQuestionTo + $"<{test.Section.NameSection}>?");
            Console.WriteLine(ConsoleMenuConstant.Y_N);
            if (Console.ReadLine() == ConsoleСommand.y.ToString())
            {
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

                    string textQuestion;
                    string answer = "";
                    List<string> answerOptions = null;
                    bool checkAnswer = false;
                    bool options = false;

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
                    management.CreateQuestion(test.Section.NameSection, question);
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
            Console.WriteLine(ConsoleMenuConstant.Y_N);
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

        private void AddSection(TreeNode test)
        {
            Console.WriteLine(ConsoleMenuConstant.AddSectionTo + $"<{test.Section.NameSection}>?");
            Console.WriteLine(ConsoleMenuConstant.Y_N);
            if (Console.ReadLine() == ConsoleСommand.y.ToString())
            {
                Console.WriteLine(ConsoleMenuConstant.HowManySectionsAddTo + $"<{test.Section.NameSection}>?");

                string temp = Console.ReadLine();
                bool isNum = int.TryParse(temp, out int sectionCount);
                if (isNum)
                {
                    Console.WriteLine(ConsoleMenuConstant.EnterNameSections);
                    for (int i = 1; i < sectionCount + 1; i++)
                    {
                        Console.WriteLine(ConsoleMenuConstant.Section + i);
                        string nameSection = Console.ReadLine();

                        management.CreateSection(test.Section.NameSection, new Section(nameSection));
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
                Console.WriteLine(ConsoleMenuConstant.Test + management.RootTest.Section.NameSection);
                TreeNode.WalkTheTree(management.RootTest, WriteResult);
            }
        }

        private void WriteSectionName(TreeNode test)
        {
            Console.WriteLine($"{ConsoleMenuConstant.Section }{test.Section.NameSection} ");
        }

        private void WriteResult(TreeNode test)
        {
            foreach (Question question in test.Section.Questions)
            {
                Console.WriteLine(question.ToString());
            }
            Console.WriteLine(test.Section.ToString());
        }

        private void WriteResultToConsole(string nameTest)
        {
            foreach (Management management in repository.GetListManagements(nameTest))
            {
                Console.WriteLine(ConsoleMenuConstant.Test + management.RootTest.Section.NameSection);
                TreeNode.WalkTheTree(management.RootTest, WriteResult);
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
                    //ReturnMainMenu();
                    break;
            }
        }

        private void VerifyOptions(Management management)
        {
            Console.WriteLine(ConsoleMenuConstant.CheckAnswerAfterInput);
            Console.WriteLine(ConsoleMenuConstant.Y_N);
            if (Console.ReadLine() == ConsoleСommand.y.ToString())
            {
                management.CheckAfterInput = true;
            }
            else
            {
                management.CheckAfterInput = false;
            }
        }

        private void AddTimer()
        {
            Console.WriteLine(ConsoleMenuConstant.AddTime);
            Console.WriteLine(ConsoleMenuConstant.Y_N);
            if (Console.ReadLine() == ConsoleСommand.y.ToString())
            {
                Console.WriteLine(ConsoleMenuConstant.EnterTimeInSeconds);

                string seconds = Console.ReadLine();

                bool isNum = int.TryParse(seconds, out int number);
                if (number >= 0 && isNum)
                {
                    management.ConsoleTitileTimer = new ConsoleTitileTimer();
                    management.ConsoleTitileTimer.TimeSeconds = number;
                }
                else
                {
                    Console.WriteLine(ConsoleMenuConstant.NotNamber);
                    Console.WriteLine(ConsoleMenuConstant.Cancel);
                    management.ConsoleTitileTimer = null;
                }
            }
            else
            {
                Console.WriteLine(ConsoleMenuConstant.DeleteTimer);
                management.ConsoleTitileTimer = null;
            }
        }

        private void TimeIsOver(TreeNode test)
        {
            TreeNode.WalkTheTree(test, AddDefaultAnswer);
        }

        private void AddDefaultAnswer(TreeNode test)
        {
            foreach (Question question in test.Section.Questions)
            {
                if (question.UserAnswer == null)
                {
                    question.Result = ConsoleMenuConstant.TimeIsOver;
                    question.UserAnswer = ConsoleMenuConstant.TimeIsOver;
                }
            }
        }

        private void ReturnMainMenu()
        {
            Console.Clear();
            Console.Title = "";
            management = new Management();
            _event = null;
            StartConsole();
        }

        private bool IsNotNumber(string str, string nameEdit)
        {
            Section section = management.RootTest.Search(management.RootTest, nameEdit).Section;
            bool isNum = int.TryParse(str, out int number);
            if (section.Questions.Count + 1 > number && number > 0
                && isNum)
            {
                return false;
            }
            else
            {
                Console.WriteLine(ConsoleMenuConstant.IncorrectInput);
                return true;
            }

        }

    }
}
