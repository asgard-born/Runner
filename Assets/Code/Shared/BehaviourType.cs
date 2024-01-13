namespace Shared
{
    /// <summary>
    /// Служит для необходимости группировать взаимоисключаемые типы поведения
    /// </summary>
    public enum BehaviourType: byte
    {
        None = 0,
        Moving = 10,
        Velocity = 20
    }
}