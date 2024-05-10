using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] float baseWidth = 135;  //NOT SURE WHY This value is what it is?

    BuildingData data;


    public void Setup(BuildingData data)
    {
        this.data = data;

        //need to scale based on sprite size vs building size and height?

        Debug.Log(data.buildingImage.texture.width + ", " + data.buildingImage.texture.height);

        var lx = baseWidth * data.buildingSize.x / data.buildingImage.texture.width;
        var ly = lx;// * (float)data.buildingImage.texture.height / data.buildingImage.texture.width;

        transform.localScale = new Vector3(lx, ly, 1);

        GetComponent<SpriteRenderer>().sprite = data.buildingImage;

        //also want to scale collider?
    }

    private void OnDrawGizmosSelected()
    {
        
        Vector3 size = data == null ? new Vector3(2, 0.05f, 2) : new Vector3(data.buildingSize.x, 0f, data.buildingSize.y);

        Gizmos.color = Color.blue;

        Vector3 pos = transform.position;
        pos.y = 0;



        pos += size / 2f;

        Gizmos.DrawCube(pos, size);
        
    }
}
