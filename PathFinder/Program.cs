namespace PathFinder;


class Program
{
    static void Main(string[] args)
    {
        var generator = new MapGenerator(new MapGeneratorOptions()
        {
            Height = 20, // 35
            Width = 35, // 90
            AddTraffic = false,
            Noise = 0.9f
        });

        string[,] map = generator.Generate();
        new MapPrinter().Print(map);
    }
}

