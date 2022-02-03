using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapGenerate : MonoBehaviour
{
    [SerializeField] private GameObject[] wallCells;
    [SerializeField] private GameObject[] fieldCells;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject hero;
    [SerializeField] private Sprite wall;
    [SerializeField] private Sprite block;
    [SerializeField] private Sprite brick;
    [SerializeField] private Sprite enemy;

    [SerializeField] private int columns;
    [SerializeField] private int rows;
    
    private int enemyCount = 0;

    private void OnDisable()
    {
        SceneManager.LoadScene("Scenes/Bombrman");
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var gObj in wallCells)
        {
            gObj.AddComponent<BoxCollider2D>();
            gObj.GetComponent<SpriteRenderer>().sprite = wall;
            gObj.layer = 8;
        }

        for (int i = 0, j = 1, currentRow = 0; i < fieldCells.Length; i++, j++)
        {
            if (i % 2 == 1 && currentRow % 2 == 1)
            {
                fieldCells[i].AddComponent<BoxCollider2D>();
                fieldCells[i].GetComponent<SpriteRenderer>().sprite = brick;
                fieldCells[i].layer = 8;
            }
            else if (i % 25 == 0)
            {
                hero.transform.position = fieldCells[i].transform.position - new Vector3(0, 0.25f);
            }
            else
            {
                if (Random.Range(0, 3) == 1)
                {
                    fieldCells[i].AddComponent<BoxCollider2D>();
                    fieldCells[i].GetComponent<SpriteRenderer>().sprite = block;
                }
                else if (enemyCount < enemies.Length && Random.Range(0, 20) == 5)
                {
                    enemies[enemyCount].SetActive(true);
                    enemies[enemyCount].AddComponent<BoxCollider2D>();
                    enemies[enemyCount].transform.position = fieldCells[i].transform.position - new Vector3(0f, 0.25f);

                    enemyCount++;
                }
            }

            if (j == rows)
            {
                j = 0;
                currentRow++;
            }
        }
    }
}
