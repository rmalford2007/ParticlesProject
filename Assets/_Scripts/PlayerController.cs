using UnityEngine;
using System.Collections;

public delegate void PlayerEventHandler();

public class PlayerController : MonoBehaviour {

    public event PlayerEventHandler PlayerDeath;

    public KeyCode forwardControl = KeyCode.W;
    public KeyCode backwardControl = KeyCode.S;
    public KeyCode leftControl = KeyCode.A;
    public KeyCode rightControl = KeyCode.D;
    public KeyCode fireControl = KeyCode.Space;

    public float forwardSpeed = 5.0f;
    public float backwardSpeed = 3.0f;
    public float rotationSpeed = 5.0f;
    public float maxVelocity = 25.0f;

    public Transform bulletSpawner;
    public GameObject bulletPrefab;
    public float reloadTime = 1.0f;
    public float bulletSpeed = 20.0f;
    public float bulletExpire = 5.0f;

    public Stat healthStat;

    public GameObject playerDisplay;

    bool bForward = false;
    bool bBackward = false;
    bool bRotateLeft = false;
    bool bRotateRight = false;
    bool bFire = false;
    bool bDead = false;
    bool isPaused = false;

    Rigidbody2D playerBody;

    
    bool bulletReloaded = true;
	// Use this for initialization
	void Start () {
        playerBody = GetComponent<Rigidbody2D>();
        healthStat.StatChanged += new StatInteractionHandler(CheckForDeath);
	}
	
	// Update is called once per frame
	void Update () {
        bForward = Input.GetKey(forwardControl);
        bBackward = Input.GetKey(backwardControl);
        bRotateLeft = Input.GetKey(leftControl);
        bRotateRight = Input.GetKey(rightControl);
        bFire = Input.GetKey(fireControl);

        if (!bDead)
        {
            MovePlayer();
            if (bFire)
            {
                FireWeapon();
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;
                GameManager.theManager.SetPause(isPaused);
            }
        }
    }

    public void ResetRespawn()
    {
        playerBody.velocity = Vector2.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        healthStat.SetToMax();
        bDead = false;
        bulletReloaded = true;
        StopAllCoroutines();
        //Collider2D nextCollider = GetComponent<Collider2D>();
        //if (nextCollider)
        //    nextCollider.enabled = true;
        playerDisplay.SetActive(true);
    }

    IEnumerator ReloadGun(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        bulletReloaded = true;
        yield return null;
    }

    void OnPlayerDeath()
    {
        bDead = true;
        StopAllCoroutines();
        //Collider2D nextCollider = GetComponent<Collider2D>();
        //if (nextCollider)
        //    nextCollider.enabled = false;
        if (playerDisplay)
        {
            playerDisplay.SetActive(false);
        }

        if (PlayerDeath != null)
        {
            PlayerDeath();
        }
    }

    void CheckForDeath(Stat checkStat)
    {
        if(checkStat.current <= 0)
        {
            //health is 0, kill object
            //Destroy(gameObject);
            OnPlayerDeath();
        }
    }

    void FireWeapon()
    {
        if(bulletReloaded)
        {
            //refire is ready, shoot a bullet
            GameObject firedBullet = Instantiate(bulletPrefab, bulletSpawner.position, bulletSpawner.rotation) as GameObject;
            Rigidbody2D bulletBody = firedBullet.GetComponent<Rigidbody2D>();
            if(bulletBody)
            {
                bulletBody.velocity = playerBody.velocity;
                bulletBody.AddForce(bulletSpawner.up * bulletSpeed, ForceMode2D.Impulse);
            }
            Destroy(firedBullet, bulletExpire);
            bulletReloaded = false;
            StartCoroutine(ReloadGun(reloadTime));
        }
    }

    void MovePlayer()
    {
        if(bRotateLeft)
        {
            transform.Rotate(new Vector3(0, 0, 1) * rotationSpeed * Time.deltaTime);
        }
        if (bRotateRight)
        {
            transform.Rotate(new Vector3(0, 0, 1) * -rotationSpeed * Time.deltaTime);
        }
        if (bForward)
        {
            playerBody.AddForce(transform.up * forwardSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
        if(bBackward)
        {
            playerBody.AddForce(-transform.up * backwardSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        if (playerBody.velocity.magnitude > maxVelocity)
        {
            //clamp velocity to maxVelocity
            playerBody.velocity -= playerBody.velocity * ((playerBody.velocity.magnitude - maxVelocity) / playerBody.velocity.magnitude);
        }
        //print(playerBody.velocity.magnitude.ToString());
    }

    void OnParticleCollision(GameObject other)
    {
        //getting hit by an enemy body
        if(healthStat)
            healthStat.IncrementBy(-5);
    }
}
