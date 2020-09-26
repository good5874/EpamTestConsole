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
        private List<Management> ListManagment;

        private string path = $"History/CheckedQuestionsRepository.dat";
        public CheckedQuestionsRepository()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory("History");
            }
            if (!File.Exists(path))
            {
                ListManagment = new List<Management>();
            }
            else
            {
                ListManagment = OpenListManagment();
            }
        }

        public void SaveListManagment(Management management)
        {
            if (management != null)
            {
                ListManagment.Add(management);
            }
            else
            {
                return;
            }
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, ListManagment);
            }
        }

        public List<Management> OpenListManagment()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                List<Management> deserilizeListManagment = (List<Management>)formatter.Deserialize(fs);
                return deserilizeListManagment;
            }
        }

        public List<Management> GetListManagements()
        {
            return ListManagment;
        }

        public List<Management> GetListManagements(string nametest)
        {
            return ListManagment.Where(x => x.RootTest.Section.NameSection == nametest).ToList();
        }
    }
}
