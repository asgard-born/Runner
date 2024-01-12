namespace Shared
{
    /// <summary>
    /// Новые виды поведения регистрируются здесь
    /// </summary>
    public enum BehaviourName : byte
    {
        None = 0,
        Run = 10,
        Fly = 20,
        Slow = 30,
        Fast = 40,
    }
}