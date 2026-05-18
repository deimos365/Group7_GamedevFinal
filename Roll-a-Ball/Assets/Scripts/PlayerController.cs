using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 5f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public int maxHP = 100;
    public int currentHP;
    public UnityEngine.UI.Slider healthBar;
    public TextMeshProUGUI healthText;
    private bool invulnerable = false;
    public TextMeshProUGUI abilityText;
    private float abilityTimer = 0f;
    private string currentAbility = "";
    public UnityEngine.UI.Image screenTint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;
        SetCountText();
        winTextObject.SetActive(false);

        currentHP = maxHP;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHP;
            healthBar.value = currentHP;
            healthText.text = "HP: " + currentHP.ToString();
        }

    }
    void Update()
    {
        if (abilityTimer > 0)
        {
            abilityTimer -= Time.deltaTime;

            if (abilityTimer <= 0)
            {
                abilityTimer = 0;
                currentAbility = "";
            }
        }

        UpdateAbilityUI();
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 11)
        {
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Win!";

            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            this.enabled = false;
        }
    }
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement*speed);

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        if (other.CompareTag("AbilityOrb"))
        {
            int roll = Random.Range(1, 11); 
            if (roll <= 2) 
            {
                StartCoroutine(Invulnerability());
            }
            else
            {
                int ability = Random.Range(0, 2);
                if (ability == 0) StartCoroutine(SpeedBoost());
                else StartCoroutine(SlowEnemies());
            }
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Spike"))
        {
            if (!invulnerable)
            {
                TakeDamage(5);
            }
        }


    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
            currentHP = 0;

        if (healthBar != null)
        {
            healthBar.value = currentHP;
            healthText.text = "HP: " + currentHP.ToString();
        }

        if (currentHP <= 0)
        {
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";

            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            this.enabled = false;
        }
    }

    private float enemyDamageCooldown = 1f; 
    private float lastEnemyHitTime = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!invulnerable && Time.time > lastEnemyHitTime + enemyDamageCooldown)
            {
                TakeDamage(10);
                lastEnemyHitTime = Time.time;
            }
        }
    }

    IEnumerator SpeedBoost()
    {
        float originalSpeed = speed;
        speed *= 1.5f;

        currentAbility = "Speed Boost";
        abilityTimer = 10f;

        SetTint(new Color(0f, 0.5f, 1f, 0.25f)); // blue glow

        yield return new WaitForSecondsRealtime(10f);

        speed = originalSpeed;

        ClearTint();
    }

    IEnumerator Invulnerability()
    {
        invulnerable = true;

        currentAbility = "Invulnerability";
        abilityTimer = 10f;

        SetTint(new Color(1f, 1f, 0f, 0.25f)); // yellow

        yield return new WaitForSecondsRealtime(10f);

        invulnerable = false;

        ClearTint();
    }

    IEnumerator SlowEnemies()
    {
        currentAbility = "Enemy Slow";
        abilityTimer = 10f;

        SetTint(new Color(0.5f, 0f, 1f, 0.25f)); // purple

        foreach (EnemyMovement e in Object.FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None))
        {
            if (e.TryGetComponent(out UnityEngine.AI.NavMeshAgent agent))
            {
                agent.speed *= 0.5f;
            }
        }

        yield return new WaitForSecondsRealtime(10f);

        foreach (EnemyMovement e in Object.FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None))
        {
            if (e.TryGetComponent(out UnityEngine.AI.NavMeshAgent agent))
            {
                agent.speed *= 2f;
            }
        }

        ClearTint();
    }
    void UpdateAbilityUI()
    {
        if (abilityTimer > 0)
        {
            abilityText.gameObject.SetActive(true);

            abilityText.text = currentAbility + " : " + abilityTimer.ToString("F1") + "s";
        }
        else
        {
            abilityText.gameObject.SetActive(false);
        }
    }
    void SetTint(Color color)
    {
        if (screenTint != null)
        {
            screenTint.color = color;
        }
    }
    void ClearTint()
    {
        if (screenTint != null)
        {
            screenTint.color = new Color(0, 0, 0, 0);
        }
    }

}
