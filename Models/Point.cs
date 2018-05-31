using System;

/// <summary>
/// A point in integer x,y coordinates.
/// </summary>
public struct Point : IEquatable<Point>
{
    public readonly int x;
    public readonly int y;

    public Point(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;
    }

    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }

    public static Point operator -(Point a, Point b)
    {
        return new Point(a.x - b.x, a.y - b.y);
    }

    public static bool operator ==(Point a, Point b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Point a, Point b)
    {
        return !a.Equals(b);
    }

    public override string ToString()
    {
        return $"({x},{y})";
    }

    public override bool Equals(object obj)
    {
        return obj is Point && this == (Point)obj;
    }

    public bool Equals(Point point)
    {
        return point.x == x && point.y == y;
    }

    public override int GetHashCode()
    {
        int hashCode = 1502939027;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        return hashCode;
    }
}
