using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpamTestConsole
{
    [Serializable]
    public class Section
    {
        private string nameSection;
        private List<Question> questions=new List<Question>();        
        private Section parent;

        public Section(string _nameSection, Section _parent=null)
        {
            NameSection = _nameSection;
        }
        public string NameSection
        {
            get { return nameSection; }
            set { nameSection = value; }
        }
        public List<Question> Questions
        {
            get { return questions; }
            set { questions = value; }
        }       
        public Section Parent
        {
            get { return parent; }
            set { parent = value; }
        }


        public void CreateQuestion(string _question, bool _checkAnswer, bool _options,
            string _answer = null, List<string> _answerOptions = null)
        {
            Question question = new Question(_question, _checkAnswer, _options, _answer, _answerOptions);           
            if (question != null) Questions.Add(question);
        } 

    }
}
