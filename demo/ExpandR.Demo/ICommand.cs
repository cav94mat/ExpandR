namespace ExpandR.DemoAPI
{
    public interface ICommand
    {
        string Syntax { get; }
        string Description { get; }
        int Call(string[] args);
    }
}
