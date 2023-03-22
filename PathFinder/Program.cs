namespace PathFinder;


class Program
{
    static void Main(string[] args)
    {
        var generator = new MapGenerator(new MapGeneratorOptions()
        {
            Height = 15,  //changed value, default = 35
            Width = 30,  //changed value, default = 90
            Noise = .8f, //added new parameter
            Seed = 1,
            AddTraffic = false ,
            TrafficSeed = 1234

        });

        string[,] map = generator.Generate();
        new MapPrinter().Print(map);
    }
}

