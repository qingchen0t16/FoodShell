using UnityEngine;

public class SrollWithCam : MonoBehaviour
{
    public Transform camTran;
    public Material mat;
    public float OffsetRat = 0.01f;
    void Start()
    {
        if (camTran == null)
            camTran = transform.parent;
    }
    private void FixedUpdate()
    {
        if (camTran != null)
        {
            mat.SetTextureOffset("_MainTex", new Vector2(camTran.transform.position.x * OffsetRat, 0));
        }
    }
}
