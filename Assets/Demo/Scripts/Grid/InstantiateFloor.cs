using UnityEngine;

public class InstantiateFloor : MonoBehaviour
{

    public float xSize, ySize;
    public GameObject tile;
    void Start()
    {
        var spriteSize = tile.GetComponent<SpriteRenderer>().size;
        var cameraDifX = xSize - spriteSize.x/2;
        var cameraDifY = ySize - spriteSize.y/2;
        
        for (var i = -cameraDifX; i < xSize; i++)
        {
            for (var j = -cameraDifY; j < ySize; j++)
            {
                var pos = new Vector3(i, j,0);
                Instantiate(tile,pos,Quaternion.identity,transform);
            }
        }
    }
    
}
