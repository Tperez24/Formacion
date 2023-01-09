using System;
using UnityEngine;
using UnityEngine.AI;

namespace Demo.Enemies.Behaviour
{
    public class BatBehaviour : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Collider2D _ownCollider;
        public float radius;
        public Color gizmosColor;

        private void Start()
        {
            TryGetComponent(out _ownCollider);
        }

        private void Update()
        {
            var collision = Physics2D.OverlapCircle(transform.position, radius);
            if(collision.IsTouching(_ownCollider)) Debug.Log("Toca");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawSphere(transform.position,radius);   
        }
    }
}
