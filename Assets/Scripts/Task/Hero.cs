using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask explosionMask;
    [SerializeField] private GameObject bomb;
    [SerializeField] private GameObject[] fire;
    [SerializeField] private GameObject mapper;

    private bool isMovement;
    private bool bombSet;
    private bool bombBoom;

    private float bombTime;
    private float fireTime;
    
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

        if (Input.GetKey(KeyCode.Space) && !bombSet)
        {
            SetBomb();
        }

        if (bombSet && !bombBoom)
        {
            bombTime += Time.deltaTime;

            if (bombTime > 1f)
            {
                BadabumBomb();
            }
        }

        if (bombSet && bombBoom)
        {
            fireTime += Time.deltaTime;

            if (fireTime > 1f)
            {
                RemoveBomb();
            }
        } 
    }

    private void SetBomb()
    {
        bombSet = true;
        bombBoom = false;
        
        bomb.transform.position = transform.position + new Vector3(0, 0.25f);
    }

    private void BadabumBomb()
    {
        var oldPos = bomb.transform.position;
        bomb.transform.position = new Vector3(10, 10, 10);
        bombBoom = true;    

        var hit1 = Physics2D.Raycast(oldPos, Vector2.down, 1f, explosionMask);
        var hit2 = Physics2D.Raycast(oldPos, Vector2.up, 1f, explosionMask);
        var hit3 = Physics2D.Raycast(oldPos, Vector2.right, 1f, explosionMask);
        var hit4 = Physics2D.Raycast(oldPos, Vector2.left, 1f, explosionMask);

        fire[0].transform.position = oldPos;
        fire[0].gameObject.SetActive(true);

        var isPlayer = false;
        
        if (hit1.collider != null)
        {
            fire[1].gameObject.SetActive(true);
            fire[1].transform.position = hit1.collider.gameObject.transform.position;

            if (hit1.collider.gameObject.layer == 3)
            {
                isPlayer = true;
            }
            
            hit1.collider.gameObject.SetActive(false);
        }

        if (hit2.collider != null)
        {
            fire[2].gameObject.SetActive(true);
            fire[2].transform.position = hit2.collider.gameObject.transform.position;
            
            if (hit2.collider.gameObject.layer == 3)
            {
                isPlayer = true;
            }
            
            hit2.collider.gameObject.SetActive(false);
        }

        if (hit3.collider != null)
        {
            fire[3].gameObject.SetActive(true);
            fire[3].transform.position = hit3.collider.gameObject.transform.position;
            
            if (hit3.collider.gameObject.layer == 3)
            {
                isPlayer = true;
            }
            
            hit3.collider.gameObject.SetActive(false);
        }

        if (hit4.collider != null)
        {
            fire[4].gameObject.SetActive(true);
            fire[4].transform.position = hit4.collider.gameObject.transform.position;
            
            if (hit4.collider.gameObject.layer == 3)
            {
                isPlayer = true;
            }
            
            hit4.collider.gameObject.SetActive(false);
        }

        if (isPlayer)
        {
            mapper.GetComponent<MapGenerate>().enabled = false;
        }
    }

    private void RemoveBomb()
    {
        bombSet = false;

        foreach (var f in fire)
        {
            f.transform.position = new Vector3(10, 10, 10);
            f.SetActive(false);
        }

        fireTime = 0f;
        bombTime = 0f;
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