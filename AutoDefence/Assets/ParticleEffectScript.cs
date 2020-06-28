using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectScript : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void SetLifeTime(float time)
    {
        lifeTime = time;
    }
}
