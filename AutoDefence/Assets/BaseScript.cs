using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseScript : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private Text hpText;

    [SerializeField] private GameObject infoText;

    // Start is called before the first frame update
    void Start()
    {
        hpText.text = "HP : " + hp.ToString();

        Vector3 vPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 12f, 0f));
        infoText = Instantiate(infoText, vPos, Quaternion.identity);
        infoText.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 12f, 0f));
        infoText.transform.position = vPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            DecreaseHP(collision.gameObject.GetComponent<Enemy>().GetBaseDamage());
            collision.gameObject.GetComponent<Enemy>().Damage(9999f);
        }
    }

    public void DecreaseHP(int amount = 1)
    {
        hp -= amount;
        hpText.text = "HP : " + hp.ToString();
        if(hp < 0)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

        }
    }

    public int GetHP()
    {
        return hp;
    }
}
