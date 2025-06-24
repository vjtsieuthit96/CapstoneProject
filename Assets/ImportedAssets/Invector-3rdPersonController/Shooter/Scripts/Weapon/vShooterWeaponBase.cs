using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Invector.vShooter
{
    [vClassHeader("Shooter Weapon", openClose = false)]
    public class vShooterWeaponBase : vMonoBehaviour
    {
        #region Variables

        [vEditorToolbar("Weapon Settings")]
        [Tooltip("The category of the weapon\n Used to the IK offset system. \nExample: HandGun, Pistol, Machine-Gun")]
        public string weaponCategory = "MyCategory";
        public bool isExplosive;
        public bool isEffectMode;
        [SerializeField, Tooltip("Frequency of shots"), FormerlySerializedAs("shootFrequency")]
        protected float _shootFrequency;
        public virtual float shootFrequency { get { return _shootFrequency; } set { _shootFrequency = value; } }

        [vEditorToolbar("Ammo")]

        [Tooltip("Unlimited ammo")]
        public bool isInfinityAmmo;
        public GameObject ExplosionPrefab;
        [Tooltip("Starting ammo")]
        [SerializeField, vHideInInspector("isInfinityAmmo", true), FormerlySerializedAs("ammo")]
        protected int _ammo;
        public virtual int ammo { get { return _ammo; } set { _ammo = value; } }

        [vEditorToolbar("Layer & Tag")]
        public List<string> ignoreTags = new List<string>();
        public LayerMask hitLayer = 1 << 0;

        [vEditorToolbar("Projectile")]
        [Tooltip("Prefab of the projectile")]
        public GameObject projectile;
        [Tooltip("Assign the muzzle of your weapon")]
        public Transform muzzle;
        [Tooltip("How many projectiles will spawn per shot")]
        [Range(1, 20)]
        public int projectilesPerShot = 1;
        [Range(0, 90)]
        [Tooltip("how much dispersion the weapon have")]
        public float dispersion = 0;
        [vToggleOption("DispersionShape", "Circle", "Quad")]
        public bool quadDispersion = false;
        [Range(0, 1000)]
        [Tooltip("Velocity of your projectile")]
        public float velocity = 380;

        [vHelpBox("If you're using the ItemManager attribute 'Damage' on your item, the damage will be always maxDamage, ignoring the distance or minDamage", vHelpBoxAttribute.MessageType.Info)]

        [Tooltip("Check this to calculate damage automatically based on distance using min and max damage, higher distance less damage, less distance more damage")]
        public bool damageByDistance = true;
        public float PlayerDamageMultiplier;
        [Tooltip("Min distance to apply damage, used to evaluate the damage between minDamage and maxDamage")]
        [SerializeField, vHideInInspector("damageByDistance"), FormerlySerializedAs("minDamageDistance")]
        protected float _minDamageDistance = 8f;
        public virtual float minDamageDistance { get { return _minDamageDistance; } set { _minDamageDistance = value; } }
        [Tooltip("Max distance to apply damage, used to evaluate the damage between minDamage and maxDamage")]
        [SerializeField, vHideInInspector("damageByDistance"), FormerlySerializedAs("maxDamageDistance")]
        protected float _maxDamageDistance = 50f;
        public virtual float maxDamageDistance { get { return _maxDamageDistance; } set { _maxDamageDistance = value; } }
        [vHideInInspector("damageByDistance")]
        [SerializeField, Tooltip("Minimum damage caused by the shot, regardless the distance"), FormerlySerializedAs("minDamage")]
        protected int _minDamage;
        public virtual int minDamage { get { return _minDamage; } set { _minDamage = value; } }
        [SerializeField, Tooltip("Maximum damage caused by the close shot"), FormerlySerializedAs("maxDamage")]
        protected int _maxDamage;
        public virtual int maxDamage { get { return _maxDamage; } set { _maxDamage = value; } }

        [vEditorToolbar("Audio & VFX")]
        [Header("Audio")]
        public AudioSource source;
        public AudioClip fireClip;
        public AudioClip emptyClip;

        [Header("Effects")]
        public bool testShootEffect;
        public Light lightOnShot;
        [SerializeField]
        public ParticleSystem[] emittShurykenParticle;
        private Dictionary<ParticleSystem, Color> defaultParticleColors = new Dictionary<ParticleSystem, Color>();
        [SerializeField] private Color electricColor = new Color(0f, 0.4f, 1f);
        [SerializeField] private Color frozenColor = new Color(0.5f, 0.8f, 1f);
        [SerializeField] private Color poisonColor = new Color(0f, 0.6f, 0.3f);
        [SerializeField] private ParticleSystem electricParticle;

        [HideInInspector]
        public OnDestroyEvent onDestroy;
        [System.Serializable]
        public class OnDestroyEvent : UnityEvent<GameObject> { }
        [System.Serializable]
        public class OnInstantiateProjectile : UnityEvent<vProjectileControl> { }

        [vEditorToolbar("Events")]
        public UnityEvent onShot, onEmptyClip;

        public OnInstantiateProjectile onInstantiateProjectile;

        protected virtual float _nextShootTime { get; set; }
        protected virtual float _nextEmptyClipTime { get; set; }
        protected virtual Transform sender { get; set; }

        [vEditorToolbar("Gun Stats")]
        public GunData gunData;
        #endregion

        #region Public Methods
        /// <summary>
        /// Apply additional velocity to the Shot projectile 
        /// </summary>
        public virtual float velocityMultiplierMod { get; set; }

        /// <summary>
        /// Apply additional damage to the projectile
        /// </summary>
        public virtual float damageMultiplierMod { get; set; }

        /// <summary>
        /// Weapon Name
        /// </summary>
        public virtual string weaponName
        {
            get
            {
                var value = gameObject.name.Replace("(Clone)", string.Empty);
                return value;
            }
        }

        /// <summary>
        /// Shoot to direction of the muzzle forward
        /// </summary>
        public virtual void Shoot()
        {
            Shoot(muzzle.position + muzzle.forward * 100f);
        }

        /// <summary>
        /// Shoot to direction of the muzzle forward
        /// </summary>
        /// <param name="sender">Sender to reference of the damage</param>
        /// <param name="successfulShot">Action to check if shoot is successful</param>
        public virtual void Shoot(Transform _sender = null, UnityAction<bool> successfulShot = null)
        {
            Shoot(muzzle.position + muzzle.forward * 100f, _sender, successfulShot);
        }

        /// <summary>
        /// Shoot to direction of the aim Position
        /// </summary>
        /// <param name="aimPosition">Aim position to override direction of the projectile</param>
        /// <param name="sender">ender to reference of the damage</param>
        /// <param name="successfulShot">Action to check if shoot is successful</param>
        public virtual void Shoot(Vector3 aimPosition, Transform _sender = null, UnityAction<bool> successfulShot = null)
        {
            Shoot(muzzle.position, aimPosition, _sender, successfulShot);
        }
        public virtual void Shoot(Vector3 startPoint, Vector3 endPoint, Transform _sender = null, UnityAction<bool> successfulShot = null)
        {
            if (HasAmmo())
            {
                if (!CanDoShot)
                {
                    return;
                }

                UseAmmo();
                this.sender = _sender != null ? _sender : transform;
                HandleShot(startPoint, endPoint);
                if (successfulShot != null)
                {
                    successfulShot.Invoke(true);
                }

                _nextShootTime = Time.time + shootFrequency;
                _nextEmptyClipTime = _nextShootTime;
            }
            else
            {
                if (!CanDoEmptyClip)
                {
                    return;
                }

                EmptyClipEffect();
                if (successfulShot != null)
                {
                    successfulShot.Invoke(false);
                }

                _nextEmptyClipTime = Time.time + shootFrequency;
            }
        }
        /// <summary>
        /// Check if can shoot by <seealso cref="shootFrequency"/>
        /// </summary>
        public virtual bool CanDoShot
        {
            get
            {
                bool _canShot = _nextShootTime < Time.time;
                return _canShot;
            }
        }
        /// <summary>
        /// Check if can do empty clip effect, <seealso cref="shootFrequency"/>
        /// </summary>
        public virtual bool CanDoEmptyClip
        {
            get
            {
                bool _canShot = _nextEmptyClipTime < Time.time;
                return _canShot;
            }
        }

        /// <summary>
        /// Use weapon Ammo
        /// </summary>
        /// <param name="count">count to use</param>
        public virtual void UseAmmo(int count = 1)
        {
            if (ammo <= 0)
            {
                return;
            }

            ammo -= count;
            if (ammo <= 0)
            {
                ammo = 0;
            }
        }

        /// <summary>
        /// Check if Weapon Has Ammo
        /// </summary>
        /// <returns></returns>
        public virtual bool HasAmmo()
        {

            return isInfinityAmmo || ammo > 0;
        }
        #endregion

        #region Protected Methods

        #region Player Class 
        public int PlayerElementClass = 0;
        public Element GunElement;
        public BulletType BulletType;
        public GunType GunType;
        public float DetentionTime;
        public float ReductEnemySpeedPercent;
        public float ElectricDamagePercent;
        public float EletricDuration;
        public float PoisonDamagePercent;
        public float PoisonDuration;
        public GameObject Gunowner;

        #endregion

        public void PlayerElementDamageClass(int PlayerClass)
        {
            switch(PlayerClass)
            {
                case 1:
                    DetentionTime = 2f;
                    ReductEnemySpeedPercent = 0.2f;
                    ElectricDamagePercent = 0.2f;
                    EletricDuration = 5f;
                    PoisonDamagePercent = 0.2f;
                    PoisonDuration = 9f;
                    break;
                case 2:
                    DetentionTime = 4f;
                    ReductEnemySpeedPercent = 0.4f;
                    ElectricDamagePercent = 0.4f;
                    EletricDuration = 5f;
                    PoisonDamagePercent = 0.4f;
                    PoisonDuration = 9f;
                    break;
                case 3:
                    DetentionTime = 6f;
                    ReductEnemySpeedPercent = 0.6f;
                    ElectricDamagePercent = 0.6f;
                    EletricDuration = 8f;
                    PoisonDamagePercent = 0.6f;
                    PoisonDuration = 12f;
                    break;
                case 4:
                    DetentionTime = 7f;
                    ReductEnemySpeedPercent = 0.7f;
                    ElectricDamagePercent = 0.7f;
                    EletricDuration = 8f;
                    PoisonDamagePercent = 0.6f;
                    PoisonDuration = 15f;
                    break;
                default:
                    DetentionTime = 0f;
                    ReductEnemySpeedPercent = 0f;
                    ElectricDamagePercent = 0f;
                    EletricDuration = 0f;
                    PoisonDamagePercent = 0f;
                    PoisonDuration = 0f;
                    break;
            }       
        }   
        

        protected virtual void OnDestroy()
        {
            onDestroy.Invoke(gameObject);
        }

        private void OnApplicationQuit()
        {
            onDestroy.RemoveAllListeners();
        }
        protected virtual void HandleShot(Vector3 startPoint, Vector3 endPoint)
        {
            ShootBullet(startPoint, endPoint);
            ShotEffect();
        }
        public virtual Vector3 Dispersion(Vector3 aim, float dispersion)
        {
            return quadDispersion ? QuadDispersion(aim, dispersion) : CircleDispersion(aim, dispersion);
        }

        public virtual Vector3 CircleDispersion(Vector3 aim, float dispersion)
        {
            var rotatedAim = Quaternion.Euler(Random.insideUnitSphere * dispersion);
            aim = (rotatedAim) * aim;
            return aim;
        }

        public virtual Vector3 QuadDispersion(Vector3 aim, float dispersion)
        {
            var rotatedAim = Quaternion.Euler
                (
                Random.Range(-dispersion, dispersion),
                Random.Range(-dispersion, dispersion),
                Random.Range(-dispersion, dispersion)
                );

            aim = (rotatedAim) * aim;

            return aim.normalized;
        }
        //IEnumerator DebugDispersion(Vector3 startPoint, Vector3 endPoint)
        //{
        //    var dir = endPoint - startPoint;
        //    float time = 10;
        //    while (time>0)
        //    {
        //        var dispersionDir = Dispersion(dir.normalized, dispersion);
        //        (startPoint + dispersionDir * dir.magnitude).DebugPoint(Color.red, 10, 0.02f);
        //        yield return null;
        //        time -= Time.deltaTime;
        //    }
        //}
        protected virtual void TryCreateDecal(RaycastHit hit) { }
        protected virtual void ShootBullet(Vector3 startPoint, Vector3 endPoint)
        {
            Debug.Log(damageMultiplier);
            var dir = endPoint - startPoint;

            Ray ray = new Ray(startPoint, dir.normalized);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 300f, hitLayer))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red, 2f);
                EnemyHitCounter.Instance?.ElementShot();
                if (hit.collider.CompareTag("Enemy"))
                {
                    EnemyHitCounter.Instance?.RegisterEnemyHit();
                    EnemyHitCounter.Instance?.RegisterElementHit();

                }

                if (isEffectMode)
                {
                    #region XỬ LÍ SÁT THƯƠNG NỔ
                    if (GunElement == Element.None)
                    {
                        if (isExplosive && BulletType == BulletType.Explosion)
                        {
                            vExplosive explosive = PoolManager.Instance.GetObject<vExplosive>("Explosion", hit.point, Quaternion.identity);
                            explosive.Owner = Gunowner;
                            if (explosive != null)
                            {
                                int raycastDamage = (int)((maxDamage / Mathf.Max(1, projectilesPerShot)) * damageMultiplier * PlayerDamageMultiplier);
                                explosive.SetOverrideDamageSender(transform);
                                explosive.SetOverDataSender(DetentionTime,ReductEnemySpeedPercent,ElectricDamagePercent,EletricDuration,PoisonDamagePercent,PoisonDuration,raycastDamage);
                                explosive.Explode();
                            }

                        }
                        else
                        {
                            EnemyHitHandler eHithandler = hit.collider.GetComponent<EnemyHitHandler>();
                            if (eHithandler != null)
                            {
                                int raycastDamage = (int)((maxDamage / Mathf.Max(1, projectilesPerShot)) * damageMultiplier * PlayerDamageMultiplier);
                                eHithandler.ApplyBleed(hit.point);
                                eHithandler.ApplyHit(raycastDamage,Gunowner);
                            }
                        }
                    }
                    #endregion
                    #region XỬ LÝ ĐÓNG BĂNG
                    else if (GunElement == Element.Frozen)
                    {
                        if (isExplosive && BulletType == BulletType.Explosion)
                        {
                            vExplosive explosive = PoolManager.Instance.GetObject<vExplosive>("IceExplosion", hit.point, Quaternion.identity);
                            explosive.Owner = Gunowner;
                            if (explosive != null)
                            {
                                int raycastDamage = (int)((maxDamage / Mathf.Max(1, projectilesPerShot)) * damageMultiplier * PlayerDamageMultiplier);
                                explosive.SetOverrideDamageSender(transform);
                                explosive.SetOverDataSender(DetentionTime, ReductEnemySpeedPercent, ElectricDamagePercent, EletricDuration, PoisonDamagePercent, PoisonDuration, raycastDamage);
                                explosive.ExplodeIce();
                            }
                            Quaternion randomRotation = Quaternion.Euler(
                                Random.Range(0f, 360f),
                                Random.Range(0f, 360f),
                                Random.Range(0f, 360f)
                            );

                            GameObject iceCube = GameObjectPoolManager.Instance.GetObject("IcePlane", hit.point, randomRotation);
                        }
                        else
                        {
                            EnemyHitHandler eHithandler = hit.collider.GetComponent<EnemyHitHandler>();
                            if (eHithandler != null)
                            {
                                // Đóng băng:
                                int raycastDamage = (int)((maxDamage / Mathf.Max(1, projectilesPerShot)) * damageMultiplier * PlayerDamageMultiplier);
                                eHithandler.ApplyHit(raycastDamage * ElectricDamagePercent, Gunowner);
                                eHithandler.ApplyFreeze(DetentionTime);

                            }
                            Quaternion randomRotation = Quaternion.Euler(
                               Random.Range(0f, 360f),
                               Random.Range(0f, 360f),
                               Random.Range(0f, 360f)
                           );

                            GameObject icePlane = GameObjectPoolManager.Instance.GetObject("IceCube", hit.point, randomRotation);
                        }
                    }
                    #endregion
                    #region XỬ LÝ ĐIỆN
                    else if (GunElement == Element.Electric)
                    {
                        if (isExplosive && BulletType == BulletType.Explosion)
                        {
                            vExplosive explosive = PoolManager.Instance.GetObject<vExplosive>("ElectricExplosion", hit.point, Quaternion.identity);
                            explosive.Owner = Gunowner;
                            if (explosive != null)
                            {
                                int raycastDamage = (int)((maxDamage / Mathf.Max(1, projectilesPerShot)) * damageMultiplier * PlayerDamageMultiplier);
                                explosive.SetOverrideDamageSender(transform);
                                explosive.SetOverDataSender(DetentionTime, ReductEnemySpeedPercent, ElectricDamagePercent, EletricDuration, PoisonDamagePercent, PoisonDuration, raycastDamage);
                                explosive.ExplodeElectric();
                            }
                        }
                        else
                        {
                            EnemyHitHandler eHithandler = hit.collider.GetComponent<EnemyHitHandler>();
                            if (eHithandler != null)
                            {
                                int raycastDamage = (int)((maxDamage / Mathf.Max(1, projectilesPerShot)) * damageMultiplier * PlayerDamageMultiplier);
                                eHithandler.ApplySlowDown(ReductEnemySpeedPercent, EletricDuration);
                                eHithandler.ApplyHit(raycastDamage * ElectricDamagePercent, Gunowner);
                                eHithandler.ApplyShock(1f);

                            }
                        }
                    }
                    #endregion
                    #region XỬ LÝ ĐỘC
                    else if (GunElement == Element.Poison)
                    {
                        if (isExplosive && BulletType == BulletType.Explosion)
                        {
                            vExplosive explosive = PoolManager.Instance.GetObject<vExplosive>("PoisonExplosion", hit.point, Quaternion.identity);
                            explosive.Owner = Gunowner;
                            if (explosive != null)
                            {
                                int raycastDamage = (int)((maxDamage / Mathf.Max(1, projectilesPerShot)) * damageMultiplier * PlayerDamageMultiplier);
                                explosive.SetOverrideDamageSender(transform);
                                explosive.SetOverDataSender(DetentionTime, ReductEnemySpeedPercent, ElectricDamagePercent, EletricDuration, PoisonDamagePercent, PoisonDuration, raycastDamage);
                                explosive.ExplodePoison();
                            }
                        }
                        else
                        {
                            EnemyHitHandler eHithandler = hit.collider.GetComponent<EnemyHitHandler>();
                            if (eHithandler != null)
                            {
                                int raycastDamage = (int)((maxDamage / Mathf.Max(1, projectilesPerShot)) * damageMultiplier * PlayerDamageMultiplier);
                                eHithandler.ApplyBleed(hit.point);
                                eHithandler.ApplyPoisonDamage(raycastDamage * PoisonDamagePercent, PoisonDuration, Gunowner);
                            }
                        }
                    }
                    #endregion

                    if (!hit.collider.CompareTag("Enemy"))
                    {
                        TryCreateDecal(hit);
                    }
                }
                else
                {
                    if (isExplosive && BulletType == BulletType.Explosion)
                    {
                        vExplosive explosive = PoolManager.Instance.GetObject<vExplosive>("Explosion", hit.point, Quaternion.identity);
                        if (explosive != null)
                        {
                            int raycastDamage = (int)((maxDamage / Mathf.Max(1, projectilesPerShot)) * damageMultiplier * PlayerDamageMultiplier);
                            explosive.SetOverrideDamageSender(transform);
                            explosive.SetOverDataSender(DetentionTime, ReductEnemySpeedPercent, ElectricDamagePercent, EletricDuration, PoisonDamagePercent, PoisonDuration, raycastDamage);
                            explosive.Explode();
                        }

                    }
                    else
                    {
                        EnemyHitHandler eHithandler = hit.collider.GetComponent<EnemyHitHandler>();
                        if (eHithandler != null)
                        {
                            int raycastDamage = (int)((maxDamage / Mathf.Max(1, projectilesPerShot)) * damageMultiplier * PlayerDamageMultiplier);
                            Debug.Log("Damage: " + raycastDamage);
                            eHithandler.ApplyBleed(hit.point);
                            eHithandler.ApplyHit(raycastDamage, Gunowner);
                        }
                    }
                    if (!hit.collider.CompareTag("Enemy"))
                    {
                        TryCreateDecal(hit);
                    }
                }
            }
            else
            {
                EnemyHitCounter.Instance?.ResetCounterElement();
                EnemyHitCounter.Instance?.ResetCounter();
                Debug.DrawRay(ray.origin, ray.direction * 300f, Color.black, 2f);
            }
        }

        protected virtual vProjectileControl CreateProjectileData(Vector3 aimPosition, float velocityChanged, Vector3 dispersionDir, vProjectileControl pCtrl)
        {
            pCtrl.instantiateData = new vProjectileInstantiateData
            {
                aimPos = aimPosition,
                dir = dispersionDir,
                vel = velocityChanged
            };
            return pCtrl;
        }

        protected virtual void ApplyForceToBullet(GameObject bulletObject, Vector3 direction, float velocityChanged)
        {
            try
            {
                var _rigidbody = bulletObject.GetComponent<Rigidbody>();
                _rigidbody.mass = _rigidbody.mass / projectilesPerShot;

                _rigidbody.AddForce((direction.normalized * velocityChanged), ForceMode.VelocityChange);
            }
            catch
            {

            }
        }

        protected virtual float damageMultiplier
        {
            get
            {
                return 1 + damageMultiplierMod;
            }
        }

        protected virtual float velocityMultiplier
        {
            get
            {
                return 1 + velocityMultiplierMod;
            }
        }

        #region Effects
        protected virtual void ShotEffect()
        {
            onShot.Invoke();

            StopCoroutine(LightOnShoot());

            if (source && fireClip)
                source.PlayOneShot(fireClip);

            StartCoroutine(LightOnShoot(0.037f));

            UpdateParticleColors();
            StartEmitters();
        }

        protected virtual void StopSound()
        {
            if (source)
            {
                source.Stop();
            }
        }

        protected virtual IEnumerator LightOnShoot(float time = 0)
        {
            if (lightOnShot)
            {
                lightOnShot.enabled = true;

                yield return new WaitForSeconds(time);
                lightOnShot.enabled = false;
            }
        }

        protected virtual void StartEmitters()
        {
            if (emittShurykenParticle != null)
            {
                foreach (ParticleSystem pe in emittShurykenParticle)
                {
                    pe.Emit(1);
                }
            }
            UpdateElectricParticle();
        }
        public void UpdateElectricParticle()
        {
            if (isEffectMode && GunElement == Element.Electric)
            {
                electricParticle.Emit(1);
            }
        }
        public void CacheDefaultParticleColors()
        {
            defaultParticleColors.Clear();

            if (emittShurykenParticle == null) return;

            foreach (var ps in emittShurykenParticle)
            {
                var renderer = ps.GetComponent<Renderer>();
                if (ps != null && renderer != null && renderer.material != null)
                {
                    defaultParticleColors[ps] = renderer.material.color;
                }
            }
        }


        private void UpdateParticleColors()
        {
            if (emittShurykenParticle == null) return;

            foreach (var ps in emittShurykenParticle)
            {
                if (ps == null || !defaultParticleColors.ContainsKey(ps)) continue;

                var renderer = ps.GetComponent<Renderer>();
                if (renderer == null || renderer.material == null) continue;

                if (!isEffectMode)
                {
                    renderer.material.color = defaultParticleColors[ps];
                }
                else
                {
                    Color newColor = defaultParticleColors[ps];

                    switch (GunElement)
                    {
                        case Element.Electric:
                            newColor = electricColor;
                            break;
                        case Element.Frozen:
                            newColor = frozenColor;
                            break;
                        case Element.Poison:
                            newColor = poisonColor;
                            break;
                        case Element.None:
                            newColor = defaultParticleColors[ps];
                            break;
                    }

                    renderer.material.color = newColor;
                }
            }
        }



        protected virtual void StopEmitters()
        {
            if (emittShurykenParticle != null)
            {
                foreach (ParticleSystem pe in emittShurykenParticle)
                {
                    pe.Stop();
                }
            }
        }

        protected virtual void EmptyClipEffect()
        {
            if (source && emptyClip)
            {
                source.PlayOneShot(emptyClip);
            }

            onEmptyClip.Invoke();
        }
        #endregion

        #endregion
    }
}