using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject coinPrefabs;          //동전 프리팹
    public GameObject MissilePrefabs;

    [Header("스폰 타이밍 설정")]
    public float minSpawnInterval = 0.5f;   //최소 생성 간격 (초)
    public float maxSpawnInterval = 2.0f;   //최대 생성 간격 (초)

    [Header("동전 스폰 확률 설정")]
    [Range(0,100)]
    public int coinSpawnChance = 50;


    public float timer = 0.0f;
    public float nextSpawnTime;             //다음 생성 시간



    // Start is called before the first frame update
    void Start()
    {
        SetNextSpawnTime();
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnObject();
            timer = 0.0f;
            SetNextSpawnTime();
        }
    }

    void SpawnObject()
    {
        Transform spawnTransform = transform;


        int randomValue = Random.Range(0, 100);
        if (randomValue < coinSpawnChance)
        {
            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);
        }
        else
        {
            Instantiate(MissilePrefabs, spawnTransform.position, spawnTransform.rotation);
        }
       
    }

    // Update is called once per frame
    void SetNextSpawnTime()
    {

        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
