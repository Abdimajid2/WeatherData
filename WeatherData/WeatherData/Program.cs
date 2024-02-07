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

            ManipulateData.IndoorsMinMax();

        }
    }
}