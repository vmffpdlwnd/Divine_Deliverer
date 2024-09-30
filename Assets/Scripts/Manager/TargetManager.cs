using UnityEngine;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> targetPrefabs;
    private GameObject currentTarget;
    private GridManager gridManager;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    public void SpawnTarget()
    {
        if (currentTarget != null)
        {
            Destroy(currentTarget);
        }
        Vector3 position = gridManager.GetRandomPosition(false);
        if (position != Vector3.zero)
        {
            position.y = 0; // Set y to 0 to place on the ground
            GameObject targetPrefab = targetPrefabs[Random.Range(0, targetPrefabs.Count)];
            currentTarget = Instantiate(targetPrefab, position, Quaternion.identity);
            Animator animator = currentTarget.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
            }
        }
    }

    public void RemoveCurrentTarget()
    {
        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }
    }
}