using System.Text.RegularExpressions;

namespace WeatherData
{
    internal static class Extensions
    {
        public static string MoldFormula(this string formula)
        {
            return formula;
        }
    }

    internal class WriteData
    {
        public static string path = "../../../Files/";

        public static void Autumn()
        {
            char degreeSymbol = '\u00B0';
            int consecutiveAutumnDays = 0;
            var matches = CollectData.ReadAll("tempdata5-med fel.txt");

            var data = matches
                .SelectMany(matchCollection => matchCollection).Cast<Match>()
                .Select(match => new
                {
                    Date = match.Groups["Date"].Value,
                    Sensor = match.Groups["Sensor"].Value,
                    Temperature = double.Parse((match.Groups["Temp"].Value)),
                });

            var averageValuesPerDay = data
                .Where(s => s.Sensor == "Ute")
                .GroupBy(d => d.Date)
                .Select(v => new
                {
                    Date = DateOnly.Parse(v.Key),
                    AvegerageTemp = v.Average(t => t.Temperature),
                });

            var coldestFromTen = averageValuesPerDay.OrderBy(t => t.AvegerageTemp >= 10).FirstOrDefault();

            //First meteorological autumn day
            string filename = "AveragePerMonth.txt";
            using (StreamWriter writer = new StreamWriter(path + filename, true))
            {
                foreach (var t in averageValuesPerDay)
                {
                    if (t.AvegerageTemp < 10.0)
                    {
                        consecutiveAutumnDays++;
                    }
                    else
                    {
                        consecutiveAutumnDays = 0;
                    }
                    if (consecutiveAutumnDays == 5)
                    {

                        writer.WriteLine($"{t.Date.AddDays(-4).ToString("yyyy-MM-dd")} was the first day of meteorological autumn with an average temperature of {Math.Round(t.AvegerageTemp, 2)}{degreeSymbol}C.");
                        break;
                    }
                }

                if (consecutiveAutumnDays < 5)
                {
                    writer.WriteLine($"No autumn this year as the daily average temperature was never equal to or below zero. Closest day was {coldestFromTen.Date.ToString("yyyy-MM-dd")} with an average temperature of {Math.Round(coldestFromTen.AvegerageTemp, 2)}{degreeSymbol}C.");
                }

                writer.WriteLine();
            }
        }

        public static void Winter()
        {
            char degreeSymbol = '\u00B0';
            int consecutiveWinterDays = 0;
            int consecutiveClosestToWinter = 0;
            var matches = CollectData.ReadAll("tempdata5-med fel.txt");

            var data = matches
                .SelectMany(matchCollection => matchCollection).Cast<Match>()
                .Select(match => new
                {
                    Date = match.Groups["Date"].Value,
                    Sensor = match.Groups["Sensor"].Value,
                    Temperature = double.Parse((match.Groups["Temp"].Value)),
                });

            var averageValuesPerDay = data
                .Where(s => s.Sensor == "Ute")
                .GroupBy(d => d.Date)
                .Select(v => new
                {
                    Date = DateOnly.Parse(v.Key),
                    AvegerageTemp = v.Average(t => t.Temperature),
                });

            var coldestAboveZero = averageValuesPerDay.OrderBy(t => t.AvegerageTemp).FirstOrDefault();

            //First meteorological winter day
            string filename = "AveragePerMonth.txt";
            using (StreamWriter writer = new StreamWriter(path + filename, true))
            {

                foreach (var t in averageValuesPerDay)
                {
                    if (t.AvegerageTemp <= 0.0)
                    {
                        consecutiveWinterDays++;
                    }
                    else
                    {
                        consecutiveWinterDays = 0;
                    }

                    if (consecutiveWinterDays == 5)
                    {

                        writer.WriteLine($"{t.Date.AddDays(-4).ToString("yyyy-MM-dd")} was the first day of meteorological winter with an average temperature of {Math.Round(t.AvegerageTemp, 2)}{degreeSymbol}C.");
                        break;
                    }
                }

                if (consecutiveWinterDays < 5)
                {
                    writer.WriteLine($"No winter this year as the daily average temperature was never equal to or below zero. Closest day was {coldestAboveZero.Date.ToString("yyyy-MM-dd")} with an average temperature of {Math.Round(coldestAboveZero.AvegerageTemp, 2)}{degreeSymbol}C.");
                }
            }
        }


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

            var humAsc = averageValuesPerMonth
                .OrderBy(h => h.AverageHumidity);

            var moldAsc = averageValuesPerMonth
                .OrderBy(m => m.AverageRiskForMold);

            string filename = "AveragePerMonth.txt";
            using (StreamWriter writer = new StreamWriter(path + filename))
            {

                writer.WriteLine("Average temperature per month(descending order)");
                foreach (var t in tempDesc)
                {
                    writer.WriteLine($"{t.Sensor} - Month: {t.Month}, {Math.Round(t.AvegerageTemp, 2)} {degreeSymbol}C");
                }
                writer.WriteLine();

                writer.WriteLine("Average humidity per month(ascending order)");
                foreach (var h in humAsc)
                {
                    writer.WriteLine($"{h.Sensor} - Month: {h.Month}, {Math.Round(h.AverageHumidity, 2)}%");
                }
                writer.WriteLine();

                writer.WriteLine("Average risk for mold per month(ascending order)");
                foreach (var m in moldAsc)
                {
                    if (m.AverageRiskForMold <= 0)
                    {
                        writer.WriteLine($"{m.Sensor} - Month: {m.Month}, No risk for mold");
                    }
                    else
                    {
                        writer.WriteLine($"{m.Sensor} - Month: {m.Month}, {Math.Round(m.AverageRiskForMold, 2)}%");
                    }
                }

                string formula = "((luftfuktighet -78) * (Temp/15))/0,22";

                writer.WriteLine($"Formula for mold: {formula.MoldFormula()}");

                writer.WriteLine();
            }

            Autumn();
            Winter();
        }
    }
}
