using UnityEngine;

public class ExperienceGem : MonoBehaviour
{
    [Header("XP")]
    public int xpAmount = 1;

    [Header("Stack Settings")]
    public float stackRange = 1.2f;
    public float stackCheckInterval = 0.2f;
    public int maxStackXP = 999;

    [Header("Movement")]
    public float pickupRange = 3f;
    public float moveSpeed = 8f;

    private Transform player;
    private float stackTimer;
    private Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        RefreshVisual();
    }

    private void Update()
    {
        stackTimer += Time.deltaTime;

        if (stackTimer >= stackCheckInterval)
        {
            TryStackWithNearbyGems();
            stackTimer = 0f;
        }

        MoveTowardPlayerIfClose();
    }

    private void TryStackWithNearbyGems()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stackRange);

        foreach (Collider2D hit in hits)
        {
            ExperienceGem otherGem = hit.GetComponent<ExperienceGem>();

            if (otherGem == null || otherGem == this)
                continue;

            if (otherGem.xpAmount >= maxStackXP)
                continue;

            // Prevent both gems from merging into each other at the same time
            if (otherGem.GetInstanceID() < GetInstanceID())
            {
                MergeInto(otherGem);
                return;
            }
        }
    }

    private void MergeInto(ExperienceGem targetGem)
    {
        int totalXP = targetGem.xpAmount + xpAmount;
        targetGem.xpAmount = Mathf.Min(totalXP, maxStackXP);

        targetGem.RefreshVisual();

        Destroy(gameObject);
    }

    private void MoveTowardPlayerIfClose()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= pickupRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerExperience playerExperience = collision.GetComponent<PlayerExperience>();

        if (playerExperience == null)
        {
            playerExperience = collision.GetComponentInParent<PlayerExperience>();
        }

        if (playerExperience != null)
        {
            playerExperience.AddXP(xpAmount);
            Destroy(gameObject);
        }
    }

    private void RefreshVisual()
    {
        float sizeMultiplier = 1f + (xpAmount * 0.05f);
        sizeMultiplier = Mathf.Clamp(sizeMultiplier, 1f, 1.8f);

        transform.localScale = baseScale * sizeMultiplier;
    }
}