using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(1).GetComponent<Text>().text = "Stage : " + GameMgr.Instance.GetStage().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText()
    {
        transform.GetChild(1).GetComponent<Text>().text = "Stage : " + GameMgr.Instance.GetStage().ToString();
    }
}
