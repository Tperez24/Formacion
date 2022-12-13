using UnityEngine;

namespace Factory_Method
{
    public class NpcInfoNpcCreator : NpcCreator
    {
        protected override INpcProduct FactoryMethod(GameObject gameObject)
        {
            return gameObject.AddComponent<InfoNpc>();
        }
    }
}
