using System.Collections.Generic;
using Demo.Player.Spells.Scripts;
using UnityEngine;

namespace Demo.ProjectileComposite
{
    public class PlayerWeaponsComposite : MonoBehaviour
    {
        private WeaponsBranch _weaponsTree;
        private SpellBranch _spellBranch;
        private void Start()
        {
            _spellBranch = new SpellBranch(new List<WeaponsTree>());
            _weaponsTree = new WeaponsBranch(new List<WeaponsTree>{_spellBranch});
        }
        public void AddLeafToSpellBranch(WeaponsTree leaf) => _spellBranch.Add(leaf);
        public void RemoveAllLeafsToSpellBranch(AttackAdapter.AttackType attackType) => _spellBranch.Remove(attackType);
        public AnimatorOverrideController GetAoc(AttackAdapter.AttackType types) => _weaponsTree.Animator(types);
        public SpellCreator.SpellTypes GetSpellType() => _weaponsTree.GetSpellType(AttackAdapter.AttackType.Spell);
    }
}