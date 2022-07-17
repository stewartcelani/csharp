namespace Composite;

public class Directory : FileSystemItem
{
    private List<FileSystemItem> _fileSystemItems = new();
    private long _size;
    
    public Directory(string name, long size) : base(name)
    {
        _size = size;
    }

    public void Add(FileSystemItem itemToAdd)
    {
        _fileSystemItems.Add(itemToAdd);
    }

    public void Remove(FileSystemItem itemToRemove)
    {
        _fileSystemItems.Remove(itemToRemove);
    }

    public override long GetSize()
    {
        var treeSize = _size;
        foreach (var fileSystemItem in _fileSystemItems)
        {
            treeSize += fileSystemItem.GetSize();
        }

        return treeSize;
    }

    /* One-liner way to do it with Linq
    public override long GetSize() => _size + _fileSystemItems.Sum(fileSystemItem => fileSystemItem.GetSize());
    */

}