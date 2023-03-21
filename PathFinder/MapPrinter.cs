namespace PathFinder;

using System;
using System.Collections.Generic;

public class MapPrinter
{
    public void Print(string[,] maze)
    {
        Point A = new Point(0, 0);
        Point B = new Point(20, 15);
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
        List<Point> open = new List<Point>();
        
  
        
        while (true)
        {
            if (curr.Column == B.Column && curr.Row == B.Row)
            {
                break;
            }
            else
            {

                path.Add(curr);
                List<Point> noval = GetTilesAround(curr, maze); // maybe us GetNeighbors native func
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

                foreach (var tile in val) //alt path
                {
                    open.Add(tile);
                }
                var lowestTest = open[0];
                if (noLast.Count == 0)
                {
                   
                    for (int i = 0; i < open.Count; i++)
                    {
                        if (open[i].Value < lowestTest.Value)
                        {
                            lowestTest = open[i];
                        }
                    }
                    curr = lowestTest;
                    open.Remove(lowestTest);
                    continue;
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

    public List<Point> GetTilesAround(Point point, string[,] maze)
    {
        int col = point.Column;
        int row = point.Row;
        List<Point> points = new List<Point>();
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                
                int col1 = col + 1;
                if (col1 >= maze.GetLength(0))
                {
                    col1--;
                }
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
                if (row1 >= maze.GetLength(1))
                {
                    row1--;
                }
                
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
            var curr = noWalls[i]; // 11 33 11
            // int val = Math.Abs(B.Column - curr.Column) + Math.Abs(B.Row - curr.Row) + Math.Abs(A.Column - curr.Column) + Math.Abs(A.Row - curr.Row); // + Int32.Parse(maze[curr.Column, curr.Row]);
            double val = Math.Abs(Math.Sqrt((B.Column - curr.Column) ^ 2 + (B.Row - curr.Row) ^ 2) + Math.Sqrt((A.Column - curr.Column)^2 + (A.Row - curr.Row)^2));
            
            curr.Value = (int)val;
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



