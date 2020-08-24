using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace EpamTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> answers = new List<string>();
            Form form = new Form();

            Console.WriteLine("Создать тест?");
            if (Console.ReadLine() == "y")
            { 
                form.CreateTest();
                form.SaveTest(form);
            }


            Console.WriteLine("Пройти тест?");
            if (Console.ReadLine() == "y")
            {               
                form = form.OpenTest();

                form.ConsoleWriteSection(form.section);


                foreach (Section s in form.sectionsLayer2)
                {
                    form.ConsoleWriteSection(s);
                }
                foreach (Section s in form.sectionsLayer3)
                {
                    form.ConsoleWriteSection(s);
                }

            }           

        }


    }
}
