using System.Collections.Generic;
using Demo.Paths;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Player.Spells.Scripts;
using UnityEngine;

namespace Demo.ProjectileComposite
{
    public class GroundSpellLeaf : WeaponsTree
    {
        public GroundSpellLeaf(List<WeaponsTree> child) : base(child)
        {
        }

        public override SpellCreator.SpellTypes GetSpellType(AttackAdapter.AttackType type) => SpellCreator.SpellTypes.GroundSpell;
        public override AnimatorOverrideController Animator(SpellCreator.SpellTypes type) => 
            Resources.Load<AnimatorOverrideController>(AnimatorOverrideControllersPaths.GroundSpell());
        public override SpellCreator.SpellTypes SpellType() => SpellCreator.SpellTypes.GroundSpell;
        
        public override AttackAdapter.AttackType AttackType() => AttackAdapter.AttackType.Spell;
    }
}