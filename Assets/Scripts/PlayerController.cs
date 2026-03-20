/* Ethan Gapic-Kott, 000923124 */

using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;

    public float sensitivity = 300f;
    public Transform cameraPivot;

    public GameObject[] wandObjects;    // Elemental wands attached to player
    public GameObject[] orbPrefabs;     // Projectiles fired by elemental wands

    public TextMeshProUGUI interactionText;

    CharacterController controller;

    float xRotation;
    int currentSpell;

    float yVelocity;
    public float gravity = -9.81f;  // Player gravity

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        UpdateWand();
    }

    void Update()
    {
        Move();
        Look();
        ScrollSpells();
        Shoot();
    }
    
    // Player movement functions
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;

        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * speed;
        velocity.y = yVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    // Functions for moving playercamera
    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseX);
    }

    // Allows wands to be cycled using the scrollwheel for changing spells
    void ScrollSpells()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0) currentSpell++;
        if (scroll < 0) currentSpell--;

        currentSpell = Mathf.Clamp(currentSpell, 0, orbPrefabs.Length - 1);

        UpdateWand();
    }

    void UpdateWand()
    {
        for (int i = 0; i < wandObjects.Length; i++)
        {
            wandObjects[i].SetActive(i == currentSpell);
        }
    }

    // Allows wands to fire projectiles using leftclick
    void Shoot()
    {

        if (Input.GetMouseButtonDown(0))
        {
            WandSlash slash = wandObjects[currentSpell].GetComponent<WandSlash>();

            if (slash != null)
                slash.PlaySlash();

            GameObject orb = Instantiate(
                orbPrefabs[currentSpell],
                cameraPivot.position + cameraPivot.forward,
                Quaternion.identity);

            Rigidbody rb = orb.GetComponent<Rigidbody>();

            rb.velocity = cameraPivot.forward * 20f;
        }
    }

    public void UpdateUI(string result)
    {
        interactionText.text = result;
    }
}