using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EpamTestConsole
{
    internal static class SaveAndOpenForm
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
                if (deserilizeManagement == null)
                {
                    return null;
                }
                else
                {
                    return deserilizeManagement;
                }
            }
        }

    }
}

