namespace Demo.Player.Player_Scripts.Player_Creator
{
    public interface IPlayerBuilder
    {
        public void AddPlayerConfiguration();
        public void AddPlayerBehaviour();
        public PlayerBuilder GetPlayer();
        public void Initialize();
    }
}