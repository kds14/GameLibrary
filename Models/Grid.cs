using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents a 2D grid of tiles
/// </summary>
public class Grid
{
    /// <summary>
    /// Data class representing a tile with an occupant and movement cost.
    /// </summary>
    private class Tile
    {
        public IGridObject occupant = null;
        public int movementCost = 1;
    }

    private readonly Tile[,] grid;

    public int Width {  get { return grid.GetLength(0); } }
    public int Height {  get { return grid.GetLength(1); } }
    /// <summary>
    /// The point of the grid with the greatest x and y value.
    /// </summary>
    public Point MaxPoint { get { return new Point(Width - 1, Height - 1);  } }

    public Grid(int width, int height)
    {
        grid = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j] = new Tile();
            }
        }
    }

    public void SetMovementCost(Point point, int movementCost)
    {
        grid[point.x, point.y].movementCost = movementCost;
    }

    public int GetMovementCost(Point point)
    {
        return grid[point.x, point.y].movementCost;
    }

    public bool IsEmpty(Point point)
    {
        return grid[point.x, point.y].occupant == null;
    }

    public IGridObject GetAtPosition(Point point)
    {
        return grid[point.x, point.y].occupant;
    }

    public void SetAtPosition(Point point, IGridObject value)
    {
        grid[point.x, point.y].occupant = value;
    }

    /// <summary>
    /// Removes occupant from source tile to destination tile. Does
    /// nothingif there is no occupant in the source tile.
    /// </summary>
    /// <param name="src">source point</param>
    /// <param name="dest">destination point</param>
    public void MoveTo(Point src, Point dest)
    {
        if (grid[src.x, src.y].occupant != null)
        {
            SetAtPosition(dest, GetAtPosition(src));
            grid[src.x, src.y].occupant = null;
        }
    }

    /// <summary>
    /// A list of neighbor points to the given point within the grid.
    /// </summary>
    /// <param name="point">The point to find neighbors of.</param>
    /// <returns>A list of neighbor points</returns>
    public List<Point> Neighbors(Point point)
    {
        List<Point> points = new List<Point>();
        if (point.x - 1 >= 0)
        {
            points.Add(new Point(point.x - 1, point.y));
        }
        if (point.x + 1 < Width)
        {
            points.Add(new Point(point.x + 1, point.y));
        }
        if (point.y - 1 >= 0)
        {
            points.Add(new Point(point.x, point.y - 1));
        }
        if (point.y + 1 < Height)
        {
            points.Add(new Point(point.x, point.y + 1));
        }
        return points;
    }
}

