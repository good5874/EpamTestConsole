using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EpamTestConsole
{
    internal static class SaveAndOpenForm
    {
        public static void SaveTest(Form form, string nameFile)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream($"{nameFile}.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, form);
            }
        }

        public static Form OpenTest(string nameFile)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream($"{nameFile}.dat", FileMode.OpenOrCreate))
            {
                Form deserilizeForm = (Form)formatter.Deserialize(fs);
                if (deserilizeForm == null)
                {
                    return null;
                }
                else
                {
                    return deserilizeForm;
                }
            }
        }

    }
}

