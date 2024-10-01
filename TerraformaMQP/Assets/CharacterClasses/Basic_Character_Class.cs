using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using UnityEngine;

public class Basic_Character_Class : MonoBehaviour
{
    [SerializeField]
    public bool turnEnded = false;
    public List<StatusEffect> effects; //Related Functions - addStatus, removeStatus
    public StatusEffect tileEffect;

    public stat health;  //Related Functions - takePhysicalDamage, takeMagicDamage, increaseHealth, decreaseHealth, checkHealth
    public stat maxHealth;  //Related Functions - increaseMaxHealth, decreaseMaxHealth
    public stat attack;  //Related Functions - increaseAttack, decreaseAttack
    public stat movementSpeed;  //Related Functions - increaseMoveSpeed, decreaseMoveSpeed
    public stat resistence;  //Related Functions - increaseResistence, decreaseResistence
    public stat defense;  //Related Functions - increaseDefense, decreaseDefense
    public stat speed;  //Related Functions - increaseSpeed, decreaseSpeed
    public stat criticalChance;  //Related Functions - increaseCritChance, decreaseCritChance, deriveCritChance
    public stat accuracy;  //Related Functions - increaseAccuracy, decreaseAccuracy
    public stat actionsLeft;  //Related Functions - useAction, resetActions
    public stat totalActions;
    public int attackReach = 1;
    public int defaultReach = 1;
    public string attackType = null;

    public StatusEffect testEffect;  //FOR TESTING PURPOSES ----- REQUIRED TO TEST APPLYING AND REMOVING A STATUS EFFECT INSIDE THIS CLASS
    public Color color;

    public int tileX = 0;
    public int tileY = 0;
    public TileType tileType;
    public ClickableTile tile;
    public TileMap map;

    public bool targeting = false;

    public bool charSelected = false;
    public bool charHover = false;

    public List<Node> path = null;

    public Camera camera;
    public Renderer renderer;





