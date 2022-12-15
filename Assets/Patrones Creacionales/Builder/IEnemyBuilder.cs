using Patrones_Creacionales.Builder;

namespace Builder
{
    //Especifica los elementos posibles a construir entre los enemigos
    public interface IEnemyBuilder
    {
        void BuildHead();
        void BuildBody();
        void BuildLegs();
        void BuildWeapon();

        EnemyBuilded GetEnemy();
    }
}
