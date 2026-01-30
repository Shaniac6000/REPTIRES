using UnityEngine;

public class LevelManagement : MonoBehaviour
{
    public static int highestLevel = 0;

    public static void SetHighest(int i)
    {
        if (i > highestLevel)
            highestLevel = i;
    }
}
