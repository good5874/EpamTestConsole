using System;
using System.Collections.Generic;
using System.Linq;

namespace EpamTestConsole
{
    [Serializable]
    public class Management
    {
        private TreeNode rootNode = new TreeNode(new Section("root"));
        public TreeNode RootNode
        {
            get
            {
                return rootNode;
            }
            set
            {
                rootNode = value;
            }
        } 

        public void CreateNameTest(string nameTest)
        {
            RootNode.Section.NameSection = nameTest;                       
        }   

        public void EditNameSection(string nameSection, string newNameSection)
        {
            var node = RootNode.Search(RootNode, nameSection);
            node.Section.NameSection = newNameSection;
        }


        public void CreateQuestion(string nameSection, string question)
        {
            var _question = question.Split('\n');
            var node = RootNode.Search(RootNode, nameSection);            

            bool checkAnswer = false;
            bool options = false;
            
            if(_question[1]=="True")
            {
                checkAnswer = true;
            }
            if (_question[2] == "True")
            {
                options = true;
            }
            var answerOption = _question[4].Split('/').ToList();

            node.Section.Questions.Add(new Question(_question[0], checkAnswer, options, _question[3], answerOption));

        }//new

        public void EditQuestion(string nameSection, string question)
        {
            var node = RootNode.Search(RootNode, nameSection);

            var _question = question.Split('\n');            

            int numberEditquestion = Convert.ToInt32(_question[0]);

            bool checkAnswer = false;
            bool options = false;

            if (_question[2] == "True")
            {
                checkAnswer = true;
            }
            if (_question[3] == "True")
            {
                options = true;
            }
            if (string.IsNullOrEmpty(_question[5]))
            {
                node.Section.Questions[numberEditquestion] = new Question(_question[1], checkAnswer, options, _question[4], null);
            }
            var answerOption = _question[5].Split('/').ToList();

            node.Section.Questions[numberEditquestion] = new Question(_question[1], checkAnswer, options, _question[4], answerOption);

        }//new

        public void CreateSection(string _sections)
        {
            var sections = _sections.Split("/");
            var node = RootNode.Search(RootNode, sections[0]);
            node.AddChildNode(new TreeNode(new Section(sections[1])));
        }//new 

        public void DeleteQuestion(string nameSection, string indexQuestion)
        {
            var node = RootNode.Search(RootNode, nameSection);
            int i = Convert.ToInt32(indexQuestion);
            node.Section.Questions.RemoveAt(i);
        }
        public void DeleteSection(string nameSection)
        {
            rootNode.Delete(ref rootNode, nameSection);
        }

    }
}
