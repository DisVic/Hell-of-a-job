using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    // ��������� � �������� ����� �������
    [SerializeField] Component component;
    // ����� ������ ���������� component, ������� ������ ����������� ��� �������������� � ��������
    [SerializeField] string methodName;

    // �����, ���������� ��� �������������� � ��������
    public void Activate()
    {
        component.SendMessage(methodName);
    }
}
