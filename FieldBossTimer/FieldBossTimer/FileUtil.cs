using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FieldBossTimer
{
    public class FileUtil
    {
        public static readonly string BossTimersFile = "bossTimers.csv";
        public static readonly string UserSaveFile = "userSave.csv";

        public FileUtil() { }

        public static Dictionary<string, int> LoadBossTimersData()
        {
            var csvContent = ReadCsv(BossTimersFile);
            return csvContent.Where(x => x.Count >= 2 && int.TryParse(x[1], out _)).ToDictionary(x => x[0], y => int.Parse(y[1]));
        }

        public static void GenerateSampleBossTimersFile()
        {
            var sampleLine1 = new List<string> { "Sample Boss1", "60" };
            var sampleLine2 = new List<string> { "Sample Boss2", "600" };
            SaveCsv(BossTimersFile, new List<List<string>> { sampleLine1, sampleLine2 });
        }

        public static Dictionary<int, string> LoadUserSaveData()
        {
            var csvContent = ReadCsv(UserSaveFile);
            return csvContent.Where(x => x.Count >= 2 && int.TryParse(x[0], out _)).ToDictionary(x => int.Parse(x[0]), y => y[1]);
        }

        public static void SaveCurrentValues(IEnumerable<IEnumerable<string>> values)
        {
            SaveCsv(UserSaveFile, values);
        }

        private static List<List<string>> ReadCsv(string filePath)
        {
            var res = new List<List<string>>();

            if (!File.Exists(filePath))
                return res;

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                var trimmedValues = values.Select(x => x.Trim()).ToList();

                res.Add(trimmedValues);
            }

            return res;
        }

        private static void SaveCsv(string filePath, IEnumerable<IEnumerable<string>> content)
        {
            var lines = new List<string>();
            
            foreach (var lineValues in content)
            {
                lines.Add(string.Join(",", lineValues.Select(x => x.Trim())));
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}
