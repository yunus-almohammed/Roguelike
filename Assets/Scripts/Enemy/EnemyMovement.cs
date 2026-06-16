using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    private Transform player;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null || enemyData == null)
            return;

        Vector2 direction = player.position - transform.position;

        // Move enemy toward player
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            enemyData.MoveSpeed * Time.deltaTime
        );

        // Flip enemy sprite based on player direction
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}