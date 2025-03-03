﻿using UnityEngine;

namespace Unk.Cheats.Core
{
    internal abstract class ExecutableCheat : Cheat
    {
        public ExecutableCheat() { }
        public ExecutableCheat(KeyCode defaultKeybind) : base(defaultKeybind) { }
        public ExecutableCheat(KeyCode defaultKeybind, bool hidden) : base(defaultKeybind, hidden) { }
        public abstract void Execute();
    }
}
