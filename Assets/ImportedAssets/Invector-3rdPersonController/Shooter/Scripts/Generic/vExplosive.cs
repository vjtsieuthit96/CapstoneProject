using System.Collections;
using UnityEngine;

namespace Invector
{
    using System.Collections.Generic;
    using Invector.vShooter;
    using vEventSystems;

    [vClassHeader("Explosive", openClose = false)]
    public class vExplosive : vMonoBehaviour
    {
        [System.Serializable]
        public class OnUpdateTime : UnityEngine.Events.UnityEvent<float> { }
        [System.Serializable]
        public class ColliderEvent : UnityEngine.Events.UnityEvent<Collider> { }
        public vDamage damage;
        [vHelpBox("Use this values To define the min and max damage based on range")]
        [Range(0, 1f)]
        public float damageOnMinRangeMultiplier = 1f;
        [Range(0, 1f)]
        public float damageOnMaxRangeMultiplier = 0f;
        [Tooltip("Assign this to set other damage sender")]
        public Transform overrideDamageSender;
        public float explosionForce;
        public float minExplosionRadius;
        public float maxExplosionRadius;
        public float upwardsModifier = 1;
        public ForceMode forceMode;
        public ExplosiveMethod method;
        public LayerMask applyDamageLayer, applyForceLayer;
        public float timeToExplode = 10f;
        public bool destroyAfterExplode = true;
        [Tooltip("convert to progress 0 to 1")]
        public bool normalizeTime;
        public bool showGizmos;
        public ParticleSystem explosionEffect;
        [SerializeField] private string ObjectKey;
        private Transform originalParent;
        public UnityEngine.Events.UnityEvent onInitTimer;
        public OnUpdateTime onUpdateTimer;
        public UnityEngine.Events.UnityEvent onExplode;
        public ColliderEvent onHit;
        private bool inTimer;
        protected List<GameObject> collidersReached;

        public float DetentionTime;
        public float ReductEnemySpeedPercent;
        public float ElectricDamagePercent;
        public float EletricDuration;
        public float PoisonDamagePercent;
        public float PoisonDuration;

        private float damageExplosive;

