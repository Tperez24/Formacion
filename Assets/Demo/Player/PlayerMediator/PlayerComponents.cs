using UnityEngine;

namespace Demo.Player.PlayerMediator
{
    public class PlayerComponents : MonoBehaviour
    {
        protected IPlayerComponentsMediator Mediator;
        protected PlayerComponents(IPlayerComponentsMediator mediator) => Mediator = mediator;
        public void SetMediator(IPlayerComponentsMediator mediator) => Mediator = mediator;
    }
}