using UnityEngine;

public class OnTreeSkill : MonoBehaviour
{
    public bool isTreeSkill = false;
    public GameObject Skill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isTreeSkill = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            isTreeSkill = !isTreeSkill;
        }
        Skill.SetActive(isTreeSkill);
    }
}
