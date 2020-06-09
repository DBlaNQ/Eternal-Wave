using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Gate_Manager gatem;

    TextMeshProUGUI currAmmo;
    TextMeshProUGUI ammoPacks;

    GameObject ammoPack = null;
    GameObject healthPack = null;
    GameObject healthBar;
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject healthParticles = null;
    [SerializeField] GameObject bulletPrefab = null;
    [SerializeField] GameObject shotBullet = null;
    [SerializeField] Transform shotPoint = null;
    [SerializeField] Transform firePoint = null;

    Animator animator;
    Rigidbody rb;

    Vector3 movement = new Vector3 (0,0,0);

    float maxHealth = 100f;
    public float health;
    int healthPacks = 0;
    [SerializeField] float speed = 2f;

    int maxAmmo = 25;
    int currentAmmo = 25;
    int ammo = 35;
    int ammoNeeded = 0;

    public bool paused = false;
    bool healthPackAvailable = false;
    bool ammoPackAvailable = false;
    bool allowFire = true;
    bool allowRunning = false;
    bool isRunning = false;
    bool isAtGate = false;
    bool isReloading = false;

    void Start()
    {
        healthBar = GameObject.Find("HealthBar").gameObject;
        healthBar.GetComponent<HealthBar>().SetMaxHealth(maxHealth);
        currAmmo = GameObject.Find("CurrentAmmo").GetComponent<TextMeshProUGUI>();
        ammoPacks = GameObject.Find("Ammo").GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Walking and Idle Animation
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            animator.SetBool("isWalking", true);
            allowRunning = true;
        }
        else if(!Input.GetButton("Horizontal") || !Input.GetButton("Vertical"))
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            allowRunning = false;
        }

        //Pause Menu
        if (Input.GetButtonDown("Cancel"))
        {
            if (!paused)
            {
                pauseMenu.SetActive(true);
                paused = true;
                Time.timeScale = 0f;
            }
            else
            {
                pauseMenu.SetActive(false);
                paused = false;
                Time.timeScale = 1f;
            }
        }

        //Use\Pick
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (isAtGate)
            {
                gatem.isOpen = !gatem.isOpen;
            }
            else if (healthPackAvailable && healthPack != null)
            {
                healthPacks += 1;
                Destroy(healthPack);
                healthPack = null;
                healthPackAvailable = false;
            }
            else if (ammoPackAvailable && ammoPack != null)
            {
                ammo += 25;
                Destroy(ammoPack);
                ammoPack = null;
                ammoPackAvailable = false;
            }
        }

        //Reloading
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (ammo > 0)
            {
                isReloading = true;
                StartCoroutine(Reloading(1f));
            }
        }

        //Healing
        if (Input.GetKeyDown(KeyCode.H) && healthPacks > 0)
        {
            if (health > maxHealth)
            {
                health = maxHealth;
                Instantiate(healthParticles, transform.position, Quaternion.identity);
                healthPacks -= 1;
            }
            else
            {
                health += 25f;
                Instantiate(healthParticles, transform.position, Quaternion.identity);
                healthPacks -= 1;
            }

        }

        //Running
        if(Input.GetKeyDown(KeyCode.LeftShift) && allowRunning)
        {
            speed *= 2;
            animator.SetBool("isRunning", true);
            allowFire = false;
            isRunning = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= 2;
            animator.SetBool("isRunning", false);
            if (!isReloading)
            {
                allowFire = true;
            }
            isRunning = false;
            if(speed < 1.8)
            {
                speed = 2f;
            }
        }

        //Movement Input
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        //UI
        healthBar.GetComponent<HealthBar>().SetHealthPacks(healthPacks);
        healthBar.GetComponent<HealthBar>().SetHealth(health);
        ammoPacks.text = ammo.ToString();
        currAmmo.text = currentAmmo.ToString();
    }

    private void FixedUpdate()
    {
        //Look Towards Mouse
        LookTowardsMouse();
        
        //Shooting
        if (Input.GetButton("Fire1") && allowFire)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                StartCoroutine(Firerate(.15f));
            }
        }

        rb.velocity = movement * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            gatem = other.GetComponent<Gate_Manager>();
            isAtGate = true;
        }
        else if (other.CompareTag("HealthPack"))
        {
            healthPackAvailable = true;
            healthPack = other.gameObject;
        }
        else if (other.CompareTag("AmmoPack"))
        {
            ammoPackAvailable = true;
            ammoPack = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            isAtGate = false;
        }
        else if (other.CompareTag("HealthPack"))
        {
            healthPackAvailable = false;
            healthPack = null;
        }
        else if (other.CompareTag("AmmoPack"))
        {
            ammoPackAvailable = false;
            ammoPack = null;
        }
    }

    void LookTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

       /* if(Physics.Raycast(ray, out hit))
        {
        }*/
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        allowFire = false;
        Instantiate(shotBullet, shotPoint.position, shotPoint.rotation);
        currentAmmo -= 1;
    }

    IEnumerator Firerate(float time)
    {
        yield return new WaitForSeconds(time);
        if(!isRunning && !isReloading)
        allowFire = true;
    }

    IEnumerator Reloading(float time)
    {
        yield return new WaitForSeconds(time);
        allowFire = true;
        ammoNeeded = 0;
        ammoNeeded = (currentAmmo - maxAmmo) * (-1);
        if (ammo < ammoNeeded)
        {
            currentAmmo += ammo;
            ammo = 0;
        }
        else
        {
            currentAmmo += ammoNeeded;
            ammo -= ammoNeeded;
        }
        isReloading = false;
    }
}
