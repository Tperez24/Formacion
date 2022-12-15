using System;
using UnityEngine;

namespace Demo.Projectile_Abstract_Factory
{
    public class IcePointers : MonoBehaviour,IAbstractPointer
    {
        public void ChangeColor(Color color)
        {
            var spriteRenderer = GetComponentInParent<SpriteRenderer>().color = color;
        }

        public void SetSpell(IAbstractSpell spell)
        {
            throw new NotImplementedException();
        }

        public string SpellType()
        {
            throw new NotImplementedException();
        }

        public Action MovePointer()
        {
            throw new NotImplementedException();
        }

        public Action InstanceSpell()
        {
            throw new NotImplementedException();
        }
    }
    
    public interface IAbstractPointer
    {
        void ChangeColor(Color color);
        void SetSpell(IAbstractSpell spell);
        string SpellType();
        Action MovePointer();
        Action InstanceSpell();
    }
}