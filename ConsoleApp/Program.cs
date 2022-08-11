namespace ConsoleApp
{
    internal class Program
    {
        static void Main()
        {
            string filename = "data.csv"; //sets the path to the file used for import
            DataReader.ImportAndPrintData(filename);
        }
    }
}
