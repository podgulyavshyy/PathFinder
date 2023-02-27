namespace PathFinder;


class Program
{
    static void Main(string[] args)
    {
        var generator = new MapGenerator(new MapGeneratorOptions()
        {
            Height = 35,
            Width = 90,
        });

        string[,] map = generator.Generate();
        new MapPrinter().Print(map);
    }
}

