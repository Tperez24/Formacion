using System;
using Demo.Projectile_Abstract_Factory;
using UnityEngine;

namespace Demo.Player.Player_Scripts.Player_Behaviour
{
    public static class SpellCreator
    {
        public enum SpellTypes
        {
            IceSpell,
            GroundSpell,
            None
        }
        
        private static IAbstractSpellFactory GetSpellType(SpellTypes spellType)
        {
            return spellType switch
            {
                SpellTypes.IceSpell => new RegularSpell(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static (IAbstractPointer pointer, IAbstractSpell spell) LaunchSpell(SpellTypes spellType)
        {
            var go = new GameObject("Spell").transform;
            
            var (pointer,pointerTransform) = GetSpellType(spellType).CreatePointer();
            var (spell,spellTransform) = GetSpellType(spellType).CreateSpell();
            
            pointerTransform.SetParent(go);
            spellTransform.SetParent(go);
            
            return (pointer,spell);
        }
    }
}