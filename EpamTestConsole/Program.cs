using System;

namespace EpamTestConsole
{
    enum Сommands { y, n, del }
    internal class Program
    {
        static void Main(string[] args)
        {  
            Management management;           

            Console.WriteLine("Создать тест?");
            if (Console.ReadLine() == Сommands.y.ToString())
            {
                management = new Management();
                Console.WriteLine("Введите тему теста");
                string nameTest = Console.ReadLine();
                management.CreateTest(nameTest);
                Save(management);
            }
            Console.WriteLine("Пройти тест?");
            if (Console.ReadLine() == Сommands.y.ToString())
            {
                Console.WriteLine("Введите имя файла теста:");
                string nameFile = Console.ReadLine();

                management = SaveAndOpenForm.OpenTest(nameFile);
                if (management == null)
                {
                    Console.WriteLine("Не удалось открыть файл");
                }
                else
                {
                    management.StartTest();
                }
            }
            Console.WriteLine("Редактировать тест?");
            if (Console.ReadLine() == Сommands.y.ToString())
            {
                Console.WriteLine("Введите имя файла теста:");
                string nameFile = Console.ReadLine();

                management = SaveAndOpenForm.OpenTest(nameFile);
                if (management == null)
                {
                    Console.WriteLine("Не удалось открыть файл");
                }
                else
                {
                    management.EditTest();
                }
                Save(management);
            }            
        }
        private static void Save(Management management)
        {
            Console.WriteLine("Сохранить?");
            if (Console.ReadLine() == Сommands.y.ToString())
            {
                Console.WriteLine("Введите имя файла:");
                string nameFile = Console.ReadLine();
                SaveAndOpenForm.SaveTest(management, nameFile);
                Console.WriteLine("Сохранено");
            }
            else
            {
                Console.WriteLine("Сохранение отменено");
            }
        }
    }
}
