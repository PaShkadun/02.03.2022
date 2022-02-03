using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BanditPlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask explosionMask;
    
    private bool isMovement;
    
    // Update is called once per frame
    void Update()
    {
        if (isMovement)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePlayerTo(Vector2.left);
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePlayerTo(Vector2.right);
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovePlayerTo(Vector2.up);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePlayerTo(Vector2.down);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var obj = RaycastFromCamera();

            if (obj != null && obj.CompareTag("Explosive"))
            {
                Destroy(obj);

                var colliders = Physics2D.OverlapCircleAll(obj.transform.position, 1f, explosionMask);

                for (var i = 0; i < colliders.Length; i++)
                {
                    Destroy(colliders[i].gameObject);
                }
            }
        }
    }

    private void MovePlayerTo(Vector2 dir)
    {
        if (Raycast(dir))
        {
            return;
        }

        isMovement = true;

        var pos = (Vector2) transform.position + dir;

        transform.DOMove(pos, 0.5f).OnComplete(() => isMovement = false);
    }

    private bool Raycast(Vector2 dir)
    {
        var hit = Physics2D.Raycast(transform.position, dir, 1f, layerMask);

        return hit.collider != null;
    }

    private GameObject RaycastFromCamera()
    {
        Debug.Log("asd");
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
        Debug.Log(hit.collider);
        
        return hit.collider != null ? hit.collider.gameObject : null;
    }
}