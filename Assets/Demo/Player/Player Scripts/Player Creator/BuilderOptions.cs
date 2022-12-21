namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class BuilderOptions
    {
        public IInputBuilder InputBuilder { get; set; }

        public IPlayerBuilder Builder { get; set; }

        public void BuildNormalPlayer()
        {
            Builder.GetPlayerBuilder();
            Builder.AddPlayerMediator();
            Builder.AddPlayerBehaviour();
            Builder.AddPlayerAttackSystem();
            Builder.AddPlayerAbilityTree();
            Builder.AddPlayerConfiguration();
            Builder.Initialize();
        }

        public void BuildInputSystem()
        {
            InputBuilder.AddInputController();
        }
    }
}