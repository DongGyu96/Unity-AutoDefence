using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner instance = null;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject boss;

    [Min(0.1f)]
    [SerializeField] private float spawnInterval;
    private int count = 0;
    private bool test = true;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            // 씬 전환이 되더라도 파괴되지 않게 한다.
            // gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
            //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
            //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
            Destroy(this.gameObject);
        }
    }

    public static EnemySpawner Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void Spawn(int stage)
    {
        test = true;
        count = 0;
        StartCoroutine(TestSpawn(stage));
    }

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TestSpawn(int stage)
    {
        yield return new WaitForSeconds(1f);

        while (test)
        {
            if((count + 1) % 10 == 0)
            {
                Instantiate(boss, new Vector3(0f + (20f * 2), 5f, 300f), Quaternion.identity);
            }
            else
            {
                for (int i = 0; i < 6; ++i)
                {
                    Instantiate(enemy, new Vector3(0f + (20f * i), 5f, 300f), Quaternion.identity);
                }
            }
            count++;
            if(count >= stage)
            {
                test = false;
            }
            yield return new WaitForSeconds(2.5f);
        }
    }
}
