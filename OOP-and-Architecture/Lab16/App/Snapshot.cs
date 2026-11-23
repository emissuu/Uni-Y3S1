namespace Lab16.App;

public record Snapshot
{
    private readonly TextEditor _editor;
    private readonly string _text;
    private readonly TimeOnly _createdAt;
    
    public Snapshot(TextEditor editor, string text)
    {
        _editor = editor;
        _text = text;
        _createdAt = TimeOnly.FromDateTime(DateTime.Now);
    }

    public string GetText()
    {
        return _text;
    }

    public TimeOnly GetTime()
    {
        return _createdAt;
    }
}