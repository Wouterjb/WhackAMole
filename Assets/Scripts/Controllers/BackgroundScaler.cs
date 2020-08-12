using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        // Setup background to be the correct fit at the start
        AdjustBackgroundSize();
    }


    // Update is called once per frame
    public void Update()
    {
        AdjustBackgroundSize();
    }

    private void AdjustBackgroundSize()
    {
        // Get components
        SpriteRenderer backgroundSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        // Setup variables
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = backgroundSpriteRenderer.sprite.bounds.size;
        Vector2 backgroundScale = Vector2.one;

        // Determine landscape/portrait and adjust
        backgroundScale.x = cameraSize.x / spriteSize.x;
        backgroundScale.y = cameraSize.y / spriteSize.y;

        // Set to background
        this.gameObject.transform.localScale = backgroundScale;
    }
}
