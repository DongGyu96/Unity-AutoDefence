﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private GameObject mainTarget;
    private GameObject[] friendlyUnits;
    [SerializeField] private GameObject hpBar;
    // [SerializeField] private GameObject mpBar;

    [Header("Enemy Status")]
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float attackDist;
    [SerializeField] private float maxAttackCoolTime;
    private float attackCoolTime;
    private float remainHp;

    private float targetResetCoolTime;
    private float MAX_TARGET_RESET_COOLTIME = 1f;

    // Start is called before the first frame update
    void Start()
    {
        remainHp = hp;

        transform.Rotate(new Vector3(0f, 180f, 0f));
        mainTarget = GameObject.FindWithTag("Base");

        SetTargetUnit();

        attackCoolTime = maxAttackCoolTime;
        targetResetCoolTime = MAX_TARGET_RESET_COOLTIME;

        //if (mpBar != null)
        //{
        //    Vector3 vPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 12f, 0f));
        //    mpBar = Instantiate(mpBar, vPos, Quaternion.identity);
        //    mpBar.transform.SetParent(GameObject.Find("Canvas").transform);
        //}

        if (hpBar != null)
        {
            Vector3 vPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 12f, 0f));
            vPos.y += 4f;
            hpBar = Instantiate(hpBar, vPos, Quaternion.identity);
            hpBar.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();

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

    public void Damage(float damage, bool shock = false)
    {
        remainHp -= damage;
        if(shock)
        {
            transform.position += -transform.forward * Time.deltaTime * damage * 2f;
        }

        if (remainHp <= 0f)
        {
            Destroy(gameObject);
            if (hpBar != null)
                Destroy(hpBar);
            //if (mpBar != null)
            //    Destroy(mpBar);
        }
    }

    void UpdateUI()
    {
        //if (mpBar == null)
        //    return;

        //Vector3 vPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 12f, 0f));
        //mpBar.transform.position = vPos;
        //mpBar.GetComponent<Slider>().value = remainMp / mp;

        if (hpBar == null)
            return;

        Vector3 vPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 12f, 0f));
        vPos.y += 4f;
        hpBar.transform.position = vPos;
        hpBar.GetComponent<Slider>().value = remainHp / hp;
    }
}
