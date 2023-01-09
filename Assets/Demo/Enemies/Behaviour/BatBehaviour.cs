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

        private void Awake()
        {
            TryGetComponent(out _ownCollider);
            TryGetComponent(out _agent);
            
            _initialPos = transform.position;
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
                case true when Vector3.Distance(transform.position,_initialPos) > 1:
                    StopSearch();
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
