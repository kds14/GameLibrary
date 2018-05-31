using System;
using System.Collections.Generic;

/// <summary>
/// Utility class that contains various search algorithms for grids.
/// </summary>
public static class GridSearch
{
    /// <summary>
    /// Inner class that contains a distance and previous field to be used
    /// by FindPath and BuildRadius. Also, overrides Equals and GetHashCode
    /// so it can be used with a HashSet.
    /// </summary>
    private class PathPoint
    {
        public readonly Point point;
        public readonly int distance;
        public readonly PathPoint previous;

        public PathPoint(Point point, int distance, PathPoint previous = null)
        {
            this.point = point;
            this.distance = distance;
            this.previous = previous;
        }

        public override bool Equals(object obj)
        {
            return obj is PathPoint && (obj as PathPoint).point == point;
        }

        public override int GetHashCode()
        {
            return point.GetHashCode();
        }
    }

    /// <summary>
    /// A* pathfinding from a source point to a destination point. 
    /// Requires a Grid class that has public methods
    /// GetMovementCost(Point p), IsEmpty(Point p), and Neighbors(Point p)
    /// </summary>
    /// <param name="grid">The grid object that contains tile data</param>
    /// <param name="src">Source, the starting point</param>
    /// <param name="dest">Destination, the ending point</param>
    /// <returns>A list of points in the path, null if no path</returns>
    public static List<Point> FindPath(Grid grid, Point src, Point dest)
    {
        if (!grid.IsEmpty(dest)) return null;

        PriorityQueue<PathPoint> pq = new PriorityQueue<PathPoint>(false);
        pq.Add(new PathPoint(src, 0, null), 0);
        HashSet<PathPoint> visited = new HashSet<PathPoint>();

        PathPoint current = null;

        while (pq.Count > 0)
        {
            current = pq.Remove();
            if (current.point == dest)
            {
                return BuildPath(current);
            }

            foreach (Point n in grid.Neighbors(current.point))
            {
                int distance = current.distance + grid.GetMovementCost(n);
                PathPoint p = new PathPoint(n, distance, current);
                if (grid.IsEmpty(n) && visited.Add(p))
                {
                    pq.Add(p, p.distance + Heuristic(n, dest));
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Builds a path starting from the destination
    /// </summary>
    /// <param name="dest">The destination as a PathPoint</param>
    /// <returns>A list of points representing the path</returns>
    private static List<Point> BuildPath(PathPoint dest)
    {
        List<Point> path = new List<Point>();
        PathPoint ptr = dest;
        while (ptr != null)
        {
            path.Add(ptr.point);
            ptr = ptr.previous;
        }

        path.Reverse();

        return path;
    }

    /// <summary>
    /// Approximation of the distance from the source to destination
    /// that is used in A* pathfinding. This approximation will always
    /// be upper bounded by the real distance.
    /// </summary>
    /// <param name="src">Source, the starting point</param>
    /// <param name="dest">Destination, the ending point</param>
    /// <returns>An integer approximation of the distance</returns>
    private static int Heuristic(Point src, Point dest)
    {
        return Math.Abs(src.x - dest.x) + Math.Abs(src.x - dest.y);
    }

    /// <summary>
    /// Builds a radius around a specific point on the grid. This
    /// extends outwards for the distance in every possible
    /// direction using breadth-first search.
    /// </summary>
    /// <param name="grid">The grid object that contains tile data</param>
    /// <param name="src">Source, the starting point</param>
    /// <param name="dest">Destination, the ending point</param>
    /// <returns>A list of points in the radius</returns>
    public static List<Point> BuildRadius(Grid grid, Point src, int distance)
    {
        Queue<PathPoint> queue = new Queue<PathPoint>();
        queue.Enqueue(new PathPoint(src, 0));
        List<Point> visited = new List<Point>() { src };

        while (queue.Count > 0)
        {
            PathPoint current = queue.Dequeue();
            foreach (Point n in grid.Neighbors(current.point))
            {
                int dist = grid.GetMovementCost(n) + current.distance;
                if (dist <= distance && grid.IsEmpty(n) && !visited.Contains(n))
                {
                    visited.Add(n);
                    queue.Enqueue(new PathPoint(n, dist));
                }
            }
        }

        return visited;
    }
}
