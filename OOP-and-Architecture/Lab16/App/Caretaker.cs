namespace Lab16.App;

public class Caretaker(TextEditor editor)
{
    private Stack<Snapshot> _backup = new();
    
    public void MakeBackup()
    {
        _backup.Push(editor.CreateSnapshot());
    }

    public void Undo()
    {
        if (_backup.Count == 0) return;
        
        var backup = _backup.Pop();
        
        editor.Restore(backup);
    }

    public IReadOnlyList<Snapshot> GetBackupLog()
    {
        return _backup.ToList();
    }
}