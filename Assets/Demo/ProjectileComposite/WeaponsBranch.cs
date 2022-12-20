using System.Collections.Generic;
using System.Linq;
using Demo.Player.Spells.Scripts;
using UnityEngine;

namespace Demo.ProjectileComposite
{
    public class WeaponsBranch : WeaponsTree
    {
        private readonly List<WeaponsTree> _children;
        
        public WeaponsBranch(List<WeaponsTree> child) : base(child) => _children = child;

        public override SpellCreator.SpellTypes GetSpellType(AttackAdapter.AttackType type) =>
            _children.Where(weapon => weapon.AttackType() == type).Select(weapon => weapon.GetSpellType(type))
                .FirstOrDefault();

        public override AnimatorOverrideController Animator(AttackAdapter.AttackType type) => 
            _children.Where(weapon => weapon.AttackType() == type).Select(weapon => weapon.Animator(type)).FirstOrDefault();

        public override void Add(WeaponsTree component) => _children.Add(component);

        public override void Remove(AttackAdapter.AttackType attackType)
        {
            var attackBranch = _children.Where(weapon => weapon.AttackType() == attackType).ToList();
            attackBranch.ForEach(attack => _children.Remove(attack));
        }
    }

    class SpellBranch : WeaponsTree
    {
        private readonly List<WeaponsTree> _children;
        public SpellBranch(List<WeaponsTree> children) : base(children) => _children = children;
        
        public override SpellCreator.SpellTypes GetSpellType(AttackAdapter.AttackType type)
        {
            return _children.Where(weapon => weapon.AttackType() == type).Select(weapon => weapon.GetSpellType(type))
                .FirstOrDefault();
        }

        public override AnimatorOverrideController Animator(AttackAdapter.AttackType type) => 
            _children.Where(weapon => weapon.AttackType() == type).Select(weapon => weapon.Animator(type)).FirstOrDefault();

        public override AttackAdapter.AttackType AttackType() => AttackAdapter.AttackType.Spell;

        public override void Add(WeaponsTree component) => _children.Add(component);

        public override void Remove(AttackAdapter.AttackType attackType)
        {
            var spellBranch = _children.Where(weapon => weapon.AttackType() == attackType).ToList();
            spellBranch.ForEach(spell => _children.Remove(spell));
        }
    }
}