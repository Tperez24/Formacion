using UnityEngine;

namespace Demo.Player.PlayerMediator
{
    public class PlayerComponent : MonoBehaviour
    {
        protected IPlayerComponentsMediator Mediator;
        protected PlayerComponent(IPlayerComponentsMediator mediator) => Mediator = mediator;
        public void SetMediator(IPlayerComponentsMediator mediator) => Mediator = mediator;
    }
}