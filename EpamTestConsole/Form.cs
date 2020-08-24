using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace EpamTestConsole
{
    //1
    [Serializable]
    public class Form
    {
        public Section section = null;
        public List<Section> sectionsLayer2 = new List<Section>();
        public List<Section> sectionsLayer3 = new List<Section>();


        public void CreateTest()
        {            
            Console.WriteLine("Введите тему теста");
            Section section1 = new Section(Console.ReadLine());

            section = section1;

            sectionsLayer2 = AddSection(section);
            foreach (Section section in sectionsLayer2)
            {
                foreach (Section sectionLayer3 in AddSection(section))
                    sectionsLayer3.Add(sectionLayer3);
            }

            AddQuestion(section);
            foreach (Section s in sectionsLayer2)
                AddQuestion(s);
            foreach (Section s in sectionsLayer3)
                AddQuestion(s);            
        }

        public void AddQuestion(Section section)
        {
            for (int i = 0; ; i++)
            {
                if (i > 0)
                {
                    Console.WriteLine("Чтобы закончить добавление вопросов введите y");
                    if (Console.ReadLine() == "y") break;
                }

                Console.WriteLine($"Добавить в {section.NameSection} добавить вопросы?");
                if (Console.ReadLine() == "y")
                {

                    Console.WriteLine("Введите вопрос:");
                    string question = Console.ReadLine();
                    Console.WriteLine("Введите ответ:");
                    string answer = Console.ReadLine();
                    Console.WriteLine("Добавить варианты ответов?");
                    List<string> answerOptions = null;

                    if (Console.ReadLine() == "y")
                    {
                        Console.WriteLine("Введите n когда добавили достаточно вариантов ответов.");
                        answerOptions = new List<string>();
                        for (; ; )
                        {
                            var tmp = Console.ReadLine();
                            if (tmp == "n") break;
                            else
                            {
                                answerOptions.Add(tmp);
                            }
                        }
                    }

                    bool checkAnswer = false;
                    if (answer != null && answer != "")
                        checkAnswer = true;

                    bool options = false;
                    if (answerOptions != null)
                        options = true;


                    section.CreateQuestion(question, checkAnswer, options, answer, answerOptions);
                }
            }
        }

        public void ConsoleWriteSection(Section section)
        {
            List<string> answers = new List<string>();

            if (section.Parent == null)
                Console.WriteLine(section.NameSection);
            else if (section.Parent.Parent == null && section.Parent != null) Console.WriteLine(section.Parent.NameSection);
            else if (section.Parent.Parent.Parent == null && section.Parent.Parent != null) Console.WriteLine(section.Parent.Parent.NameSection);


            foreach (Question question in section.Questions)
            {
                string answer;
                Console.WriteLine("Вопрос: " + question.TextQuestion);
                if (question.Options)
                {
                    int i = 1;
                    Console.WriteLine("Варианты ответа:");
                    foreach (var str in question.AnswerOptions)
                    {
                        Console.WriteLine($" {i})" + str);
                        i++;
                    }
                    Console.WriteLine("Введите нужный вариант ответа:");
                }
                else Console.WriteLine("Введите ответ:");
                answer = Console.ReadLine();
                answers.Add(answer);
            }


        }//проверить варианты ответы

        public List<Section> AddSection(Section section)
        {
            List<Section> sections = new List<Section>();
            Console.WriteLine($"Добавить потемы к {section.NameSection} ?");
            if (Console.ReadLine() == "y")
            {
                Console.WriteLine($"Сколько добавить к {section.NameSection} подтем?");
                int sectionCount = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите название подтем:");
                for (int i = 0; i < sectionCount; i++)
                {
                    sections.Add(new Section(Console.ReadLine(), section));
                }
            }
            return sections;
        }

        public void SaveTest(Form form)
        {
            Console.WriteLine("Сохранить?");
            if (Console.ReadLine() == "y")
            {
                Console.WriteLine("Введите имя файла:");
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream($"{Console.ReadLine()}.dat", FileMode.OpenOrCreate))
                {                    
                    formatter.Serialize(fs, form);

                    Console.WriteLine("Сохранено");
                }
            }
        }
      
        public Form OpenTest()
        {
            Console.WriteLine("Открыть?");
            if (Console.ReadLine() == "y")
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Console.WriteLine("Введите имя файла:");
                using (FileStream fs = new FileStream($"{Console.ReadLine()}.dat", FileMode.OpenOrCreate))
                {
                    Form deserilizeForm = (Form)formatter.Deserialize(fs);
                    return deserilizeForm;
                }
            }
            else
            {
                new Exception("файл не найден");
                return null;
            }
        }


    }
}
