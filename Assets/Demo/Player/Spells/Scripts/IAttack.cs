using Demo.ProjectileComposite;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demo.Player.Spells.Scripts
{
    public interface IAttack
    {
        public void Charge(object sender, InputAction.CallbackContext callbackContext);
        public void Launch(object sender, InputAction.CallbackContext callbackContext);
        public void Cancel(object sender, InputAction.CallbackContext callbackContext);
    }
}