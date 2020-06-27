using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceUnit : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float attackDist;
    [SerializeField] private float maxattackCoolTime;
    private float attackCoolTime;

    private float remainhp;

    // Start is called before the first frame update
    void Start()
    {
        ResetObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        remainhp -= damage;
        if(remainhp < 0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void ResetObject()
    {
        remainhp = hp;
    }
}
