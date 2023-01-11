using ObjectManagement.Scripts;

namespace Demo.SaveManager
{
    public interface ISaver
    {
        void Save(GameDataWriter writer);

        void Load(GameDataReader reader);
    }
}