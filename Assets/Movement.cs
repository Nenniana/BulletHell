using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private LayerMask dashLayerMask;
    [SerializeField]
    private LayerMask explodeLayerMask;
    [SerializeField]
    private GameObject youLost;
    [SerializeField]
    private GameObject youWon;

    private new Rigidbody2D rigidbody2D;
    private Vector3 moveDir;
    private float fixedDeltaTime;
    private bool isStopTime;
    private bool won;
    private bool lost;

    public float MOVE_SPEED;
    public float MOVE_SPEED_ORIGINAL;
    public float MOVE_SPEED_SLOW;

    private AbilityState dashState;
    private bool isDashButtonDown;
    public float DASH_AMOUNT;
    public float DASH_TIMER;
    public float MAX_DASH_TIME = 3f;

    private AbilityState explodeState;
    private bool isExplodeButtonDown;
    public float EXPLODE_RADIUS = 3;
    public float EXPLODE_TIMER;
    public float MAX_EXPLODE_TIME = 3f;

    private AbilityState slowState;
    private bool isSlowButtonDown;
    public float SLOW_USING_TIME = 1f;

    public Vector2 Velocity;
    public GameObject explosionPrefab;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        this.fixedDeltaTime = Time.fixedDeltaTime;
        MOVE_SPEED_ORIGINAL = MOVE_SPEED;
        youLost.SetActive(false);
        youWon.SetActive(false);
    }

    private void Start()
    {
        isStopTime = false;
        Time.timeScale = 1;
    }

    private void Explode ( )
    {
        explosionPrefab = GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        ParticleSystem exp = explosionPrefab.GetComponent<ParticleSystem>();
        exp.textureSheetAnimation.SetSprite(0, spriteRenderer.sprite);
        spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((explodeLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            //collision.GetComponent<ProjectileControl>().ExplodeDisable();
            Explode();

            isStopTime = true;
        }
        else if (collision.CompareTag("Goal"))
        {
            won = true;
            isStopTime = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isStopTime)
        {
            Time.timeScale = 0.99f * Time.timeScale;

            if (Time.timeScale <= 0.01f)
            {
                if (!won)
                {
                    lost = true;
                    youLost.SetActive(true);
                } else
                {
                    youWon.SetActive(true);
                }
                
            }

            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && dashState == AbilityState.Ready)
        {
            isDashButtonDown = true;
        }

        if (Input.GetKeyDown(KeyCode.O) && explodeState == AbilityState.Ready)
        {
            isExplodeButtonDown = true;
        }

        if (Input.GetKeyDown(KeyCode.I) && slowState == AbilityState.Ready)
        {
            isSlowButtonDown = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.fixedDeltaTime = this.fixedDeltaTime;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FixedUpdate()
    {
        Velocity = rigidbody2D.velocity;
        rigidbody2D.velocity = (MOVE_SPEED * moveDir);

        // DASH
        switch (dashState)
        {
            case AbilityState.Ready:
                if (isDashButtonDown)
                {
                    isDashButtonDown = false;
                    Vector3 dashPostion = transform.position + moveDir * DASH_AMOUNT;

                    RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, moveDir, DASH_AMOUNT, dashLayerMask);
                    if (raycastHit2D.collider != null)
                    {
                        dashPostion = raycastHit2D.point;
                    }

                    RaycastHit2D[] raycastHit2Ds = Physics2D.BoxCastAll(transform.position, new Vector2(1.3f,1.3f), 0f, moveDir, DASH_AMOUNT);

                    foreach (var hitCollider in raycastHit2Ds)
                    {
                        if (hitCollider.collider.CompareTag("Projectile"))
                        {
                            hitCollider.collider.GetComponent<ProjectileControl>().ExplodeDisable();
                        }
                    }

                    rigidbody2D.MovePosition(dashPostion);
                    dashState = AbilityState.Using;
                }
                break;
            case AbilityState.Using:
                DASH_TIMER += Time.deltaTime * 3;
                if (DASH_TIMER >= MAX_DASH_TIME)
                {
                    DASH_TIMER = MAX_DASH_TIME;
                    dashState = AbilityState.Cooldown;
                }
                break;
            case AbilityState.Cooldown:
                DASH_TIMER -= Time.deltaTime;
                if (DASH_TIMER <= 0)
                {
                    DASH_TIMER = 0;
                    dashState = AbilityState.Ready;
                }
                break;
        }

        // Explode
        switch (explodeState)
        {
            case AbilityState.Ready:
                if (isExplodeButtonDown)
                {
                    isExplodeButtonDown = false;

                    ExplosionDamage(gameObject.transform.position, EXPLODE_RADIUS);
                    explodeState = AbilityState.Using;
                }
                break;
            case AbilityState.Using:
                EXPLODE_TIMER += Time.deltaTime * 3;
                if (EXPLODE_TIMER >= MAX_EXPLODE_TIME)
                {
                    EXPLODE_TIMER = MAX_EXPLODE_TIME;
                    explodeState = AbilityState.Cooldown;
                }
                break;
            case AbilityState.Cooldown:
                EXPLODE_TIMER -= Time.deltaTime;
                if (EXPLODE_TIMER <= 0)
                {
                    EXPLODE_TIMER = 0;
                    explodeState = AbilityState.Ready;
                }
                break;
        }

        // SLOW
        switch (slowState)
        {
            case AbilityState.Ready:
                if (isSlowButtonDown)
                {
                    isSlowButtonDown = false;
                    Time.timeScale = 0.1f;
                    Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
                    MOVE_SPEED = MOVE_SPEED_SLOW;
                    slowState = AbilityState.Using;
                }
                break;
            case AbilityState.Using:
                EXPLODE_TIMER += Time.deltaTime * 3;
                if (EXPLODE_TIMER >= SLOW_USING_TIME)
                {
                    if (Time.timeScale < 1)
                    {
                        Time.timeScale += 0.1f;
                        MOVE_SPEED += ((MOVE_SPEED_ORIGINAL - MOVE_SPEED_SLOW) / 9);
                    }

                    Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

                }
                if (EXPLODE_TIMER >= MAX_EXPLODE_TIME)
                {
                    EXPLODE_TIMER = MAX_EXPLODE_TIME;
                    slowState = AbilityState.Cooldown;
                }
                break;
            case AbilityState.Cooldown:
                EXPLODE_TIMER -= Time.deltaTime;
                if (EXPLODE_TIMER <= 0)
                {
                    EXPLODE_TIMER = 0;
                    slowState = AbilityState.Ready;
                }
                break;
        }
    }

    void ExplosionDamage(Vector2 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius, explodeLayerMask);

        foreach (var hitCollider in hitColliders)
        {
            hitCollider.GetComponent<ProjectileControl>().ExplodeDisable();
        }
    }
}



public enum AbilityState
{
    Ready,
    Using,
    Cooldown
}