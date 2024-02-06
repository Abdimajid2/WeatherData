using System.Text.RegularExpressions;

namespace WeatherData
{
    internal class CollectData
    {
        public static string path = "../../../Files/";
        public static List<MatchCollection> ReadAll(string file)
        {
            List<MatchCollection> matchlist = new List<MatchCollection>();
            using (StreamReader reader = new StreamReader(path + file))
            {
                string line = reader.ReadLine();
                int rowcount = 0;
                Regex regex = new Regex(@"^(?<Date>(?<Year>2016)-\b(?<Month>0[6-9]|1[0-2])\b-(?<Day>\d{2})) (?<Hour>[0-2][0-9]):(?<Minute>[0-5][0-9]):(?<Second>[0-5][0-9]),(?<Sensor>Ute|Inne),(?<Temp>\d{0,2}\.\d{0,1}),(?<Humidity>\d{1,2})$");

                while (line != null)
                {
                    MatchCollection matches = regex.Matches(line);

                    matchlist.Add(matches);
                    rowcount++;
                    line = reader.ReadLine();
                }
            }
            return matchlist;
        }
    }
}
