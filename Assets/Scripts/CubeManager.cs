using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public CubeGenerator[] generatedCubes = new CubeGenerator[5];       

    public float timer = 0f;
    public float interval = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;        //특정 시간마다 호출
        if(timer >= interval)
        {
            RandomizeCubeActivation();  //함수 호출
            timer = 0f;                 //타이머 초기화
        }
    }

    public void RandomizeCubeActivation()
    {
        for(int i = 0; i < generatedCubes.Length; i++)              //각 큐브를 랜덤하게 활성 또는 비활성화
        {
            int randomNum = Random.Range(0, 2);                     //0 또는 1
            if(randomNum == 1)
            {
                generatedCubes[i].GenCube();
            }
        }
    }
}
