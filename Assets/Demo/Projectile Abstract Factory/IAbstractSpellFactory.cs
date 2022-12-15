namespace Demo.Projectile_Abstract_Factory
{
    public interface IAbstractSpellFactory
    {
        IAbstractPointer CreatePointer();

        IAbstractSpell CreateSpell();
    }
}