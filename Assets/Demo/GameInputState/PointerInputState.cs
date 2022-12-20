using UnityEngine;

namespace Demo.GameInputState
{
    public class PointerInputState : InputState
    {
        public override void Move()
        {
            throw new System.NotImplementedException();
        }

        public override void PressAttack()
        {
            Debug.LogWarning("Can't press attack button while moving the pointer");
        }

        public override void ChargeSpecialAttack()
        {
            Debug.LogWarning("Can't press special attack button while moving the pointer");
        }

        public override void LaunchSpecialAttack()
        {
            Debug.LogWarning("Can't launch the attack while moving the pointer");
        }

        public override void CancelSpecialAttack()
        {
            Debug.LogWarning("Can't cancel the attack while moving the pointer");
        }

        public override void TransitionTo(InputState state) => InputContext.TransitionTo(state);
    }
}