using System.Text.RegularExpressions;

namespace WeatherData
{
    internal class WriteData
    {
        public static string path = "../../../Files/";

        public static void WriteAll()
        {
            char degreeSymbol = '\u00B0';
            var matches = CollectData.ReadAll("tempdata5-med fel.txt");

            var data = matches
                .SelectMany(matchCollection => matchCollection).Cast<Match>()
                .Select(match => new
                {
                    Month = match.Groups["Month"].Value,
                    Sensor = match.Groups["Sensor"].Value,
                    Temperature = double.Parse((match.Groups["Temp"].Value)),
                    Humidity = double.Parse(match.Groups["Humidity"].Value)
                });

            var averageValuesPerMonth = data

                .GroupBy(d => new { d.Month, d.Sensor })
                .Select(v => new
                {
                    Month = v.Key.Month,
                    Sensor = v.Key.Sensor,
                    AvegerageTemp = v.Average(t => t.Temperature),
                    AverageHumidity = v.Average(h => h.Humidity),
                    AverageRiskForMold = v.Average(m => ((m.Humidity - 78) * (m.Temperature / 15) / 0.22))
                });
            var tempDesc = averageValuesPerMonth
                .OrderByDescending(t => t.AvegerageTemp);

            string filename = "AveragePerMonth.txt";
            using (StreamWriter writer = new StreamWriter(path + filename))
            {

                writer.WriteLine("Average temperature per month(descending order)");
                foreach (var t in tempDesc)
                {
                    writer.WriteLine($"{t.Sensor} - Month: {t.Month}, {Math.Round(t.AvegerageTemp, 2)} {degreeSymbol}C");
                }
            }
        }
    }
}
