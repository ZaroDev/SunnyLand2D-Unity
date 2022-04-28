using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public IntReference playerHealth;

    List<HealthUI> hearts = new List<HealthUI> ();

    private void OnEnable()
    {
        PlayerHealth.OnDamage += DrawHearts;
    }
    private void OnDisable()
    {
        PlayerHealth.OnDamage -= DrawHearts;
    }
    private void Start()
    {
        DrawHearts();
    }
    public void DrawHearts()
    {
        ClearHearts();

        int heartsToMake = playerHealth.Value;

        for(int i = 0; i < heartsToMake; i++)
        {
            CreateHeart();
        }
    }

    public void CreateHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);
        Debug.Log("Creating a heart");
        HealthUI heartComponent = newHeart.GetComponent<HealthUI>();
       
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthUI>();
    }
}
