using System.Runtime.InteropServices.JavaScript;

namespace PathFinder;

using System;
using System.Collections.Generic;

public class MapPrinter
{
    public void Print(string[,] maze) //add new argument  List<Point> path
    {
        //declaring Startpoint/EndPoint;
        Point startPoint = new Point(9, 0);
        Point endPoint = new Point(5, 6);
        
        
        //dictionaries 
        var distances = new Dictionary<Point, int>();
        var origins = new Dictionary<Point, Point>();

        var time = 0;
        var range = 1; // declare 1 cell distance
        
        PrintTopLine();
        //PaintBFS(startPoint);
        AStar(startPoint);
        Printmaze();
        System.Console.WriteLine(string.Format("\nThe time of traveling: {0}",time));
        
        
        void Printmaze()
        {
            for (var row = 0; row < maze.GetLength(1); row++)
            {
                Console.Write($"{row}\t");
                for (var column = 0; column < maze.GetLength(0); column++)
                {
                    
                    //hideTraffic(column, row);
                    
                
                    Console.Write(maze[column, row]);
                }

                Console.WriteLine();
            }
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
        
        void hideTraffic(int column, int row)
        {
            if (char.IsDigit(Convert.ToChar(maze[column, row]))) //hide traffic 
            {
                maze[column, row] = " ";
            }   
        }
        
        //
        List<Point> GetNeighbours(Point point)
        {
            //create list to store neighbours
            var neighbours = new List<Point>();
                
            var column = point.Column;
            var row = point.Row;
                
            //search neighbours
            TryAddWithOffset(-1,0, true);
            TryAddWithOffset(1,0, true);
            TryAddWithOffset(0,-1, true);
            TryAddWithOffset(0,1, true);
                
                
            //For neighbour search
            void TryAddWithOffset(int offsetX, int offsetY, bool checkWalls)
            {
                var newColumn = column + offsetX;
                var newRow = row + offsetY;
                    
                //check if the point is within maze
                if (newColumn >= 0 && newRow >= 0 && newColumn < maze.GetLength(0) && newRow < maze.GetLength(1))
                {
                    //check if the point is not wall
                    if (!checkWalls || maze[newColumn, newRow] == " " || char.IsDigit(Convert.ToChar(maze[newColumn, newRow])))
                    {
                        neighbours.Add(new Point(newColumn, newRow));
                    }
                }
            }
                
            //returns list of neighbours
            return neighbours;
        }

        void AStar(Point point)
        {
            //dictionaries
            var aStarValues = new Dictionary<Point, int>();
            var usedPoints = new Dictionary<Point, int>();
            
            //current point
            var next = point;
            usedPoints[next] = 0;
            while ((next.Column != endPoint.Column) || (next.Row != endPoint.Row))
            {
                if (aStarValues.Count != 0) //checks if dictionary is empty
                {
                    var minValue = aStarValues.Values.Min();
                    next = aStarValues.FirstOrDefault(x => x.Value == minValue).Key;
                    aStarValues.Remove(next); // to be sure that it would not take that point as min value many times
                    usedPoints[next] = minValue;
                }
                
                foreach (var neighbour in GetNeighbours(next))
                {
                    //checks if we already took this point
                    if (usedPoints.ContainsKey(neighbour))
                    {
                        continue;
                    }
                    // Set the value to traffic number 
                    //to make without traffic change to 1
                    var n = 1;  //int.Parse(maze[neighbour.Column, neighbour.Row]);


                    //Check if that neighbour not in the Distances / compare values
                    if (!aStarValues.ContainsKey(neighbour))
                    {
                        AstarValue(neighbour, n);
                    }
                }
            }
            FindPathA(endPoint, startPoint);
            void FindPathA(Point pointB, Point pointA)
            {   
                //stack to store final path
                var path = new Stack<Point>();
                
                
                path.Push(pointB); // endPoint
                
                var currentPoint = pointB;
                usedPoints.Remove(pointB);
                //prints all checked ppoints
                foreach (var (key, value) in usedPoints)
                {
                    if (maze[key.Column, key.Row] == " ")
                    {
                        maze[key.Column, key.Row] = "1";
                    }
                }
                
                //check if we are one point before start, to not paint letter A
                while ((Math.Abs(pointA.Column - currentPoint.Column) + Math.Abs(pointA.Row - currentPoint.Row)) != 1)
                {   
                    //dictionary to store neighbours of point (max 4 p)
                    var neighboursDictionary = new Dictionary<Point, int>();

                    //add to neighboursDictionary neighbour and it's value
                    foreach (var neighbour in GetNeighbours(currentPoint))
                    {
                        if (usedPoints.ContainsKey(neighbour)){
                            
                            //adding neighbours to dict, which we used as path
                            neighboursDictionary.Add(neighbour, usedPoints[neighbour]);
                            
                            //remove to exept using one point many times
                            usedPoints.Remove(neighbour);
                        }
                    }
                    
                    var previousValue = neighboursDictionary.Values.Min();
                    var previousPoint = neighboursDictionary.FirstOrDefault(x => x.Value == previousValue).Key;

                    //time += previousValue;
                    
                    //add point to final path stack
                    
                    path.Push(previousPoint);
                    //set the point to current 
                    currentPoint = previousPoint;
                    
                    //set the path into maze
                    maze[previousPoint.Column, previousPoint.Row] = "#";
                    
                }
                
                path.Push(pointA);
                
                
                
                //set the point into maze
                maze[pointB.Column, pointB.Row] = "B";
                maze[pointA.Column, pointA.Row] = "A";
                
            }

            void AstarValue(Point point, int n)
            {
                double valB = Double.Sqrt(Math.Abs(endPoint.Column - point.Column)^2 + Math.Abs(endPoint.Row - point.Row)^2);
                double valA = Double.Sqrt(Math.Abs(startPoint.Column - point.Column)^2 + Math.Abs(startPoint.Row - point.Row)^2);
                double valTotal = valA + valB;
                
                int val = Math.Abs(endPoint.Column - point.Column) + Math.Abs(endPoint.Row - point.Row) + Math.Abs(startPoint.Column - point.Column) + Math.Abs(startPoint.Row - point.Row);
                aStarValues[point] = val;
            } 
        }
        
        void PaintBFS(Point point)
        {
            // declare
            var queue = new Queue<Point>();
            queue.Enqueue(point);
            
            
            Visit(point, point, 0);

            
            while (queue.Count > 0)
            {
                var next = queue.Dequeue();

                foreach (var neighbour in GetNeighbours(next))
                {
                    
                    // Set the value to traffic number 
                    //to make without traffic change to 1
                    var n = int.Parse(maze[neighbour.Column, neighbour.Row]);

                    
                    //Check if that neighbour not in the Distances / compare values
                    if (!distances.ContainsKey(neighbour) || (distances[neighbour] > (distances[next] + n))) 
                    {
                        //if Decstra
                        Visit(neighbour, next, n);
                        queue.Enqueue(neighbour);
                    }
                }
                
            }
            
            //Throw Exception if the start point in the wall or there is no path
            if (distances.ContainsKey(endPoint))
            {
                FindPath(endPoint, startPoint);
            }
            else
            {
                throw new Exception("No way from start point to end point");
            }
            
            //Get neighbours was here

            void Visit(Point point, Point previousPoint, int n)
            {
            
                var nPrevious = 0; //default value
                var speed = 60 - 6 * (n - 1); //if need to use speed
                
                if (distances.ContainsKey(previousPoint))
                {
                    nPrevious = distances[previousPoint]; // set number to value of previous point
                }

                if (!distances.ContainsKey(point))
                {
                    distances.Add(point, nPrevious + n); // add point to dictionary Distances, setting the value 
                                                      // to previous + it's value taken from traffic(n)
                    
                    origins.Add(point, previousPoint); // add point to dictionary Origins, to find later previous element from selected
                }
                else if (distances[point] > nPrevious + n) // if value of point is gather then could be, change the value to less one
                {
                    //change values corespondent 
                    distances[point] = nPrevious + n; 
                    origins[point] = previousPoint;
                }
                
                //to see distances on screen
                //maze[point.Column, point.Row] = string.Format("{0}", number);
            
            }
            
            void FindPath(Point pointB, Point pointA)
            {   
                //stack to store final path
                var path = new Stack<Point>();
                
                
                path.Push(pointB); // endPoint
                
                var currentPoint = pointB;
                
                //check if we are one point before start, to not paint letter A
                while (distances[currentPoint] > 1)
                {   
                    //dictionary to store neighbours of point (max 4 p)
                    var neighboursDictionary = new Dictionary<Point, int>();

                    //add to neighboursDictionary neighbour and it's value
                    foreach (var neighbour in GetNeighbours(currentPoint))
                    {
                        neighboursDictionary.Add(neighbour, distances[neighbour]);
                    }
                    
                    var previousValue = neighboursDictionary.Values.Min();
                    var previousPoint = neighboursDictionary.FirstOrDefault(x => x.Value == previousValue).Key;

                    time += previousValue;
                    //add point to final path stack
                    
                    path.Push(previousPoint);
                    //set the point to current 
                    currentPoint = previousPoint;
                    
                    //set the path into maze
                    maze[previousPoint.Column, previousPoint.Row] = "#";
                    
                }
                
                path.Push(pointA);
                
                //set the point into maze
                maze[pointB.Column, pointB.Row] = "B";
                maze[pointA.Column, pointA.Row] = "A";
                
            }
        }
    }
}