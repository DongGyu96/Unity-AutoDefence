using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject mainTarget;
    private GameObject[] friendlyUnits;

    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float attackDist;
    [SerializeField] private float maxAttackCoolTime;
    private float attackCoolTime;

    private float targetResetCoolTime;
    private float MAX_TARGET_RESET_COOLTIME = 1f;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(0f, 180f, 0f));
        mainTarget = GameObject.FindWithTag("Base");

        SetTargetUnit();

        attackCoolTime = maxAttackCoolTime;
        targetResetCoolTime = MAX_TARGET_RESET_COOLTIME;
    }

    // Update is called once per frame
    void Update()
    {
        attackCoolTime -= Time.deltaTime;
        targetResetCoolTime -= Time.deltaTime;
        if(targetResetCoolTime < 0f)
        {
            targetResetCoolTime = MAX_TARGET_RESET_COOLTIME;
            SetTargetUnit();
        }

        if (friendlyUnits.Length > 0)
        {
            for (int i = 0; i < friendlyUnits.Length; ++i)
            {
                if (friendlyUnits[i].activeInHierarchy)
                {
                    if (Vector3.Distance(friendlyUnits[i].transform.position, transform.position) < attackDist / 10f)
                    {
                        if(attackCoolTime < 0f)
                        {
                            attackCoolTime = maxAttackCoolTime;
                            friendlyUnits[i].GetComponent<DefenceUnit>().Damage(damage);
                        }
                    }
                    else
                    {
                        MoveToTarget(friendlyUnits[i]);
                    }
                    return;
                }
            }
        }
        else
        {
            MoveToTarget(mainTarget);
            return;
        }
    }

    void SetTargetUnit()
    {
        friendlyUnits = GameObject.FindGameObjectsWithTag("Friendly");

        if (friendlyUnits.Length > 0)
        {
            Array.Sort(friendlyUnits, delegate (GameObject obj1, GameObject obj2)
            {
                float dist1 = Vector3.Distance(obj1.transform.position, transform.position);
                float dist2 = Vector3.Distance(obj2.transform.position, transform.position);
                return dist1.CompareTo(dist2);
            });
        }
    }

    void MoveToTarget(GameObject target)
    {
        Vector3 vDir = target.transform.position - transform.position;
        vDir.Normalize();
        Vector3 vLook = Vector3.Slerp(transform.forward, vDir, Time.deltaTime);
        transform.position += vDir * Time.deltaTime * speed;
        transform.rotation = Quaternion.LookRotation(vLook);
    }
}
