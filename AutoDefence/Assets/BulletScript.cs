using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private GameObject effectParticle;

    private GameObject targetObject;
    private float speed;
    private float damage;
    private bool shock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObject == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveToTarget(targetObject);
    }

    public void SetTarget(GameObject obj)
    {
        targetObject = obj;
    }
    public void SetStatus(float speed, float damage, bool shock)
    {
        this.speed = speed;
        this.damage = damage;
        this.shock = shock;
    }

    void MoveToTarget(GameObject target)
    {
        Vector3 targetPos = target.transform.position;
        targetPos.y += 6f;
        Vector3 vDir = targetPos - transform.position;
        vDir.Normalize();
        Vector3 vLook = Vector3.Slerp(transform.forward, vDir, Time.deltaTime);
        transform.position += vDir * Time.deltaTime * speed;
        transform.rotation = Quaternion.LookRotation(vLook);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Instantiate(effectParticle, transform.position, effectParticle.transform.rotation);

            collision.gameObject.GetComponent<Enemy>().Damage(damage, shock);

            Destroy(gameObject);
        }
    }
}
