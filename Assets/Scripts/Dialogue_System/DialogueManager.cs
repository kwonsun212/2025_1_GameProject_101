using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    [Header("UI ��� -  Inspector ���� ����")]
    public GameObject DialoguePanel;            //��ȭâ ��ü �г� 
    public Image characterImage;                //ĳ���� �̹���
    public TextMeshProUGUI characternameText;   //ĳ���� �̸�
    public TextMeshProUGUI dialogueText;        //��ȭ ���� ǥ�� ��ư
    public Button nextButton;                   //���� ��ư

    [Header("�⺻����")]
    public Sprite defaultCharacterImage;        //ĳ���� �⺻ �̹���

    [Header("Ÿ���� ȿ�� ����")]
    public float typingSpeed = 0.05f;           //���� �ϳ��� Ǯ�� �ӵ�
    public bool skipTypingOnClick = true;       //Ŭ�� �� Ÿ���� ��� �Ϸ� ����

    //���� ������
    private DialogueDataSO currentDialogue;     //���� ��ȭ���� ��ȭ ������
    private int currentLineIndex = 0;           //���� �� ��° ��ȭ ������ (0 ���� ����)
    private bool isDialogueActive = false;      //��ȭ ���������� Ȯ�� �ϴ� �÷���
    private bool isTyping = false;              //���� Ÿ���� ȿ���� ���� ������ Ȯ��
    private Coroutine typingCoroutine;          //Ÿ���� ȿ�� �ڷ�Ƽ ���� (������)



    // Start is called before the first frame update
    void Start()
    {
        DialoguePanel.SetActive(false);         //��ȭâ �����
        nextButton.onClick.AddListener(HandIeNextInput);    //"����" ��ư�� ���ο� �Է� ó�� ����
    }

    // Update is called once per frame
    void Update()
    {
        if(isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandIeNextInput();          //���� �Է� ó�� (Ÿ���� ���̸� �Ϸ�,�ƴϸ� ���� ��)
        }
    }

    IEnumerator TypeText(string textToType)             //Ÿ���� �� ��ü �ؽ�Ʈ
    {
        isTyping = true;                                //Ÿ���� ����
        dialogueText.text = "";                         //Ÿ���� �ʱ�ȭ
                
        for(int i = 0; i< textToType.Length; i++)       //�ؽ�Ʈ�� �� ���ھ� �߰�
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);   //��� �ð� ����
        }

        isTyping = false;               //Ÿ���� �Ϸ�
    }

    private void CompleteTyping()           //Ÿ���� ȿ���� ��� �Ϸ��ϴ� �Լ�
    {
        if(typingCoroutine != null)         
        {
            StopCoroutine(typingCoroutine);     //�ڷ�ƾ ����
        }
        isTyping = false;           //Ÿ���� ���� ����

        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];           //���� ���� ��ü �ؽ�Ʈ�� ��� ǥ��
        }
    }


    void ShowCurrentLine()      //���� ��ȭ ���� ������ Ÿ���� ȿ���� �Բ� ȭ�鿡 ǥ���ϴ� �Լ�
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)  //��ȭ �ε����� ��ȿ���� Ȯ��
        {
            if(typingCoroutine != null)                     //���� Ÿ���� ȿ���� �ִٸ� �߱�
            {
                StopCoroutine(typingCoroutine);     //�ڷ�ƾ ����
            }
        }
        //���� ���� ��ȭ �������� Ÿ���� ����
        string currentText = currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }

    void EndDialogue()              //��ȭ�� ������ ���� �ϴ� �Լ�
    {
        if (typingCoroutine != null)        //Ÿ���� ȿ�� ����
        {
        StopCoroutine(typingCoroutine);
        typingCoroutine = null;
        }
        isDialogueActive = false;           //��ȭ�� ��Ȱ��ȭ
        isTyping = false;                   //Ÿ���� ���� ����
        DialoguePanel.SetActive(false);     //��ȭâ �����
        currentLineIndex = 0;               //�ε��� �ʱ�ȭ

    }

    public void ShowNextLine()              //���� ��ȣ�� �ٷ� �̵� ��Ű�� �Լ� (Ÿ���� �Ϸ�� �Ŀ��� ȣ��)
    {
        currentLineIndex++;                 //���� �ٷ� �ε��� ����

        if(currentLineIndex >= currentDialogue.dialogueLines.Count)     //������ ��ȭ������ Ȯ��
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();                                          //��ȭ�� ���������� ������ ǥ��
        }
    }
    public void HandIeNextInput()                   //�����̽��ٳ� ��ư Ŭ�� �� ȣ�� �Ǵ� �Է� ó�� �Լ� 
    {
        if(isTyping && skipTypingOnClick)
        {
            CompleteTyping();               //Ÿ���� ���̶�� ��� �Ϸ�
        }
        else if(!isTyping)
        {
            ShowNextLine();                 //Ÿ���� �Ϸ� ���¶�� ���� �� ǥ��
        }
    }
    public void SkipDialogue()
    {
        EndDialogue();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void StartDialogue(DialogueDataSO dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return;        

        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialogueActive = true;

        DialoguePanel.SetActive(true);
        characternameText.text = dialogue.characterName;

        if(characterImage != null)
        {
            characterImage.sprite = dialogue.characterImage;
        }
        else
        {
            characterImage.sprite = defaultCharacterImage;
        }
        

        ShowCurrentLine();
    }
}
