﻿using AmeisenFSM.Interfaces;
using static AmeisenFSM.Objects.Delegates;

namespace AmeisenFSM.Actions
{
    public class ActionIdle : IAction
    {
        public Start StartAction { get { return Start; } }
        public DoThings StartDoThings { get { return DoThings; } }
        public Exit StartExit { get { return Stop; } }

        // Idle DoThings code here
        public void DoThings() { }

        // Idle Start code here
        public void Start() { }

        // Idle Stop code here
        public void Stop() { }
    }
}