using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Demo.Input_Adapter
{
    public interface IInput
    {
        public List<InputAction> GetInput();
    }

    public class XboxInputAdapter : IInput
    {
        public List<InputAction> GetInput()
        {
            var master = new Master();
            master.Enable();
            
            return new List<InputAction>
            { master.PlayerInputXbox.Dash, master.PlayerInputXbox.Movement, master.PlayerInputXbox.SwordAttack };
        }
    }

    public class KeyboardInputAdapter : IInput
    { 
        public List<InputAction> GetInput()
        {
            var master = new Master();
            master.Enable();
            
            return new List<InputAction>
            {
                master.PlayerInputKeyboard.Dash, master.PlayerInputKeyboard.Movement,
                master.PlayerInputKeyboard.SwordAttack
            };
        }
    }
}
