using System.Text.RegularExpressions;

namespace WeatherData
{
    internal class ManipulateData
    {
        //public static List<dynamic> GetGroups()
        //{
        //    var matches = CollectData.ReadAll("tempdata5-med fel.txt");
        //    var data = matches.SelectMany(matchCollection => matchCollection).Cast<Match>()
        //       .Select(match => new
        //       {
        //           Date = match.Groups["Date"].Value,
        //           Year = match.Groups["Year"].Value,
        //           Month = match.Groups["Month"].Value,
        //           Day = match.Groups["Day"].Value,
        //           Hour = match.Groups["Hour"].Value,
        //           Minute = match.Groups["Minute"].Value,
        //           Second = match.Groups["Second"].Value,
        //           Sensor = match.Groups["Sensor"].Value,
        //           Temperature = double.Parse((match.Groups["Temp"].Value)),
        //           Humidity = match.Groups["Humidity"].Value
        //       });

        //    return data;
        //}

        public static void OutdoorsAverage()
        {
            char degreeSymbol = '\u00B0';
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please enter a date for the readings you want to see:");
                string date = Console.ReadLine();

                Regex regex = new Regex(@"^(?<Date>(?<Year>2016)-\b(?<Month>0[6-9]|1[0-2])\b-(?<Day>\d{2}))$");
                var matches = CollectData.ReadAll("tempdata5-med fel.txt");
                if (regex.IsMatch(date))
                {
                    var data = matches.SelectMany(matchCollection => matchCollection).Cast<Match>()
                        .Where(match => match.Groups["Date"].Value == date)
                        .Select(match => new
                        {
                            Date = match.Groups["Date"].Value,
                            Sensor = match.Groups["Sensor"].Value,
                            Temperature = double.Parse((match.Groups["Temp"].Value)),
                            Humidity = double.Parse(match.Groups["Humidity"].Value)
                        });

                    var averageTempPerDay = data
                        .Where(s => s.Sensor == "Ute")
                        .GroupBy(d => d.Date)
                        .Select(v => new
                        {
                            Date = v.Key,
                            AvegerageTemp = v.Average(t => t.Temperature),
                            AverageHumidity = v.Average(h => h.Humidity)
                        });

                    if (averageTempPerDay.Any())
                    {
                        foreach (var t in averageTempPerDay)
                        {
                            Console.WriteLine($"Average temperature and humidity for {t.Date}:");
                            Console.WriteLine($"   Temperature: {Math.Round(t.AvegerageTemp, 2)}{degreeSymbol}C");
                            Console.WriteLine($"   Humidity: {Math.Round(t.AverageHumidity, 2)}%");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No match found for this date");
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect format");
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to try again");
                Console.WriteLine("Press 'M' to go back to the main menu");
                ConsoleKeyInfo mainmenu = Console.ReadKey();
                if (mainmenu.KeyChar == 'm')
                {
                    break;
                }

            }
            Console.Clear();
            Program.Outdoors();
        }

        public static void OutdoorsMinMax()
        {
            Console.Clear();
            char degreeSymbol = '\u00B0';
            var matches = CollectData.ReadAll("tempdata5-med fel.txt");

            var data = matches
                .SelectMany(matchCollection => matchCollection).Cast<Match>()
                .Select(match => new
                {
                    Date = match.Groups["Date"].Value,
                    Sensor = match.Groups["Sensor"].Value,
                    Temperature = double.Parse((match.Groups["Temp"].Value)),
                    Humidity = double.Parse(match.Groups["Humidity"].Value)
                });

            var averageValuesPerDay = data
                .Where(s => s.Sensor == "Ute")
                .GroupBy(d => d.Date)
                .Select(v => new
                {
                    Date = v.Key,
                    AvegerageTemp = v.Average(t => t.Temperature),
                    AverageHumidity = v.Average(h => h.Humidity),
                    AverageRiskForMold = v.Average(m => ((m.Humidity - 78) * (m.Temperature / 15) / 0.22))
                });

            //Average temperature per day
            var tempDesc = averageValuesPerDay
                .OrderByDescending(t => t.AvegerageTemp);

            Console.WriteLine("Average temperature (descending order)");
            foreach (var t in tempDesc)
            {
                Console.WriteLine($"   {t.Date}, {Math.Round(t.AvegerageTemp, 2)}{degreeSymbol}C");
            }
            Console.WriteLine();

            //Average humidity per day
            var humidityAsc = averageValuesPerDay
                .OrderBy(h => h.AverageHumidity);

            Console.WriteLine("Average humidity (ascending order)");
            foreach (var h in humidityAsc)
            {
                Console.WriteLine($"\t{h.Date}, {Math.Round(h.AverageHumidity, 2)}%");
            }
            Console.WriteLine();

            //Average mold risk per day
            var moldRiskAsc = averageValuesPerDay
                .OrderBy(m => m.AverageRiskForMold);

            Console.WriteLine("Average risk for mold (ascending order)");
            foreach (var m in moldRiskAsc)
            {
                if (m.AverageRiskForMold > 0.0)
                {
                    Console.WriteLine($"\t{m.Date}, {Math.Round(m.AverageRiskForMold, 2)}%");
                }
                else
                {
                    Console.WriteLine($"\t{m.Date}, no risk for mold");
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press 'M' to go back to the main menu");
            ConsoleKeyInfo mainmenu = Console.ReadKey();
            if (mainmenu.KeyChar == 'm')
            {
                Console.Clear();
                Program.Outdoors();
            }
        }

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

                    Console.WriteLine($"{t.Date.AddDays(-4).ToString("yyyy-MM-dd")} was the first day of meteorological autumn with an average temperature of {Math.Round(t.AvegerageTemp, 2)}{degreeSymbol}C.");
                    break;
                }
            }
            if (consecutiveAutumnDays < 5)
            {
                Console.WriteLine($"No autumn this year as the daily average temperature was never equal to or below zero. Closest day was {coldestFromTen.Date.ToString("yyyy-MM-dd")} with an average temperature of {Math.Round(coldestFromTen.AvegerageTemp, 2)}{degreeSymbol}C.");
            }

            Console.ReadLine();
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

                    Console.WriteLine($"{t.Date.AddDays(-4).ToString("yyyy-MM-dd")} was the first day of meteorological winter with an average temperature of {Math.Round(t.AvegerageTemp, 2)}{degreeSymbol}C.");
                    break;
                }
            }

