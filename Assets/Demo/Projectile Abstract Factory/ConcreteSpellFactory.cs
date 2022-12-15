using System.IO;
using Demo.Paths;
using Unity.VisualScripting;
using UnityEngine;

namespace Demo.Projectile_Abstract_Factory
{
    public class IcedStalagmiteSpell : IAbstractSpellFactory
    {
        private GameObject _spellParent;

        public IcedStalagmiteSpell()
        {
            _spellParent = new GameObject("Ice Spell");
        }
       
        public IAbstractPointer CreatePointer()
        {
            var pointer = Object.Instantiate(Resources.Load<GameObject>(PrefabsPath.IcePointerPath()), _spellParent.transform, true);

            return pointer.AddComponent<IcePointers>();
        }

        public IAbstractSpell CreateSpell()
        {
            var spell = Object.Instantiate(Resources.Load<GameObject>(PrefabsPath.IceSpellPath()), _spellParent.transform, true);

            return spell.AddComponent<IceSpells>();
        }
    }
    
    public class GroundedStalagmiteSpell : IAbstractSpellFactory
    {
        private GameObject _spellParent;

        public GroundedStalagmiteSpell()
        {
            _spellParent = new GameObject("Ground Spell");
        }

        public IAbstractPointer CreatePointer()
        {
            var pointer = Object.Instantiate(Resources.Load<GameObject>(PrefabsPath.GroundPointerPath()), _spellParent.transform, true);

            return pointer.AddComponent<IcePointers>();
        }

        public IAbstractSpell CreateSpell()
        {
            var spell = Object.Instantiate(Resources.Load<GameObject>(PrefabsPath.GroundSpellPath()), _spellParent.transform, true);

            return spell.AddComponent<IceSpells>();
        }
    }
}