        void OnDrawGizmosSelected()
        {
            if (!showGizmos) return;
            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, minExplosionRadius);
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, maxExplosionRadius);
        }

        public void SetOverrideDamageSender(Transform target) => overrideDamageSender = target;

        public void SetOverDataSender(float DetentionTime, float ReductEnemySpeedPercent, 
            float EletricDamagePercent, float ElectricDuration, float PoisonDamagePercent, 
            float PoisonDuration, float damage)
        {
            this.DetentionTime = DetentionTime;
            this.ReductEnemySpeedPercent = ReductEnemySpeedPercent;
            this.ElectricDamagePercent = EletricDamagePercent;
            this.EletricDuration = ElectricDuration;
            this.PoisonDamagePercent = PoisonDamagePercent;
            this.PoisonDuration = PoisonDuration;
            this.damageExplosive = damage;
        }    

        public void SetDamage(vDamage damage)
        {
            this.damage = damage;
        }

        public enum ExplosiveMethod
        {
            collisionEnter,
            collisionEnterTimer,
            remote,
            timer,
            remoteTimer
        }

        protected virtual void Start()
        {
            collidersReached = new List<GameObject>();
            if (method == ExplosiveMethod.timer)
            {
                StartCoroutine(StartTimer());
            }
        }

        protected virtual IEnumerator StartTimer()
        {
            if (!inTimer)
            {
                onInitTimer.Invoke();
                inTimer = true;
                var startTime = Time.time;
                var time = 0f;
                while (time < timeToExplode)
                {
                    yield return new WaitForEndOfFrame();
                    time = Time.time - startTime;
                    onUpdateTimer.Invoke(normalizeTime ? (time / timeToExplode) : time);
                }
                if (gameObject)
                {
                    onUpdateTimer.Invoke(normalizeTime ? (1f) : timeToExplode);
                    Explode();
                }
            }
        }

        protected virtual IEnumerator DestroyBomb()
        {
            yield return new WaitForSeconds(4f);
            PoolManager.Instance.ReturnObject(ObjectKey, this);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (method == ExplosiveMethod.collisionEnter)
                Explode();
            else if (method == ExplosiveMethod.collisionEnterTimer)
                StartCoroutine(StartTimer());
        }

        public virtual void Explode()
        {
            onExplode.Invoke();
            var colliders = Physics.OverlapSphere(transform.position, maxExplosionRadius, applyDamageLayer);

            if (collidersReached == null)
            {
                collidersReached = new List<GameObject>();
            }

            for (int i = 0; i < colliders.Length; ++i)
            {
                if (colliders[i] != null && colliders[i].gameObject != null && !collidersReached.Contains(colliders[i].gameObject))
                {
                    collidersReached.Add(colliders[i].gameObject);
                    var _damage = new vDamage(damage);
                    _damage.sender = overrideDamageSender ? overrideDamageSender : transform;

                    _damage.hitPosition = colliders[i].ClosestPointOnBounds(transform.position);
                    _damage.receiver = colliders[i].transform;
                    var distance = Vector3.Distance(transform.position, _damage.hitPosition);
                    var damageValue = distance <= minExplosionRadius ? damageExplosive * damageOnMinRangeMultiplier : Mathf.Lerp(damageExplosive * damageOnMaxRangeMultiplier, damageExplosive * damageOnMinRangeMultiplier, EvaluateDistance(distance));
                    _damage.activeRagdoll = distance > maxExplosionRadius * 0.5f ? false : _damage.activeRagdoll;
                    _damage.damageValue = (int)damageValue;
                    onHit.Invoke(colliders[i]);
                    colliders[i].gameObject.ApplyDamage(_damage, null);
                    //EnemyHitHandler eHithandler = colliders[i].GetComponent<EnemyHitHandler>();
                    EnemyHitHandler eHithandler = colliders[i].GetComponent<EnemyHitHandler>();
                    if (eHithandler != null)
                    {                        
                        eHithandler.ApplyHit(damageValue);
                    }
                }
            }
            StartCoroutine(ApplyExplosionForce());
            if (destroyAfterExplode) StartCoroutine(DestroyBomb());
        }
        public virtual void ExplodeIce()
        {
            onExplode.Invoke();
            var colliders = Physics.OverlapSphere(transform.position, maxExplosionRadius, applyDamageLayer);

            if (collidersReached == null)
            {
                collidersReached = new List<GameObject>();
            }

            for (int i = 0; i < colliders.Length; ++i)
            {
                if (colliders[i] != null && colliders[i].gameObject != null && !collidersReached.Contains(colliders[i].gameObject))
                {
                    collidersReached.Add(colliders[i].gameObject);
                    var _damage = new vDamage(damage);
                    _damage.sender = overrideDamageSender ? overrideDamageSender : transform;

                    _damage.hitPosition = colliders[i].ClosestPointOnBounds(transform.position);
                    _damage.receiver = colliders[i].transform;
                    var distance = Vector3.Distance(transform.position, _damage.hitPosition);
                    var damageValue = distance <= minExplosionRadius ? damageExplosive * damageOnMinRangeMultiplier : Mathf.Lerp(damageExplosive * damageOnMaxRangeMultiplier, damageExplosive * damageOnMinRangeMultiplier, EvaluateDistance(distance));
                    _damage.activeRagdoll = distance > maxExplosionRadius * 0.5f ? false : _damage.activeRagdoll;

                    _damage.damageValue = (int)damageValue;
                    onHit.Invoke(colliders[i]);
                    colliders[i].gameObject.ApplyDamage(_damage, null);
                    //EnemyHitHandler eHithandler = colliders[i].GetComponent<EnemyHitHandler>();
                    EnemyHitHandler eHithandler = colliders[i].GetComponent<EnemyHitHandler>();
                    if (eHithandler != null)
                    {
                        eHithandler.ApplyHit(damageValue * ElectricDamagePercent);
                        eHithandler.ApplyFreeze(DetentionTime);
                    }
                }
            }
            StartCoroutine(ApplyExplosionForce());
            if (destroyAfterExplode) StartCoroutine(DestroyBomb());
        }
        public virtual void ExplodeElectric()
        {
            onExplode.Invoke();
            var colliders = Physics.OverlapSphere(transform.position, maxExplosionRadius, applyDamageLayer);

            if (collidersReached == null)
            {
                collidersReached = new List<GameObject>();
            }

            for (int i = 0; i < colliders.Length; ++i)
            {
                if (colliders[i] != null && colliders[i].gameObject != null && !collidersReached.Contains(colliders[i].gameObject))
                {
                    collidersReached.Add(colliders[i].gameObject);
                    var _damage = new vDamage(damage);
                    _damage.sender = overrideDamageSender ? overrideDamageSender : transform;

                    _damage.hitPosition = colliders[i].ClosestPointOnBounds(transform.position);
                    _damage.receiver = colliders[i].transform;
                    var distance = Vector3.Distance(transform.position, _damage.hitPosition);
                    var damageValue = distance <= minExplosionRadius ? damageExplosive * damageOnMinRangeMultiplier : Mathf.Lerp(damageExplosive * damageOnMaxRangeMultiplier, damageExplosive * damageOnMinRangeMultiplier, EvaluateDistance(distance));
                    _damage.activeRagdoll = distance > maxExplosionRadius * 0.5f ? false : _damage.activeRagdoll;

                    _damage.damageValue = (int)damageValue;
                    onHit.Invoke(colliders[i]);
                    colliders[i].gameObject.ApplyDamage(_damage, null);
                    //EnemyHitHandler eHithandler = colliders[i].GetComponent<EnemyHitHandler>();
                    EnemyHitHandler eHithandler = colliders[i].GetComponent<EnemyHitHandler>();
                    if (eHithandler != null)
                    {
                        eHithandler.ApplySlowDown(ReductEnemySpeedPercent, EletricDuration);
                        eHithandler.ApplyHit(damageValue * ElectricDamagePercent);
                        eHithandler.ApplyShock(1f);
                    }
                }
            }
            StartCoroutine(ApplyExplosionForce());
            if (destroyAfterExplode) StartCoroutine(DestroyBomb());
        }
        public virtual void ExplodePoison()
        {
            onExplode.Invoke();
            var colliders = Physics.OverlapSphere(transform.position, maxExplosionRadius, applyDamageLayer);

            if (collidersReached == null)
            {
                collidersReached = new List<GameObject>();
            }

            for (int i = 0; i < colliders.Length; ++i)
            {
                if (colliders[i] != null && colliders[i].gameObject != null && !collidersReached.Contains(colliders[i].gameObject))
                {
                    collidersReached.Add(colliders[i].gameObject);
                    var _damage = new vDamage(damage);
                    _damage.sender = overrideDamageSender ? overrideDamageSender : transform;

                    _damage.hitPosition = colliders[i].ClosestPointOnBounds(transform.position);
                    _damage.receiver = colliders[i].transform;
                    var distance = Vector3.Distance(transform.position, _damage.hitPosition);
                    var damageValue = distance <= minExplosionRadius ? damageExplosive * damageOnMinRangeMultiplier : Mathf.Lerp(damageExplosive * damageOnMaxRangeMultiplier, damageExplosive * damageOnMinRangeMultiplier, EvaluateDistance(distance));
                    _damage.activeRagdoll = distance > maxExplosionRadius * 0.5f ? false : _damage.activeRagdoll;

                    _damage.damageValue = (int)damageValue;
                    onHit.Invoke(colliders[i]);
                    colliders[i].gameObject.ApplyDamage(_damage, null);
                    //EnemyHitHandler eHithandler = colliders[i].GetComponent<EnemyHitHandler>();
                    EnemyHitHandler eHithandler = colliders[i].GetComponent<EnemyHitHandler>();
                    if (eHithandler != null)
                    {
                        eHithandler.ApplyPoisonDamage(damageValue * PoisonDamagePercent, PoisonDuration);

                    }
                }
            }
            StartCoroutine(ApplyExplosionForce());
            if (destroyAfterExplode) StartCoroutine(DestroyBomb());
        }
        protected virtual IEnumerator ApplyExplosionForce()
        {
            yield return new WaitForSeconds(0.0f);

            var colliders = Physics.OverlapSphere(transform.position, maxExplosionRadius, applyForceLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                var _rigdbody = colliders[i].GetComponent<Rigidbody>();
                var distance = Vector3.Distance(transform.position, colliders[i].ClosestPointOnBounds(transform.position));
                var force = distance <= minExplosionRadius ? explosionForce : GetPercentageForce(distance, explosionForce);
                if (_rigdbody)
                {
                    _rigdbody.AddExplosionForce(force, transform.position, maxExplosionRadius, upwardsModifier, forceMode);
                }
            }
        }

        protected float EvaluateDistance(float distance)
        {
            if (distance > maxExplosionRadius) distance = maxExplosionRadius;

            var distanceLimit = maxExplosionRadius - minExplosionRadius;
            var distanceCalc = Mathf.Clamp(distance - minExplosionRadius, 0, distanceLimit);
            var distanceResult = Mathf.Clamp(distanceLimit - (distanceCalc), 0, distanceLimit);
            var multiple = ((distanceResult / distanceLimit) * 100f) * 0.01f;
            return multiple;
        }
        protected float GetPercentageForce(float distance, float value)
        {

            return value * EvaluateDistance(distance);
        }

        public virtual void SetCollisionEnterMethod()
        {
            method = ExplosiveMethod.collisionEnter;
        }

        public virtual void SetCollisionEnterTimerMethod(int timer)
        {
            method = ExplosiveMethod.collisionEnterTimer;
            this.timeToExplode = timer;
        }

        public virtual void SetRemoveMethod()
        {
            method = ExplosiveMethod.remote;
        }

        public virtual void SetRemoveTimerMethod(int timer)
        {
            method = ExplosiveMethod.remoteTimer;
            this.timeToExplode = timer;
        }

        public virtual void SetTimerMethod(int timer)
        {
            method = ExplosiveMethod.timer;
            this.timeToExplode = timer;
        }

        public virtual void ActiveExplosion()
        {
            if (method == ExplosiveMethod.remote)
                Explode();
            else if (method == ExplosiveMethod.remoteTimer)
            {
                StartCoroutine(StartTimer());
            }
        }

        public void RemoveParent()
        {
            transform.parent = null;
        }

        public void RemoveParentOfOther(Transform other)
        {
            originalParent = other.parent;
            other.parent = null;

            float explosionDuration = explosionEffect.main.duration + explosionEffect.main.startLifetime.constantMax;
            StartCoroutine(RestoreParentAfterDelay(other, 4f));
        }
        private IEnumerator RestoreParentAfterDelay(Transform other, float delay)
        {
            yield return new WaitForSeconds(delay);
            RestoreParentOfOther(other);
        }

        public void RestoreParentOfOther(Transform other)
        {
            other.parent = originalParent;
        }
    }
}