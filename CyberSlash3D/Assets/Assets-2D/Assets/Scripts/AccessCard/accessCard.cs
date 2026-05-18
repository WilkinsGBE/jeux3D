using UnityEngine;

public class accessCard : MonoBehaviour
{
    public AudioClip cardSound;

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
            GameManager2D.instance.collectCard();
            AudioSource.PlayClipAtPoint(cardSound, transform.position);
            Destroy(gameObject);
            Debug.Log("Carte ramassée");
            
        }

     
    }
}
