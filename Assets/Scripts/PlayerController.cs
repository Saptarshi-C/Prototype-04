using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Moves the Player
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10;
    public float powerupStrength = 10;
    public GameObject powerupIndicator;

    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * forwardInput * moveSpeed * Time.deltaTime);

        powerupIndicator.gameObject.transform.position = transform.position + new Vector3(0, -0.4f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup )
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer.normalized * powerupStrength, ForceMode.Impulse);
            //Debug.Log("Collided with " + collision.gameObject.name + " with hasPowerup set to " + hasPowerup);
        }
    }
}
