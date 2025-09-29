using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{

    private Rigidbody2D rb2d;

    private Vector3 startPosition;        // posi��o inicial do n�vel
    private Vector3 currentCheckpoint;  // �ltimo checkpoint ativado

    public int maxLives = 3;
    private int currentLives;

    public List<GameObject> checkpoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        currentCheckpoint = startPosition;
        currentLives = maxLives;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform.position;
            collision.gameObject.SetActive(false);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Spike"))
        {
            Respawn();
        }
    }

    // Fun��o para voltar ao checkpoint ou in�cio
    public void Respawn()
    {
        rb2d.linearVelocity = Vector2.zero; // reseta velocidade
        currentLives--;

        if (currentLives <= 0)
        {
            // Volta para o in�cio do n�vel
            transform.position = startPosition;
            currentCheckpoint = startPosition;
            currentLives = maxLives;

            // Reabilita todos os checkpoints
            foreach (GameObject cp in checkpoints)
            {
                cp.SetActive(true);
            }
        }
        else
        {
            // Volta para o �ltimo checkpoint
            transform.position = currentCheckpoint;
        }
    }
}
