﻿using UnityEngine;

namespace PatronesDeComportamiento.State
{
    public class MoveOnInterfaceState : MovementInputState
    {
        public override void Move()
        {
            Debug.Log("Moving on the interface");
        }
        public override void Transition() => _movementInput.TransitionTo(new MoveEnemyState());
    }
}