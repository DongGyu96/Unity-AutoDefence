using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
    private Stuff[] StuffScript = new Stuff[5];

    // Start is called before the first frame update
    void Start()
    {
        StuffScript[0] = transform.Find("Stuff_1").GetComponent<Stuff>();
        StuffScript[1] = transform.Find("Stuff_2").GetComponent<Stuff>();
        StuffScript[2] = transform.Find("Stuff_3").GetComponent<Stuff>();
        StuffScript[3] = transform.Find("Stuff_4").GetComponent<Stuff>();
        StuffScript[4] = transform.Find("Stuff_5").GetComponent<Stuff>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        if(GameMgr.Instance.UpdateMoney(-1))
        {
            for (int i = 0; i < 5; ++i)
            {
                StuffScript[i].Refresh();
            }
        }
    }
}
