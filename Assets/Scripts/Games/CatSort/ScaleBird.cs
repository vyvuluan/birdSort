using UnityEngine;

public class ScaleBird : MonoBehaviour
{
    private float worldHeight;
    private float worldWidth;
    private void Awake()
    {
        worldHeight = Camera.main.orthographicSize * 2f;
        worldWidth = worldHeight * Screen.width / Screen.height;
        float scaleXYZ = (worldWidth) / 9.8f;
        transform.localScale = new Vector3(scaleXYZ,scaleXYZ,scaleXYZ);
    }
}
