using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUIScript : MonoBehaviour
{
    [SerializeField] private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfoUI();
    }

    public void SetTarget(GameObject obj)
    {
        if(obj == null)
        {
            target = null;
            gameObject.SetActive(false);
            return;
        }

        if(obj.tag == "Friendly" || obj.tag == "Enemy")
        {
            target = obj;
            UpdateInfoUI();
        }
    }

    void UpdateInfoUI()
    {
        if (target == null)
            return;

        if(target.tag == "Friendly")
        {
            DefenceUnit targetInfo = target.GetComponent<DefenceUnit>();
            if(targetInfo.GetRemainHP() <= 0f)
            {
                target = null;
                gameObject.SetActive(false);
                return;
            }

            transform.GetChild(2).GetComponent<Image>().sprite = targetInfo.GetImage();

            string[] name = target.name.Split('(');
            transform.GetChild(3).GetComponent<Text>().text = name[0];

            transform.GetChild(4).GetComponent<Slider>().value =
                targetInfo.GetRemainHP() / targetInfo.GetHP();

            transform.GetChild(5).GetComponent<Slider>().value =
                targetInfo.GetRemainMP() / targetInfo.GetMP();

            transform.GetChild(6).GetComponent<Text>().text =
                targetInfo.GetRemainHP().ToString() + "/" + targetInfo.GetHP();

            transform.GetChild(7).GetComponent<Text>().text =
                targetInfo.GetRemainMP().ToString() + "/" + targetInfo.GetMP();

            transform.GetChild(8).GetComponent<Text>().text = "Skill : " + 
                targetInfo.GetSkillInfo();

            transform.GetChild(9).GetComponent<Text>().text = "Attack : " +
                targetInfo.GetDamage().ToString();

            transform.GetChild(10).GetComponent<Text>().text = "Dist : " +
                targetInfo.GetAttackDist().ToString();

            transform.GetChild(11).GetComponent<Text>().text = "MoveSpeed : " +
                (targetInfo.GetMoveSpeed() * 10).ToString();

            transform.GetChild(12).GetComponent<Text>().text = "AttackSpeed : " +
                targetInfo.GetAttackSpeed().ToString();
        }
        else if(target.tag == "Enemy")
        {
            Enemy targetInfo = target.GetComponent<Enemy>();
            if (targetInfo.GetRemainHP() <= 0f)
            {
                target = null;
                gameObject.SetActive(false);
                return;
            }

            transform.GetChild(2).GetComponent<Image>().sprite = targetInfo.GetImage();

            string[] name = target.name.Split('(');
            transform.GetChild(3).GetComponent<Text>().text = name[0];

            transform.GetChild(4).GetComponent<Slider>().value =
                targetInfo.GetRemainHP() / targetInfo.GetHP();

            transform.GetChild(5).GetComponent<Slider>().value =
                targetInfo.GetRemainMP() / targetInfo.GetMP();

            transform.GetChild(6).GetComponent<Text>().text =
                targetInfo.GetRemainHP().ToString() + "/" + targetInfo.GetHP();

            transform.GetChild(7).GetComponent<Text>().text =
                targetInfo.GetRemainMP().ToString() + "/" + targetInfo.GetMP();

            transform.GetChild(8).GetComponent<Text>().text = "Enemy\n" +
                targetInfo.GetSkillInfo();

            transform.GetChild(9).GetComponent<Text>().text = "Attack : " +
                targetInfo.GetDamage().ToString();

            transform.GetChild(10).GetComponent<Text>().text = "Dist : " +
                targetInfo.GetAttackDist().ToString();

            transform.GetChild(11).GetComponent<Text>().text = "MoveSpeed : " +
                (targetInfo.GetMoveSpeed() * 10).ToString();

            transform.GetChild(12).GetComponent<Text>().text = "AttackSpeed : " +
                targetInfo.GetAttackSpeed().ToString();
        }
    }

    public GameObject GetTarget()
    {
        return target;
    }
}
