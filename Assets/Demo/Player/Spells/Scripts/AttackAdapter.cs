using Demo.Player.PlayerMediator;

namespace Demo.Player.Spells.Scripts
{
    public class AttackAdapter : PlayerComponent
    {
        protected AttackAdapter(IPlayerComponentsMediator mediator) : base(mediator) { }

        public enum AttackType
        {
            Spell,
            Normal
        }
        
        private IAttack GetAttack(AttackType attackType)
        {
            return attackType switch
            {
                AttackType.Normal => Mediator.GetReference(MediatorActionNames.AttackController()) as IAttack,
                AttackType.Spell => Mediator.GetReference(MediatorActionNames.SpellController()) as IAttack,
                _ => null
            };
        }
    }
}