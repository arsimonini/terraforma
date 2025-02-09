using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Basic_Character_Class : MonoBehaviour
{
    [SerializeField]
    public bool turnEnded = false; //Whether the unit's turn has been ended, true if so, false if not
    public List<StatusEffect> effects; //Related Functions - addStatus, removeStatus
    public StatusEffect tileEffect; //Current tileEffect acting upon the character ---SHOULD (ALMOST) ALWAYS BE SET TO THE TILE UNDER THE CHARACTER---


    public string name; //The name of the character
    public Sprite char_img; //Image that is displayed in the UI when character is selected
    public int health;  //Current health value of the character - Related Functions - takePhysicalDamage, takeMagicDamage, increaseHealth, decreaseHealth, checkHealth
    public stat maxHealth;  //The maximum health value that a character can have - Related Functions - increaseMaxHealth, decreaseMaxHealth
    public stat attack;  //The amount of attack that a character deals - Related Functions - increaseAttack, decreaseAttack
    public stat movementSpeed;  //The distance a character can move in a single turn - Related Functions - increaseMoveSpeed, decreaseMoveSpeed
    public stat resistence;  //How much the character can resist the damage from magical attacks - Related Functions - increaseResistence, decreaseResistence
    public stat defense;  //How much the character can resiste the damage from physical attacks - Related Functions - increaseDefense, decreaseDefense
    public stat speed;  //The chance that a character has to dodge attacks - Related Functions - increaseSpeed, decreaseSpeed
    public stat criticalChance;  //The chance a character has to deal critical damage on an attack - Related Functions - increaseCritChance, decreaseCritChance, deriveCritChance
    public stat accuracy;  //The chance a character has to land an attack - Related Functions - increaseAccuracy, decreaseAccuracy
    public stat actionsLeft;  //The amount of actions that the character can still perform on their turn - Related Functions - useAction, resetActions
    public stat totalActions; //The total amount of actions a character can perform on their turn
    public int attackReach = 1; //The reach the character can hit with the currently being targeted spell or attack
    public int defaultReach = 1; //The default reach the character can hit with an attack
    public string attackType = null; //What is being targeting, either a "Spell" or "Attack", if not currently targeting will be null
    public List<BuffClass> buffs = new List<BuffClass>();
    public GameObject atkPrefab;
    public GameObject critPrefab;
    public GameObject missPrefab;
    public GameObject abilityPrefab;

    public Color color; //Color of the shape ---WILL BE DELETED WHEN MODELS ARE ADDED---

    public int tileX = 0; //The X value of the tile the character is on
    public int tileY = 0; //The Y value of the tile the character is on
    public TileType tileType; //The type of tile the character is on
    public ClickableTile tile; //Reference to the instance of the tile the character is on
    public TileMap map; //Reference to the map

    public bool targeting = false;
    public bool isMoving = false;

    public bool charSelected = false; //If the character is currently selected, true if selected, false if not
    public bool charHover = false; //If the character is currently being hovered over by the mouse, true if being hovered over, false if not

    public List<Node> path = null; //The current path the character is travelling? ---NOT SURE WHY THIS IS HERE, NOT BEING USED AT ALL INSIDE THIS SCRIPT---

    public Camera camera; //Reference to the camera ---ALSO NOT SURE WHY THIS IS HERE, NOT BEING USED AT ALL INSIDE THIS SCRIPT---
    public SpriteRenderer renderer; //Reference to the renderer that is attached to the GameObject
    //TODO NOTE FROM ANDREW: CHANGED RENDERER TO SPRITERENDERER

    public Nameplate nameplate;
    public GameObject np2;

    public GameObject atkMenu;
    public GameObject spellList;
    
    public bool hasWalked = false;
    public bool spellListIsUp = false;

    public CombatLog comlog;

    public PauseMenu pm;

    [SerializeField] private AudioClip[] physicallyAttacked;
    [SerializeField] private AudioClip[] openAtkMenu;
    [SerializeField] private AudioClip[] closeAtkMenu;
    [SerializeField] private AudioClip[] missedAttack;


    public int startHP = 0; //This is needed for Stone Body to work, as it tracks the HP at the start of Wold's spell and





    // Start is called before the first frame update
    //IEnumerator is used to ensure that the start function for all characters is run after the creation of the tile map
    IEnumerator Start()
    {
        //Wait for the map to be created
        while(map.mapCreated != true){
            yield return null;
        }
        //UnityEngine.Debug.Log("Character Created");
        //Retrieve the unit's base color
        color = renderer.material.color;
        //After map is created, set the characters to be located on their designated tiles on the map
        map.clickableTiles[tileX, tileY].characterOnTile = this.gameObject;
        tile = map.clickableTiles[tileX, tileY];    
        //Make sure the correct tileEffect is applied to the character
        map.addTileEffect(tileX, tileY, this.gameObject);
        //Set the tile to be unwalkable because there is a unit occupying it
        map.clickableTiles[tileX, tileY].isWalkable = false;
        //nameplate = transfrom.root.GetComponent<Nameplate>();

        tileType = map.tileTypes[map.tiles[tileX, tileY]];
        
    }

    void Update()
    {

        // if (charSelected && Input.GetKeyDown(KeyCode.Y)){
        //     //map.displayMovementLimit(tileX, tileY);
        // }

        if (charSelected == true && turnEnded == false & map.moving == false & map.moveButtonPressed == false && targeting == false)
        {
            if(isMoving == false) {
                displayAttackMenu(true);
            }

            // if (Input.GetKeyDown(KeyCode.N))
            // {
            //     //Start targeting an attack
            //     attackType = "Attack";
            //     beginTargeting(attackReach);
            // }
            //Check if the player is pressing the M key, not currently targeting an attack, and is a hero character
            // else if (Input.GetKeyDown(KeyCode.M) && targeting == false && gameObject.GetComponent<Hero_Character_Class>() != null)
            // {
            //     //Open the menu to allow the character to select a spell to cast by calling the openSpellBook function within the Hero Class ---CAN ONLY BE EXECUTED BY HERO CLASS CHARACTERS---
            //     gameObject.GetComponent<Hero_Character_Class>().openSpellBook();
            // }
            //Check if the player is pressing the B key
            // else if (Input.GetKeyDown(KeyCode.B)){
            //     //End the unit's turn
            //     //endTurn();
            //     //map.hidePath();
            // }
            updateCharStats();
        }

        if(map.moving == true) {
            displayAttackMenu(false);
            displaySpellList(false);
        }

        if(this.gameObject.tag == "EnemyTeam") {
            //updateCharStats();
        }


        // if(spellListIsUp == false && charSelected == true && this.gameObject.GetComponent<Hero_Character_Class>() == true) {

        //     //UnityEngine.Debug.Log("PLEASE HELP ME");

        //     GameObject t = GameObject.Find("SpellDescCanvas(Clone)");
        //     Destroy(t);
        // }

        if(charSelected == false && pm.forDeselecting == true) {
            Invoke("deselectOnResume", 0.05f);
        }
        // if (Input.GetKeyDown(KeyCode.B) && charSelected){
        //     abilityButtonUI(0);
        // }
    }

    //Deals Physical Damage to the character and checks if it reduces the health total below 0. Reduces the Damage value by the amount of Defense the character has
    //Input - Amount of Physical Damage Taken

    public void takePhysicalDamage(int damage){
        float mitigatedDamage = Mathf.Round((float)damage * (20f/(20f + (float)defense.moddedValue)));
        health = health - (int)mitigatedDamage;
        //UnityEngine.Debug.Log("Took " +  mitigatedDamage + " physical damage");
        
        SFXController.instance.PlayRandomSFXClip(physicallyAttacked, transform, 1f);
        
        checkHealth();
        return;
    }

    //Deals Magic Damage to the character and checks if it reduces the health total below 0. Reduces the Damage value by the amount of Resistence the character has
    //Input - Amount of Magic Damage Taken

    public void takeMagicDamage(int damage, string magicType){
        float mitigatedDamage = Mathf.Round((float)damage * (20f/(20f + (float)resistence.moddedValue)));
        health = health - (int)mitigatedDamage;
        //UnityEngine.Debug.Log("Took " + mitigatedDamage + " magic damage");
        checkHealth();
        comlog.addText("  -> " + name + " has Taken " + mitigatedDamage.ToString() + " " + magicType + " Damage");
    }

    //Increases the Health Total of the character
    //Input - Amount of health to increase by

    void increaseHealth(int amount){
        health = health + amount;
        if (health > maxHealth.moddedValue) {
            health = maxHealth.moddedValue;
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
        //UnityEngine.Debug.Log("Destroyed");
        tile.isWalkable = true;
        tile.characterOnTile = null;
        displayNameplate(false);
        Destroy(gameObject);
        return;
    }

    //Checks if the Character's Health is below 0 and destroys it if so

    void checkHealth() {
        if (health <= 0){
            map.heroes.Remove(gameObject);
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

    public void addStatus(StatusEffect effect, bool fromTile, List<int> tempAmounts = null, List<string> tempNames = null, bool fromReact = false)
    {
        //Iterates over the list of stats that need to be effected
        for (int i = 0; i < effect.statToEffect.Count; i++)
        {
            //Checks which stat to effect and then applies the change
            switch (effect.statToEffect[i])
            {
                case "health":
                    if (fromReact){
                        increaseHealth(effect.amount[i]);    
                    }
                    increaseHealth(effect.amount[i]);
                    break;

                case "attack":
                    if (fromReact){
                        increaseAttack(effect.amount[i]);    
                    }
                    increaseAttack(effect.amount[i]);
                    break;

                case "speed":
                    if (fromReact){
                        increaseSpeed(effect.amount[i]);    
                    }
                    increaseSpeed(effect.amount[i]);
                    break;

                case "maxHealth":
                    if (fromReact){
                        increaseMaxHealth(effect.amount[i]);    
                    }
                    increaseMaxHealth(effect.amount[i]);
                    break;

                case "movementSpeed":
                    if (fromReact){
                        increaseMoveSpeed(effect.amount[i]);    
                    }
                    increaseMoveSpeed(effect.amount[i]);
                    break;

                case "resistence":
                    if (fromReact){
                        increaseResistence(effect.amount[i]);    
                    }
                    increaseResistence(effect.amount[i]);
                    break;

                case "defense":
                    if (fromReact){
                        increaseDefense(effect.amount[i]);    
                    }
                    increaseDefense(effect.amount[i]);
                    break;

                case "criticalChance":
                    if (fromReact){
                        increaseCritChance(effect.amount[i]);    
                    }
                    increaseCritChance(effect.amount[i]);
                    break;

                case "accuracy":
                    if (fromReact){
                        increaseAccuracy(effect.amount[i]);    
                    }
                    increaseAccuracy(effect.amount[i]);
                    break;

                case "totalActions":
                    if (fromReact){
                        increaseTotalActions(effect.amount[i]);    
                    }
                    increaseTotalActions(effect.amount[i]);
                    break;

                case "magic":
                    if (this.gameObject.GetComponent<Hero_Character_Class>()){
                        if (fromReact){
                            this.gameObject.GetComponent<Hero_Character_Class>().increaseMagic(effect.amount[i]);
                        }
                        this.gameObject.GetComponent<Hero_Character_Class>().increaseMagic(effect.amount[i]);
                    }
                    break;

            }
        }
        //Checks if the effect came from a tile or a different source
        if (fromTile == false)
        {
            //If not from a tile adds the effect to the list of current effects on the unit
            effects.Add(effect);

        }
        //If from a tile then sets the tileEffect to the new effect
        else
        {
            //Checks if there was already a tileEffect on the character
            if (tileEffect != null)
            {
                //If so, removes the effect from the character by calling the removeStatus function
                removeStatus(tileEffect, true);
            }
            tileEffect = effect;
        }
    }

    //Removes a Status effect from the character
    //Input - Effect to remove, If the effect was from a tile or a normal Buff/Debuff

    public void removeStatus(StatusEffect effect, bool fromTile)
    {
        //Iterates over the list of stats to affect
        for (int i = 0; i < effect.statToEffect.Count; ++i)
        {
            //Checks which stat to effect and then applies the change
            switch (effect.statToEffect[i])
            {
                case "health":
                    decreaseHealth(effect.amount[i]);
                    break;

                case "attack":
                    UnityEngine.Debug.Log(effect.amount[i]);
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
                
                case "magic":
                    if (this.gameObject.GetComponent<Hero_Character_Class>()){
                        this.gameObject.GetComponent<Hero_Character_Class>().decreaseMagic(effect.amount[i]);
                    }
                    break;

            }
        }
        //Checks if the effect came from a tile or another source
        if (fromTile == false)
        {
            //If from another source, the effect is located within the list of effects on the character and then removed
            effects.Remove(effect);
        }
    }

    //Called when the player tries to use a physical attack on an enemy
    //Takes in the selected enemy and the amount of damage to deal
    //Returns true if the character has no actions left after the attack, false if the character still has at least one action
    public bool attackCharacter(GameObject target, int damageAmount)
    {
        //Calls the takePhysicalDamage function on the target, passing in the damage amount
        Basic_Character_Class targetCharacter = target.GetComponent<Basic_Character_Class>();
        ClickableTile targetTile = target.GetComponent<ClickableTile>();
        if (targetCharacter == null){
            targetCharacter = targetTile.characterOnTile.GetComponent<Basic_Character_Class>();
        }
        if (targetCharacter != null){

            int targetCurrHealth = targetCharacter.health;

            if (checkAccuracy(targetCharacter.speed.moddedValue)) { //Attack Lands
                if (checkCrit()) {
                    targetCharacter.takePhysicalDamage(2*damageAmount); //Critical Hit!!!
                    comlog.addText("-> " + name + " Has Landed a Critical Attack on " + targetCharacter.name + " Dealing " + (targetCurrHealth - targetCharacter.health).ToString() + " Damage");
                    //UnityEngine.Debug.Log("FAIR AND BALANCED");
                    Vector3 newPos = Vector3.Lerp(this.gameObject.transform.position, target.gameObject.transform.position, 0.5f);
                    
                    
                    if(this.gameObject.tag == "EnemyTeam") {
                        GameObject callToPrefab = Instantiate(critPrefab);    
                        callToPrefab.GetComponent<Billboard>().cam = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Billboard>().cam;
                        callToPrefab.transform.position = newPos;
                    }
                    else if(this.gameObject.tag == "PlayerTeam") {
                        if(this.gameObject.GetComponent<Hero_Character_Class>() == null) {
                            GameObject callToPrefab = Instantiate(critPrefab);
                        }else {
                            GameObject callToPrefab = Instantiate(critPrefab);    
                            callToPrefab.GetComponent<Billboard>().cam = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Billboard>().cam;
                            callToPrefab.transform.position = newPos;
                        }

                    }
                } else {
                    Vector3 newPos = Vector3.Lerp(this.gameObject.transform.position, target.gameObject.transform.position, 0.5f);

                    if(this.gameObject.tag == "EnemyTeam") {
                        GameObject callToPrefab = Instantiate(atkPrefab);    
                        callToPrefab.GetComponent<Billboard>().cam = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Billboard>().cam;
                        callToPrefab.transform.position = newPos;
                    }
                    else if(this.gameObject.tag == "PlayerTeam") {
                        if(this.gameObject.GetComponent<Hero_Character_Class>() == null) {
                            GameObject callToPrefab = Instantiate(critPrefab);
                        }else {
                            GameObject callToPrefab = Instantiate(critPrefab);    
                            callToPrefab.GetComponent<Billboard>().cam = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Billboard>().cam;
                            callToPrefab.transform.position = newPos;
                        }
                    }

                    //Commented this out as new attack prefab doesn't require it
                    
                    targetCharacter.takePhysicalDamage(damageAmount);
                    comlog.addText("-> " +  name + " Has Landed an Attack on " + targetCharacter.name + " Dealing " + (targetCurrHealth - targetCharacter.health).ToString() + " Damage");
                }

                targetCharacter.updateCharStats();
            } else { //Attack Misses
                comlog.addText("-> " +  name + " Has Missed their Attack on " + targetCharacter.name);
                SFXController.instance.PlayRandomSFXClip(missedAttack, transform, 1f);
                                    //UnityEngine.Debug.Log("FAIR AND BALANCED");
                Vector3 newPos = Vector3.Lerp(this.gameObject.transform.position, target.gameObject.transform.position, 0.5f);
                GameObject callToPrefab = Instantiate(missPrefab);
                callToPrefab.GetComponent<Billboard>().cam = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Billboard>().cam;
                callToPrefab.transform.position = newPos;
            }
        } else if (targetTile != null && targetTile.isBreakable) { //Damage Tile
            targetTile.hp -= damageAmount;
            UnityEngine.Debug.Log("Tile Health: " + targetTile.hp);
            SFXController.instance.PlayRandomSFXClip(physicallyAttacked, transform, 1f);


            if (targetTile.hp <= 0) {
                targetTile.breakTile();
            }
            //callToPrefab.transform.Find("slash-effect_0").GetComponent<SpriteRenderer>().flipX = true;
        }
   
        //Stops targeting
        stopTargeting();
        //Uses the action, and then checks if there are still actions remaining
        if (takeAction() == false)
        {
            //The character still has at least one action
            renderer.material.color = Color.red;
            return false;
        }
        //The character has no actions left
        return true;
    }

    //Checks whether the enemy will hit or dodge
    public bool checkAccuracy(float speed) {
        float acc = accuracy.moddedValue;
        float cover = 0;

		float scale = 3;
		
        //Random rand = new Random();
		//double scale = 3;
		
		int hit = (int) Math.Round(75+scale*(acc-speed-cover));
		int miss = (int) (UnityEngine.Random.Range(0,100));
		
		if (hit>miss) return true;
		return false;
    }

    public bool checkAccuracyOld(float speed) {
        float acc = accuracy.moddedValue;
        float cover = 0;

		float scale = 2;
		float foeScale = 2;
		
		//Calculates hit rate; should average around 80 without considering enemy evasion
		int playerFormula = (int) (UnityEngine.Random.Range(0,100)-acc*scale);
		int enemyFormula = (int) Math.Round(75-foeScale*(speed+cover));
		
		if (playerFormula < enemyFormula) return true;
        
        return false;
    }
    //Checks crit
    public bool checkCrit() {
        float crit = criticalChance.moddedValue;
        int formula = (UnityEngine.Random.Range(0,20));

        if (crit > formula) return true;
        return false;
    }

    //Called when player tries to physically attack a piece of terrain at a location, likely a Wold Wall
    //Takes in the selected target and the type of damage to deal
    //
    public bool attackTile(int xPos, int yPos, int damageAmount)
    {
        //Calls the takePhysicalDamage function on the target, passing in the damage amount
        

        //Stops targeting
        stopTargeting();
        //Uses the action, and then checks if there are still actions remaining
        if (takeAction() == false) {
            //The character still has at least one action
            renderer.material.color = Color.red;
            return false;
        }
        //The character has no actions left
        return true;
    }

    //Called when the player tries to cast a spell
    //Takes in a list of GameObjects as targets
    //Returns true if the character has no actions left after the spell, false if the character still has at least one action
    public bool castSpell(List<GameObject> targets)
    {
        if (gameObject.GetComponent<Hero_Character_Class>()){
            //Calls the castSpell function in the Hero Class, passing the list of targets as the argument
            gameObject.GetComponent<Hero_Character_Class>().castSpell(targets);
            //Calls the useMana function in the Hero Class to reduce the amount of mana the unit has remaining
            gameObject.GetComponent<Hero_Character_Class>().useMana(gameObject.GetComponent<Hero_Character_Class>().selectedSpell.manaCost);
            //Stops targeting
            stopTargeting();
            //Uses the action, and then checks if there are still actions remaining
            if (takeAction() == false)
            {
                //The character sill has at least one action
                renderer.material.color = Color.red;
                return false;
            }
            //The character has no actions left
            return true;
        }
        else {
            gameObject.GetComponent<SummonClass>().useAbility(targets);

            GameObject callToPrefab = Instantiate(abilityPrefab);    

            stopTargeting();
            if(takeAction() == false){
                renderer.material.color = Color.red;
                return false;
            }
            return true;
        }
    }

    //Ends the unit's turn
    public void endTurn()
    {
        //Sets the current remaining actions to 0, changes the color to gray, sets the turnEnded variable to true, and deselects the character
        comlog.addText("-> " + name + " is Waiting");

        deselectCharacter();
        actionsLeft.moddedValue = 0;
        //if (!this.gameObject.GetComponent<Enemy_Character_Class>()){
        renderer.material.color = Color.gray;
        
        turnEnded = true;
        charSelected = false;
        //Turns off attack menu
        displayAttackMenu(false);
        displaySpellList(false);
        if (gameObject.GetComponent<Hero_Character_Class>())
        {
            gameObject.GetComponent<Hero_Character_Class>().pickingSpell = false;
        }
    }

    //Stops targeting, removes the highlighted range, resets the attackType, attackReach variables
    public void stopTargeting()
    {
        map.removeAOEDisplay();
        //Checks if the player was targeting a spell or normal attack
        if ((attackType == "Spell" || attackType == "Ability") && gameObject.GetComponent<Hero_Character_Class>() != null)
        {
            //If the player was targeting a spell, sets the selectedSpell in the Hero Class to null
            gameObject.GetComponent<Hero_Character_Class>().selectedSpell = null;
        }
        //Sets attackType to null, clears the highlighted reach, sets the attackReach back to the defaultReach, sets targeting to false, resets the color
        attackType = null;
        removeReach();
        attackReach = defaultReach;
        targeting = false;
        renderer.material.color = Color.red;
    }

    //Called when targeting a standard attack
    //Takes in an integer value of the reach the attack can hit
    public void beginTargeting(int reach)
    {
        //Sets unit color to yellow
        renderer.material.color = Color.yellow;
        //UnityEngine.Debug.Log("Targeting an Attack");
        //Sets targeting to true
        targeting = true;
        //Calls the drawReach function with the reach of the attack, the inability to target tiles, and the inability to target allies
        drawReach(reach, false, false, true, false, false, true, false, false, null, false, null, tile,true);
    }

    //Called when targeting a Spell
    //Takes in the reach of the spell and the Spell being targeted
    public void beginTargetingSpell(int reach, Basic_Spell_Class spell)
    {
        //Sets the attack type to spell
        attackType = "Spell";
        //Sets the unit color to magenta to designate casting a Spell
        renderer.material.color = Color.magenta;
        //Sets targeting to true
        targeting = true;
        //Sets the attackReach to the inputted reach
        attackReach = reach;
        //Calls the drawReach function with the reach of the spell, the spell's ability to target tiles, and the spell's ability to target allies
        drawReach(reach, spell.targetTiles, spell.targetAllies, spell.targetEnemies, spell.hitOwnTile, spell.hitSelf, spell.targetWalls, spell.hyperSpecificTargeting, spell.needSpecificTileEffects, spell.specificTileEffects, spell.needSpecificTiles, spell.specificTiles, tile, targetBreakables: spell.targetBreakables);
    }

    public void beginTargetingAbility(int reach, Basic_Spell_Class spell){
        attackType = "Ability";
        renderer.material.color = Color.magenta;
        targeting = true;
        attackReach = reach;
        drawReach(reach, spell.targetTiles, spell.targetAllies, spell.targetEnemies, spell.hitOwnTile, spell.hitSelf, spell.targetWalls, spell.hyperSpecificTargeting, spell.needSpecificTileEffects, spell.specificTileEffects, spell.needSpecificTiles, spell.specificTiles, tile, targetBreakables: spell.targetBreakables);
    }

    //Called when selecting a unit, displays the current health and, if possible, the mana of the selected unit, along with their char_img
    //Takes in a Boolean, if true, sets the canvas to active, if false the canvas is set to inactive
    //---MIGHT WANT TO WRAP MOST FUNCTIONALITY INSIDE IF STATEMENT, DOESN'T NEED TO BE SET WHEN THE CANVAS IS JUST BEING SET TO INACTIVE---
    public void displayNameplate(bool b)
    {
        //Sets the values of the nameplate
        nameplate.displayName(name);
        nameplate.displayImage(char_img);
        nameplate.displayHealth(health, maxHealth);

        nameplate.displayStatBlock(attack, defense, resistence, accuracy, criticalChance, speed);
        nameplate.displayHealthValue(health, maxHealth);

        //Displays the Effects
        nameplate.getTileNum(buffs, tileType.name, tile, b);
        // nameplate.displayEffect(b);
        // nameplate.exEffButtonNeeded(b);
        // if(b == false) {
        //     nameplate.openExtraEffects(b);
        // }

        //Checks if the character is a Hero and has mana
        if (gameObject.GetComponent<Hero_Character_Class>() != null)
        {
            //If the character does have mana, it is also passed to the nameplate and the mana bar is set to active
            nameplate.displayMana(gameObject.GetComponent<Hero_Character_Class>().mana, gameObject.GetComponent<Hero_Character_Class>().maxMana);
            nameplate.displayManaValue(gameObject.GetComponent<Hero_Character_Class>().mana, gameObject.GetComponent<Hero_Character_Class>().maxMana);
            nameplate.mana.gameObject.SetActive(true);
            nameplate.displayMag(gameObject.GetComponent<Hero_Character_Class>().magic);
            nameplate.displayMagBuff(gameObject.GetComponent<Hero_Character_Class>().magic);
            nameplate.displayMagicArea(true);
        }
        else
        {
            //If the character doesn't have mana, the mana bar is just set to inactive
            nameplate.mana.gameObject.SetActive(false);
            nameplate.displayMagicArea(false);
        }
        //Set the canvas to either active or inactive, depending on the inputted bool
        np2.SetActive(b);
    }

    public void displayAttackMenu(bool b)
    {

        if(b == false && atkMenu.GetComponent<Billboard>() != null) {
            atkMenu.GetComponent<Billboard>().uiHover = false;
        }


        atkMenu.SetActive(b);

        if(turnEnded == true && b == true) {
            atkMenu.SetActive(false);
            if (spellList != null){
                spellList.SetActive(false);
            }
        }
    }

    public void displaySpellList(bool b)
    {
        if (spellList != null){
            if(b == true) {
                spellList.SetActive(b);
                spellListIsUp = true;
            }
            else if (b == false) {
                spellList.SetActive(b);
                spellListIsUp = false;
            }
        }
        if(turnEnded == true && b == true) {
            if (spellList != null){
                spellList.SetActive(false);
                spellListIsUp = false;
            }
            atkMenu.SetActive(false);
        }
    }

    public void displayAbilityList(bool b){
        //Needs to be filled out
    }

    //Recolors when mouse is hovering over a unit
    public void OnMouseEnter()
    {
        if(pm.GameIsPaused != true) {

            if (this.gameObject.GetComponent<Enemy_Character_Class>() != null && map.movingEnemy == true){
                displayNameplate(true);
                renderer.material.color = Color.blue;
            }

            //Checks if the unit is currently selected
            if (charSelected == false && (map.selectedUnit == null || this.gameObject.tag != map.selectedUnit.tag))
            {
                //If not, the color is changed to the highlight color
                renderer.material.color = Color.blue;
                //if unit is not selected, display a nameplate
                displayNameplate(true);
            }
            //UnityEngine.Debug.Log("Mouse Entered");
            //Set the hover variable to true
            charHover = true;

            tile.OnMouseEnter();
        }
    }

    //Resets when mouse has stopped hovering over a unit
    public void OnMouseExit()
    {
        if(pm.GameIsPaused != true) {
            //Checks if the unit is currently selected
            if (charSelected == false && (map.selectedUnit == null || this.gameObject.tag != map.selectedUnit.tag))
            {
                //Checks if the unit's turn has been ended
                if (turnEnded == false)
                {
                    //If the unit's turn isn't over and isn't selected, reset the color to it's base
                    renderer.material.color = color;
                    //If the character is not selected, and the turn is not over turn off nameplate
                    displayNameplate(false);
                }
                else
                {
                    //Otherwise set the color to gray
                    renderer.material.color = Color.gray;
                    displayNameplate(false);

                }
            }
            if (this.gameObject.GetComponent<Enemy_Character_Class>() != null && map.movingEnemy == true){
                renderer.material.color = color;
                displayNameplate(false);
                //updateCharStats();
            }
            //Set the hover variable to false
            charHover = false;
            //UnityEngine.Debug.Log("Mouse Exited");

            tile.OnMouseExit();
        }
    }

    //Calls the drawReach function within the map, passing the same variables from the parameters as arguments
    private void drawReach(int reach, bool targetTiles, bool targetAllies, bool targetEnemies, bool hitOwnTile, bool hitSelf, bool targetWalls, bool hyperSpecificTargeting, bool needSpecificTileEffects, List<string> specificTileEffects, bool needSpecificTiles, List<string> specificTiles, ClickableTile tile, bool targetBreakables = false)
    {
        map.drawReach(reach, targetTiles, targetAllies, targetEnemies, targetWalls, hyperSpecificTargeting, needSpecificTileEffects, specificTileEffects, needSpecificTiles, specificTiles, tile,targetBreakables);
        if(!hitOwnTile){
            if (map.targetList.Contains(tile.gameObject)){
                map.targetList.Remove(tile.gameObject);
                tile.endHighlight();
            }
        }
        if (!hitSelf && map.targetList.Contains(this.gameObject)){
            map.targetList.Remove(this.gameObject);
            this.tile.endHighlight();
        }
    }

    //Calls the removeReach function within the map
    public void removeReach()
    {
        map.removeReach();
    }

    //Selects the current character
    public void selectCharacter()
    {
        if (gameObject.tag == "PlayerTeam")
        {
            //If the unit is part of the player team, allow the player to move it and perform actions
            charSelected = true;
            SFXController.instance.PlayRandomSFXClip(openAtkMenu, transform, 1f);
        }
        //Display the health and mana of the selected unit
        displayNameplate(true);  
        //Set the unit's color to red      
        renderer.material.color = Color.red;
    }

    //Deselects the current character
    public void deselectCharacter()
    {
        //Sets the charSelected variable to false, removes the arrow path from the map, hides the selection UI
        charSelected = false;
        map.hidePath();
        displayNameplate(false);
        displayAttackMenu(false);
        displaySpellList(false);
        map.setMoveButtonPressed(false);
        map.removeMovementLimit();
        isMoving = false;

        GameObject t = GameObject.Find("SpellDesc(Clone)");
        if(t != null) {
            Destroy(t);
        }

        
        if (turnEnded == false)
        {
            //If it hasn't ended, reset its color to its base color
            renderer.material.color = color;
            OnMouseExit();
        }
        else
        {
            //If it has ended, change the color to gray
            renderer.material.color = Color.gray;
        }
    }

    //Takes an action, returns true if the character then has no more remaining actions, and false if it still has actions
    public bool takeAction()
    {
        //Reduce the amount of actions the character has left by 1
        actionsLeft.moddedValue--;
        //Check if the amount of actions left is below or equal to 0
        if (actionsLeft.moddedValue <= 0)
        {
            //If so, deselect the character and end it's turn before returning true 
            deselectCharacter();
            endTurn();
            return true;
        }
        //Else, return false
        return false;
    }

    //Check if a selected GameObject is within the reach of a unit while targeting
    //Takes in a GameObject as a target
    //Returns true if the target is within reach, false if not
    public bool withinReach(GameObject selectedTarget)
    {
        //Call the checkForTarget function in the map, using the selectedTarget parameter and the attackReach variable as the arguments
        if (map.checkForTarget(selectedTarget, attackReach))
        {
            //UnityEngine.Debug.Log("Target Found");
            //Return true if the target is found
            return true;
        }
        //Return false is not found
        return false;
    }

    //Resets the unit to take a new turn
    public void resetTurn()
    {
        turnEnded = false;
        renderer.material.color = color;
        actionsLeft.moddedValue = actionsLeft.value;
    }

    public void moveButtonUI() {
    UnityEngine.Debug.Log("Move is Clicked");
        map.setMoveButtonPressed(true);
        displayAttackMenu(false);
        isMoving = true;
        map.displayMovementLimit(tileX, tileY);
    }

    public void attackButtonUI() {
        UnityEngine.Debug.Log("Attack is Clicked");
        attackType = "Attack";
        displayAttackMenu(false);
        beginTargeting(attackReach);
    }

    public void magicButtonUI() {
        UnityEngine.Debug.Log("Magic is Clicked");
        displaySpellList(true);
        SFXController.instance.PlayRandomSFXClip(openAtkMenu, transform, 1f);
        
    }

    public void spellButtonUI(int i) {
        if(this.gameObject.GetComponent<Hero_Character_Class>().enoughMana(this.gameObject.GetComponent<Hero_Character_Class>().spellList[i].manaCost) == true) {
            this.gameObject.GetComponent<Hero_Character_Class>().selectedSpell = this.gameObject.GetComponent<Hero_Character_Class>().spellList[i];
            beginTargetingSpell(this.gameObject.GetComponent<Hero_Character_Class>().spellList[i].range, this.gameObject.GetComponent<Hero_Character_Class>().spellList[i]);
            displaySpellList(false);
            displayAttackMenu(false);
        }
    }

    public void abilityButtonUI(int i){
        if (this.gameObject.GetComponent<SummonClass>().offCooldown(i)){
            UnityEngine.Debug.Log("Here");
            this.gameObject.GetComponent<SummonClass>().selectedAbility = this.gameObject.GetComponent<SummonClass>().abilityList[i];
            beginTargetingAbility(this.gameObject.GetComponent<SummonClass>().abilityList[i].range, this.gameObject.GetComponent<SummonClass>().abilityList[i]);
            displayAbilityList(false);
            displayAttackMenu(false);
        }
    }

    public void updateCharStats() {
        //Sets the values of the nameplate
        nameplate.displayName(name);
        nameplate.displayImage(char_img);
        nameplate.displayHealth(health, maxHealth);
        nameplate.displayAtk(attack);
        nameplate.displayDef(defense);
        nameplate.displayRes(resistence);
        nameplate.displayAcc(accuracy);
        nameplate.displayCrit(criticalChance);
        nameplate.displaySpd(speed);
                //Checks if the character is a Hero and has mana
        if (gameObject.GetComponent<Hero_Character_Class>() != null)
        {
            //If the character does have mana, it is also passed to the nameplate and the mana bar is set to active
            nameplate.displayMana(gameObject.GetComponent<Hero_Character_Class>().mana, gameObject.GetComponent<Hero_Character_Class>().maxMana);
            nameplate.mana.gameObject.SetActive(true);
            nameplate.displayMag(gameObject.GetComponent<Hero_Character_Class>().magic);
            nameplate.displayMagicArea(true);
        }
        else
        {
            //If the character doesn't have mana, the mana bar is just set to inactive
            nameplate.mana.gameObject.SetActive(false);
            nameplate.displayMagicArea(false);
        }
    }

    public void addBuff(BuffClass buff){
        for (int i = 0; i < buff.statsToEffect.Count; i++)
        {
            //Checks which stat to effect and then applies the change
            switch (buff.statsToEffect[i])
            {
                case "health":
                    increaseHealth(buff.amountsToEffect[i]);
                    break;

                case "attack":
                    increaseAttack(buff.amountsToEffect[i]);
                    break;

                case "speed":
                    increaseSpeed(buff.amountsToEffect[i]);
                    break;

                case "maxHealth":
                    increaseMaxHealth(buff.amountsToEffect[i]);
                    break;

                case "movementSpeed":
                    increaseMoveSpeed(buff.amountsToEffect[i]);
                    break;

                case "resistence":
                    increaseResistence(buff.amountsToEffect[i]);
                    break;

                case "defense":
                    increaseDefense(buff.amountsToEffect[i]);
                    break;

                case "criticalChance":
                    increaseCritChance(buff.amountsToEffect[i]);
                    break;

                case "accuracy":
                    increaseAccuracy(buff.amountsToEffect[i]);
                    break;

                case "totalActions":
                    increaseTotalActions(buff.amountsToEffect[i]);
                    break;

                case "magic":
                    if (this.gameObject.GetComponent<Hero_Character_Class>()){
                        this.gameObject.GetComponent<Hero_Character_Class>().increaseMagic(buff.amountsToEffect[i]);
                    }
                    break;

            }
        }
        buffs.Add(buff);
    }

    public void removeBuff(BuffClass buff){
        for (int i = 0; i < buff.statsToEffect.Count; ++i)
        {
            //Checks which stat to effect and then applies the change
            switch (buff.statsToEffect[i])
            {
                case "health":
                    decreaseHealth(buff.amountsToEffect[i]);
                    break;

                case "attack":
                    decreaseAttack(buff.amountsToEffect[i]);
                    break;

                case "speed":
                    decreaseSpeed(buff.amountsToEffect[i]);
                    break;

                case "maxHealth":
                    decreaseMaxHealth(buff.amountsToEffect[i]);
                    break;

                case "movementSpeed":
                    decreaseMoveSpeed(buff.amountsToEffect[i]);
                    break;

                case "resistence":
                    decreaseResistence(buff.amountsToEffect[i]);
                    break;

                case "defense":
                    decreaseDefense(buff.amountsToEffect[i]);
                    break;

                case "criticalChance":
                    decreaseCritChance(buff.amountsToEffect[i]);
                    break;

                case "accuracy":
                    decreaseAccuracy(buff.amountsToEffect[i]);
                    break;

                case "totalActions":
                    decreaseTotalActions(buff.amountsToEffect[i]);
                    break;
                
                case "magic":
                    if (this.gameObject.GetComponent<Hero_Character_Class>()){
                        this.gameObject.GetComponent<Hero_Character_Class>().decreaseMagic(buff.amountsToEffect[i]);
                    }
                    break;

            }
        }
        buffs.Remove(buff);
    }

    public void deselectOnResume() 
    {
        if (charSelected == false && (map.selectedUnit == null || this.gameObject.tag != map.selectedUnit.tag))
        {
            //Checks if the unit's turn has been ended
            if (turnEnded == false)
            {
                //If the unit's turn isn't over and isn't selected, reset the color to it's base
                renderer.material.color = color;
                //If the character is not selected, and the turn is not over turn off nameplate
                displayNameplate(false);
            }
            else
            {
                //Otherwise set the color to gray
                renderer.material.color = Color.gray;
                displayNameplate(false);

            }
        }
        if (this.gameObject.GetComponent<Enemy_Character_Class>() != null && map.movingEnemy == true){
            renderer.material.color = color;
            displayNameplate(false);
            //updateCharStats();
        }
        //Set the hover variable to false
        charHover = false;
        //UnityEngine.Debug.Log("Mouse Exited");

        tile.OnMouseExit();
        pm.setDeslection(false);
    }


}

//Basic stat class
//Two integers
// -value --- Base value
// -moddedValue -- Value that is changed as the unit is buffed and debuffed
[System.Serializable]
public class stat
{
    
    public int value;
    public int moddedValue;

}

