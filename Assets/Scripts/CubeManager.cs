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
        timer += Time.deltaTime;        //Ư�� �ð����� ȣ��
        if(timer >= interval)
        {
            RandomizeCubeActivation();  //�Լ� ȣ��
            timer = 0f;                 //Ÿ�̸� �ʱ�ȭ
        }
    }

    public void RandomizeCubeActivation()
    {
        for(int i = 0; i < generatedCubes.Length; i++)              //�� ť�긦 �����ϰ� Ȱ�� �Ǵ� ��Ȱ��ȭ
        {
            int randomNum = Random.Range(0, 2);                     //0 �Ǵ� 1
            if(randomNum == 1)
            {
                generatedCubes[i].GenCube();
            }
        }
    }
}
