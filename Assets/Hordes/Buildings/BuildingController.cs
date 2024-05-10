using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] float baseWidth = 135;  //NOT SURE WHY This value is what it is?
    [SerializeField] Color constructionColor;
    [SerializeField] SpriteRenderer spriteRenderer;
    public BuildingData data { get; private set; }

    public void Setup(BuildingData data)
    {
        this.data = data;

        //setup building sprite
        spriteRenderer.transform.localScale = data.GetSpriteScale();//new Vector3(lx, ly, 1);
        spriteRenderer.sprite = data.buildingImage;

        //also want to scale collider?
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.center = new Vector3(data.buildingSize.x / 2f, data.colliderHeight / 2f, data.buildingSize.y / 2f);
        
        bc.size = new Vector3(data.buildingSize.x, data.colliderHeight, data.buildingSize.y);


        //begin building itself
        StartCoroutine(ConstructionCorountine());
    }

    IEnumerator ConstructionCorountine()
    {
        spriteRenderer.color = constructionColor;
        yield return new WaitForSeconds(data.buildTime);
        spriteRenderer.color = Color.white;
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
