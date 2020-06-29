using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vPos = transform.position;
        vPos.y += -100f * Time.deltaTime;
        transform.position = vPos;
    }

    public void SetStatus(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Environment" || other.gameObject.tag == "Enemy")
        {
            Vector3 vPos = transform.position;
            Vector3 vOtherPos = other.gameObject.transform.position;
            vPos.y = vOtherPos.y + 2f;
            GameObject obj = Instantiate(explosionEffect, vPos, Quaternion.identity);
            obj.GetComponent<ExplosionScript>().SetStatus(damage);
            Destroy(gameObject);
        }
    }
}
