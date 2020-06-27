using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ObjType
{
    KNIGHT,
    ROBOWARRIOR_1,
    ROBOWARRIOR_2,
    ROBOWARRIOR_3,
    SOLIDER,
    END
}

public class Stuff : MonoBehaviour, IPointerClickHandler
{
    private GameObject ImageObj;
    private GameObject NameObj;
    private int type;

    private bool bActive;

    public Sprite[] ObjSprite;

    public GameObject[] ObjPrefeb;

    // Start is called before the first frame update
    void Start()
    {
        ImageObj = transform.Find("StuffImage").gameObject;
        NameObj = transform.Find("StuffName").gameObject;

        type = Random.Range(0, ObjSprite.Length);
        ImageObj.GetComponent<Image>().sprite = ObjSprite[type];
        NameObj.GetComponent<Text>().text = ObjPrefeb[type].name;

        bActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        type = Random.Range(0, ObjSprite.Length);
        ImageObj.GetComponent<Image>().sprite = ObjSprite[type];
        NameObj.GetComponent<Text>().text = ObjPrefeb[type].name;

        ImageObj.SetActive(true);
        NameObj.SetActive(true);

        bActive = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            //Debug.Log("Mouse Click Button : Left");
            // 캐릭터 선택
            if (GameMgr.Instance.GetStart())
                return;

            if (!bActive)
                return;

            if (!GameMgr.Instance.SpawnUnit(ObjPrefeb[type]))
                return;

            ImageObj.SetActive(false);
            NameObj.SetActive(false);
            bActive = false;
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            //Debug.Log("Mouse Click Button : Middle");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //Debug.Log("Mouse Click Button : Right");
        }
        //Debug.Log("Mouse Position : " + eventData.position);
    }
}
