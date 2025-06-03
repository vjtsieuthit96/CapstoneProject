using UnityEngine;
using System.Collections;

namespace Invector
{    
    [vClassHeader("Destroy GameObject", openClose = false)]
    public class vDestroyGameObject : vMonoBehaviour
    {
        public float delay;
        public UnityEngine.Events.UnityEvent onDestroy;
        public float Damage;
        IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            onDestroy.Invoke();

            Destroy(gameObject);
        }

        #region Hàm TakeDamage enemy
        private void OnTriggerEnter(Collider other)
        {
           if(other.CompareTag("Enemy"))
            {
                var EnemyHealth = other.GetComponent<MonsterStats>();
                var EnemyAni = other.GetComponent<MonsterAI>();
                EnemyHealth.TakeDamage(Damage);
                int Rate = Random.Range(0,100);
                if(Rate <= 25)
                {
                    EnemyAni.SetAnimatorParameter(MonsterAnimatorHash.takeHitHash, null);
                }

            }
        }

        #endregion
    }
}