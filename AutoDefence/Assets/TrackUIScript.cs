using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackUIScript : MonoBehaviour
{
    [SerializeField] private GameObject infoObject;
    [SerializeField] private float offsetY = 12f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 vPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, offsetY, 0f));
        infoObject = Instantiate(infoObject, vPos, Quaternion.identity);
        infoObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, offsetY, 0f));
        infoObject.transform.position = vPos;
    }
}
