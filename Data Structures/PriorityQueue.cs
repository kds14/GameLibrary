using System;
using System.Collections.Generic;

/// <summary>
/// A generic priority queue using a min or max heap.
/// </summary>
/// <typeparam name="T">The type of the items to be stored.</typeparam>
public class PriorityQueue<T>
{
    private class Node
    {
        public readonly T item;
        public readonly int priority;

        public Node(T item, int priority)
        {
            this.item = item;
            this.priority = priority;
        }
    }

    /// <summary>
    /// True for a max heap. False for a min heap.
    /// </summary>
    private readonly bool maxHeap;
    private readonly List<Node> heap;
    private int count = 0;

    public int Count { get { return count; } }

    public PriorityQueue(bool maxHeap = true, int capacity = 10)
    {
        heap = new List<Node>(10);
        this.maxHeap = maxHeap;
    }

    /// <summary>
    /// Adds a new item to the priority queue based on priority.
    /// This is an O(n) operation, where n is Count.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="priority">The priority of the item.</param>
    public void Add(T item, int priority)
    {
        Node node = new Node(item, priority);
        heap.Add(node);
        SiftUp(heap.Count - 1);
        count++;
    }

    /// <summary>
    /// Removes and returns the next item in the priority queue.
    /// This is an O(n) operation, where n is Count.
    /// </summary>
    /// <returns>The next item in the priority queue</returns>
    public T Remove()
    {
        int temp = 0;
        return Remove(out temp);
    }

    /// <summary>
    /// Removes and returns the next item in the priority queue.
    /// This is an O(n) operation, where n is Count.
    /// </summary>
    /// <param name="priority">An out parameter to receive the priority</param>
    /// <returns>The next item in the priority queue.</returns>
    public T Remove(out int priority)
    {
        if (count == 0)
        {
            throw new InvalidOperationException("The Priority Queue is empty");
        }

        int last = heap.Count - 1;
        Swap(0, last);
        Node result = heap[last];
        heap.RemoveAt(last);
        SiftDown(0);

        count--;
        priority = result.priority;
        return result.item;
    }

    public override string ToString()
    {
        string str = "";
        for (int i = 0; i < heap.Count; i++)
        {
            Node node = heap[i];
            str += $"({node.item},{node.priority})";
        }
        return str;
    }

    private void SiftUp(int index)
    {
        if (heap.Count <= 1) return;
        int ptr = index;
        while (ptr > 0)
        {
            int parent = Parent(ptr);
            if (Compare(ptr, parent))
            {
                Swap(ptr, parent);
            }
            ptr = parent;
        }
    }

    private void SiftDown(int index)
    {
        if (heap.Count <= 1) return;
        int ptr = index;
        int lastParent = Parent(heap.Count - 1);
        while (ptr <= lastParent)
        {
            int child0 = Child0(ptr);
            int child1 = Child1(ptr);
            int child = Compare(child0, child1) ? child0 : child1;
            if (Compare(child, ptr))
            {
                Swap(ptr, child);
            }
            ptr = child;
        }
    }

    private void Swap(int a, int b)
    {
        Node temp = heap[a];
        heap[a] = heap[b];
        heap[b] = temp;
    }

    private bool Compare(int a, int b)
    {
        if (a < heap.Count && b >= heap.Count)
        {
            return true;
        }
        else if (a >= heap.Count && b < heap.Count)
        {
            return false;
        }
        if (maxHeap)
        {
            return heap[a].priority > heap[b].priority;
        }
        else
        {
            return heap[a].priority < heap[b].priority;
        }
    }

    private int Parent(int index)
    {
        return (index - 1) / 2;
    }

    private int Child0(int index)
    {
        return 2 * index + 1;
    }

    private int Child1(int index)
    {
        return 2 * index + 2;
    }
}