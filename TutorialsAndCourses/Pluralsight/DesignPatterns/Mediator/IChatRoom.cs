namespace Mediator;

/// <summary>
/// Mediator
/// </summary>
public interface IChatRoom
{ 
    void Register(TeamMember teamMember);
    void Send(string from, string message);
    void Send(string from, string to, string message);
    void SendTo<T>(string from, string message) where T : TeamMember;
}