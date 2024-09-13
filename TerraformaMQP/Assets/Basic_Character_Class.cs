using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Serialization;
using UnityEngine;

public class Basic_Character_Class : MonoBehaviour
{
    [SerializeField]
    public bool turnEnded = false;
    public List<StatusEffect> effects; //Related Functions - addStatus, removeStatus
    public StatusEffect tileEffect;

    public stat health;  //Related Functions - takePhysicalDamage, takeMagicDamage, increaseHealth, decreaseHealth, checkHealth
    public stat maxHealth;
    public stat attack;  //Related Functions - increaseAttack, decreaseAttack
    public stat movementSpeed;  //Related Functions - increaseMoveSpeed, decreaseMoveSpeed
    public stat resistence;  //Related Functions - increaseResistence, decreaseResistence
    public stat defense;  //Related Functions - increaseDefense, decreaseDefense
    public stat speed;  //Related Functions - increaseSpeed, decreaseSpeed
    public stat criticalChance;  //Related Functions - increaseCritChance, decreaseCritChance, deriveCritChance
    public stat accuracy;  //Related Functions - increaseAccuracy, decreaseAccuracy
    public stat actionsLeft;  //Related Functions - useAction, resetActions
    public stat totalActions;



    

    // Start is called before the first frame update
    void Start()
    {

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
            this.takeMagicDamage(1, "Fire");
        } 
        
    }

    //Deals Physical Damage to the character and checks if it reduces the health total below 0. Reduces the Damage value by the amount of Defense the character has
    //Input - Amount of Physical Damage Taken

    void takePhysicalDamage(int damage){
        health.value = health.value - (damage - defense.moddedValue);
        UnityEngine.Debug.Log("Took " +  (damage - defense.moddedValue) + " physical damage");
        checkHealth();
        return;
    }

    //Deals Magic Damage to the character and checks if it reduces the health total below 0. Reduces the Damage value by the amount of Resistence the character has
    //Input - Amount of Magic Damage Taken

    void takeMagicDamage(int damage, string magicType){
        health.value = health.value - (damage - resistence.moddedValue);
        UnityEngine.Debug.Log("Took " + (damage - resistence.moddedValue) + " magic damage");
        checkHealth();
    }

    //Increases the Health Total of the character
    //Input - Amount of health to increase by

    void increaseHealth(int amount){
        health.value = health.value + amount;
        if (health.value > maxHealth.moddedValue) {
            health.value = maxHealth.moddedValue;
        }
    }

    //Reduces the Health Total of the character
    //Input - Amount of health to decrease by

    void decreaseHealth(int amount){
        health.value = health.value - amount;
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
        if (health.value <= 0){
            destroy();
        }
    }

    //Increases the character's attack value
    //Input - Amount of attack to increase by

    void increaseAttack(int amount){
        attack.moddedValue += amount;
    }

    //Decreases the character's attack value
    //Input - Amount of attack to decrease by

    void decreaseAttack(int amount){
        attack.moddedValue -= amount;
    }

    //Increases the character's movement speed value 
    //Input - Amount of speed to increase by

    void increaseMoveSpeed(int amount){
        movementSpeed.moddedValue += amount;
    }

    //Decreases the character's movement speed value
    //Input - Amount of speed to decrease by

    void decreaseMoveSpeed(int amount){
        movementSpeed.moddedValue -= amount;
    }

    //Increases the characters resistence value
    //Input - Amount of resistence to increase by

    void increaseResistence(int amount){
        resistence.moddedValue += amount;
    }

    //Decreases the character's resistence value
    //Input - Amount of resistence to decrease by

    void decreaseResistence(int amount) {
        resistence.moddedValue -= amount;
    }

    //Increases the character's defense value
    //Input - Amount of defense to increase by

    void increaseDefense(int amount){
        defense.moddedValue += amount;
    }

    //Decreases the character's defense value
    //Input - Amount of defense to decrease by

    void decreaseDefense(int amount){
        defense.moddedValue -= amount;
    }

    //Increases the character's speed value
    //Input - Amount of speed to increase by

    void increaseSpeed(int amount){
        speed.moddedValue += amount;
    }

    //Decreases the character's speed value
    //Input - Amount of speed to decrease by

    void decreaseSpeed(int amount){
        speed.moddedValue -= amount;
    }

    //Increases the character's critical chance value
    //Input - Amount of critical chance to increase by

    void increaseCritChance(int amount){
        criticalChance.moddedValue += amount;
    }

    //Decreases the character's critical chance value
    //Input - Amount of critical chance to decrease by

    void decreaseCritChance(int amount){
        criticalChance.moddedValue -= amount;
    }

    //Derives the character's critical chance value based on the accuracy stat
    //Input - None

    void deriveCritChance() {
        //Need formula for changing critical chance based on accuracy
        criticalChance.moddedValue = accuracy.moddedValue / 100;
    }

    //Increases the character's accuracy value
    //Input - Amount of accuracy to increase by

    void increaseAccuracy(int amount){
        accuracy.moddedValue += amount;
        deriveCritChance();
    }

    //Decreases the character's accuracy value
    //Input - Amount of accuracy to decrease by

    void decreaseAccuracy(int amount){
        accuracy.moddedValue -= amount;
        deriveCritChance();
    }

    //Decreases the amount of actions a character has left by 1
    //Input - None

    void useAction(){
        actionsLeft.moddedValue--;
    }

    //Resets the character's amount of actions
    //Input - None

    void resetActions(){
        actionsLeft.moddedValue = totalActions.moddedValue; 
    }

    //Adds a status effect to the character
    //Input - Effect to add, If the effect is coming from a tile or Buff/Debuff

    public void addStatus(StatusEffect effect, bool fromTile)
    {
        for (int i = 0; i < effect.statToEffect.Count; i++)
        {
            switch (effect.statToEffect[i])
            {
                case "health":
                    increaseHealth(effect.amount[i]);
                    break;

                case "attack":
                    increaseAttack(effect.amount[i]);
                    break;

                case "speed":
                    increaseSpeed(effect.amount[i]);
                    break;
            }
        }
        if (fromTile == false)
        {
            effects.Add(effect);

        }
        else
        {
            if (tileEffect != null)
            {
                removeStatus(tileEffect, true);
            }
            tileEffect = effect;
        }
    }

    //Removes a Status effect from the character
    //Input - Effect to remove, If the effect was from a tile or a normal Buff/Debuff


    public void removeStatus(StatusEffect effect, bool fromTile)
    {
        for (int i = 0; i < effect.statToEffect.Count; ++i)
        {
            switch (effect.statToEffect[i])
            {
                case "health":
                    decreaseHealth(effect.amount[i]);
                    break;

                case "attack":
                    decreaseAttack(effect.amount[i]);
                    break;

                case "speed":
                    decreaseSpeed(effect.amount[i]);
                    break;
            }
        }
        if (fromTile == false)
        {
            effects.Remove(effect);
        }
    }


}

[System.Serializable]
public class stat
{
    
    public int value;
    public int moddedValue;

}
