using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace EpamTestConsole
{
    [Serializable]
    public class CheckedQuestionsRepository
    {
        private List<List<CheckedQuestion>> ListCheckedQuestions;

        private string nameFile = "CheckedQuestionsRepository";

        public CheckedQuestionsRepository()
        {
            if (!File.Exists(nameFile + ".dat"))
            {
                ListCheckedQuestions = new List<List<CheckedQuestion>>();
            }
            else
            {
                ListCheckedQuestions = OpenCheckedQuestions();                
            }
        }

        public void SaveCheckedQuestions(List<CheckedQuestion> checkedQuestions)
        {
            if (checkedQuestions.Count != 0)
            {
                ListCheckedQuestions.Add(checkedQuestions);
            }
            else
            {
                return;
            }
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream($"{nameFile}.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, ListCheckedQuestions);
            }
        }

        public List<List<CheckedQuestion>> OpenCheckedQuestions()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream($"{nameFile}.dat", FileMode.OpenOrCreate))
            {
                List<List<CheckedQuestion>> deserilizeListCheckedQuestions = (List<List<CheckedQuestion>>)formatter.Deserialize(fs);
                return deserilizeListCheckedQuestions;
            }
        }

        public List<List<CheckedQuestion>> GetChekedQuestions()
        {
            return ListCheckedQuestions;
        }

        public List<List<CheckedQuestion>> GetChekedQuestions(string nametest)
        {
            return ListCheckedQuestions.Where(x => x[0].NameSection == nametest).ToList();             
        }
    }
}
