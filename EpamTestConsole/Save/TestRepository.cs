using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EpamTestConsole
{
    public static class TestRepository
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
            try
            {
                using (FileStream fs = new FileStream($"{nameFile}.dat", FileMode.Open))
                {
                    Management deserilizeManagement = (Management)formatter.Deserialize(fs);
                    return deserilizeManagement;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
