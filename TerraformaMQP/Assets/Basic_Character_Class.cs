using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Basic_Character_Class : MonoBehaviour
{
    [SerializeField]
    public int health;
    public int maxHealth;
    public int attack;
    public int movementSpeed;
    public int resistence;
    public int defense;
    public int speed;
    public int criticalChance;
    public int accuracy;
    public int actions;
    public string[] statuses;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log("Here");
    }

    // Update is called once per frame
    void Update()
    {
        //FOR TESTING PURPOSES ----- ALLOWS THE CHARACTER TO TAKE PHYSICAL DAMAGE WHEN P IS PRESSED AND MAGIC DAMAGE WHEN M IS PRESSED
        if (Input.GetKeyDown(KeyCode.P)) {
            this.takePhysicalDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            this.takeMagicDamage(1);
        }
        
        
    }

    //General
    void destroy();

    //Taking Damage
    void takePhysicalDamage(int damage);
    void takeMagicDamage(int damage);

    //Health
    void increaseHealth(int amount);
    void decreaseHealth(int amount);
    void checkHealth();

    //Attack
    void increaseAttack(int amount);
    void decreaseAttack(int amount);

    //Movement Speed
    void increaseMoveSpeed(int amount);
    void decreaseMoveSpeed(int amount);

    //Deals Physical Damage to the character and checks if it reduces the health total below 0. Reduces the Damage value by the amount of Defense the character has
    //Input - Amount of Physical Damage Taken

    void takePhysicalDamage(int damage){
        health = health - (damage - defense);
        UnityEngine.Debug.Log("Took " +  (damage - defense) + " physical damage");
        checkHealth();
        return;
    }

    //Deals Magic Damage to the character and checks if it reduces the health total below 0. Reduces the Damage value by the amount of Resistence the character has
    //Input - Amount of Magic Damage Taken

    void takeMagicDamage(int damage){
        health = health - (damage - resistence);
        UnityEngine.Debug.Log("Took " + (damage - resistence) + " magic damage");
        checkHealth();
    }

    //Increases the Health Total of the character
    //Input - Amount of health to increase by

    void increaseHealth(int amount){
        health = health + amount;
        if (health > maxHealth) {
            health = maxHealth
        }
    }

    //Reduces the Health Total of the character
    //Input - Amount of health to decrease by

    void decreaseHealth(int amount){
        health = health - amount;
        checkHealth();
    }

    //Destroys the GameObject

    void destroy(){
        UnityEngine.Debug.Log("Destroyed");
        Destroy(gameObject);
        return;
    }

    //Checks if the Character's Health is below 0 and destroys it if so

    void checkHealth() {
        if (health <= 0){
            destroy();
        }
    }

    //Increases the character's attack value
    //Input - Amount of attack to increase by

    void increaseAttack(int amount){
        attack += amount;
    }

    //Decreases the character's attack value
    //Input - Amount of attack to decrease by

    void decreaseAttack(int amount){
        attack -= amount;
    }

    //Increases the character's movement speed value 
    //Input - Amount of speed to increase by

    void increaseMoveSpeed(int amount){
        movementSpeed += amount;
    }

    //Decreases the character's movement speed value
    //Input - Amount of speed to decrease by

    void decreaseMoveSpeed(int amount){
        movementSpeed -= amount;
    }


}
