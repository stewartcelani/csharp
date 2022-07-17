namespace Memento;

/// <summary>
/// Command
/// </summary>
public interface ICommand
{
    bool CanExecute();
    void Execute();
    void Undo();
}