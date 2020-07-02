using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    private static GameMgr instance = null;

    [Header("Start Stage Number")]
    [SerializeField] private int iStage = 0;
    private bool stageStart;

    [Header("UI Object")]
    [SerializeField] private Text moneyText;
    [SerializeField] private int money;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject objectInfo;

    [Header("InGame Object")]
    [SerializeField] private GameObject positionObject;
    [SerializeField] private GameObject baseObj;


    private GameObject[] defenceObject = new GameObject[12];
    private GameObject[] enemyObject;

    private float checkCoolTime;
    private float MAX_CHECK_COOLTIME = 3f;

    private bool drag = false;
    private GameObject dragObject;
    private int dragIndex;

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

    public void DestoryAll()
    {
        for(int i = 0; i < 12; ++i)
        {
            if(defenceObject[i] != null)
            {
                defenceObject[i].GetComponent<DefenceUnit>().Damage(9999f);
            }
        }

        GameObject[] trashObject = GameObject.FindGameObjectsWithTag("Effect");
        for (int i = 0; i < trashObject.Length; ++i)
        {
            Destroy(trashObject[i]);
        }

        enemyObject = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i<enemyObject.Length; ++i)
        {
            enemyObject[i].GetComponent<Enemy>().Damage(9999f);
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
        UpdateMoney();
        objectInfo.SetActive(false);
        checkCoolTime = MAX_CHECK_COOLTIME;
    }

    public bool UpdateMoney(int amount = 0)
    {
        if(money + amount >= 0)
        {
            money += amount;
            moneyText.text = "Gold : " + money.ToString();
            return true;
        }
        return false;
    }

    public void StartStage()
    {
        if (stageStart == false)
        {
            checkCoolTime = MAX_CHECK_COOLTIME + 6f;
            positionObject.SetActive(false);
            stageStart = true;
            iStage++;

            for (int i = 0; i < 12; ++i)
            {
                if (defenceObject[i] != null)
                {
                    Vector3 vPos = positionObject.transform.GetChild(i).transform.position;
                    vPos.y += 1f;
                    defenceObject[i].transform.position = vPos;
                }
            }
            GetComponent<AudioSource>().Play();

            EnemySpawner.Instance.Spawn(iStage);
        }
    }

    public void FinishStage()
    {
        GetComponent<AudioSource>().Play();

        UpdateMoney(3);

        positionObject.SetActive(true);

        stageStart = false;

        for (int i = 0; i < 12; ++i)
        {
            if (defenceObject[i] != null)
            {
                defenceObject[i].SetActive(true);
                Vector3 vPos = positionObject.transform.GetChild(i).transform.position;
                vPos.y += 5f;
                defenceObject[i].transform.position = vPos;
                defenceObject[i].GetComponent<DefenceUnit>().ResetObject();
                defenceObject[i].GetComponent<DefenceUnit>().UIActive(true);
            }
        }

        shop.GetComponent<ShopScript>().Refresh();

        GameObject[] trashObject = GameObject.FindGameObjectsWithTag("Effect");
        for(int i = 0; i < trashObject.Length; ++i)
        {
            Destroy(trashObject[i]);
        }
    }

    public bool SpawnUnit(GameObject obj)
    {
        for(int i = 0; i < 12; ++i)
        {
            if(defenceObject[i] == null)
            {
                if (money > 0)
                {
                    Vector3 vPos = positionObject.transform.GetChild(i).transform.position;
                    vPos.y += 5f;
                    defenceObject[i] = Instantiate(obj, vPos, Quaternion.identity);
                    UpdateMoney(-1);
                    return true;
                }
                else
                    return false;
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
        if(Input.GetMouseButtonDown(0))
        {
            GameObject obj = GetClickObject();
            
            if(obj.tag == "Friendly" || obj.tag == "Enemy")
            {
                if (!obj.Equals(objectInfo.GetComponent<InfoUIScript>().GetTarget()))
                {
                    objectInfo.SetActive(true);
                    objectInfo.GetComponent<InfoUIScript>().SetTarget(obj);
                }
                else
                {
                    objectInfo.SetActive(false);
                    objectInfo.GetComponent<InfoUIScript>().SetTarget(null);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            UpdateMoney(1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            baseObj.GetComponent<BaseScript>().DecreaseHP(-1);
        }

        if (stageStart)
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
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(!drag)
                {
                    GameObject obj = GetClickObject();

                    if (obj != null)
                    {
                        for (int i = 0; i < 12; ++i)
                        {
                            if (defenceObject[i] == null)
                                continue;
                            if (defenceObject[i].Equals(obj))
                            {
                                drag = true;
                                dragObject = defenceObject[i];
                                dragIndex = i;
                                dragObject.layer = 2;
                                RigidbodyActive(false);
                                break;
                            }
                        }
                    }
                }
            }
            else if(Input.GetMouseButton(0))
            {
                if(drag)
                {
                    if (dragObject != null)
                    {
                        DragClickObject();
                    }
                }
            }
            else if(Input.GetMouseButtonUp(0))
            {
                if(drag)
                {
                    GameObject obj = GetClickObject();

                    bool check = false;

                    if (obj != null)
                    {
                        dragObject.layer = 9;
                        for (int i = 0; i < 12; ++i)
                        {
                            if(defenceObject[i] != null)
                            {
                                if(defenceObject[i].Equals(obj))
                                {
                                    GameObject temp = defenceObject[i];
                                    defenceObject[i] = defenceObject[dragIndex];
                                    defenceObject[dragIndex] = temp;

                                    if (defenceObject[i] != null)
                                    {
                                        Vector3 vPos = positionObject.transform.GetChild(i).transform.position;
                                        vPos.y += 5f;
                                        defenceObject[i].transform.position = vPos;
                                    }
                                    if (defenceObject[dragIndex] != null)
                                    {
                                        Vector3 vPos = positionObject.transform.GetChild(dragIndex).transform.position;
                                        vPos.y += 5f;
                                        defenceObject[dragIndex].transform.position = vPos;
                                    }
                                    check = true;
                                    break;
                                }
                            }
                            if (positionObject.transform.GetChild(i).gameObject.Equals(obj))
                            {
                                GameObject temp = defenceObject[i];
                                defenceObject[i] = defenceObject[dragIndex];
                                defenceObject[dragIndex] = temp;

                                if(defenceObject[i] != null)
                                {
                                    Vector3 vPos = positionObject.transform.GetChild(i).transform.position;
                                    vPos.y += 5f;
                                    defenceObject[i].transform.position = vPos;
                                }
                                if(defenceObject[dragIndex] != null)
                                {
                                    Vector3 vPos = positionObject.transform.GetChild(dragIndex).transform.position;
                                    vPos.y += 5f;
                                    defenceObject[dragIndex].transform.position = vPos;
                                }
                                check = true;
                                break;
                            }
                        }
                        if(positionObject.transform.GetChild(12).gameObject.Equals(obj))
                        {
                            dragObject.GetComponent<DefenceUnit>().Remove();
                            defenceObject[dragIndex] = null;
                            check = true;
                        }
                    }
                    if(check)
                    {
                        RigidbodyActive(true);
                        drag = false;
                        dragObject = null;
                        dragIndex = -1;
                    }
                    else
                    {
                        Vector3 vPos = positionObject.transform.GetChild(dragIndex).transform.position;
                        vPos.y += 5f;
                        defenceObject[dragIndex].transform.position = vPos;

                        RigidbodyActive(true);
                        drag = false;
                        dragObject = null;
                        dragIndex = -1;
                    }
                }
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

    private GameObject GetClickObject()
    {
        RaycastHit hit;
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray.origin, ray.direction*10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }

    private void DragClickObject()
    {
        RaycastHit hit;
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
            dragObject.transform.position = hit.point;
        }
    }

    private void RigidbodyActive(bool active)
    {
        for(int i=0;i<12;++i)
        {
            if(defenceObject[i] != null)
            {
                defenceObject[i].GetComponent<Rigidbody>().isKinematic = !active;
            }
        }
    }
}
