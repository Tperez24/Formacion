using UnityEngine;
using UnityEngine.Events;

namespace Demo.Player.PlayerMediator
{
    public interface IPlayerComponentsMediator
    {
        void Notify(object sender, string ev);
        object GetReference(string ev);
    }
}