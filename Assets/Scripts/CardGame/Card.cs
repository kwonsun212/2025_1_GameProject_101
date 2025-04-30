using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardValue;               //ī�� ��
    public Sprite cardImage;            //ī�� �̹���
    public TextMeshPro cardText;        //ī�� �ؽ�Ʈ

    //ī�� ���� �ʱ�ȭ �Լ�


    // Start is called before the first frame update
    public void InitCard(int Value, Sprite image)           //ī�� ���� �ʱ�ȭ �Լ�
    {
        cardValue = Value;
        cardImage = image;

        //ī�� �̹��� ����
        GetComponent<SpriteRenderer>().sprite = image;      //�ش� �̹����� ī�忡 ǥ��


        //ī�� �ؽ�Ʈ ����(�ִ� ���)
        if(cardText != null)
        {
            cardText.text = cardValue.ToString();           //ī�� ���� ǥ�� �Ѵ�.
        }




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
