namespace Shared
{
    /// <summary>
    /// Internal separation of actions within a specific behaviour
    /// </summary>
    public enum CharacterAction : byte
    {
        None,
        Moving,
        Jumping,
        Falling,
    }
}