using System;
using System.Linq;
using Demo.Player.Player_Scripts.Player_Behaviour;
using Demo.Player.Spells.Scripts;
using Demo.ProjectileComposite;
using UnityEngine;

public class AddAbilityOnCollision : MonoBehaviour
{
    public SpellCreator.SpellTypes spellTypes;
    private WeaponsTree GetSpellType(SpellCreator.SpellTypes type)
    {
        return type switch
        {
            SpellCreator.SpellTypes.GroundSpell => new GroundSpellLeaf(null),
            SpellCreator.SpellTypes.IceSpell => new IceSpellLeaf(null),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.GetComponentsInChildren<PlayerWeaponsComposite>().First().AddLeafToSpellBranch(GetSpellType(spellTypes));
    }
}
