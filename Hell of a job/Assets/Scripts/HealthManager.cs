using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    // максимальное количество здоровья
    [SerializeField] protected float maxHealth = 100f;
    // текущее количество здоровья
    [SerializeField] protected float currentHealth = 100f;

    // множитель для получаемого урона
    [SerializeField] protected float damageMultiplier = 1f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // получить урон
    public void TakeDamage(float amount)
    {
        currentHealth -= amount * damageMultiplier;
    }

    // восстановить здоровье
    public void Heal(float amount)
    {
        currentHealth += amount;
    }

    // увеличить получаемый урон на %
    public void IncreaseDamageMultiplier(float percentage)
    {
        damageMultiplier += percentage * 0.01f;
    }

    // временно увеличить получаемый урон на %
    public void IncreaseDamageMultiplier(float percentage, float seconds)
    {
        StartCoroutine(TemporarilyIncreaseDamageMultiplier(percentage, seconds));
    }

    // корутина для временного увеличения получаемого урона на %
    IEnumerator TemporarilyIncreaseDamageMultiplier(float percentage, float seconds)
    {
        damageMultiplier += percentage * 0.01f;
        yield return new WaitForSeconds(seconds);
        damageMultiplier -= percentage * 0.01f;
    }

    // уменьшить получаемый урон на %
    public void DecreaseDamageMultiplier(float percentage)
    {
        damageMultiplier -= percentage * 0.01f;
    }

    // временно уменьшить получаемый урон на %
    public void DecreaseDamageMultiplier(float percentage, float seconds)
    {
        StartCoroutine(TemporarilyDecreaseDamageMultiplier(percentage, seconds));
    }

    // корутина для временного уменьшения получаемого урона на %
    IEnumerator TemporarilyDecreaseDamageMultiplier(float percentage, float seconds)
    {
        damageMultiplier -= percentage * 0.01f;
        yield return new WaitForSeconds(seconds);
        damageMultiplier += percentage * 0.01f;
    }
}
