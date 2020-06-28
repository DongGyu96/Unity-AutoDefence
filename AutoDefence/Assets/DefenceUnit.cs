using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SKILL_TYPE
{
    NONE
};

public enum UINT_TYPE
{
    SHORT_DISTANCE,
    LONG_DISTANCE,
    END,
};

public class DefenceUnit : MonoBehaviour
{
    [SerializeField] private GameObject hpBar;
    [SerializeField] private GameObject mpBar;

    [Header("Unit Status")]
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float attackDist;
    [SerializeField] private float maxAttackCoolTime;

    [Header("Unit Type")]
    [SerializeField] private UINT_TYPE objectType;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool shock;
    [SerializeField] private GameObject attackParticle;
    private float attackCoolTime;

    [Header("Skill Status")]
    [SerializeField] private float mp;
    [SerializeField] private SKILL_TYPE skillType;

    private float remainMp;
    private float remainHp;

    private float targetResetCoolTime;
    private float MAX_TARGET_RESET_COOLTIME = 1f;

    private GameObject[] enemyObject;

    // Start is called before the first frame update
    void Start()
    {
        ResetObject();

        if (mpBar != null)
        {
            Vector3 vPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 12f, 0f));
            mpBar = Instantiate(mpBar, vPos, Quaternion.identity);
            mpBar.transform.SetParent(GameObject.Find("Canvas").transform);
        }

        if(hpBar != null)
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


        if (!GameMgr.Instance.GetStart())
            return;

        targetResetCoolTime -= Time.deltaTime;
        attackCoolTime -= Time.deltaTime;
        if (targetResetCoolTime < 0f)
        {
            targetResetCoolTime = MAX_TARGET_RESET_COOLTIME;
            SetTargetUnit();
        }

        if (enemyObject != null)
        {
            for (int i = 0; i < enemyObject.Length; ++i)
            {
                if (enemyObject[i] != null)
                {
                    float dist = Vector3.Distance(enemyObject[i].transform.position, transform.position);
                    if (dist < attackDist / 10f)
                    {
                        if (attackCoolTime < 0f)
                        {
                            if (objectType == UINT_TYPE.SHORT_DISTANCE)
                            {
                                LookToTarget(enemyObject[i]);
                                attackCoolTime = maxAttackCoolTime;
                                enemyObject[i].GetComponent<Enemy>().Damage(damage, shock);
                                Vector3 vPos = enemyObject[i].transform.position;
                                vPos.y += 6f;
                                Instantiate(attackParticle, vPos, attackParticle.transform.rotation);
                            }
                            else if(objectType == UINT_TYPE.LONG_DISTANCE)
                            {
                                LookToTarget(enemyObject[i]);
                                Vector3 vPos = transform.position;
                                vPos.y += 6f;
                                vPos.z += 5f;
                                GameObject obj = Instantiate(bullet, vPos, bullet.transform.rotation);
                                obj.GetComponent<BulletScript>().SetTarget(enemyObject[i]);
                                obj.GetComponent<BulletScript>().SetStatus(bulletSpeed, damage, shock);
                                attackCoolTime = maxAttackCoolTime;
                            }
                            remainMp += 10f;
                            if(remainMp > mp)
                            {
                                remainMp = 0;
                                Debug.Log("Skill !");
                            }
                        }
                    }
                    else if (dist < 60f)
                    {
                        MoveToTarget(enemyObject[i]);
                    }
                    return;
                }
            }
        }
    }

    void UpdateUI()
    {
        if (mpBar == null)
            return;

        Vector3 vPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 12f, 0f));
        mpBar.transform.position = vPos;
        mpBar.GetComponent<Slider>().value = remainMp / mp;

        if (hpBar == null)
            return;

        vPos.y += 4f;
        hpBar.transform.position = vPos;
        hpBar.GetComponent<Slider>().value = remainHp / hp;
    }

    public void Damage(float damage)
    {
        remainHp -= damage;
        if(remainHp <= 0f)
        {
            this.gameObject.SetActive(false);
            mpBar.SetActive(false);
            hpBar.SetActive(false);
        }
    }

    public void UIActive(bool active)
    {
        if (hpBar != null)
            hpBar.SetActive(active);
        if (mpBar != null)
            mpBar.SetActive(active);
    }

    public void ResetObject()
    {
        remainHp = hp;
        remainMp = 0;
        attackCoolTime = maxAttackCoolTime;
        targetResetCoolTime = MAX_TARGET_RESET_COOLTIME;
    }

    void SetTargetUnit()
    {
        enemyObject = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemyObject.Length > 0)
        {
            Array.Sort(enemyObject, delegate (GameObject obj1, GameObject obj2)
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

    void LookToTarget(GameObject target)
    {
        Vector3 vDir = target.transform.position - transform.position;
        vDir.Normalize();
        Vector3 vLook = Vector3.Slerp(transform.forward, vDir, Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(vLook);
    }
}
