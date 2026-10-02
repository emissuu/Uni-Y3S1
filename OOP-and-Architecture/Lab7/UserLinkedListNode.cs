namespace Lab7;

public sealed class UserLinkedListNode<TNode>
{
    internal UserLinkedList<TNode>? list;
    internal UserLinkedListNode<TNode>? next;
    internal UserLinkedListNode<TNode>? prev;
    private TNode data;
        
        
    public UserLinkedListNode(TNode value)
    {
        data = value;
    }

    public UserLinkedListNode(UserLinkedList<TNode> list, TNode value)
    {
        this.list = list;
        data = value;
    }

    public TNode Value
    {
        get => data;
        set => data = value;
    }
    public UserLinkedList<TNode> List => list;
    public UserLinkedListNode<TNode> Next => next;
    public UserLinkedListNode<TNode> Prev => prev;
}