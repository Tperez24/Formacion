namespace Demo.Player.Player_Scripts.Player_Creator
{
    public interface IPlayerBuilder
    {
        public void AddPlayerConfiguration();
        public void AddPlayerMediator();
        public void AddPlayerAttackSystem();
        public void AddPlayerBehaviour();
        public PlayerBuilder GetPlayerBuilder();
        public void Initialize();
        void AddPlayerAbilityTree();
    }

    public interface IInputBuilder
    {
        public void AddInputController();
        public InputBuilder GetInputBuilder();
    }
}