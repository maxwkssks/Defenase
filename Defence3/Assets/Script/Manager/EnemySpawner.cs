using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [CreateAssetMenu(fileName = "WaveInfo", menuName = "Scriptable Object/WaveInfo")]
    public class WaveInfo : ScriptableObject
    {
        public int   WaveNumber;
        public int MonsterSpawnCount = 15;
        public int Originalgoblin = 13;
        public int Specialgoblin = 2;

    }

    public Transform SpawnPosition; //ó�� Enemy�� ������ ��ġ
    public GameObject[] WayPoints; //Enemy�� ������ ����Ʈ
    public GameObject EnemyPrefab;
    public GameObject EnemyPrefab2;
    public float SpawnCycleTime = 1f; //���� �ֱ�

    private bool _bCanSpawn = true;
    private object EnemySpawnTransform;

    private void Start()
    {
        Activate();
    }

    public void Activate()
    {
        StartCoroutine(SpawnEnemy()); //SpawnEnemy�ڷ�ƾ ����
    }

    public void DeActivate()
    {
        StopCoroutine(SpawnEnemy()); //SpawnEnemy�ڷ�ƾ ����
    }

    IEnumerator SpawnEnemy()
    {
        while (_bCanSpawn) //��ȯ�� �� �ִ� ����
        {
            yield return new WaitForSeconds(SpawnCycleTime); //��Ÿ�Ӹ�ŭ ���

            GameObject EnemyInst = Instantiate(EnemyPrefab, SpawnPosition.position, Quaternion.identity); //SpawnPosition��ġ�� Enemy ������ ����
            Enemy EnemyCom = EnemyInst.GetComponent<Enemy>(); //������ ������Ʈ�� EnemyŬ���� �Ҵ�
            if (EnemyCom) 
            {
                EnemyCom.WayPoints = WayPoints; //Ʈ������ �迭 �ʱ�ȭ
            }
            
        }
    }
    IEnumerator SpawnEnemy2()
    {
        while (_bCanSpawn) //��ȯ�� �� �ִ� ����
        {
            yield return new WaitForSeconds(SpawnCycleTime); //��Ÿ�Ӹ�ŭ ���

            GameObject EnemyInst = Instantiate(EnemyPrefab2, SpawnPosition.position, Quaternion.identity); //SpawnPosition��ġ�� Enemy ������ ����
            Enemy EnemyCom = EnemyInst.GetComponent<Enemy>(); //������ ������Ʈ�� EnemyŬ���� �Ҵ�
            if (EnemyCom)
            {
                EnemyCom.WayPoints = WayPoints; //Ʈ������ �迭 �ʱ�ȭ
            }

        }
    }

}
