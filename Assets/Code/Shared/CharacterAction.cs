﻿namespace Shared
{
    /// <summary>
    /// Разделения действий внутри поведений
    /// </summary>
    public enum CharacterAction : byte
    {
        None = 0,
        Running = 10,
        Jumping = 20,
        Falling = 30
    }
}