            if (consecutiveWinterDays < 5)
            {
                Console.WriteLine($"No winter this year as the daily average temperature was never equal to or below zero. Closest day was {coldestAboveZero.Date.ToString("yyyy-MM-dd")} with an average temperature of {Math.Round(coldestAboveZero.AvegerageTemp, 2)}{degreeSymbol}C.");
            }
            Console.ReadLine();
        }

        public static void IndoorsAverage()
        {

            char degreeSymbol = '\u00B0';
            while (true)
            {


                Console.Clear();
                Console.WriteLine("Please enter a date for the readings you want to see:");
                string date = Console.ReadLine();

                Regex regex = new Regex(@"^(?<Date>(?<Year>2016)-\b(?<Month>0[6-9]|1[0-2])\b-(?<Day>\d{2}))$");
                var matches = CollectData.ReadAll("tempdata5-med fel.txt");
                if (regex.IsMatch(date))
                {
                    var data = matches.SelectMany(matchCollection => matchCollection).Cast<Match>()
                        .Where(match => match.Groups["Date"].Value == date)
                        .Select(match => new
                        {
                            Date = match.Groups["Date"].Value,
                            Sensor = match.Groups["Sensor"].Value,
                            Temperature = double.Parse((match.Groups["Temp"].Value)),
                            Humidity = double.Parse(match.Groups["Humidity"].Value)
                        });

                    var averageTempPerDay = data
                        .Where(s => s.Sensor == "Inne")
                        .GroupBy(d => d.Date)
                        .Select(v => new
                        {
                            Date = v.Key,
                            AvegerageTemp = v.Average(t => t.Temperature),
                            AverageHumidity = v.Average(h => h.Humidity)
                        });

                    if (averageTempPerDay.Any())
                    {
                        foreach (var t in averageTempPerDay)
                        {
                            Console.WriteLine($"Average temperature and humidity for {t.Date}:");
                            Console.WriteLine($"   Temperature: {Math.Round(t.AvegerageTemp, 2)}{degreeSymbol}C");
                            Console.WriteLine($"   Humidity: {Math.Round(t.AverageHumidity, 2)}%");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No match found for this date");
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect format. Press any key to try again or 'M' to go back to the main menu");
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to try again");
                Console.WriteLine("Press 'M' to go back to the main menu");
                ConsoleKeyInfo mainmenu = Console.ReadKey();
                if (mainmenu.KeyChar == 'm')
                {
                    break;
                }

            }
            Program.TempMenu();
        }

        public static void IndoorsMinMax()
        {
            Console.Clear();
            char degreeSymbol = '\u00B0';
            var matches = CollectData.ReadAll("tempdata5-med fel.txt");

            var data = matches
                .SelectMany(matchCollection => matchCollection).Cast<Match>()
                .Select(match => new
                {
                    Date = match.Groups["Date"].Value,
                    Sensor = match.Groups["Sensor"].Value,
                    Temperature = double.Parse((match.Groups["Temp"].Value)),
                    Humidity = double.Parse(match.Groups["Humidity"].Value)
                });

            var averageValuesPerDay = data
                .Where(s => s.Sensor == "Inne")
                .GroupBy(d => d.Date)
                .Select(v => new
                {
                    Date = v.Key,
                    AvegerageTemp = v.Average(t => t.Temperature),
                    AverageHumidity = v.Average(h => h.Humidity),
                    AverageRiskForMold = v.Average(m => ((m.Humidity - 78) * (m.Temperature / 15) / 0.22))
                });

            //Average temperature per day
            var tempDesc = averageValuesPerDay
                .OrderByDescending(t => t.AvegerageTemp);

            Console.WriteLine("Average temperature (descending order)");
            foreach (var t in tempDesc)
            {
                Console.WriteLine($"   {t.Date}, {Math.Round(t.AvegerageTemp, 2)}{degreeSymbol}C");
            }
            Console.WriteLine();

            //Average humidity per day
            var humidityAsc = averageValuesPerDay
                .OrderBy(h => h.AverageHumidity);

            Console.WriteLine("Average humidity (ascending order)");
            foreach (var h in humidityAsc)
            {
                Console.WriteLine($"\t{h.Date}, {Math.Round(h.AverageHumidity, 2)}%");
            }
            Console.WriteLine();

            //Average mold risk per day
            var moldRiskAsc = averageValuesPerDay
                .OrderBy(m => m.AverageRiskForMold);

            Console.WriteLine("Average risk for mold (ascending order)");
            foreach (var m in moldRiskAsc)
            {
                if (m.AverageRiskForMold > 0.0)
                {
                    Console.WriteLine($"\t{m.Date}, {Math.Round(m.AverageRiskForMold, 2)}%");
                }
                else
                {
                    Console.WriteLine($"\t{m.Date}, no risk for mold");
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press 'M' to go back to the menu");
            ConsoleKeyInfo mainmenu = Console.ReadKey();
            if (mainmenu.KeyChar == 'm')
            {
                Console.Clear();
                Program.Outdoors();
            }
        }
    }
}
