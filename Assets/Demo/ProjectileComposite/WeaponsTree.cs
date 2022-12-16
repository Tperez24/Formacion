using System;
using System.Collections.Generic;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Player.Spells.Scripts;
using UnityEngine;

namespace Demo.ProjectileComposite
{
    public abstract class WeaponsTree
    {
        protected WeaponsTree(List<WeaponsTree> child) { }

        protected SpellCreator.SpellTypes Weapon;

        public abstract SpellCreator.SpellTypes GetSpellType(AttackAdapter.AttackType type);
        public abstract AnimatorOverrideController Animator(SpellCreator.SpellTypes type);
        
        public virtual AttackAdapter.AttackType AttackType()
        {
            throw new NotImplementedException();
        }
        public virtual SpellCreator.SpellTypes SpellType()
        {
            throw new NotImplementedException();
        }
        public virtual void Add(WeaponsTree weaponBranch)
        {
            throw new NotImplementedException();
        }
        public virtual void Remove(WeaponsTree weaponBranch)
        {
            throw new NotImplementedException();
        }
    }
}