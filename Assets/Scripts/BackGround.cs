using UnityEngine;

public enum BackgroundType { Blue,Brown,Gray,Green,Pink,Purple,Yellow }
public class BackGround : MonoBehaviour
{
    [SerializeField] private Vector2 movementDiraction;
    private MeshRenderer rd;

    [SerializeField] private BackgroundType backGround;
    [SerializeField] private Texture2D[] textures;
    private void Awake()
    {
        rd = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        rd.material.mainTextureOffset += (movementDiraction * Time.deltaTime);
    }
    [ContextMenu("Change Background Texture")]
    private void UpdateBackgroundTexture()
    {
        if(rd == null)
            rd = GetComponent<MeshRenderer>();
        rd.sharedMaterial.mainTexture = textures[((int)backGround)];
    }
}
