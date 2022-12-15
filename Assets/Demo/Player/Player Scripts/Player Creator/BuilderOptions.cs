namespace Demo.Player.Player_Scripts.Player_Creator
{
    public class BuilderOptions
    {
        private IPlayerBuilder _playerBuilderInterface;

        public IPlayerBuilder Builder
        {
            set => _playerBuilderInterface = value;
        }

        public void BuildNormalPlayer()
        {
            _playerBuilderInterface.GetPlayer();
            _playerBuilderInterface.AddPlayerBehaviour();
            _playerBuilderInterface.AddPlayerConfiguration();
            _playerBuilderInterface.Initialize();
        }
    }
}