using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour, IDataPersistance
{
    private Animator catAnimator;
    private Rigidbody2D myRigidbody;
    private float moveSpeed = 2f; // Adjust the speed as needed
    private float flipTimer = 4f; // Time to wait before flipping
    private float currentTimer;
    private bool killed = false;
    [SerializeField] private string id;
    private SpriteRenderer visual;

    [SerializeField]
    private AudioSource meaw;

    [SerializeField]
    private AudioSource death;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        visual = this.GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        catAnimator = GetComponent<Animator>();
        currentTimer = flipTimer; // Initialize the timer
    }

    void Update()
    {
        // Move the cat
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);

        // Update the timer
        currentTimer -= Time.deltaTime;

        // Check if it's time to flip the cat
        if (currentTimer <= 0f)
        {
            FlipCatFacing();
            currentTimer = flipTimer; // Reset the timer
        }
    }

    public void LoadData(GameData data, string prevScene)
    {
        data.catsKilled.TryGetValue(id, out killed);
        if (killed)
        {
            visual.gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.catsKilled.ContainsKey(id))
        {
            data.catsKilled.Remove(id);
        }
        data.catsKilled.Add(id, killed);
    }

    void FlipCatFacing()
    {
        meaw.Play();
        // Flip the cat's direction based on the velocity
        if (myRigidbody.velocity.x > 0.1f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            moveSpeed *= -1;
        }
        else if (myRigidbody.velocity.x < -0.1f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            moveSpeed *= -1;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider has the specified name
        if (other.gameObject.name == "SwordCollider")
        {
            // Trigger the "Death" animation
            if (catAnimator != null)
            {
                catAnimator.SetTrigger("Death");
                death.Play();
                killed = true;
                StartCoroutine(WaitForDeathAnimation());
            }
        }
    }

    IEnumerator WaitForDeathAnimation()
    {
        // Wait for the duration of the "Death" animation
        yield return new WaitForSeconds(1);

        // Destroy the cat GameObject after the animation finishes
        visual.gameObject.SetActive(false);
    }
}