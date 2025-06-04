using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    [Header("UI 요소 -  Inspector 에서 연결")]
    public GameObject DialoguePanel;            //대화창 전체 패널 
    public Image characterImage;                //캐릭터 이미지
    public TextMeshProUGUI characternameText;   //캐릭터 이름
    public TextMeshProUGUI dialogueText;        //대화 내용 표시 버튼
    public Button nextButton;                   //다음 버튼

    [Header("기본설정")]
    public Sprite defaultCharacterImage;        //캐릭터 기본 이미지

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;           //글자 하나당 풀력 속도
    public bool skipTypingOnClick = true;       //클릭 시 타이핑 즉시 완료 여부

    //내부 변수들
    private DialogueDataSO currentDialogue;     //현재 대화중인 대화 데이터
    private int currentLineIndex = 0;           //현재 몇 번째 대화 중인지 (0 부터 시작)
    private bool isDialogueActive = false;      //대화 진행중인지 확인 하는 플래그
    private bool isTyping = false;              //현재 타이핑 효과가 진행 중인지 확인
    private Coroutine typingCoroutine;          //타이핑 효과 코루티 참조 (중지용)



    // Start is called before the first frame update
    void Start()
    {
        DialoguePanel.SetActive(false);         //대화창 숨기기
        nextButton.onClick.AddListener(HandIeNextInput);    //"다음" 버튼에 새로운 입력 처리 연결
    }

    // Update is called once per frame
    void Update()
    {
        if(isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandIeNextInput();          //다음 입력 처리 (타이핑 중이면 완료,아니면 다음 줄)
        }
    }

    IEnumerator TypeText(string textToType)             //타이핑 할 전체 텍스트
    {
        isTyping = true;                                //타이핑 시작
        dialogueText.text = "";                         //타이핑 초기화
                
        for(int i = 0; i< textToType.Length; i++)       //텍스트를 한 글자씩 추가
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);   //대기 시간 설정
        }

        isTyping = false;               //타이핑 완료
    }

    private void CompleteTyping()           //타이핑 효과를 즉시 완료하는 함수
    {
        if(typingCoroutine != null)         
        {
            StopCoroutine(typingCoroutine);     //코루틴 중지
        }
        isTyping = false;           //타이핑 사태 해제

        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];           //현재 줄의 전체 텍스트를 즉시 표시
        }
    }


    void ShowCurrentLine()      //현재 대화 둘의 내용을 타이핑 효과와 함께 화면에 표시하는 함수
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)  //대화 인덱스다 유효한지 확인
        {
            if(typingCoroutine != null)                     //이전 타이핑 효과가 있다면 중기
            {
                StopCoroutine(typingCoroutine);     //코루틴 중지
            }
        }
        //현재 줄의 대화 내용으로 타이핑 시작
        string currentText = currentDialogue.dialogueLines[currentLineIndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }

    void EndDialogue()              //대화를 완전히 종료 하는 함수
    {
        if (typingCoroutine != null)        //타이핑 효과 정리
        {
        StopCoroutine(typingCoroutine);
        typingCoroutine = null;
        }
        isDialogueActive = false;           //대화장 비활성화
        isTyping = false;                   //타이핑 상태 해제
        DialoguePanel.SetActive(false);     //대화창 숨기기
        currentLineIndex = 0;               //인덱스 초기화

    }

    public void ShowNextLine()              //다음 대호하 줄로 이동 시키는 함수 (타이핑 완료된 후에만 호출)
    {
        currentLineIndex++;                 //다음 줄로 인덱스 증가

        if(currentLineIndex >= currentDialogue.dialogueLines.Count)     //마지막 대화였는지 확인
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();                                          //대화가 남아있으면 다음줄 표시
        }
    }
    public void HandIeNextInput()                   //스페이스바나 버튼 클릭 시 호출 되는 입력 처리 함수 
    {
        if(isTyping && skipTypingOnClick)
        {
            CompleteTyping();               //타이핑 중이라면 즉시 완료
        }
        else if(!isTyping)
        {
            ShowNextLine();                 //타이핑 완료 상태라며 다음 줄 표시
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
