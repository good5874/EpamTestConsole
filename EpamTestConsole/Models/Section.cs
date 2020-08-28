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
    }
}
