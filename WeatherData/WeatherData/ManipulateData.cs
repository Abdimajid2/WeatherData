﻿using System.Text.RegularExpressions;

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
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
            }
        }

        public static void OutdoorsMinMax()
        {
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

            var averageTempPerDay = data
                .Where(s => s.Sensor == "Ute")
                .GroupBy(d => d.Date)
                .Select(v => new
                {
                    Date = v.Key,
                    AvegerageTemp = v.Average(t => t.Temperature),
                    AverageHumidity = v.Average(h => h.Humidity)
                });

            var tempDesc = averageTempPerDay
                .OrderByDescending(t => t.AvegerageTemp);

            Console.WriteLine("Average temperature (descending order)");
            foreach (var t in tempDesc)
            {
                Console.WriteLine($"   {t.Date}, {Math.Round(t.AvegerageTemp, 2)}{degreeSymbol}C");
            }
            Console.WriteLine();

            var humidityAsc = averageTempPerDay
                .OrderBy(h => h.AverageHumidity);

            Console.WriteLine("Average humidity (ascending order)");
            foreach (var h in humidityAsc)
            {
                Console.WriteLine($"\t{h.Date}, {Math.Round(h.AverageHumidity, 2)}%");
            }
            Console.WriteLine();
        }
    }
}
