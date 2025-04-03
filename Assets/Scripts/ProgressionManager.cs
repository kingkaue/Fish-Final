using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    int level = 1;
    bool mageUnlocked, rogueUnlocked;
    
    public void LevelUp()
    {
        if (level >= 2) mageUnlocked = true;
        if (level >= 3) rogueUnlocked = true;
    }

    public void Save()
    {

    }

    public void Load()
    {

    }
    
}
