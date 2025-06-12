using UnityEngine;

public abstract class ObjectMove : MonoBehaviour
{
    [SerializeField] private float time;
    float m_time;
    float m_time2;
    [SerializeField] private float MoveSpeed = 10;
    [SerializeField] private bool AbleHit;
    [SerializeField] private float HitDelay;       
    [SerializeField] private float MaxLength;
    [SerializeField] private float DestroyTime2;

    protected virtual void Start()
    {       
        m_time = Time.time;
        m_time2 = Time.time;
    }

    protected virtual void LateUpdate()
    {
        if (Time.time > m_time + time)
            Destroy(gameObject);

        transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
        if(AbleHit)
        { 
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, MaxLength))
            {
                if (Time.time > m_time2 + HitDelay)
                {
                    m_time2 = Time.time;
                    HitObj(hit);
                }
            }
        }
    }

    protected abstract void HitObj(RaycastHit hit);
    
    //void HitObj(RaycastHit hit)
    //{
    //    m_makedObject = Instantiate(m_hitObject, hit.point, Quaternion.LookRotation(hit.normal)).gameObject;
    //    Destroy(m_makedObject, DestroyTime2);
    //}

}
