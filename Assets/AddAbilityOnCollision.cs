using System;
using Demo.Player.Spells.Scripts;
using Demo.ProjectileComposite;
using UnityEngine;

public class AddAbilityOnCollision : MonoBehaviour
{
    public SpellCreator.SpellTypes spellToAdd;
    private WeaponsTree GetSpellType(SpellCreator.SpellTypes type)
    {
        return type switch
        {
            SpellCreator.SpellTypes.GroundSpell => new GroundSpellLeaf(null),
            SpellCreator.SpellTypes.IceSpell => new IceSpellLeaf(null),
            SpellCreator.SpellTypes.None => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        var weaponsComposite = col.gameObject.GetComponentInChildren<PlayerWeaponsComposite>();
        weaponsComposite.RemoveAllLeafsToSpellBranch(AttackAdapter.AttackType.Spell);
        weaponsComposite.AddLeafToSpellBranch(GetSpellType(spellToAdd));
        Destroy(gameObject);
    }
}
