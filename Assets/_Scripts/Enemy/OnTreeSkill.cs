using UnityEngine;

public class OnTreeSkill : MonoBehaviour
{
    public bool isTreeSkill = false;
    public GameObject Skill;
    public BoxCollider boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isTreeSkill = false;
    }

    // Update is called once per frame
    void Update()
    {
        Skill.SetActive(isTreeSkill);
        if (boxCollider != null)
            boxCollider.enabled = isTreeSkill;
    }
}
