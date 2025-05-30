using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float GameTimer = 0.0f;          //게임 타이머를 설정 한다.
    public GameObject MonsterGo;            //몬스터 게임 오브젝트를 선언 한다.

    // Update is called once per frame
    void Update()
    {
        GameTimer -= Time.deltaTime;           //시간을 매 프레임마다 감소 시킨다(deltaTime 프레임간의 시간 간격을 의미한다.)

        if (GameTimer <= 0)                    //만약 Timer의 수치가 0이하로 내려갈 경우
        {
            GameTimer = 3.0f;                   //다시 3초로 변경 시켜준다.

            GameObject Temp = Instantiate(MonsterGo);                   //해당 게임 오브젝트를 복사 생성 시켜준다.
            Temp.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-4, 4), 0.0f);
            //X -10 ~ 10 Y -4 ~ 4 의 범위의 랜덤으로 위치 시킨다.
        }




        if (Input.GetMouseButtonDown(0))                                    //마우스 버튼을 누르면
        {
            RaycastHit hit;                                                 //Ray 선언
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //카메라에서 레이를 쏴서 검출한다.

            if (Physics.Raycast(ray, out hit))                              //Hit 된 오브젝트를 검출한다.
            {
                if (hit.collider != null)                                   //Hit 된 오브젝트를 있을 경우.
                {
                    hit.collider.gameObject.GetComponent<Monster>().CharacterHit(10);                           
                }
            }
        }
    }
}
