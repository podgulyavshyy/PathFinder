namespace PathFinder;


class Program
{
    
    static void Main(string[] args)
    {
        var generator = new MapGenerator(new MapGeneratorOptions()
        {
            Height = 15,  //changed value, default = 35
            Width = 30,  //changed value, default = 90
            Noise = .9f, //added new parameter
            Seed = 1,
            AddTraffic = false ,
            TrafficSeed = 1234

        });

        //string[,] map = generator.Generate();
        //generator.SetSeed(0.2);
        MapPrinter test = new MapPrinter(); // .Print(map);
        int numOfRuns = 40;
        //int currSeed = 0;
        var dic = new Dictionary<int, int[]>();
        for (int run = 0; run < numOfRuns; run++)
        {
            //currSeed = Random.Next(1,9) / 10;
            generator.SetSeed(run/100);
            string[,] map = generator.Generate();
            dic.Add(run,test.Print(map));
            
        }

        int totalPointsD = 0;
        int totalPointsA = 0;
        int totalStepsD = 0;
        int totalStepsA = 0;
        
        foreach (var val in dic)
        {
            Console.WriteLine("{0}, {1}", val.Key, String.Join(", ", val.Value));
            totalPointsD += val.Value[0];
            totalPointsA += val.Value[2];
            totalStepsD += val.Value[1];
            totalStepsA += val.Value[3];
        }
        Console.WriteLine("Avg points for Dijkstra = " + totalPointsD/numOfRuns);
        Console.WriteLine("Avg points for AStar = " + totalPointsA/numOfRuns);
        Console.WriteLine("Avg steps for Dijkstra = " + totalStepsD/numOfRuns);
        Console.WriteLine("Avg steps for AStar = " + totalStepsA/numOfRuns);
        //Console.WriteLine("test");
        //var testDic = test.Print(map);
    }
}

