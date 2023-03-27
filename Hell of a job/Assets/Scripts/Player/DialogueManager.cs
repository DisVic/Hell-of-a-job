using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // ������ �� �������� ����������� ����
    [SerializeField] Animator animator;
    // ������ �� �����, ������������ ��� NPC
    [SerializeField] Text nameText;
    // ������ �� �����, ������������ ������� �������
    [SerializeField] Text dialogueText;

    // ������������ �� ������ ��������� ��������
    [SerializeField] bool useTyping = true;
    // ����� ��������� ���������� ������� � ��������
    [SerializeField] float timeBetweenTypedCharacters = 0.05f;

    // ������ �� ������ ��������
    Person movementScript;
    // ������ �� �������� ��������������
    InteractionManager interactionManager;

    // ������� ����������� �������� �������
    Queue<string> sentences = new Queue<string>();

    void Start()
    {
        movementScript = GetComponent<Person>();
        interactionManager = GetComponent<InteractionManager>();
    }

    void Update()
    {
        if (animator.GetBool("isOpen") && Input.GetButtonDown("Interact"))
            DisplayNextSentence();
    }

    // ������ �������
    public void StartDialogue(string npcName, Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        movementScript.enabled = false;
        interactionManager.enabled = false;

        nameText.text = npcName;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    // ����������� ��������� ������� � ���������� ����
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        StopAllCoroutines();

        if (useTyping)
            StartCoroutine(TypeSentence(sentences.Dequeue()));
        else
            dialogueText.text = sentences.Dequeue();
    }

    // �������� ��� ������� ��������� ��������
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        
        foreach (char ch in sentence.ToCharArray())
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(timeBetweenTypedCharacters);
            // �� ���������� "yield return null", ������ ��� ��� ����� �������
            // �������� �������� ��������� ����� �������� �� fps
        }
    }

    // ��������� �������
    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        movementScript.enabled = true;
        interactionManager.enabled = true;
    }
}
