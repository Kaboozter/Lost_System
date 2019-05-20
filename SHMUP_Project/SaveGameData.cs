using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace SHMUP_Project
{
    class SaveGameData
    {
        public SaveGameData()
        {

        }

        public static string GetFullDirectory => GetDirectory + @myFileName;
        public static string GetDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LostSystem\LostSystemData\";

        static string myFileName = "saveData.ls";

        public void Save()
        {
            if (!Directory.Exists(GetDirectory))
            {
                Directory.CreateDirectory(GetDirectory);
            }

            File.WriteAllText(GetFullDirectory, "Hello World");
        }

    }
}
