using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleContainerScript : MonoBehaviour
{
    public Transform targetItem;
    public Transform targetItem2;
    public List<Transform> destroyBlocks1;
    public List<Transform> destroyBlocks2;

    public AudioSource audioSource;

    public List<Transform> createBlocks;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Distance(targetItem.position, transform.position));
        if (targetItem != null && Vector3.Distance(targetItem.position, transform.position) < 1.5){
            Destroy(targetItem.gameObject);
            foreach (Transform t in createBlocks){
                t.gameObject.SetActive(true);
            }
            foreach (Transform t in destroyBlocks1){
                Destroy(t.gameObject);
            }
            audioSource.Play();
        }

        if (targetItem2 != null && Vector3.Distance(targetItem2.position, transform.position) < 1.5){
            Destroy(targetItem2.gameObject);
            foreach (Transform t in createBlocks){
                t.gameObject.SetActive(true);
            }
            foreach (Transform t in destroyBlocks2){
                Destroy(t.gameObject);
            }
            audioSource.Play();
        }
    }
}
