using UnityEngine;

public class Magazine : MonoBehaviour
{
    public int Capacity => capacity;
    public int NumberOfBullets => numberOfBullets;
    public Caliber Caliber => caliber;
    public ItemProperties Properties => properties;
    
    [SerializeField] private int capacity = 30;
    [SerializeField] private int numberOfBullets = 30;
    [SerializeField] private Caliber caliber;
    [SerializeField] private ItemProperties properties;

    public void AddRounds(int numberOfBulletsToAdd)
    {
        int totalInMagazine = numberOfBullets + numberOfBulletsToAdd;
        if (totalInMagazine > capacity)
        {
            totalInMagazine = capacity;
        }

        numberOfBullets = totalInMagazine;
    }

    public void RemoveRound()
    {
        numberOfBullets--;
    }
}