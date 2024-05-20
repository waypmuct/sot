using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ThrowingKnives : MonoBehaviour
{

    // objects & prefab
    [Header("References")]
    public Transform direction;
    public Transform attackPoint;
    public GameObject objectToThrow;

    // number of throws and cd
    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    // force and ballistic
    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public UnityEvent animEvent;
    public float throwForce;
    public float throwUpwardForce;

    bool readyToThrow;

    private void Start()
    {
        readyToThrow = true; // reset cd
    }

    private void Update() // check if keypressed and cd is true and char have knives
    {
        if(Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            StartCoroutine(Wait());
        }
    }

    // a lil input before knife ll go, it s need to look more good with anim
    IEnumerator Wait()
    {
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, direction.rotation, 1.0f * Time.deltaTime); //unused old
        transform.rotation = Quaternion.Euler(0, direction.transform.localRotation.eulerAngles.y, 0); // set char to camera's dir
        animEvent.Invoke(); // we need to use anim before throw, its from another script system
        yield return new WaitForSeconds(1/2); // input wait is 0.5 secs
        Throw();
    }

    private void Throw()
    {   
        readyToThrow = false; // cd is now no yet

        // creatin object from chosen prefab
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, direction.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction of object
        Vector3 forceDirection = direction.transform.forward;

        // creatin ray to navigate
        RaycastHit hit;

        if(Physics.Raycast(direction.position, direction.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force to object
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        // now char have less knives
        totalThrows--;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}