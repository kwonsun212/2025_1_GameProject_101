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


   void OnMouseDown() //���콺 Ŭ�� �� �巡�� ���� 
    {
        isDragging = true;
                
        startPosition = transform.position;             //���� ��ġ�� �θ� ����
        startParent = transform.parent;

        GetComponent<SpriteRenderer>().sortingOrder = 10; //�巡�� ���� ī�尡 �ٸ� ī�庸�� �տ� ���̵��� �Ѵ�.
    }

     void OnMouseUp()   //���콺 ��ư ���� ��
    {
        isDragging = false;
        GetComponent<SpriteRenderer>().sortingOrder = 1;            //�巡�� ���� ī�尡 �ٸ� ī�庸�� �տ� ���̵��� �Ѵ�

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
