﻿using unk.Util;
using System;
using Unity.VisualScripting;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;

namespace unk.Cheats.Core
{
    public abstract class ToggleCheat : Cheat
    {
        public bool Enabled = false;

        public virtual void OnGui() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }

    }
}