    // Start is called before the first frame update
    void Start()
    {
        color = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        //FOR TESTING PURPOSES ----- ALLOWS THE CHARACTER TO TAKE PHYSICAL DAMAGE WHEN P IS PRESSED AND MAGIC DAMAGE WHEN M IS PRESSED
        /*
        if (Input.GetKeyDown(KeyCode.P)) {
            this.takePhysicalDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            this.takeMagicDamage(1, "Fire");
        }
        */

        //FOR TESTING PURPOSES ----- APPLIES A BUFF TO THE CHARACTER WITH THE B KEY AND THEN REMOVES IT WITH THE N KEY ---- REQUIRES THE TESTEFFECT VARIABLE
        /*
        if (Input.GetKeyDown(KeyCode.B))
        {
            StatusEffect newEffect = new StatusEffect();
            List<string> stats = new List<string>();
            stats.Add("attack");
            List<int> amounts = new List<int>();
            amounts.Add(-10);
            newEffect.initializeStatusEffect(10, stats, "Cripple", amounts, this.gameObject);
            testEffect = newEffect;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            removeStatus(testEffect, false);
            testEffect = null;
        }
        */

        if (charSelected == true && turnEnded == false)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                attackType = "Attack";
                beginTargeting(attackReach);
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                if (gameObject.GetComponent<Hero_Character_Class>())
                {
                    gameObject.GetComponent<Hero_Character_Class>().openSpellBook();
                }
            }
            else if (Input.GetKeyDown(KeyCode.W)){
                endTurn();
            }
        }



    }

    //Deals Physical Damage to the character and checks if it reduces the health total below 0. Reduces the Damage value by the amount of Defense the character has
    //Input - Amount of Physical Damage Taken

    public void takePhysicalDamage(int damage){
        health.value = health.value - (damage - defense.moddedValue);
        UnityEngine.Debug.Log("Took " +  (damage - defense.moddedValue) + " physical damage");
        checkHealth();
        return;
    }

    //Deals Magic Damage to the character and checks if it reduces the health total below 0. Reduces the Damage value by the amount of Resistence the character has
    //Input - Amount of Magic Damage Taken

    public void takeMagicDamage(int damage, string magicType){
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
        tile.isWalkable = true;
        tile.characterOnTile = null;
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
        //criticalChance.moddedValue = accuracy.moddedValue / 100;
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

    //Increases the characters max health value
    //Input - Amount of health to increase by

    void increaseMaxHealth(int amount){
        maxHealth.moddedValue += amount;
    }

    //Decreases the character's max health value
    //Input - Amount of health to decrease by

    void decreaseMaxHealth(int amount){
        maxHealth.moddedValue -= amount;
    }

    //Increases the character's total actions value
    //Input - Amount to increase the total actions by

    void increaseTotalActions(int amount)
    {
        totalActions.moddedValue += amount;
    }

    //Decreases the character's total actions value
    //Input - Amount to decrease the total actions by

    void decreaseTotalActions(int amount)
    {
        totalActions.moddedValue -= amount;
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

                case "maxHealth":
                    increaseMaxHealth(effect.amount[i]);
                    break;

                case "movementSpeed":
                    increaseMoveSpeed(effect.amount[i]);
                    break;

                case "resistence":
                    increaseResistence(effect.amount[i]);
                    break;

                case "defense":
                    increaseDefense(effect.amount[i]);
                    break;

                case "criticalChance":
                    increaseCritChance(effect.amount[i]);
                    break;

                case "accuracy":
                    increaseAccuracy(effect.amount[i]);
                    break;

                case "totalActions":
                    increaseTotalActions(effect.amount[i]);
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

                case "maxHealth":
                    decreaseMaxHealth(effect.amount[i]);
                    break;

                case "movementSpeed":
                    decreaseMoveSpeed(effect.amount[i]);
                    break;

                case "resistence":
                    decreaseResistence(effect.amount[i]);
                    break;

                case "defense":
                    decreaseDefense(effect.amount[i]);
                    break;

                case "criticalChance":
                    decreaseCritChance(effect.amount[i]);
                    break;

                case "accuracy":
                    decreaseAccuracy(effect.amount[i]);
                    break;

                case "totalActions":
                    decreaseTotalActions(effect.amount[i]);
                    break;

            }
        }
        if (fromTile == false)
        {
            effects.Remove(effect);
        }
    }

    public bool attackCharacter(GameObject target, int damageAmount)
    {
        target.GetComponent<Basic_Character_Class>().takePhysicalDamage(damageAmount);
        stopTargeting();
        if (takeAction() == false)
        {
            renderer.material.color = Color.red;
            return false;
        }
        return true;
    }

    public bool castSpell(GameObject target)
    {
        gameObject.GetComponent<Hero_Character_Class>().castSpell(target);
        stopTargeting();
        if (takeAction() == false)
        {
            renderer.material.color = Color.red;
            return false;
        }
        return true;
    }

    public void endTurn()
    {
        actionsLeft.moddedValue = 0;
        renderer.material.color = Color.gray;
        turnEnded = true;
        charSelected = false;
        if (gameObject.GetComponent<Hero_Character_Class>())
        {
            gameObject.GetComponent<Hero_Character_Class>().pickingSpell = false;
        }
    }

    public void stopTargeting()
    {
        if (attackType == "Spell")
        {
            gameObject.GetComponent<Hero_Character_Class>().selectedSpell = null;
        }
        attackType = null;
        removeReach(attackReach);
        attackReach = defaultReach;
        targeting = false;
        renderer.material.color = Color.red;
        displayStats();
    }

    public void beginTargeting(int reach)
    {
        renderer.material.color = Color.yellow;
        UnityEngine.Debug.Log("Targeting an Attack");
        targeting = true;
        drawReach(reach);
    }

    public void beginTargetingSpell(int reach, Basic_Spell_Class spell)
    {
        attackType = "Spell";
        renderer.material.color = Color.magenta;
        targeting = true;
        attackReach = reach;
        drawReach(reach);
        if (spell.targetTiles)
        {
            drawSpellReach(reach, spell);
        }
    }

    public void displayStats()
    {
        UnityEngine.Debug.Log("Press A to Attack, M to cast Magic, or W to Wait");
    }

    //Recolors when mouse is hovering over a unit
    public void OnMouseEnter()
    {
        if (charSelected == false)
        {
            renderer.material.color = Color.blue;
        }
        UnityEngine.Debug.Log("Mouse Entered");
        charHover = true;
    }

    //Resets when mouse has stopped hovering over a unit
    public void OnMouseExit()
    {
        if (charSelected == false)
        {
            if (turnEnded == false)
            {
                renderer.material.color = color;
            }
            else
            {
                renderer.material.color = Color.gray;
            }
        }
        charHover = false;
        UnityEngine.Debug.Log("Mouse Exited");
    }

    private void drawReach(int reach)
    {
        map.drawReach(reach);
    }

    private void drawSpellReach(int reach, Basic_Spell_Class spell)
    {
        map.drawSpellReach(reach, spell);
    }

    public void removeReach(int reach)
    {
        map.removeReach(reach);
    }

    public void selectCharacter()
    {
        if (gameObject.tag == "PlayerTeam")
        {
            charSelected = true;
        }
        displayStats();
        renderer.material.color = Color.red;
    }

    public void deselectCharacter()
    {
        charSelected = false;
        if (turnEnded == false)
        {
            renderer.material.color = color;
        }
        else
        {
            renderer.material.color = Color.gray;
        }
    }

    public bool takeAction()
    {
        actionsLeft.moddedValue--;
        if (actionsLeft.moddedValue <= 0)
        {
            deselectCharacter();
            endTurn();
            return true;
        }
        return false;
    }

    public bool withinReach(GameObject selectedTarget)
    {
        if (map.checkForTarget(selectedTarget, attackReach))
        {
            return true;
        }
        return false;
    }

    public void resetTurn()
    {
        turnEnded = false;
        renderer.material.color = color;
        actionsLeft.moddedValue = actionsLeft.value;
    }


}

[System.Serializable]
public class stat
{
    
    public int value;
    public int moddedValue;

}
