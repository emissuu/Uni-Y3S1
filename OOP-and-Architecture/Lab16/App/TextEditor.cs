namespace Lab16.App;

public class TextEditor
{
    private string _text = "Hello, World!";
    
    public Snapshot CreateSnapshot()
    {
        return new Snapshot(this, _text);
    }
    
    public void Restore(Snapshot snapshot)
    {
        _text = snapshot.GetText();
    }
    
    public string GetText()
    {
        return _text;
    }
    
    public void SetText(string text)
    {
        _text = text;
    }
}