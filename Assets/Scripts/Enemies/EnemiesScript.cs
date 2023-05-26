using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemiesScript : MonoBehaviour
{
    public int maxHealth = 20;
    public int dano;
    public int currentHealth;
    public GameObject fxDie;
    public GameObject fxHit;
    public GameObject barril;
    public Transform whereToAddEffect;
    Transform customWhereToAdd;
    PlayerHealth myHealthSystem;
    public float knockbackForce;
    public float knockbackForceUp;
    public bool knockbacked;
    Rigidbody2D rb;
    public ScreenShakeController screenShake;

    public SimpleFlash flash;

    public bool isEnemyDestroy1Hit;

    private CinemachineVirtualCamera activeVirtualCamera;


    private void Awake()
    {
        myHealthSystem = GameObject.FindObjectOfType<PlayerHealth>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        screenShake = activeVirtualCamera.gameObject.GetComponent<ScreenShakeController>();
        

    }
    private void Update()
    {

        CinemachineVirtualCamera newActiveCamera = CheckCameraActivation();
        if (newActiveCamera != activeVirtualCamera)
        {
            activeVirtualCamera = newActiveCamera;

            screenShake = activeVirtualCamera.gameObject.GetComponent<ScreenShakeController>();
        }


        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "BossLevel")
        {
            screenShake = GameObject.FindGameObjectWithTag("BossCam").GetComponent<ScreenShakeController>();
        }
    }
    private CinemachineVirtualCamera CheckCameraActivation()
    {
        // Verificar o estado de ativa��o de todas as c�meras na cena
        CinemachineVirtualCamera[] allVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (CinemachineVirtualCamera virtualCamera in allVirtualCameras)
        {
            if (virtualCamera.enabled)
            {
                return virtualCamera;
            }
        }
        return null;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Instantiate(fxHit, whereToAddEffect.position, Quaternion.identity);
        if (flash != null)
        {
            flash.Flash();
            screenShake.startShake(.35f, 0.5f);
        }
        Debug.Log("Damage!");
        if (currentHealth <= 0)
        {

            Die();
        }
    }
    void Die()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;



        if (screenShake != null)
        {
            screenShake.startShake(.5f, 0.7f);
        }
        if (sceneName == "BossLevel")
        {
            Instantiate(barril, whereToAddEffect.position, Quaternion.identity);
        }
        Instantiate(fxDie, whereToAddEffect.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && dano > 0)
        {
            if (!isEnemyDestroy1Hit)
            {

                var player = GameObject.FindGameObjectWithTag("PlayerManager").GetComponentInChildren<knockbackPlayer>();
                if (player != null)
                {
                    player.forceY = 15f;
                    player.forceX = 100f;
                    player.knockback();
                }
                myHealthSystem.Hit(dano);


            }
            else
            {
                var player = GameObject.FindGameObjectWithTag("PlayerManager").GetComponentInChildren<knockbackPlayer>();
                if (player != null)
                {
                    screenShake.startShake(.5f, 0.4f);
                    Instantiate(fxDie, whereToAddEffect.position, Quaternion.identity);
                    Destroy(gameObject);
                    player.forceY = 15f;
                    player.forceX = 100f;
                    player.knockback();
                    myHealthSystem.Hit(dano);


                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && dano > 0)
        {
            if (!isEnemyDestroy1Hit)
            {

                var player = GameObject.FindGameObjectWithTag("PlayerManager").GetComponentInChildren<knockbackPlayer>();
                if (player != null)
                {
                    player.forceY = 15f;
                    player.forceX = 100f;
                    player.knockback();
                }
                myHealthSystem.Hit(dano);


            }
            else
            {
                var player = GameObject.FindGameObjectWithTag("PlayerManager").GetComponentInChildren<knockbackPlayer>();
                if (player != null)
                {
                    screenShake.startShake(.5f, 0.4f);
                    Instantiate(fxDie, whereToAddEffect.position, Quaternion.identity);
                    Destroy(gameObject);
                    player.forceY = 15f;
                    player.forceX = 100f;
                    player.knockback();
                    myHealthSystem.Hit(dano);


                }
            }
        }

    }
    public void knockback()
    {
        Transform attacker = getClosestDamageSource();
        Vector2 knockbackDirection = new Vector2(transform.position.x - attacker.position.x, 0);
        rb.AddForce(new Vector2(knockbackDirection.x, knockbackForceUp) * knockbackForce);
    }
    public Transform getClosestDamageSource()
    {
        GameObject[] DamageSources = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform currentClosestDamageSource = null;
        foreach (GameObject go in DamageSources)
        {
            float currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                currentClosestDamageSource = go.transform;
            }
        }
        return currentClosestDamageSource;
    }
}