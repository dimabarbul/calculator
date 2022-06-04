namespace Calculator.Collections;

public class MyStack<T>
{
    private const int DefaultCapacity = 10;

    private T[] items;

    private int size = 0;
    private int capacity;

    public int Count => this.size;

    public MyStack(int capacity = DefaultCapacity)
    {
        if (capacity < 1)
        {
            throw new ArgumentException("Capacity cannot be less than 1.", nameof(capacity));
        }

        this.capacity = capacity;
        this.items = new T[capacity];
    }

    public void Push(T item)
    {
        this.GrowIfNeeded();
        this.items[this.size++] = item;
    }

    public T Pop()
    {
        if (this.size == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        return this.items[--this.size];
    }

    public T Peek()
    {
        if (this.size == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        return this.items[this.size - 1];
    }

    public ArraySegment<T> Pop(int count)
    {
        if (count < 1)
        {
            throw new ArgumentException("Count cannot be less than 1.", nameof(count));
        }

        if (this.size < count)
        {
            throw new InvalidOperationException($"Stack contains {this.size} elements, but requested to return {count} elements.");
        }

        ArraySegment<T> result = new(this.items, this.size - count, count);
        this.size -= count;

        return result;
    }

    private void GrowIfNeeded()
    {
        if (this.size == this.capacity)
        {
            this.Grow();
        }
    }

    private void Grow()
    {
        int newSize = this.capacity * 2;
        Array.Resize(ref this.items, newSize);
    }
}