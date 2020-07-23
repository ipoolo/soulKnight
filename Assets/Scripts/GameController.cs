using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]public int level;
    [SerializeField]public int baseEnemyNum;
    private object[] enemyPrefabs;
    private Transform[] enemyStartPoints;
    public float spwanStepWaitTime;


    private List<Enemy> enemyList;
    private int lastStartIndex = -1;
    private int lastEnemyPrefabIndex = -1;

    public int coinNum;

    // Start is called before the first frame update
    void Start()
    {
        configEnemyPrefabs();
        collectEnemyStartPoints();
        configEnemyList();
        configDefault();
        WaveSpawn();
        
    }

    private void configDefault()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void collectEnemyStartPoints()
    {
        GameObject[] gbArray = GameObject.FindGameObjectsWithTag("WavePoint");
        enemyStartPoints = new Transform[gbArray.Length];
        for(int i = 0 ; i < gbArray.Length; i++)
        {
            enemyStartPoints[i] = gbArray[i].transform;
        }
    }

    public void configEnemyPrefabs()
    {
        enemyPrefabs = Resources.LoadAll("Enemy");
    }
    public void configEnemyList()
    {
        enemyList = new List<Enemy>();
    }

    public void WaveSpawn() 
    {
        StartCoroutine(WaveSpawn(level));
    }


    IEnumerator WaveSpawn(int _level)
    {
        int num = _level + baseEnemyNum;
        for (int i = 0; i < num; i++)
        {
            Debug.Log("a");
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
        Enemy enemy = Instantiate(prefabs, startTransform.position,startTransform.rotation).GetComponentInChildren<Enemy>();
        configEnemy(enemy, _level);
        enemyList.Add(enemy);
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
        return getRamdomxIndexWithLengAndLastIndexAndOut(enemyStartPoints.Length, lastStartIndex, out lastStartIndex);

    }

    public int getRamdomEnemyTypeIndex()
    {
        return getRamdomxIndexWithLengAndLastIndexAndOut(enemyPrefabs.Length, lastEnemyPrefabIndex, out lastEnemyPrefabIndex);

    }

    public int getRamdomxIndexWithLengAndLastIndexAndOut(int length,int lastIndex, out int outPut)
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
