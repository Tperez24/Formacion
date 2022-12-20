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
        public void StartCharging(AttackType type) => GetAttack(type).Charge();
        public void LaunchAttack(AttackType type) => GetAttack(type).Launch();
        public void AttackCanceled(AttackType type) => GetAttack(type).Cancel();
    }
}