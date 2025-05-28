using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBallController : MonoBehaviour
{

    [Header("�⺻����")]
    public float power = 10f;               //Ÿ�� ��
    public Sprite arrowSprite;              //ȭ��ǥ �̹���


    private Rigidbody rb;                   //���� ����
    private GameObject arrow;               //ȭ��ǥ ������Ʈ
    private bool isDragging = false;        //�巡�� ������ Ȯ�� �ϴ� Bool
    private Vector3 startPos;               //�巡�� ���� ��ġ

    // Start is called before the first frame update
    void Start()
    {
        SetupBall();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdatArrow();
    }
    void SetupBall()                        //�� ����
    {
        rb = GetComponent<Rigidbody>();         //���� ������Ʈ ��������
        if(rb  == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();          //���� ��� �ٿ��ش�
        }

        //��������
        rb.mass = 1;
        rb.drag = 1;
    }

    public bool IsMoving()          //���� ������ Ȯ��
    {   
        return rb.velocity.magnitude > 0.2f;        //���� �ӵ��� ����� ������ �����δٰ� �Ǵ�
    }

    void HandleInput()
    {
        if (!SimpleTurnmanager.canPlay) return;
        if (SimpleTurnmanager.anyBallMoving) return;

        if (IsMoving()) return;                     //���� �����̰� ������ ���� �Ұ�

        if (Input.GetMouseButtonDown(0))            //���콺 Ŭ�� ����
        {
            StartDrag();
        }
        if(Input.GetMouseButtonUp(0) && isDragging) //�巡�� ���ε� ���콺 ��ư �� ���� ���
        {
            Shoot();
        }
    } 
    
    void Shoot()
    {
        Vector3 mouseDelta = Input.mousePosition - startPos;
        float force = mouseDelta.magnitude * 0.01f * power;

        if (force < 5) force = 5;

        Vector3 direction = new Vector3(mouseDelta.x, 0, - mouseDelta.y).normalized;         //���� ���

        rb.AddForce(direction * force, ForceMode.Impulse);

        SimpleTurnmanager.OnBallHit();          //�� �Ŵ������� ���� �ƴٰ� �˸�

        //�� �߻� ���� ���� ����
        isDragging = false;
        Destroy(arrow);
        arrow = null;

        Debug.Log("�߻�!��:" + force);
    }

    void CreateArrow()
    {
        if(arrow != null)
        {
            Destroy(arrow);
        }

        arrow = new GameObject("Arrow");
        SpriteRenderer sr = arrow.AddComponent<SpriteRenderer>();

        sr.sprite = arrowSprite;
        sr.color = Color.green;
        sr.sortingOrder = 10;

        arrow.transform.position = transform.position + Vector3.up;
        arrow.transform.localScale = Vector3.one;
    }

    void UpdatArrow()
    {
        if (!isDragging || arrow == null) return;

        Vector3 mouseDelta = Input.mousePosition - startPos;
        float distance = mouseDelta.magnitude;

        float size = Mathf.Clamp(distance * 0.01f, 0.5f, 2.0f);
        arrow.transform.localScale = Vector3.one * size;

        SpriteRenderer sr = arrow.GetComponent<SpriteRenderer>();
        float colorRatio = Mathf.Clamp01(distance * 0.005f);
        sr.color = Color.Lerp(Color.green, Color.red, colorRatio);

        if(distance > 10f)
        {
            Vector3 direction = new Vector3(mouseDelta.x, 0, mouseDelta.y);

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(90, angle, 0);
        }
    }

    void StartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit))
        {
            if(hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                startPos = Input.mousePosition;
                CreateArrow();
                Debug.Log("�巡�� ����");
            }
        }
    }
}
