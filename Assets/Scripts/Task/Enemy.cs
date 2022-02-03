using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool isMovement;
    [SerializeField] private bool moveHorizontal;
    [SerializeField] private bool positiveMove;
    [SerializeField] private LayerMask layerMask;
    
    private void Update()
    {
        if (isMovement)
        {
        }
        else
        {
            GameObject ray = null;
            
            if (!moveHorizontal && !positiveMove)
            {
                Raycast(Vector2.down);
            }
            else if (!moveHorizontal && positiveMove)
            {
                Raycast(Vector2.up);
            }
            else if (moveHorizontal && !positiveMove)
            {
                Raycast(Vector2.left);
            }
            else if (moveHorizontal && positiveMove)
            {
                Raycast(Vector2.right);
            }
        }
    }

    private void MovePlayerTo(Vector2 dir)
    {
        isMovement = true;

        var pos = (Vector2) transform.position + dir;

        transform.DOMove(pos, 1f).OnComplete(() => isMovement = false);
    }

    private void Raycast(Vector2 dir)
    {
        var hit = Physics2D.Raycast(transform.position, dir, 1f, layerMask);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            if (Random.Range(1, 3) == 2)
            {
                moveHorizontal = true;
            }
            else
            {
                moveHorizontal = false;
            }

            if (Random.Range(1, 3) == 2)
            {
                positiveMove = true;
            }
            else
            {
                positiveMove = false;
            }
        }
        else
        {
            MovePlayerTo(dir);
        }
    }
}