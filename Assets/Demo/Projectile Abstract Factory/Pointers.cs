using System;
using UnityEngine;

namespace Demo.Projectile_Abstract_Factory
{
    public class Pointer : MonoBehaviour,IAbstractPointer
    {
        public string SpellType() => throw new NotImplementedException();
        public void MovePointer(Vector2 direction,float speed) => transform.Translate(direction * (Time.deltaTime * speed));
        public void Destroy() => Destroy(gameObject);
        public Vector2 GetPosition() => transform.position;
        public void Translate(Vector3 transformPosition) => transform.position = transformPosition;
    }
    
    public interface IAbstractPointer
    {
        string SpellType();
        void MovePointer(Vector2 direction,float speed = 2);
        void Destroy();
        Vector2 GetPosition();
        void Translate(Vector3 transformPosition);
    }
}