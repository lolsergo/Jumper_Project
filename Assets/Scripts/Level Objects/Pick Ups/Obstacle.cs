using UnityEngine;
using Zenject;

public class Obstacle : LevelObject
{
    // 2. ����������
    [Inject(Optional = true)]
    private readonly Collider2D _collider;
    [Inject(Optional = true)]
    private readonly SpriteRenderer _spriteRenderer;
    protected PlayerHealth _playerHealth;

    [Inject]
    private void Construct(PlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
    }

    // 4. ��������� ����������� (�������������� ������� �����)
    public override void Activate(Vector3 position)
    {
        base.Activate(position); // �������� ������� ���������

        transform.Rotate(0, 0, Random.Range(0f, 360f));
        if (_collider != null) _collider.enabled = true;
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
    }

    // 5. ����������� (������� � ���)
    public override void Deactivate()
    {
        base.Deactivate(); // ������� �����������
        if (_collider != null) _collider.enabled = false; // ��������� ���������
    }

    // 6. ��������� ������������ � �������
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActive) return; // ���� ������ ��������� - ����������

        _speedManager.DecreaseGameSpeed();
        _playerHealth.TakeDamage();
    }
}
