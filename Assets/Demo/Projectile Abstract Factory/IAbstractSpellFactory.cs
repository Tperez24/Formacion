using UnityEngine;

namespace Demo.Projectile_Abstract_Factory
{
    public interface IAbstractSpellFactory
    {
        (IAbstractPointer pointer,Transform transform) CreatePointer();
        (IAbstractSpell spell,Transform transform) CreateSpell();
    }
}