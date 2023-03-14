namespace PathFinder;

public struct Point
{
    public int Column { get; set; }
    public int Row { get; set; }

    public int Value;

    public Point(int column, int row)
    {
        Column = column;
        Row = row;
    }
}