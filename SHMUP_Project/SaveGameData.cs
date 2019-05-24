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
        public static string GetFullDirectory => GetDirectory + @myFileName;
        public static string GetDirectory => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LostSystem\LostSystemData\";

        static string myFileName = "saveData.ls";

        public static string[] mySaveData = new string[5];
        public static int[] mySplittedSaveData = new int[5];
        public static string[] mysplittedSaveDataString = new string[5]
        {
            "Parts Unlocked Ship 1",
            "Parts Unlocked Ship 2",
            "Parts Unlocked Ship 3",
            "Current ship",
            "Highscore"
        };

        public static int[] GetSaveData()
        {
            mySaveData = File.ReadAllLines(GetFullDirectory);
            for (int i = 0; i < mySaveData.Length; i++)
            {
                mysplittedSaveDataString[i] = mySaveData[i].Split(':')[0];
                mySplittedSaveData[i] = int.Parse(mySaveData[i].Split(':')[1]);
            }
            return mySplittedSaveData;
        }

        public static void Save()
        {
            if (!Directory.Exists(GetDirectory))
            {
                Directory.CreateDirectory(GetDirectory);
            }

            mySplittedSaveData[0] = 5;

            for (int i = 0; i < 5; i++)
            {
                mySaveData[i] = mysplittedSaveDataString[i] + ":" + mySplittedSaveData[i];
            }

            //File.WriteAllText(GetFullDirectory, "Bye World");
            File.WriteAllLines(GetFullDirectory,mySaveData);
        }

    }
}
