using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    // ��������� � ����������� ��������������
    [SerializeField] GameObject hint;

    // �������� �� ��������������
    bool canInteract = false;
    // ������ ���� ������������� ��������, � �������� �������� ����������������� � ������ ������
    List<GameObject> interactiveOverlaps = new List<GameObject>();

    void Update()
    {
        if (canInteract && Input.GetButtonDown("Interact"))
        {
            foreach (GameObject obj in interactiveOverlaps)
                obj.GetComponent<Interactive>().Activate();
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Interactive"))
        {
            interactiveOverlaps.Add(col.gameObject);

            canInteract = true;
            hint.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Interactive"))
        {
            interactiveOverlaps.Remove(col.gameObject);

            if (interactiveOverlaps.Count == 0)
            {
                canInteract = false;
                hint.SetActive(false);
            }
        }
    }
}
