using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements

public class MovementScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject GreenArrow;
    public Sprite left_testudo;
    public Sprite right_testudo;
   

    private float angle;
    private float speed = 0;
    private Vector3 mousePos;
    private Vector3 objPos;

    private Rigidbody2D playerRb;
    private SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        playerSpriteRenderer = Player.GetComponent<SpriteRenderer>();
        playerRb = Player.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        // movement
        if (Input.GetKeyUp(KeyCode.Mouse0) && PlayerInteraction.inputReady && playerRb.bodyType == RigidbodyType2D.Dynamic) {
            // set the velocity of testudo based on the direction of the arrow and then
            // change the sprite on testudo to look in the direction of the velocity
            // to allow the movement to feel a bit more natural
            playerRb.velocity = (GreenArrow.transform.up * (20 * speed));
            if (playerRb.velocity.x < 0) { playerSpriteRenderer.sprite = left_testudo;  }
            else { playerSpriteRenderer.sprite = right_testudo; }
        }

        // get input of mouse 0 (left click) to determine arrow fill and release speed
        speed = Input.GetAxis("Fire1");
        // arrow direction and fill
        controlArrow(speed);        
    }

    private void controlArrow(float speed)
    {
        mousePos = Input.mousePosition;
        objPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objPos.x;
        mousePos.y = mousePos.y - objPos.y;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        GreenArrow.GetComponent<Image>().fillAmount = speed;
    }
}
