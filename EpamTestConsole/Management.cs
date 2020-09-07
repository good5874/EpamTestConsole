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
        
        public void newEditTest(string nameEditSection, string newNameSection = null, string numberQuestionEdit = null,
            string numberQuestionDelete = null, string numberSectionDelete = null,
            string question = null)
        {
            var node = RootNode.Search(RootNode, nameEditSection);

            if (string.IsNullOrEmpty(newNameSection))
            {                
                node.Section.NameSection = newNameSection;
            }
            else if(string.IsNullOrEmpty(numberQuestionEdit)
                && string.IsNullOrEmpty(question))
            {
                int i = Convert.ToInt32(numberQuestionEdit);
                                
                DeleteQuestion(node.Section, numberQuestionEdit); 
                CreateQuestion(nameEditSection, question);                
            }
            else if (string.IsNullOrEmpty(numberQuestionDelete))
            {
                int i = Convert.ToInt32(numberQuestionDelete);
                node.Section.Questions.RemoveAt(i);
            }
            else if (string.IsNullOrEmpty(numberSectionDelete))
            {
                rootNode.Delete(ref rootNode, nameEditSection);
            }
        }//new  
        
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

        public void CreateSection(string _sections)
        {
            var sections = _sections.Split("/");
            var node = RootNode.Search(RootNode, sections[0]);
            node.AddChildNode(new TreeNode(new Section(sections[1])));

        }//new 

        private void DeleteQuestion(Section section, string indexQuestion)
        {
            int i = Convert.ToInt32(indexQuestion);
            //Console.WriteLine($"Вопрос удалён <{section.Questions[i].TextQuestion}>");
            section.Questions.RemoveAt(i);
        }     

    }
}
