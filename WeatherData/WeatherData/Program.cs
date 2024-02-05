namespace WeatherData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rowcount = 0;

            var matches =  CollectData.ReadAll("tempdata5-med fel.txt");
            foreach (var match in matches)
            {


                foreach (var item in match)
                {

                    Console.WriteLine(rowcount + " " + item);
                    rowcount++;

                }

            }
                        
        }
    }
}