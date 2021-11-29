namespace Heus.Core;
/// <summary>
/// This class can be used to provide an action when
/// Dispose method is called.
/// </summary>
public class DisposeAction : IDisposable
{
    private readonly Action _action;

    /// <summary>
    /// Creates a new <see cref="DisposeAction"/> object.
    /// </summary>
    /// <param name="action">Action to be executed when this object is disposed.</param>
    public DisposeAction(Action action)
    {
        Check.NotNull(action, nameof(action));

        _action = action;
    }

    public void Dispose()
    {
        _action();
    }
}

