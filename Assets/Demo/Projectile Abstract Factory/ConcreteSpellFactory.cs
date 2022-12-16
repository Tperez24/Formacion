using Demo.Paths;
using UnityEngine;

namespace Demo.Projectile_Abstract_Factory
{
    public class RegularSpell : IAbstractSpellFactory
    {
        public (IAbstractPointer, Transform) CreatePointer()
        {
            var pointer = Object.Instantiate(Resources.Load<GameObject>(PrefabsPath.IcePointerPath()));

            return (pointer.AddComponent<Pointer>(),pointer.transform);
        }

        public (IAbstractSpell, Transform) CreateSpell()
        {
            var spell = Object.Instantiate(Resources.Load<GameObject>(PrefabsPath.IceSpellPath()));

            return (spell.AddComponent<Spell>(),spell.transform);
        }
    }
    
}