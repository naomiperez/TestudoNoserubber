using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInteraction : MonoBehaviour
{
    // gameobjects on screem
    public GameObject TestudoShell;
    public Camera Cam;
    public GameObject ScreenOverlay;
    public Text InteractionText;

    // camera animation variables
    public List<GameObject> Statues;
    public int CameraAnimationTime = 15;
    public bool camAnimationEnabled = true;
    private Vector3 startPos;
    public static bool inputReady = false;

    // on screen gameobject components
    private SpriteRenderer playerSpriteRenderer;
    private CapsuleCollider2D playerCollider;
    private Rigidbody2D playerRigidboy;

    //gameplay variables
    private bool blocking = false;
    private int livesLost = 0;
    private int score = 0;

    // Variables to use for holding items
    private bool isHolding = false;
    private bool itemNearby = false;
    private GameObject nearestItem;
    private GameObject heldItem;

    

    

    private Collider2D currStatueCollider = null;

    void Start()
    {
        playerSpriteRenderer = this.GetComponent<SpriteRenderer>();
        playerCollider = this.GetComponent<CapsuleCollider2D>();
        playerRigidboy = this.GetComponent<Rigidbody2D>();
        startPos = this.transform.position;

        if(camAnimationEnabled) { StartCoroutine(cameraAnimation()); } else { inputReady = true; }

        

    }

    // Update is called once per frame
    void Update()
    {
        // blocking
        if (Input.GetKeyDown(KeyCode.Space) && playerRigidboy.velocity == Vector2.zero && inputReady)
        {
            Debug.Log("IN SPACE");
            if (blocking)
            {
                playerRigidboy.bodyType = RigidbodyType2D.Dynamic; // make testudo rigidboy dynamic so it can move again
                playerCollider.enabled = true;               // re enable testudo collider
                playerSpriteRenderer.enabled = true;         // re enable testudo image
                //little reset of shell position because i liked the drop it does when you start a block and you
                // have to reset the position every time if you want that
                TestudoShell.transform.position = new Vector3(TestudoShell.transform.position.x, TestudoShell.transform.position.y + .5f, TestudoShell.transform.position.z);
                TestudoShell.SetActive(false);             // get rid of testudo shell and all its components
                Cam.transform.parent = this.transform;    // give back cam ownership to testudo
                blocking = false;                           // flip flag
            }
            else
            {
                playerRigidboy.bodyType = RigidbodyType2D.Static;     // make testudo rigidbody static so it wont move
                playerCollider.enabled = false;                 // disable testudo collider so things wont run into it
                playerSpriteRenderer.enabled = false;           // this will make the sprite of testudo dissapear
                TestudoShell.SetActive(true);                  // activate shell and all its components
                Cam.transform.parent = TestudoShell.transform; // take ownership of camera and give to shell
                blocking = true;                                // flip flag

            }

        }

        if (Input.GetKeyDown(KeyCode.E)  && inputReady){
            Debug.Log("E");
            Debug.Log("Item nearby? " + itemNearby + " isHolding? " + isHolding);
            if (isHolding){
                isHolding = false;
                heldItem.transform.SetParent(null);
            } else if (itemNearby && !isHolding){
                isHolding = true;
                nearestItem.transform.SetParent(transform, false);
                nearestItem.transform.localPosition = Vector3.up;
                nearestItem.transform.localPosition += Vector3.back * 5;
                nearestItem.transform.localScale = Vector3.one;
                heldItem = nearestItem;
            }else if(InteractionText.text == "Press E to Rub"){
                // if the text is present, that means we're within trigger distance of a statue
                // so we can update the text and update the nearby statues tag
                ScreenOverlay.transform.GetChild(3 + score++).gameObject.GetComponent<Graphic>().color = Color.white;
                InteractionText.text = "Nose Rubbed!";
                currStatueCollider.tag = "Rubbed";
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // check if the trigger we ran into is an enemy. We'll do different things depending if we're blocking or not
        if(!blocking && collider.gameObject.tag == "Enemy")
        {
            // if we're not blocking decrement a life and reflect that on the lives container
            Debug.Log("LOST LIFE!!");
            ScreenOverlay.transform.GetChild(livesLost++).gameObject.SetActive(false);
            if(livesLost == 3) { triggerDeath(); }
        }

        // Update state depending on if an item is nearby or not
        if (collider.gameObject.tag == "Item"){
            itemNearby = true;
            nearestItem = collider.gameObject;
            Debug.Log("Nearest Item is: " + nearestItem.name);
        }

        // check if the collider is A) an unrubbed statue or B) a rubbed statue and update overhead text accordingly
        // currStatueCollider is used to keep track of which statue we're nearby so that in update when the E
        // key is pressed we have the collider on hand to change its tag to rub
        if(collider.gameObject.tag == "Statue"){
            InteractionText.text = "Press E to Rub";
            currStatueCollider = collider;
        }else if(collider.gameObject.tag == "Rubbed"){
            InteractionText.text = "Nose Already Rubbed!";
            currStatueCollider = collider;
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if (collider.gameObject.tag == "Item"){
            itemNearby = false;
            nearestItem = null;
            Debug.Log("Nearest Item is null");
        }

        // we're leaving the vicinity of the statue so set back to defaults
        currStatueCollider = null;
        InteractionText.text = "";

    }

    private void triggerDeath()
    {
        // reactivate 3 lifes sprites
        ScreenOverlay.transform.GetChild(0).gameObject.SetActive(true);
        ScreenOverlay.transform.GetChild(1).gameObject.SetActive(true);
        ScreenOverlay.transform.GetChild(2).gameObject.SetActive(true);
        this.transform.position = startPos;
        playerRigidboy.velocity = Vector2.zero;
        livesLost = 0;
    }

    private IEnumerator cameraAnimation(){
        int timePerStatue = CameraAnimationTime / 4;
        Vector3 initCamPos = Cam.transform.position;
        float initCamSize = Cam.orthographicSize;
        foreach(GameObject curr in Statues){
            float delta = 0;
            Vector3 camPos = Cam.transform.position;
            while (delta < timePerStatue)
            {
                delta += Time.deltaTime;
                Cam.transform.position = Vector3.Lerp(camPos, curr.transform.position - new Vector3(0, 0, 1), delta / timePerStatue);
                yield return null;
            }
            yield return new WaitForSeconds(1);

        }        
        Cam.transform.position = initCamPos;
        Cam.orthographicSize = 70;
        yield return new WaitForSeconds(4);
        Cam.orthographicSize = initCamSize;
        inputReady = true;
    }
}
