using Demo.ProjectileComposite;
using UnityEngine;

namespace Demo.Player.Spells.Scripts
{
    public interface IAttack
    {
        public void Charge();
        public void Launch();
        public void Cancel();
    }
}