using System.Collections;

namespace Lab7;

public class UserLinkedList<T> : IEnumerable
{
    public UserLinkedListNode<T>? Head = null;
    public UserLinkedListNode<T>? Tail = null;

    public UserLinkedListNode<T> AddFirst(T value)
    {
        var node = new UserLinkedListNode<T>(this, value);
        node.next = Head;
        
        if (Head is not null)
        {
            Head.prev = node;
        }
        else
        {
            Tail = node;
        }
        
        Head = node;
        return node;
    }

    public UserLinkedListNode<T> AddLast(T value)
    {
        var node = new UserLinkedListNode<T>(this, value);
        node.prev = Tail;

        if (Tail is not null)
        {
            Tail.next = node;
        }
        else
        {
            Head = node;
        }
        
        Tail = node;
        return node;
    }

    public void Clear()
    {
        Head = null;
        Tail = null;
    }

    public bool Contains(T value)
    {
        return Find(value) != null;
    }

    public UserLinkedListNode<T>? Find(T value)
    {
        var current = Head;
        while (current is not null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Value, value))
                return current;
            current = current.Next;
        }
        return null;
    }

    public UserLinkedListNode<T>? FindLast(T value)
    {
        var current = Tail;
        while (current is not null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Value, value))
                return current;
            current = current.Prev;
        }
        return null;
    }

    public bool Remove(T value)
    {
        var node = Find(value);
        if (node is null) return false;

        if (node.next is null)
        {
            Tail = node.prev;
        }
        else
        {
            node.next.prev = node.prev;
        }

        if (node.prev is null)
        {
            Head = node.next;
        }
        else
        {
            node.prev.next = node.next;
        }
        
        node.next = null;
        node.prev = null;
        node.list = null;
        return true;
    }

    public void RemoveFirst()
    {
        if (Head is null)
            return;
        
        var node = Head;
        Head = node.next;
        if (Head is null)
        {
            Tail = null;
        }
        else
        {
            Head.prev = null;
        }
        node.next = null;
        node.list = null;
    }

    public void RemoveLast()
    {
        if (Tail is null)
            return;
        
        var node = Tail;
        Tail = node.prev;
        if (Tail is null)
        {
            Head = null;
        }
        else
        {
            Tail.next = null;
        }
        node.prev = null;
        node.list = null;
    }

    public IEnumerator GetEnumerator()
    {
        UserLinkedListNode<T>? current = Head;

        while (current is not null)
        {
            yield return current.Value;
            current = current.next;
        }
    }
}