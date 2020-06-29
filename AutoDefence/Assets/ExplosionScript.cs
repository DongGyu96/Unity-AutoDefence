using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float damage;

    private float lifeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime < 0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetStatus(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<Enemy>().Damage(damage);
        }
    }
}
