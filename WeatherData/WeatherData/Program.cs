namespace WeatherData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //WriteData.WriteAll();

            MyDelegate myDelegate = TempMenu;

            DrawMenu(myDelegate);
        }

        public static void DrawMenu(MyDelegate del)
        {
            del();
        }

        public delegate void MyDelegate();

        public static void TempMenu()
        {
            while (true)
            {
                Console.Clear();
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
                    default:
                        Console.WriteLine("Wrong input, try again");
                        Console.ReadLine();
                        break;
                }
            }
        }

        public static void Outdoors()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose the information you would like to see for outdoors");
                Console.WriteLine("1.View average temperature and humidity for a specific date");
                Console.WriteLine("2.Daily average information");
                Console.WriteLine("3.Back to main menu");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManipulateData.OutdoorsAverage();
                        break;
                    case "2":
                        ManipulateData.OutdoorsMinMax();
                        break;
                    case "3":
                        TempMenu();
                        break;
                    default:
                        Console.WriteLine("Wrong input, try again");
                        Console.ReadLine();
                        break;
                }
            }
        }

        public static void Indoor()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("choose the information you would like to see for indoors");
                Console.WriteLine("1.View average temperature and humidity for a specific date");
                Console.WriteLine("2.Daily average information");
                Console.WriteLine("3.Back to main menu");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManipulateData.IndoorsAverage();
                        break;
                    case "2":
                        ManipulateData.IndoorsMinMax();
                        break;
                    case "3":
                        TempMenu();
                        break;
                    default:
                        Console.WriteLine("Wrong input, try again");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}