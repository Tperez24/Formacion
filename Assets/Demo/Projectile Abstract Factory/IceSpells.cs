using System;
using UnityEngine;

namespace Demo.Projectile_Abstract_Factory
{
    public class IceSpells : MonoBehaviour,IAbstractSpell
    {
        public Action PlayAnimation()
        {
            throw new NotImplementedException();
        }

        public void Instantiate()
        {
            throw new NotImplementedException();
        }
    }
    public interface IAbstractSpell
    {
        Action PlayAnimation();
        void Instantiate();
    }
}