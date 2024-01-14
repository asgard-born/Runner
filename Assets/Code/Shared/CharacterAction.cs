namespace Shared
{
    /// <summary>
    /// Разделения действий внутри поведений для удобства работы во избежание многочисленных bool полей
    /// </summary>
    public enum CharacterAction : byte
    {
        None = 0,
        Running = 10,
        Jumping = 20,
        Falling = 30,
        Landing = 40,
    }
}