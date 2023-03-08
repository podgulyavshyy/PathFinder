namespace PathFinder;


class Program
{
    static void Main(string[] args)
    {
        var generator = new MapGenerator(new MapGeneratorOptions()
        {
            Height = 15,  //changed value, default = 35
            Width = 30,  //changed value, default = 90
            Noise = .5f, //added new parametr
            AddTraffic = true,
            TrafficSeed = 1

        });

        string[,] map = generator.Generate();
        new MapPrinter().Print(map);
    }
}

