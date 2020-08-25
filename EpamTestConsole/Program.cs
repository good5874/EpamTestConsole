using System;

namespace EpamTestConsole
{
    enum Сommands { y, n }

    internal class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();

            Console.WriteLine("Создать тест?");
            if (Console.ReadLine() == "y")
            {
                Console.WriteLine("Введите тему теста");
                string nameTest = Console.ReadLine();
                form.CreateTest(nameTest);

                Console.WriteLine("Сохранить?");
                if (Console.ReadLine() == "y")
                {
                    Console.WriteLine("Введите имя файла:");
                    string nameFile = Console.ReadLine();
                    SaveAndOpenForm.SaveTest(form, nameFile);
                    Console.WriteLine("Сохранено");
                }
                else
                {
                    Console.WriteLine("Сохранение отменено");
                }
            }

            Console.WriteLine("Пройти тест?");
            if (Console.ReadLine() == "y")
            {
                Console.WriteLine("Введите имя файла теста:");
                string nameFile = Console.ReadLine();

                form = SaveAndOpenForm.OpenTest(nameFile);
                if (form == null)
                {
                    Console.WriteLine("Не удалось открыть файл");
                }
                else
                {
                    form.WriteSectionToConsole(form.FirstLevelSection);

                    foreach (Section section in form.SecondLevelSection)
                    {
                        form.WriteSectionToConsole(section);
                    }
                    foreach (Section section in form.ThirdLevelSection)
                    {
                        form.WriteSectionToConsole(section);
                    }
                }
            }
        }
    }
}
