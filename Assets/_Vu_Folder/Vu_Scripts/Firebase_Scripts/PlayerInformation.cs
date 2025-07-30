using UnityEngine;

public class PlayerInformation 
{
    public string playerName;
    public int gold;
    public int health;
    public int energy;

    public PlayerInformation(string playerName, int gold, int health, int energy)
    {
        this.playerName = playerName;
        this.gold = gold;
        this.health = health;
        this.energy = energy;
    }
}
