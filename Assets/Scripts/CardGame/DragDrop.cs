using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public bool isDragging = false;
    public Vector3 startPosition;
    public Transform startParent;

    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;     
        startParent = transform.parent;

        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; 
            transform.position = mousePos;


        }
    }


   void OnMouseDown() //마우스 클릭 시 드래그 시작 
    {
        isDragging = true;
                
        startPosition = transform.position;             //시작 위치와 부모 저장
        startParent = transform.parent;

        GetComponent<SpriteRenderer>().sortingOrder = 10; //드래그 중인 카드가 다른 카드보다 앞에 보이도록 한다.
    }

     void OnMouseUp()   //마우스 버튼 놓을 떄
    {
        isDragging = false;
        GetComponent<SpriteRenderer>().sortingOrder = 1;            //드래그 중인 카드가 다른 카드보다 앞에 보이도록 한다

        ReturnToOriginalPosition();
    }


    void ReturnToOriginalPosition()
    {
        transform.position = startPosition;
        transform.SetParent(startParent);


        if(gameManager != null)
        {


            if(startParent == gameManager.handArea)
            {
                gameManager.ArrangeHand();
            }
        }
    }


    bool IsOverArea(Transform area)
    {
        if(area == null)
        {
            return false;
        }

        Collider2D areaCollider = area.GetComponent<Collider2D>();
        if (areaCollider == null)
            return false;

        //
        return areaCollider.bounds.Contains(transform.position);
    }


}
