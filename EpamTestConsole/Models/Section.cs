using System;
using System.Collections.Generic;

namespace EpamTestConsole
{
    [Serializable]
    public class Section
    {
        public string NameSection { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();

        public Section(string _nameSection)
        {
            NameSection = _nameSection;
        }
        private string GetTruePercentAnswers()
        {
            double i = 0;
            double percent = 0;
            foreach (Question item in Questions)
            {
                if (item.Result == "true")
                {
                    i++;
                }
            }

            percent = i / Questions.Count;

            return Math.Round((percent * 100.0), 3).ToString();
        }

        public override string ToString()
        {
            if (Questions.Count != 0)
            {
                return $"\t{ConsoleMenuConstant.Section} {NameSection} пройден на  {GetTruePercentAnswers()}%";
            }
            else
            {
                return $"\t{ConsoleMenuConstant.Section} {NameSection} не содержит вопросов";
            }
        }
    }
}
