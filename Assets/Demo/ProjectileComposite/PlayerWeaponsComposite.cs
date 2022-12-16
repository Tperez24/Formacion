using System.Collections.Generic;
using Demo.Player.Player_Scripts.Player_Behaviour;
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
            _weaponsTree = new WeaponsBranch(new List<WeaponsTree>(){_spellBranch});
        }

        public void AddLeafToSpellBranch(WeaponsTree leaf) => _spellBranch.Add(leaf);
        public void RemoveLeafToSpellBranch(WeaponsTree leaf) => _spellBranch.Remove(leaf);

        public AnimatorOverrideController GetAoc(SpellCreator.SpellTypes types) => _weaponsTree.Animator(types);
        public SpellCreator.SpellTypes GetSpellType() => _weaponsTree.GetSpellType(AttackAdapter.AttackType.Spell);
    }
}