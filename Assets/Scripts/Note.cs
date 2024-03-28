using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Note : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody2D noteRigidbody;
    private Collider2D noteCollider;

    private Camera mainCamera;

    public int points = 1;

    private bool isOffScreen;
    private bool slicable = false;

    private void Awake() {
        noteRigidbody = GetComponent<Rigidbody2D>();
        noteCollider = GetComponent<Collider2D>();
        mainCamera = Camera.main;
    }

    private void Update() {
        // Destroy the note if it goes off the screen, but it starts below the screen, so we need to check if it goes above the screen
        if (transform.position.y > mainCamera.ViewportToWorldPoint(Vector3.zero).y && !isOffScreen) {
            isOffScreen = true;
        }

        if (transform.position.y < mainCamera.ViewportToWorldPoint(Vector3.zero).y && isOffScreen) {
            // if the note is still whole, reset the combo
            if (whole.activeSelf) {
                FindObjectOfType<GameManager>().ResetCombo();
            }
            
            Destroy(gameObject);    // Destroy the note
        }
    }



    private void Slice(Vector3 direction, Vector3 position, float force) {
        FindObjectOfType<GameManager>().IncreaseCombo(points);
        
        whole.SetActive(false);
        sliced.SetActive(true);

        noteCollider.enabled = false;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody2D[] slices = sliced.GetComponentsInChildren<Rigidbody2D>();

        // Calculate the center height of the circle
       // float circleCenterHeight = transform.position.y;

        // Calculate the center height of the box
        //float boxCenterHeight = boxCollider.bounds.center.y;

        // Calculate the distance between the center heights
        //float centerHeightDistance = Mathf.Abs(circleCenterHeight - boxCenterHeight);

        foreach (Rigidbody2D slice in slices) {
            slice.velocity = noteRigidbody.velocity;
            Vector2 dir2 = new() {
                x = direction.x,
                y = direction.y
            };

            Vector2 pos2 = new() {
                x = position.x,
                y = direction.y
            };

            slice.AddForceAtPosition(dir2 * force, pos2, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(this.tag)){
            slicable = true;
        }
        if (other.CompareTag("Player") && slicable == true) {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
         }

    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag(this.tag)){
            slicable = false;
        }
    }
}
