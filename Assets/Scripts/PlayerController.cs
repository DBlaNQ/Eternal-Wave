using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Gate_Manager gatem;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject shotBullet;
    [SerializeField] Transform shotPoint;
    [SerializeField] Transform firePoint;

    Animator animator;
    Rigidbody rb;

    Vector3 movement;

    float maxHealth = 100f;
    public float health;
    [SerializeField] float speed = 2f;

    bool allowFire = true;
    bool allowRunning = false;
    bool isAtGate = false;

    void Start()
    {
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

        //Gate
        if(Input.GetKeyDown(KeyCode.E) && isAtGate)
        {
            gatem.isOpen = !gatem.isOpen;
        }

        //Shooting
        if (Input.GetButton("Fire1") && allowFire)
        {
            Shoot();
            StartCoroutine(Firerate(.15f));
        }

        //Look Towards Mouse
        LookTowardsMouse();

        //Running
        if(Input.GetKeyDown(KeyCode.LeftShift) && allowRunning)
        {
            speed *= 2;
            animator.SetBool("isRunning", true);
            allowFire = false;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= 2;
            animator.SetBool("isRunning", false);
            allowFire = true;
            if(speed < 1.8)
            {
                speed = 2f;
            }
        }

        //Movement Input
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            gatem = other.GetComponent<Gate_Manager>();
            isAtGate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            isAtGate = false;
        }
    }

    void LookTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        allowFire = false;
        Instantiate(shotBullet, shotPoint.position, shotPoint.rotation);
    }

    IEnumerator Firerate(float time)
    {
        yield return new WaitForSeconds(time);
        allowFire = true;
    }
}
