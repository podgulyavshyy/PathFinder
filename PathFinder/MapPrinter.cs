namespace PathFinder;

using System;
using System.Collections.Generic;

public class MapPrinter
{
    public void Print(string[,] maze)
    {
        Point A = new Point(0, 0);
        Point B = new Point(5, 5);
        maze[B.Column, B.Row] = "B";
        
        var path = AStar(maze, A, B);
        // OldPrint(maze);
        for (var row = 0; row < maze.GetLength(1); row++)
        {
            Console.Write($"{row}\t");
            for (var column = 0; column < maze.GetLength(0); column++)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    if (path[i].Column == column && path[i].Row == row)
                    {
                        maze[column, row] = "#";
                    }
                }
            }
        }
        maze[A.Column, A.Row] = "A";

        OldPrint(maze);
    }

    public List<Point> AStar(string[,] maze, Point A, Point B)
    {

        Point curr = A;

        List<Point> path = new List<Point>();
        
        
        while (true)
        {
            if (curr.Column == B.Column && curr.Row == B.Row)
            {
                break;
            }
            else
            {

                path.Add(curr);
                List<Point> noval = GetTilesAround(curr);
                List<Point> val = CalcValue(noval, maze, A, B);
                Stack<Point> noLast = new Stack<Point>();
                int k = val.Count;
                for (int j = 0; j < k; j++)
                {
                    var thisPoint = val[j];
                    
                    if (path.Contains(thisPoint))
                    {

                    }
                    else
                    {
                        noLast.Push(thisPoint);
                    }
                }

                var lowestValPoint = noLast.Peek();
                for (int j = 0; j < noLast.Count; j++)
                {
                    var currPoint = noLast.Pop();
                    if (currPoint.Value <= lowestValPoint.Value)
                    {
                        lowestValPoint = currPoint;
                    }
                }

                //path.Add(lowestValPoint);
                curr = lowestValPoint;
            }

        }

        return path;
        //Console.Write("");
    }

    public List<Point> GetTilesAround(Point point)
    {
        int col = point.Column;
        int row = point.Row;
        List<Point> points = new List<Point>();
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                int col1 = col + 1;
                points.Add(new Point(col1, row));
            }
            else if (i == 1)
            {
                int col1 = col - 1;
                if (col1 < 0)
                {
                    col1 = 0;
                }
                points.Add(new Point(col1, row));
            }
            else if (i == 2)
            {
                int row1 = row + 1;
                points.Add(new Point(col, row1));
            }
            else if (i == 3)
            {
                int row1 = row - 1;
                if (row1 < 0)
                {
                    row1 = 0;
                }
                points.Add(new Point(col, row1));
            }
        }

        return points;
    }

    public List<Point> CalcValue(List<Point> points, string[,] maze, Point A, Point B)
    {
        List<Point> pointsVal = new List<Point>();
        List<Point> noWalls = new List<Point>();
        
        
        for (int i = 0; i < points.Count; i++)
        {
            var curr = points[i];
            
            if (maze[curr.Column, curr.Row] != "█")
            {
                noWalls.Add(curr);
            }
        }


        int j = noWalls.Count;
        for (int i = 0; i < j; i++)
        {
            var curr = noWalls[i];
            int colValB = Math.Abs(curr.Column - B.Column);
            int rowValB = Math.Abs(curr.Row - B.Row);
            int colValA = Math.Abs(curr.Column - A.Column);
            int rowValA = Math.Abs(curr.Row - A.Row);

            int val = colValB + rowValB + rowValA + colValA; // + Int32.Parse(maze[curr.Column, curr.Row]);
            curr.Value = val;
            pointsVal.Add(curr);
        }

        return pointsVal;
    }

    public void OldPrint(string[,] maze)
    {
        PrintTopLine();
        for (var row = 0; row < maze.GetLength(1); row++)
        {
            Console.Write($"{row}\t");
            for (var column = 0; column < maze.GetLength(0); column++)
            {
                Console.Write(maze[column, row]);
            }

            Console.WriteLine();
        }

        void PrintTopLine()
        {
            Console.Write($" \t");
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                Console.Write(i % 10 == 0? i / 10 : " ");
            }
    
            Console.Write($"\n \t");
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                Console.Write(i % 10);
            }
    
            Console.WriteLine("\n");
        }
    }
}



