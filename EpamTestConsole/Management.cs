using System;
using System.Linq;

namespace EpamTestConsole
{
    [Serializable]
    public class Management
    {
        private TreeNode rootTest = new TreeNode(new Section("root"));
        public TreeNode RootTest
        {
            get
            {
                return rootTest;
            }
            set
            {
                rootTest = value;
            }
        }

        public void CreateNameTest(string nameTest)
        {
            RootTest.Section.NameSection = nameTest;
        }

        public void EditNameSection(string nameSection, string newNameSection)
        {
            var test = RootTest.Search(RootTest, nameSection);
            test.Section.NameSection = newNameSection;
        }

        public void CreateQuestion(string nameSection, Question question)
        {
            var test = RootTest.Search(RootTest, nameSection);
            test.Section.Questions.Add(question);
        }

        public void EditQuestion(string nameSection, Question question, string indexQuestion)
        {
            var test = RootTest.Search(RootTest, nameSection);
            int i = int.Parse(indexQuestion);
            test.Section.Questions[i] = question;
        }

        public void CreateSection(string  name, Section section)
        {           
            var test = RootTest.Search(RootTest, name);
            test.AddChildNode(new TreeNode(section));
        }

        public void DeleteQuestion(string nameSection, string indexQuestion)
        {
            var test = RootTest.Search(RootTest, nameSection);
            int i = Convert.ToInt32(indexQuestion);
            test.Section.Questions.RemoveAt(i);
        }

        public void DeleteSection(string nameSection)
        {
            rootTest.Delete(ref rootTest, nameSection);
        }

        public static void Save(Management management)
        {
            if (management == null)
            {
                return;
            }
            Console.WriteLine(ConsoleMenuConstant.Save + "?");
            if (Console.ReadLine() == ConsoleСommand.y.ToString())
            {
                Console.WriteLine(ConsoleMenuConstant.EnterFileName);
                string nameFile = Console.ReadLine();
                TestRepository.SaveTest(management, nameFile);
                Console.WriteLine(ConsoleMenuConstant.Saved);
            }
            else
            {
                Console.WriteLine(ConsoleMenuConstant.Cancel);
            }
        }
    }
}
