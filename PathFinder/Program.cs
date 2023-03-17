namespace PathFinder;


class Program
{
    static void Main(string[] args)
    {
        var generator = new MapGenerator(new MapGeneratorOptions()
        {
            Height = 15, // 35
            Width = 25, // 90
            AddTraffic = true,
            //Noise = 0.9f
        });

        string[,] map = generator.Generate();
        new MapPrinter().Print(map);
    }
}

