using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ThrowingKnives : MonoBehaviour
{

    // objects & prefab
    [Header("References")]
    [SerializeField]
    private Transform Direction;
    [SerializeField]
    private Transform AttackPoint;
    [SerializeField]
    private GameObject ObjectToThrow;

    // number of throws and cd
    [Header("Settings")]
    [SerializeField]
    private int TotalThrows;
    [SerializeField]
    private float ThrowCooldown;
    [SerializeField]
    public bool ThrowEnabled = true;

    // force and ballistic
    [Header("Throwing")]
    [SerializeField]
    private KeyCode ThrowKey = KeyCode.Mouse0;
    [SerializeField]
    private UnityEvent AnimEvent;
    [SerializeField]
    private float ThrowForce;
    [SerializeField]
    private float ThrowUpwardForce;

    bool ReadyToThrow;
    int BeforeDelay = 0;

    private void Start()
    {
        ReadyToThrow = true; // reset cd
    }

    public void AddKnives(int value) // can be used when player picks up
    {
        TotalThrows += value;
    }

    public void DelKnives() // can be used after char's death
    {
        TotalThrows = 0;
    }

    public void Switch(bool State) // disable or enable throwing
    {
        ThrowEnabled = State;
    }


    private void Update() // check if keypressed and cd is true and char have knives
    {
        if (Input.GetKeyDown(ThrowKey) && ReadyToThrow && TotalThrows > 0 && ThrowEnabled)
        {
            StartCoroutine(Wait());
        }
    }

    // a lil input before knife ll go, it s need to look more good with anim
    private IEnumerator Wait()
    {
        transform.rotation = Quaternion.Euler(0, Direction.transform.localRotation.eulerAngles.y, 0); // set char to camera's dir
        AnimEvent.Invoke(); // we need to use anim before throw, its from another script system
        yield return new WaitForSeconds(BeforeDelay); // input wait is BeforeDelay secs
        Throw();
    }

    private void Throw()
    {
        ReadyToThrow = false; // cd is now no yet

        // creatin object from chosen prefab
        GameObject projectile = Instantiate(ObjectToThrow, AttackPoint.position, Direction.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction of object
        Vector3 forceDirection = Direction.transform.forward;

        // add force to object
        Vector3 forceToAdd = forceDirection * ThrowForce + transform.up * ThrowUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        // now char have less knives
        TotalThrows--;

        // implement ThrowCooldown
        Invoke(nameof(ResetThrow), ThrowCooldown);
    }

    private void ResetThrow()
    {
        ReadyToThrow = true;
    }
}