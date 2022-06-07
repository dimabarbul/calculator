using Calculator.Collections;

namespace Calculator.Collections.Tests;

public class MyStackTests
{
    [Fact]
    public void CanPushBeyondCapacity()
    {
        MyStack<int> stack = new(1);

        stack.Push(1);
        stack.Push(2);

        Assert.Equal(2, stack.Count);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void CannotCreateWithWrongCapacity(int capacity)
    {
        Assert.Throws<ArgumentException>(() => new MyStack<int>(capacity));
    }

    [Fact]
    public void CanPop()
    {
        MyStack<int> stack = new();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        int popped = stack.Pop();

        Assert.Equal(3, popped);
        Assert.Equal(2, stack.Count);
    }

    [Fact]
    public void CannotPopFromEmptyStack()
    {
        MyStack<int> stack = new();

        Assert.Throws<InvalidOperationException>(() => stack.Pop());
    }

    [Fact]
    public void CanPopArray()
    {
        MyStack<int> stack = new();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        ArraySegment<int> popped = stack.Pop(2);

        Assert.Equal(2, popped.Count);
        Assert.Equal(popped, new[] { 2, 3 });
        Assert.Equal(1, stack.Count);
    }

    [Fact]
    public void CanPopArrayWithSigleElement()
    {
        MyStack<int> stack = new();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        ArraySegment<int> popped = stack.Pop(1);

        Assert.Single(popped);
        Assert.Equal(popped, new[] { 3 });
        Assert.Equal(2, stack.Count);
    }

    [Fact]
    public void CannotPopArrayFromEmptyStack()
    {
        MyStack<int> stack = new();

        Assert.Throws<InvalidOperationException>(() => stack.Pop(1));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void CannotPopArrayWithInvalidCount(int count)
    {
        MyStack<int> stack = new();

        Assert.Throws<ArgumentException>(() => stack.Pop(count));
    }

    [Fact]
    public void CanPeek()
    {
        MyStack<int> stack = new();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        int peeked = stack.Peek();

        Assert.Equal(3, peeked);
        Assert.Equal(3, stack.Count);
    }

    [Fact]
    public void CannotPeekFromEmptyStack()
    {
        MyStack<int> stack = new();

        Assert.Throws<InvalidOperationException>(() => stack.Peek());
    }
}