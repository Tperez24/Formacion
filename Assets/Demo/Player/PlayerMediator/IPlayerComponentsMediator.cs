using UnityEngine;
using UnityEngine.Events;

namespace Demo.Player.PlayerMediator
{
    public interface IPlayerComponentsMediator
    {
        void Notify(object sender, string ev);
        object GetReference(string ev);
        void SubscribeTo(string subscribeFrom,UnityAction<Vector2> ev,bool subscribe);
    }
}