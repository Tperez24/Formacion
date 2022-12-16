using Demo.ProjectileComposite;
using UnityEngine;

namespace Demo.Player.Spells
{
    public interface IAttack
    {
        public void SetAnimator(Animator animator);
        public void Charge();
        public void Launch();
        public void Cancel();
        void SetWeaponsComposite(PlayerWeaponsComposite playerWeaponsComposite);
    }
}