using System;
using Demo.Projectile_Abstract_Factory;

namespace Demo.Player.Player_Scripts.Player_Behaviour
{
    public static class SpellCreator
    {
        public enum SpellTypes
        {
            IceSpell,
            GroundSpell
        }
        
        private static IAbstractSpellFactory GetSpellType(SpellTypes spellType)
        {
            return spellType switch
            {
                SpellTypes.IceSpell => new IcedStalagmiteSpell(),
                SpellTypes.GroundSpell => new GroundedStalagmiteSpell(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static (IAbstractPointer pointer, IAbstractSpell spell) LaunchSpell(SpellTypes spellType) =>
            (GetSpellType(spellType).CreatePointer(), GetSpellType(spellType).CreateSpell());
    }
}