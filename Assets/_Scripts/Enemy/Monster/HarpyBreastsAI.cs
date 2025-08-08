
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class HarpyBreastsAI : MonsterAI
{
    [Header("-----Addon Components------")]    
    [SerializeField] private GameObject player;
    [SerializeField] private Transform catchPoint;
    [SerializeField] private float catchRadius = 2f;
    [SerializeField] private LayerMask catchableLayer;    
    private bool isCatch;
    private bool isFlying;   
    private bool isLanding;
    private bool isTakeOff;
    private bool isFalling;  
    private bool isFlyToPlayer;
    private bool isHovering;
    private bool isRoar = false; 
    private float monsterHeight;   
    private float riseVelocity = 0f;
    private float fallVelocity = 0f;
    private float maxCatchDuration;
    private float catchTimer;
    public float CatchTimer => catchTimer;
    private Rigidbody rb;
    private MonsterAudio Audio;


    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();    
        Audio = GetComponent<MonsterAudio>();
        RepeatEvaluateBehaviorTree(0f, 1f);
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Không tìm thấy GameObject có tag 'Player'");
        }
        monsterHeight = monsterAgent.height;
    }
    protected override void Update()
    {
        base.Update();        
        if (isFlying) {SetAnimatorParameter(MonsterAnimatorHash.isFlyingHash, true);}
        else {SetAnimatorParameter(MonsterAnimatorHash.isFlyingHash, false); }      
        Landing();
        AdjustFlyHeight();
        StartCatchTimer();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        rb.isKinematic = false;
    }

    protected override Node CreateBehaviorTree()
    {
        return new Selector(new List<Node>
        {
        new CatchPreyNode(this, monsterAgent), 

            new Sequence(new List<Node>
            {
                new CheckPlayerInFOVNode(this),
                new CatchPreyNode(this, monsterAgent)
            }),
        new PatrolNode(this, monsterAgent)
        });
    }

    private void StartCatchTimer()
    {
        if (isCatch)
        {
            catchTimer -= Time.deltaTime;        
        }        
    }

    private void AdjustFlyHeight()
    {
        if (!isDead)
        {
            if (isCatch)
                SetBaseOffSet(20f, ref riseVelocity, 5f);

            if (isTakeOff)
                SetBaseOffSet(10f, ref riseVelocity, 2f);

            if (isFlyToPlayer)
                SetBaseOffSet(1.15f, ref fallVelocity, 1.5f);

            if (isHovering)
                SetBaseOffSet(15f, ref riseVelocity, 5f);            
        }
        if(isFalling)
                SetBaseOffSet(0f, ref fallVelocity, 0.5f);

    }
    private void SetBaseOffSet(float target, ref float velocity, float smoothTime)
    {
        monsterAgent.baseOffset = Mathf.SmoothDamp(monsterAgent.baseOffset, target, ref velocity, smoothTime);
        monsterAgent.height = monsterHeight + monsterAgent.baseOffset;
    }
    private void HitTheGround()
    {
        monsterAgent.baseOffset = 0f;
        rb.isKinematic = true;
    }

    public void SetIsFalling()
    {
        isFalling = true;
    }   
    public bool IsFalling()
    {
        return isFalling;
    }
    public void SetIsHovering(bool value)
    {
        isHovering = value;
    }

    #region Catch&Release

    public bool IsCatch() => isCatch;
    public void CatchPrey()
    {
        if (isCatch) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, catchRadius, catchableLayer);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                isCatch = true;       
                maxCatchDuration = Random.Range(15f, 20f);
                catchTimer = maxCatchDuration;
                // Di chuyển player đến điểm bắt
                hit.transform.position = catchPoint.position;

                // Gắn joint
                FixedJoint joint = this.AddComponent<FixedJoint>();
                joint.connectedBody = hit.GetComponent<Rigidbody>();
                joint.massScale = 0.01f;
                joint.connectedMassScale = 0.01f;            
                SetAnimatorParameter(MonsterAnimatorHash.CatchedHash,true);
                return;
            }
        }    
    }

    public void ReleasePrey()
    {
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint != null)
        {            
            Destroy(joint);            
            isCatch = false;
            catchTimer = 0f;
            SetAnimatorParameter(MonsterAnimatorHash.CatchedHash,false);
        }    
    }   

    #endregion

    #region Fly&Land
    public bool IsFlying() => isFlying;
    public void SetIsFlying(bool value)
    {
        isFlying = value;
    }
    public void TakeOff()
    {
        isTakeOff = true;
    }
    public void DoneTakeOff()
    {
        isTakeOff = false;
    }
    public void FlyToPlayer(bool value)
    {
        isFlyToPlayer = value;       
    }

    private void Landing()
    {
        if (isLanding)
        {
            if (monsterAgent.enabled && monsterAgent.baseOffset > 0.15f)
            {
                monsterAgent.baseOffset = Mathf.Lerp(monsterAgent.baseOffset, 0f, Time.deltaTime*1.5f);
                monsterAgent.height = monsterHeight + monsterAgent.baseOffset;
            }
            else
            {
                SetAnimatorParameter(MonsterAnimatorHash.landHash,null);
                monsterAgent.baseOffset = 0f;
                monsterAgent.height = monsterHeight;
                isLanding = false;               
            }
        }
    }
    public void SetIsLanding()
    {
        isLanding = true;
    }

    #endregion
    #region Sound
    private IEnumerator PlayRoarSound()
    {
        isRoar = true;
        Audio.PlayRoar();        
        yield return new WaitForSecondsRealtime(Random.Range(3f,10f));    
        isRoar = false;
    }
    public void PlayRoar()
    {
        if (!isRoar)
        {
            StartCoroutine(PlayRoarSound());
        }      
    }
    #endregion
}



