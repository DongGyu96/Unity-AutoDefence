using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseScript : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private Text hpText;

    // Start is called before the first frame update
    void Start()
    {
        hpText.text = "HP : " + hp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            DecreaseHP();
            Destroy(collision.gameObject);
        }
    }

    public void DecreaseHP(int amount = -1)
    {
        hp += amount;
        hpText.text = "HP : " + hp.ToString();
    }

    public int GetHP()
    {
        return hp;
    }
}
