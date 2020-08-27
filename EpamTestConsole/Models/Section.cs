using System;
using System.Collections.Generic;

namespace EpamTestConsole
{
    [Serializable]
    public class Section
    {
        public string NameSection { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public Section Parent { get; set; }

        public Section(string _nameSection, Section _parent = null)
        {
            NameSection = _nameSection;
            Parent = _parent;
        }
    }
}
