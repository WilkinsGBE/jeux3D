using UnityEngine;

public class coin : MonoBehaviour
{
    public AudioClip coinSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        movePlayer player = collision.GetComponent<movePlayer>();

        if (collision.CompareTag("Player"))
        {
            player.coinAmount ++;
            GameManager2D.instance.CoinCollected();
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            Destroy(gameObject);
            Debug.Log("Piece ramassée");
        }


    }
}
