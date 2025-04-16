using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("�⺻ �̵� ����")]
    public float moveSpeed = 5f;            //�̵� �ӵ� ���� ����
    public float jumpForce = 7f;            //������ �� ���� �ش�
    public float turnSpeed = 10f;           //ȸ�� �ӵ�


    [Header("���� ���� ����")]
    public float falMultiplier = 2.5f;          //�ϰ� �߷� ����
    public float lowJumpMultiplier = 2.0f;      //ª�� ���� ����

    [Header("���� ���� ����")]
    public float coyoteTime = 0.15f;             //���� ���� �ð�
    public float coyoteTimeCounter;             //���� Ÿ�̸�
    public bool realGrouned = true;              //���� ���� ����




    [Header("�۶��̴� ����")]
    public GameObject gliderObject;             //�۶��̴� ������Ʈ
    public float gliderFallSpeed = 1.0f;        //�۶��̴� ���� �ӵ�
    public float gliderMoveSpeed = 7.0f;        //�۶��̴� �̵� �ӵ�
    public float gliderMaxTime = 5.0f;          //�ִ� ��� �ð�
    public float gliderTimeLeft;                //���� ��� �ð�
    public bool isGliding = false;             //�۶��̵� ������ ����


    public bool isGrounded = true;          //���� �ִ��� üũ �ϴ� ����(true/false)

    public int coinCount = 0;               ///���� ȹ�� ���� ����
    public int totalCoins = 5;              //�� ���� ȹ��� �ʿ� ���� ����

    public Rigidbody rb;                    //�÷��̾� ��ü�� ����

    // Start is called before the first frame update
    void Start()
    {
        



        //�۶��̴� ������Ʈ �ʱ�ȭ
        if(gliderObject != null)
        {
            gliderObject.SetActive(false);              //���� �� ��Ȱ��ȭ
        }
        gliderTimeLeft = gliderMaxTime;                 //�׶��̴� �ð� �ʱ�ȭ


        coyoteTimeCounter = 0;


    }

    // Update is called once per frame
    void Update()
    {
        UPdateGroundedState();

        //������ �Է�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        //�̵� ���� ����
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);    //�̵� ���� ����
          
        if(movement.magnitude > 0.1f)   //�Է��� ���� ���� ȸ��
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);  //�̵� ������ �ٶ󺸵��� �ε巴�� ȸ��
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }



        //GŰ�� �۶��̴� ����(������ ���ȸ� Ȱ��ȭ)
        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if(!isGliding)
            {
                //�۶��̴� Ȱ��ȭ �Լ�(�Ʒ� ����)
                EnableGlider();

            }

            //�۶��̴� ��� �ð� ����
            gliderTimeLeft -= Time.deltaTime;

            
            if(gliderTimeLeft <=0)
            {
                DisableGlider();
            }
        }
        else if(isGliding)
        {
            DisableGlider();
        }

        if(isGliding)
        {


            ApplyGliderMovement(moveHorizontal, moveVertical);

        }
        else
        {
                //�ӵ��� ���� �̵�
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

                //���� ���� ���� ����
            if(rb.velocity.y < 0)
            {
                //�ϰ� �� �߷� ��ȭ
                rb.velocity += Vector3.up * Physics.gravity.y * (falMultiplier - 1) * Time.deltaTime; //�ϰ� �� �߷� ��ȭ
            }
            else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime; // ����� ���� ��ư�� ���� ���� ����

            }
        }

          



        //���� �Է�
        if (Input.GetButtonDown("Jump") && isGrounded)                  //&& �� ���� �����Ҷ� -> (�����̽� ��ư�� �������� �� is Grounded �� true �϶�)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);     //�������� �����ϴ� ����ŭ ��ü���� �ش�.
            isGrounded = false;                                         //������ �ϴ� ���� ������ �������� ������ false��� ���ش�.
            realGrouned = false;
            coyoteTimeCounter = 0;                                      //�ڿ��� Ÿ�� ��� ����
        }

        //���鿡 ������ �۶��̴� �ð� ȸ�� �� �۶��̴� ��Ȱ��ȭ
        if(isGrounded)
        {
            if(isGliding)
            {
                DisableGlider();

            }


            gliderTimeLeft = gliderMaxTime;
        }
    }



    //�۶��̴� Ȱ��ȭ �Լ�

    void EnableGlider()
    {
        isGliding = true;

        //�۶��̴� ������Ʈ ǥ��
        if(gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);

    }


    //�۶��̴� ��Ȱ��ȭ �Լ�

    void DisableGlider()
    {
        isGliding = false;

        if(gliderObject != null)
        {
            gliderObject.SetActive(false);

        }
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

    }




    void ApplyGliderMovement(float horizontal,float vertical)

    {
        Vector3 gliderVelocity = new Vector3(
            horizontal * gliderMoveSpeed,
            -gliderFallSpeed,
            vertical * gliderMoveSpeed
            
            );

        rb.velocity = gliderVelocity;
    }

         void OnCollisionEnter(Collision collision)
    
        { 
            if (collision.gameObject.CompareTag("Ground"))       //�浹�� �Ͼ ��ü�� Tag�� Ground ���
            {
                realGrouned = true;          //���� �浹�ϸ� isGronded�� true�� �����Ѵ�
            }
        }

        void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))       //�浹�� �Ͼ ��ü�� Tag�� Ground ���
            {
                realGrouned = true;          //���� �浹�ϸ� isGronded�� true�� �����Ѵ�
            }

        }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = false;
        }
    }

    private void OnTriggerEnter(Collider other)       //Ʈ���� ���� �ȿ� ���Գ��� �˻��ϴ� �Լ�
    {
        if(other.CompareTag("Coin"))                //���� Ʈ���ſ� �浹�ϸ�
        {
            coinCount++;                            //coinCount = coinCount + 1 ���� ������ 1 �ö󰣴�.
            Destroy(other.gameObject);              //���� ������Ʈ�� ������.
            Debug.Log($"���� ���� : {coinCount}/{totalCoins}");
        }


        //������ ���� �� ���� �α� ���
        if(other.gameObject.tag == "Door" && coinCount == totalCoins)            //��� ������ ȹ���Ŀ� ������ ���� ���� ����
        {
            Debug.Log("���� Ŭ����");
            //���� �Ϸ� ���� �߰� ����
        }
    }



    //���� ���� ������Ʈ �Լ�

    void UPdateGroundedState()
    {
        if(realGrouned)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if(coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;

            }
            else
            {
                isGrounded = false;
            }
                
        }


    }
}

