using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    // ������ �� Rigidbody
    Rigidbody2D rb;
    // ������ �� ������
    SpriteRenderer sp;

    // ����� ���� �������� ��� ��������, �������� �� �����
    [SerializeField] LayerMask groundLayerMask;

    // ������� ����������� ��������
    int MoveInput = 0;
    // ����������� ����
    int direction = 0;

    // ������������ ���� � ��������
    [SerializeField] float startdash = 0.25f;
    // �������� ���� � ����/���
    [SerializeField] float dashspeed = 20f;
    // ���������� ��� ������� ������� ����
    float dashtime = 0f;

    // �������� ���� � ����/���
    [SerializeField] float speed;
    // ���� ������
    [SerializeField] float jumpforce;

    // �������� �� �����
    bool IsGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        CheckGrounded();
        CheckMove();
    }

    void Update()
    {
        if (Input.GetButton("Horizontal"))
            Run();
        else
            rb.velocity = new Vector2(0f, rb.velocity.y);

        if (IsGrounded && Input.GetButtonDown("Jump"))
            Jump();
        
        Dash();
    }

    // ��������, �������� �� �����
    private void CheckGrounded()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f, groundLayerMask);
        IsGrounded = collider.Length > 1;
    }

    // �������� �������� ����������� ��������
    private void CheckMove()
    {
        Vector2 dir = transform.right * Input.GetAxisRaw("Horizontal");
        MoveInput = dir.x < 0 ? -1 : dir.x > 0 ? 1 : 0;
    }

    // ���
    private void Run()
    {
        Vector2 dir = transform.right * Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
        sp.flipX = dir.x < 0.0f;
    }

    // ������
    private void Jump()
    {
        rb.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
    }

    // ���
    private void Dash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            direction = (MoveInput != 0) ? MoveInput : (sp.flipX ? -1 : 1);
            dashtime = startdash;
        }

        if (direction == 0)
            return;

        if (dashtime <= 0)
        {
            direction = 0;
            dashtime = startdash;
        }
        else
        {
            dashtime -= Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x + direction * dashspeed, 0f);
        }
    }
}