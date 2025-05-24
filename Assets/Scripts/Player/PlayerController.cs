using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Configurações de movimento
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumpCount = 2;

    // Componentes
    private Rigidbody2D rb;
    private Animator anim;

    // Variáveis de estado
    private float moveInput;
    private int jumpCount = 0;
    private bool isGrounded = false;
    private bool isDead = false;

    // Ataque
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    // Sistema de vida
    [Header("Configurações de Vida")]
    public float maxHealth = 100f;
    private float currentHealth;
    private float damageAmount = 30f;

    // Referência para a barra de vida (UI)
    [Header("Referência da UI da Vida")]
    public Image healthBarImage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        if (isDead) return; // Impede ações se estiver morto

        HandleMovement();
        HandleJump();
        HandleAttack();

        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(damageAmount);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(10f);
            Debug.Log("Recuperando vida!");
        }
    }

    void HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("IsRun", moveInput != 0 && isGrounded);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }

        anim.SetBool("IsJump", !isGrounded && rb.velocity.y > 0.1f);
        anim.SetBool("IsFall", !isGrounded && rb.velocity.y < -0.1f);
    }

    void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;
            anim.SetTrigger("IsAttack");
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);
        Debug.Log("Vida reduzida! Vida atual: " + currentHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBarImage == null) return;

        float healthPercentage = currentHealth / maxHealth;
        healthBarImage.fillAmount = Mathf.Clamp01(healthPercentage);
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // Impede movimentação física
        anim.Play("Death"); // Toca a animação diretamente
        Debug.Log("Player morreu!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
