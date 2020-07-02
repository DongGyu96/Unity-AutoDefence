using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SKILL_TYPE
{
    HEAL,
    POWERUP_SPEED,
    POWERUP_DAMAGE,
    SNIPE,
    BOMB,
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
    [SerializeField] private Sprite face;

    [Header("Unit Status")]
    [SerializeField] private float hp;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float attackDist;
    [SerializeField] private float runDist = 1000;
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
    [SerializeField] private GameObject skillEffect;
    [SerializeField] private float skillValue;
    [SerializeField] private float skillDurationTime;
    [SerializeField] private GameObject skillObject;
    [SerializeField] private string skillTip;
    private bool activeSkillOn;
    private int activeSkillCount;
    private bool skillOn;
    private float skillRemainTime;

    private float remainMp;
    private float remainHp;

    private float targetResetCoolTime;
    private float MAX_TARGET_RESET_COOLTIME = 1f;

    private GameObject[] enemyObject;

    private Animator animator;
    private float MAX_RUN_ANIMATION_TIME = 1f;
    private float runAnimationTime;

    private AudioSource attackAudio;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        attackAudio = GetComponent<AudioSource>();

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

        UpdateSkill();

        if (!GameMgr.Instance.GetStart())
            return;


        targetResetCoolTime -= Time.deltaTime;
        attackCoolTime -= Time.deltaTime;
        runAnimationTime -= Time.deltaTime;

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
                                vPos.y += 7.5f;
                                vPos.z += 6.5f;
                                GameObject obj = Instantiate(bullet, vPos, bullet.transform.rotation);
                                obj.GetComponent<BulletScript>().SetTarget(enemyObject[i]);
                                obj.GetComponent<BulletScript>().SetStatus(bulletSpeed, damage, shock);
                                attackCoolTime = maxAttackCoolTime;
                                Instantiate(attackParticle, vPos, transform.rotation);
                            }
                            PlaySound();

                            if (!skillOn)
                                remainMp += 10f;

                            if(remainMp >= mp)
                            {
                                remainMp = 0;
                                ActiveSkill();
                            }
                            animator.SetTrigger("Shoot");
                        }
                    }
                    else if (dist < runDist / 10f)
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
            skillRemainTime = -1f;
            UpdateSkill();

            this.gameObject.SetActive(false);
            mpBar.SetActive(false);
            hpBar.SetActive(false);
        }
    }

    public void Remove()
    {
        Destroy(mpBar);
        Destroy(hpBar);
        Destroy(gameObject);
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
        animator.SetTrigger("Idle");
        remainHp = hp;
        // remainMp = 0;
        attackCoolTime = maxAttackCoolTime;
        targetResetCoolTime = MAX_TARGET_RESET_COOLTIME;
        runAnimationTime = MAX_RUN_ANIMATION_TIME;
        skillRemainTime = skillDurationTime;
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
        Vector3 vLook = Vector3.Slerp(transform.forward, vDir, 1f);
        transform.position += vDir * Time.deltaTime * speed;
        transform.rotation = Quaternion.LookRotation(vLook);

        if (runAnimationTime < 0f)
        {
            animator.SetTrigger("Run");
            runAnimationTime = MAX_RUN_ANIMATION_TIME;
        }
    }

    void LookToTarget(GameObject target)
    {
        Vector3 vDir = target.transform.position - transform.position;
        vDir.Normalize();
        Vector3 vLook = Vector3.Slerp(transform.forward, vDir, Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(vLook);
    }

    void ActiveSkill()
    {
        if (skillType == SKILL_TYPE.HEAL)
        {
            GameObject effect = Instantiate(skillEffect, transform.position, skillEffect.transform.rotation);
            effect.GetComponent<ParticleEffectScript>().SetLifeTime(skillDurationTime);
            remainHp += skillValue;
            if (remainHp > hp)
                remainHp = hp;
            skillRemainTime = skillDurationTime;
            skillOn = true;
        }
        else if (skillType == SKILL_TYPE.POWERUP_DAMAGE)
        {
            GameObject effect = Instantiate(skillEffect, transform.position, skillEffect.transform.rotation);
            effect.GetComponent<ParticleEffectScript>().SetLifeTime(skillDurationTime);
            damage *= skillValue;
            skillRemainTime = skillDurationTime;
            skillOn = true;
        }
        else if (skillType == SKILL_TYPE.POWERUP_SPEED)
        {
            GameObject effect = Instantiate(skillEffect, transform.position, skillEffect.transform.rotation);
            effect.GetComponent<ParticleEffectScript>().SetLifeTime(skillDurationTime);
            maxAttackCoolTime /= skillValue;
            attackCoolTime = -1f;
            skillRemainTime = skillDurationTime;
            skillOn = true;
        }
        else if(skillType == SKILL_TYPE.SNIPE)
        {
            GameObject effect = Instantiate(skillEffect, transform.position, skillEffect.transform.rotation);
            effect.GetComponent<ParticleEffectScript>().SetLifeTime(skillDurationTime);
            skillRemainTime = skillDurationTime;
            skillOn = true;
            activeSkillCount = 0;
            activeSkillOn = true;
            StartCoroutine(ActiveSnipeSkill());
        }
        else if (skillType == SKILL_TYPE.BOMB)
        {
            GameObject effect = Instantiate(skillEffect, transform.position, skillEffect.transform.rotation);
            effect.GetComponent<ParticleEffectScript>().SetLifeTime(skillDurationTime);
            skillRemainTime = skillDurationTime;
            skillOn = true;
            activeSkillCount = 0;
            activeSkillOn = true;
            StartCoroutine(ActiveBombSkill());
        }
    }

    void UpdateSkill()
    {
        if (!skillOn)
            return;

        skillRemainTime -= Time.deltaTime;
        if (skillRemainTime < 0f)
        {
            if (skillType == SKILL_TYPE.POWERUP_DAMAGE)
            {
                damage /= skillValue;
            }
            else if(skillType == SKILL_TYPE.POWERUP_SPEED)
            {
                maxAttackCoolTime *= skillValue;
            }
            skillOn = false;
        }
        
    }

    IEnumerator ActiveSnipeSkill()
    {
        yield return new WaitForSeconds(0.1f);

        while (activeSkillOn)
        {
            activeSkillCount++;
            if (activeSkillCount >= 10)
            {
                activeSkillOn = false;
                skillOn = false;
            }

            Vector3 vPos = transform.position;
            vPos += transform.forward * (10f * activeSkillCount);
            GameObject obj = Instantiate(skillObject, vPos, transform.rotation);
            obj.GetComponent<ExplosionScript>().SetStatus(skillValue);

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ActiveBombSkill()
    {
        yield return new WaitForSeconds(0.1f);

        while (activeSkillOn)
        {
            activeSkillCount++;
            if (activeSkillCount >= 15)
            {
                activeSkillOn = false;
                skillOn = false;
            }

            Vector3 vPos = transform.position;
            vPos.y += 200f;
            vPos.z += UnityEngine.Random.Range(0, 120) - 5f;
            vPos.x += UnityEngine.Random.Range(0, 120) - 60f;

            GameObject obj = Instantiate(skillObject, vPos, transform.rotation);
            obj.GetComponent<BombScript>().SetStatus(skillValue);

            yield return new WaitForSeconds(0.1f);
        }
    }

    void PlaySound()
    {
        attackAudio.Play();
    }

    public float GetHP()
    {
        return hp;
    }
    public float GetRemainHP()
    {
        return remainHp;
    }
    public float GetMP()
    {
        return mp;
    }
    public float GetRemainMP()
    {
        return remainMp;
    }
    public float GetDamage()
    {
        return damage;
    }
    public float GetMoveSpeed()
    {
        return speed;
    }
    public float GetAttackSpeed()
    {
        return maxAttackCoolTime;
    }
    public float GetAttackDist()
    {
        return attackDist;
    }
    public string GetSkillInfo()
    {
        return skillTip;
    }
    public Sprite GetImage()
    {
        return face;
    }
}
