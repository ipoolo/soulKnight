using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBlockController : BlockController
{

    [SerializeField] public int level;
    [SerializeField] public int baseEnemyNum;
    private object[] enemyPrefabs;
    private Transform[] enemyStartPoints;
    public float spwanStepWaitTime;
    protected bool isBattleState = false;
    protected bool isFinishBattle = false;
    public int maxStepSpwanNum = 1;
    protected int alreadySpwanNum;
    protected int maxEnemyNum;

    private List<Enemy> enemySurvivalList;
    private int lastStartIndex = -1;
    private int lastEnemyPrefabIndex = -1;
    private System.Action<Enemy> removeEnmeyAction;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        ConfigEnemyPrefabs();
        CollectEnemyStartPoints();
        ConfigSurvivaEnemyList();
        maxEnemyNum = level + baseEnemyNum;
        removeEnmeyAction = new System.Action<Enemy>(removeFromSurvivalList);
        WaveSpawn();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        checkEnemySurvivalListCount();
    }



    public void checkEnemySurvivalListCount()
    {
        if (enemySurvivalList.Count == 0)
        {
            if(alreadySpwanNum < maxEnemyNum)
            {
                WaveSpawn();
            }else if (!isFinishBattle) { 
            
                BattleFinish();
                //battleEnd
            }
        }
    }
    public override void ReceivePlayerEnter()
    {
        //同一帧情况下,isFinishBattle还没有被BattleFinish 赋值 doorArea触发了 ontrigger 导致判断失效. 固加判断条件(非战斗状态))
        base.ReceivePlayerEnter();
        if (blockType == BlockType.battleType && isFinishBattle == false && isBattleState != true)
        {
            //这里还要判断block的类型
            gameObject.BroadcastMessage("BattleStart");
            isBattleState = true;
            EnemyGoBattleState();
        }
    }

    public void BattleFinish()
    {
        //放置宝箱
        //开门
        base.ReceivePlayerEnter();
        if (blockType == BlockType.battleType)
        {
            //这里还要判断block的类型
            gameObject.BroadcastMessage("BattleEnd");
            isBattleState = false;
            isFinishBattle = true;

            //创造宝箱
            SwpanTreasure();
        }
    }

    public void SwpanTreasure()
    {
        float treasureX = Random.Range(transform.position.x + 1, transform.position.x + blockWidth - 1);
        float treasureY = Random.Range(transform.position.y - 1, transform.position.y - blockWidth + 1);
        Vector2 treasurePoistion = new Vector2(treasureX, treasureY);
        Treasure treasure = ((GameObject)Instantiate(Resources.Load("Item/Treasure"), treasurePoistion, Quaternion.identity)).GetComponent<Treasure>();
    }

    public void EnemyGoBattleState()
    {
        enemySurvivalList.ForEach(e =>
        {
            e.isPause = !isBattleState;
        });
    }


    public void CollectEnemyStartPoints()
    {

        GameObject[] gbArray = GameObject.FindGameObjectsWithTag("WavePoint");
        List<GameObject> tmpList = new List<GameObject>(gbArray);
        List<GameObject> validList = tmpList.FindAll((ob) => {
            if(ob.transform.position.x > (transform.position.x ) 
            && ob.transform.position.x < (transform.position.x + blockWidth)
            && ob.transform.position.y > (transform.position.y - blockWidth)
            && ob.transform.position.y < (transform.position.y )) 
            {
                return true;
            }
            else
            {
                return false;
            }
            ; 
        });

        enemyStartPoints = new Transform[validList.Count];
        for (int i = 0; i < validList.Count; i++)
        {
            enemyStartPoints[i] = validList[i].transform;
        }
    }

    public void ConfigEnemyPrefabs()
    {
        enemyPrefabs = Resources.LoadAll("Enemy");
    }
    public void ConfigSurvivaEnemyList()
    {
        enemySurvivalList = new List<Enemy>();
    }

    public void WaveSpawn()
    {
        StartCoroutine(WaveSpawn(level));
    }


    IEnumerator WaveSpawn(int _level)
    {

        //_level + baseEnemyNum为生成总量
        int num = maxEnemyNum - alreadySpwanNum;
        num = Mathf.Clamp(num,0, maxStepSpwanNum);
        for (int i = 0; i < num; i++)
        {
            SpawnEneny(_level);
            yield return new WaitForSeconds(spwanStepWaitTime);
        }

    }

    public void SpawnEneny(int _level)
    {
        Transform startTransform;
        startTransform = enemyStartPoints[getRamdomStartIndex()];
        //获得随机初始怪物预制体(不能与上一个相同)
        GameObject prefabs = (GameObject)enemyPrefabs[getRamdomEnemyTypeIndex()];
        Enemy enemy = Instantiate(prefabs, startTransform.position, startTransform.rotation).GetComponentInChildren<Enemy>();
        enemy.destoryDelegate = removeEnmeyAction;
        enemy.isPause = !isBattleState;
        configEnemy(enemy, _level);
        enemy.transform.parent.parent = transform;
        enemySurvivalList.Add(enemy);
        alreadySpwanNum++;

    }

    public void removeFromSurvivalList(Enemy e)
    {
        enemySurvivalList.Remove(e);
    }

    public void configEnemy(Enemy _enmey, int _level)
    {
        if (!_enmey.enemyLevelUpRulesBody(_enmey, _level))
        {
            configEnemyDefaultBody(_enmey, _level);
        }


    }

    //预留给配置，继承，修改怪物属性（这里其实可以通过预制体获得相应配置规则） enemy.rulsBody;
    public void configEnemyDefaultBody(Enemy _enmey, int _level)
    {
        //默认规则 自定义规则可能将其设置覆盖
        _enmey.moveSpeed += _level;
        _enmey.damage += _level;
    }



    public int getRamdomStartIndex()
    {
        return getRamdomxIndexWithLengthAndLastIndexAndOut(enemyStartPoints.Length, lastStartIndex, out lastStartIndex);

    }

    public int getRamdomEnemyTypeIndex()
    {
        return getRamdomxIndexWithLengthAndLastIndexAndOut(enemyPrefabs.Length, lastEnemyPrefabIndex, out lastEnemyPrefabIndex);

    }

    public int getRamdomxIndexWithLengthAndLastIndexAndOut(int length, int lastIndex, out int outPut)
    {
        int _index = Random.Range(0, length - 1);
        while (_index == lastIndex && length != 1)
        {
            _index = Random.Range(0, length - 1);
        }
        outPut = _index;
        return _index;
    }
}
