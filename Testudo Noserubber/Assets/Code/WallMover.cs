using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WallMover : MonoBehaviour
{
    public Sprite disabledSprite;
    public Sprite enabledSprite;
    public GameObject targetObject;
    private Vector3 disabledPosition;
    public Vector3 enabledPosition;
    public float duration = 1;
    private float t = 0;
    private bool started = false;
    private bool towardsEnabled = false;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        disabledPosition = targetObject.transform.localPosition;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void swapDirection()
    {
        towardsEnabled = !towardsEnabled;
        spriteRenderer.sprite = towardsEnabled ? enabledSprite : disabledSprite;
    }

    float getDirection()
    {
        return towardsEnabled ? 1 : -1;
    }

    public IEnumerator DoTrigger()
    {
        swapDirection();
        if (started) { yield break; }
        started = true;

        t = Mathf.Clamp(t, 0.00001f, 0.99999f);

        while (t <= 1 && t >= 0)
        {
            t += getDirection() * Time.deltaTime / duration;
            targetObject.transform.localPosition = Vector3.Lerp(disabledPosition, enabledPosition, t);
            yield return null;
        }

        started = false;
    }
}
