using System.Collections;

namespace Lab7;

public class UserLinkedList<T> : IEnumerable
{
    public T Data;
    public UserLinkedList<T>? Head = null;

    public UserLinkedList(T value)
    {
        Data = value;
    }

    public void AddFirst(T value)
    {
        var oldData = Data;
        var oldHead = Head;

        Data = value;

        Head = new UserLinkedList<T>(oldData)
        {
            Head = oldHead
        };
    }

    public UserLinkedList<T> AddLast(T value)
    {
        if (Head is null)
        {
            Head = new UserLinkedList<T>(value);
        }
        else
        {
            Head.AddLast(value);
        }
        return this;
    }

    public UserLinkedList<T> Clear()
    {
        Head = null;
        return this;
    }

    public bool Contains(T value)
    {
        return Find(value) != null;
    }

    public UserLinkedList<T>? Find(T value)
    {
        if (EqualityComparer<T>.Default.Equals(Data, value))
            return this;
        if (Head is null)
            return null;
        
        return Head.Find(value);
    }

    public UserLinkedList<T>? FindLast(T value)
    {
        UserLinkedList<T>? current = this;
        UserLinkedList<T>? lastFound = null;
        while (current is not null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Data, value))
                lastFound = current;
            current = current.Head;
        }
        return lastFound;
    }

    public bool Remove(T value)
    {
        if (EqualityComparer<T>.Default.Equals(Data, value))
        {
            if (Head is null)
                return false;

            Data = Head.Data;
            Head = Head.Head;
            return true;
        }
        
        UserLinkedList<T>? current = this;
        while (current.Head is not null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Head.Data, value))
            {
                current.Head = current.Head.Head;
                return true;
            }
            
            current = current.Head;
        }

        return false;
    }

    public void RemoveFirst()
    {
        if (Head is null)
            throw new InvalidOperationException();
        Data = Head.Data;
        Head = Head.Head;
    }

    public void RemoveLast()
    {
        if (Head is null) return; // 1 2 3 4
        
        UserLinkedList<T> current = this;
        
        

        while (current.Head!.Head is not null)
        {
            if (current.Head is null) throw new InvalidOperationException();
            current = current.Head;
        }

        current.Head = null;
    }

    public IEnumerator GetEnumerator() => new Enumerator(this);

    public struct Enumerator(UserLinkedList<T> list) : IEnumerator<T>
    {
        private UserLinkedList<T>? _current = null;

        public bool MoveNext()
        {
            if (_current == null)
            {
                _current = list;
            }
            else 
                _current = _current.Head;
            
            return _current != null;
        }

        public void Reset()
        {
            _current = null;
        }

        public T Current {
            get {
                if (_current is null)
                    throw new InvalidOperationException();
                return _current.Data;
            }
        }

        T IEnumerator<T>.Current => Current;

        object? IEnumerator.Current => Current;

        public void Dispose() {}
    }

}