using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player{
    public int health = 0;
    public int maxHealth = 100;
    public IInventory inventory;
    public bool isWeaponEquipped = false;

    IWeapon weapon;

    public Player()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
    }

    public void Equip(IWeapon newWeapon)
    {
        if (weapon == null)
        {
            isWeaponEquipped = true;
            weapon = newWeapon;
        }
    }

    public float Damage(float multiplier)
    {
        return weapon.Damage() * multiplier;
    }

    public void AddToInventory(IItem item)
    {
        inventory.Add(item);
    }
}

public interface IWeapon
{
    int Damage();
}

public interface IInventory
{
    void Add(IItem item);
}

public interface IItem
{
    void ShowInfo();
}
