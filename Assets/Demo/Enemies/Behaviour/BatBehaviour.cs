using System.Collections.Generic;
using Demo.LevelsManager;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace Demo.Enemies.Behaviour
{
    public class BatBehaviour : MonoBehaviour,IDamageReceiver
    {
        private NavMeshAgent _agent;
        private Collider2D _ownCollider;
        private Vector3 _initialPos;
        
        public int damage = 1;
        public float radius;
        public Color gizmosColor;

        public void Initialize(Vector3 position)
        {
            _ownCollider = GetComponent<Collider2D>();
            _agent = GetComponent<NavMeshAgent>();
            
            _initialPos = position;
            transform.position = _initialPos;

            _ownCollider.enabled = true;
            _agent.enabled = true;
            
            _agent.updateRotation = false; 
            _agent.updateUpAxis = false;
        }
        
        private void Update()
        {
            var collision = Physics2D.OverlapCircle(transform.position, radius,LayerMask.GetMask(nameof(Player)));
            var colliderNull = ReferenceEquals(collision, null);
            switch (colliderNull)
            {
                case false:
                    _agent.SetDestination(collision.transform.position);
                    break;
                case true:
                    if(Vector3.Distance(transform.position,_initialPos) > 1) StopSearch();
                    break;
            }
        }

        private void StopSearch()
        {
            StopAllCoroutines();
            _agent.SetDestination(_initialPos);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;
            var player = col.gameObject.GetComponentInChildren<IDamageReceiver>();
            player.ReceiveDamage(damage);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawSphere(transform.position,radius);   
        }

        public void ReceiveDamage(int damageReceived)
        {
            var pas = Resources.Load<ParticleSystem>("ParticleSystem/PAS_Explosion");
            Instantiate(pas,transform.position,quaternion.identity);
            Destroy(gameObject);
        }
    }
}
