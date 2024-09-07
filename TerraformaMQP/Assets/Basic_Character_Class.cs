using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Basic_Character_Class : MonoBehaviour
{
    [SerializeField]
    public int health;  //Related Functions - takePhysicalDamage, takeMagicDamage, increaseHealth, decreaseHealth, checkHealth
    public int maxHealth;
    public int attack;  //Related Functions - increaseAttack, decreaseAttack
    public int movementSpeed;  //Related Functions - increaseMoveSpeed, decreaseMoveSpeed
    public int resistence;  //Related Functions - increaseResistence, decreaseResistence
    public int defense;  //Related Functions - increaseDefense, decreaseDefense
    public int speed;  //Related Functions - increaseSpeed, decreaseSpeed
    public float criticalChance;  //Related Functions - increaseCritChance, decreaseCritChance, deriveCritChance
    public int accuracy;  //Related Functions - increaseAccuracy, decreaseAccuracy
    public int actionsLeft;  //Related Functions - useAction, resetActions
    public int totalActions;

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
            health = maxHealth;
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

    //Increases the characters resistence value
    //Input - Amount of resistence to increase by

    void increaseResistence(int amount){
        resistence += amount;
    }

    //Decreases the character's resistence value
    //Input - Amount of resistence to decrease by

    void decreaseResistence(int amount) {
        resistence -= amount;
    }

    //Increases the character's defense value
    //Input - Amount of defense to increase by

    void increaseDefense(int amount){
        defense += amount;
    }

    //Decreases the character's defense value
    //Input - Amount of defense to decrease by

    void decreaseDefense(int amount){
        defense -= amount;
    }

    //Increases the character's speed value
    //Input - Amount of speed to increase by

    void increaseSpeed(int amount){
        speed += amount;
    }

    //Decreases the character's speed value
    //Input - Amount of speed to decrease by

    void decreaseSpeed(int amount){
        speed -= amount;
    }

    //Increases the character's critical chance value
    //Input - Amount of critical chance to increase by

    void increaseCritChance(float amount){
        criticalChance += amount;
    }

    //Decreases the character's critical chance value
    //Input - Amount of critical chance to decrease by

    void decreaseCritChance(float amount){
        criticalChance -= amount;
    }

    //Derives the character's critical chance value based on the accuracy stat
    //Input - None

    void deriveCritChance() {
        //Need formula for changing critical chance based on accuracy
        criticalChance = accuracy / 100;
    }

    //Increases the character's accuracy value
    //Input - Amount of accuracy to increase by

    void increaseAccuracy(int amount){
        accuracy += amount;
        deriveCritChance();
    }

    //Decreases the character's accuracy value
    //Input - Amount of accuracy to decrease by

    void decreaseAccuracy(int amount){
        accuracy -= amount;
        deriveCritChance();
    }

    //Decreases the amount of actions a character has left by 1
    //Input - None

    void useAction(){
        actionsLeft--;
    }

    //Resets the character's amount of actions
    //Input - None

    void resetActions(){
        actionsLeft = totalActions; 
    }


}
