namespace Shared
{
    /// <summary>
    /// Служит для необходимости маппинга типа поведения на его имя,
    /// Для работы со взаимоисключаемыми типами поведения (Fly/Run, Fast/Slow)
    /// </summary>
    public enum BehaviourKey: byte
    {
        None = 0,
        Moving = 10,
        Velocity = 20
    }
}