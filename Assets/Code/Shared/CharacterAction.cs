namespace Shared
{
    /// <summary>
    /// Разделения действий внутри поведений для удобства работы во избежание многочисленных bool полей
    /// </summary>
    public enum CharacterAction : byte
    {
        None = 0,
        Idle = 10,
        Moving = 20,
        Jumping = 30,
        Falling = 40,
        Lifting = 50,
        Landing = 60,
        Crash = 70,
        Finish = 250
    }
}