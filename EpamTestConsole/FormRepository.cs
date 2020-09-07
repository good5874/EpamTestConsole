using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EpamTestConsole
{
    public static class FormRepository
    {
        public static void SaveTest(Management management, string nameFile)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream($"{nameFile}.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, management);
            }
        }

        public static Management OpenTest(string nameFile)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream($"{nameFile}.dat", FileMode.OpenOrCreate))
            {
                Management deserilizeManagement = (Management)formatter.Deserialize(fs);
                return deserilizeManagement;
            }
        }

    }
}

