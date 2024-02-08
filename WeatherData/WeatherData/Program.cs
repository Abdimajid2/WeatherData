namespace WeatherData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var rowcount = 0;


            //foreach (var match in matches)
            //{
            //    foreach (var item in match)
            //    {
            //        Console.WriteLine(rowcount + " " + item);
            //        rowcount++;
            //    }
            //}

            //ManipulateData.OutdoorsAverage();

            //ManipulateData.OutdoorsMinMax();

            //ManipulateData.Autumn();
            //ManipulateData.Winter();

            //ManipulateData.IndoorsAverage();

            //ManipulateData.IndoorsMinMax();

            //WriteData.WriteAll();
            TempMenu();

            //Outdoors();

        }

        public static void TempMenu()
        {
            Console.WriteLine("Choose the information you would like to see");
            Console.WriteLine("1. View information for outdoors");
            Console.WriteLine("2. View information for indoors");
            Console.WriteLine("3. View  meteorological autumn");
            Console.WriteLine("4. View  meteorological winter");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();    
                    Outdoors();
                    break;
                case "2":
                    Console.Clear();
                    Indoor();
                    break;
                case "3":
                    Console.Clear();
                    ManipulateData.Autumn();
                    TempMenu();
                    break;
                case "4":
                    Console.Clear();
                    ManipulateData.Winter();
                    TempMenu();
                    break;
            }
        }

       public static void Outdoors()
        {
            Console.WriteLine("choose the information you would like to see for outdoors");
            Console.WriteLine("1.View average temperature and humidity for a specific date");
            Console.WriteLine("2.Daily average information");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ManipulateData.OutdoorsAverage();
                    break;
                case "2":
                    Console.Clear();
                    ManipulateData.OutdoorsMinMax();
                    break;                 
            }
        }

        public static void Indoor()
        {
            Console.WriteLine("choose the information you would like to see for indoors");
            Console.WriteLine("1.View average temperature and humidity for a specific date");
            Console.WriteLine("2.Daily average information");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ManipulateData.IndoorsAverage();
                    break;
                case "2":
                    Console.Clear();
                    ManipulateData.IndoorsMinMax();
                    break;


            }

        }
    }
}