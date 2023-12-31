using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{
    public GameObject target;
    public GameObject head;
    public GameObject forearm;
    public GameObject cleaverHandle;
    public GameObject handHolding;
    public GameObject dangerZone;
    public Transform attackPosition;
    public float maxfollowTimer, maxholdTimer, maxchopTimer;
    public float followTimer, holdTimer, chopTimer;
    public float offsetX, offsetY, offsetZ;
    public float denominator;
    public bool up, stop, down;

    private SoundManager soundManagerScript;
    private GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        // Locate scripts
        soundManagerScript = GameObject.Find("Tomato Controller").GetComponent<SoundManager>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        // chef watches tomato and follows his movements with the knife
        head.transform.forward = target.transform.position - head.transform.position;
        cleaverHandle.transform.position = handHolding.transform.position;
        forearm.transform.forward = target.transform.position - forearm.transform.position;

        if (up)
        {
            followTimer -= 1;
            dangerZone.SetActive(false);
        }

        if (followTimer <= 0)
        {
            up = false;
            stop = true;
            followTimer = maxfollowTimer / denominator;
            dangerZone.SetActive(true);
            // Danger zone follows player but stays flush with cutting board
            attackPosition.position = new Vector3(target.transform.position.x, 0f, target.transform.position.z) - new Vector3(offsetX, offsetY, offsetZ);
        }

        if (stop)
        {
            holdTimer -= 1;
        }

        if (holdTimer <= 0 && !gameManagerScript.gameOver)
        {
            stop = false;
            down = true;
            holdTimer = maxholdTimer / denominator;
            soundManagerScript.PlaySound(soundManagerScript.cleaver);
        }

        if (down)
        {
            cleaverHandle.transform.position = attackPosition.position;
            chopTimer -= 1;
        }

        if (chopTimer <= 0)
        {
            down = false;
            up = true;
            denominator += 0.1f;
            chopTimer = maxchopTimer;
        }
    }
}
