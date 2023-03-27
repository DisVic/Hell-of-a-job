using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // ������ �� �������� ��������
    DialogueManager dialogueManager;

    // ���, ������������ � ���������� ����
    [SerializeField] string displayName;
    // ������ ��������
    [SerializeField] List<Dialogue> dialogues = new List<Dialogue>();
    // ������ �������� �������
    int currentDialogue = 0;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // ������ ������� � ������ NPC
    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(displayName, dialogues[currentDialogue]);

        if (currentDialogue < dialogues.Count - 1)
            currentDialogue++;
    }
}

