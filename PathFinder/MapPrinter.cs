using System.Runtime.InteropServices.JavaScript;

namespace PathFinder;

using System;
using System.Collections.Generic;

public class MapPrinter
{
    public void Print(string[,] maze) //add new argument  List<Point> path
    {
        //declaring Startpoint/EndPoint;
        Point startPoint = new Point(1, 4);
        Point endPoint = new Point(6, 2);
        
        
        //dictionaries 
        var distances = new Dictionary<Point, int>();
        var origins = new Dictionary<Point, Point>();
        
        PrintTopLine();
        PaintBFS(startPoint);
        
        
        for (var row = 0; row < maze.GetLength(1); row++)
        {
            Console.Write($"{row}\t");
            for (var column = 0; column < maze.GetLength(0); column++)
            {
                //change sign " " for "." in maze (the free path)
                if (maze[column, row] == " ")
                {
                    maze[column, row] = " ";
                }
                
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
        
        //new
        void PaintBFS(Point point)
        {
            // declare
            var queue = new Queue<Point>();
            
            Visit(point, point);
            queue.Enqueue(point);

            while (queue.Count > 0)
            {
                var next = queue.Dequeue();

                foreach (var neighbour in GetNeighbours(next))
                {
                    //need to compare value
                    
                    
                    if (!distances.ContainsKey(neighbour) || (distances[neighbour] > (distances[next] + 1)))
                    {
                        if (origins.ContainsKey(neighbour))
                        {
                            
                        }
                        //origins.Add(neighbour, next);
                        Visit(neighbour, next);
                        queue.Enqueue(neighbour);
                    }
                }
                
            }

            if (distances.ContainsKey(endPoint))
            {
                FindPath(endPoint, startPoint);
            }
            else
            {
                throw new Exception("No way from start point to end point");
            }





                List<Point> GetNeighbours(Point point)
            {
                var neighbours = new List<Point>();
                var column = point.Column;
                var row = point.Row;
                
                
                TryAddWithOffset(-1,0, true);
                TryAddWithOffset(1,0, true);
                TryAddWithOffset(0,-1, true);
                TryAddWithOffset(0,1, true);
                
                void TryAddWithOffset(int offsetX, int offsetY, bool checkWalls)
                {
                    var newColumn = column + offsetX;
                    var newRow = row + offsetY;
                    if (newColumn >= 0 && newRow >= 0 && newColumn < maze.GetLength(0) && newRow < maze.GetLength(1))
                    {
                        if (!checkWalls || maze[newColumn, newRow] == " " || char.IsDigit(Convert.ToChar(maze[newColumn, newRow])))
                        {
                            neighbours.Add(new Point(newColumn, newRow));
                        }
                    }
                }

                return neighbours;
            }
        
        
            void Visit(Point point, Point previousPoint)
            {
            
                var number = 0;
                var n = int.Parse(maze[point.Column, point.Row]); //default = 1
                var speed = 60 - 6 * (n - 1); //if need to use speed
                
                if (distances.ContainsKey(previousPoint))
                {
                    number = distances[previousPoint];
                }

                if (!distances.ContainsKey(point))
                {
                    distances.Add(point, number + n); //change value
                    origins.Add(point, previousPoint);
                }
                else if (distances[point] > number + n)
                {
                    distances[point] = number + n;
                    origins[point] = previousPoint;
                }
                
                //to see distances on screen
                //maze[point.Column, point.Row] = string.Format("{0}", number);
            
            }
            
            void FindPath(Point pointB, Point pointA)
            {
                var path = new Stack<Point>();
                path.Push(pointB); // endPoint
                var currentPoint = pointB;

                while (distances[currentPoint] > 1)
                {
                    var neighboursDictionary = new Dictionary<Point, int>();
                    var x = GetNeighbours(currentPoint);
                    
                    foreach (var neighbour in GetNeighbours(currentPoint))
                    {
                        neighboursDictionary.Add(neighbour, distances[neighbour]);
                    }
                    
                    var previousValue = neighboursDictionary.Values.Min();
                    var previousPoint = neighboursDictionary.FirstOrDefault(x => x.Value == previousValue).Key; //neighboursDictionary.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;
                    path.Push(previousPoint);
                    currentPoint = previousPoint;
                    maze[previousPoint.Column, previousPoint.Row] = "#";
                }
                path.Push(pointA);
                maze[pointB.Column, pointB.Row] = "B";
                maze[pointA.Column, pointA.Row] = "A";
                
            }
        }

        
    }
}