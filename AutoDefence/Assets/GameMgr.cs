using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    private static GameMgr instance = null;

    [SerializeField] private int iStage = 0;
    [SerializeField] private bool stageStart;

    [SerializeField] private Text moneyText;
    [SerializeField] private int money;
    [SerializeField] private GameObject positionObject;

    [SerializeField] private GameObject baseObj;

    private GameObject[] defenceObject = new GameObject[12];
    private GameObject[] enemyObject;

    private float checkCoolTime;
    private float MAX_CHECK_COOLTIME = 3f;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            InitGame();

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

    public bool GetStart()
    {
        return stageStart;
    }

    public int GetStage()
    {
        return iStage;
    }

    public static GameMgr Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void InitGame()
    {
        moneyText.text = money.ToString();
        checkCoolTime = MAX_CHECK_COOLTIME;
    }

    public void StartStage()
    {
        if (stageStart == false)
        {
            checkCoolTime = MAX_CHECK_COOLTIME + 6f;
            positionObject.SetActive(false);
            stageStart = true;
            iStage++;
            EnemySpawner.Instance.Spawn(iStage);
        }
    }

    public void FinishStage()
    {
        positionObject.SetActive(true);

        stageStart = false;

        for (int i = 0; i < 12; ++i)
        {
            if (defenceObject[i] != null)
            {
                defenceObject[i].SetActive(true);
                Vector3 vPos = positionObject.transform.GetChild(i).transform.position;
                vPos.y += 6f;
                defenceObject[i].transform.position = vPos;
                defenceObject[i].GetComponent<DefenceUnit>().ResetObject();
            }
        }
    }

    public bool SpawnUnit(GameObject obj)
    {
        for(int i = 0; i < 12; ++i)
        {
            if(defenceObject[i] == null)
            {
                Vector3 vPos = positionObject.transform.GetChild(i).transform.position;
                vPos.y += 6f;
                defenceObject[i] = Instantiate(obj, vPos, Quaternion.identity);
                return true;
            }
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stageStart)
        {
            checkCoolTime -= Time.deltaTime;
            if (checkCoolTime < 0f)
            {
                if(CheckEnemyObject())
                {
                    FinishStage();
                }
                checkCoolTime = MAX_CHECK_COOLTIME;
            }
        }
    }

    bool CheckEnemyObject()
    {
        enemyObject = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemyObject.Length > 0)
            return false;

        return true;
    }
}
