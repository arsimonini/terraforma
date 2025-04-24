using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBallScript : BuffActions
{
    public override void endOfDurationEffect(Basic_Character_Class character){
        UnityEngine.Debug.Log("Solar Ball Has Ended, attacking the map");
        TileMap map = character.map;

        Lancin l = character.gameObject.GetComponent<Lancin>();

        if (map != null && l != null) {
            int dmg = 32;

            placeTileEffect(map.getTile(l.SolarBallX+0,l.SolarBallY+0),dmg);

            placeTileEffect(map.getTile(l.SolarBallX-1,l.SolarBallY+0),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+1,l.SolarBallY+0),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+0,l.SolarBallY+1),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+0,l.SolarBallY-1),dmg);

            placeTileEffect(map.getTile(l.SolarBallX-2,l.SolarBallY+0),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+2,l.SolarBallY+0),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+0,l.SolarBallY+2),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+0,l.SolarBallY-2),dmg);

            placeTileEffect(map.getTile(l.SolarBallX-1,l.SolarBallY-1),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+1,l.SolarBallY-1),dmg);
            placeTileEffect(map.getTile(l.SolarBallX-1,l.SolarBallY+1),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+1,l.SolarBallY+1),dmg);

            placeTileEffect(map.getTile(l.SolarBallX-2,l.SolarBallY-1),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+2,l.SolarBallY-1),dmg);
            placeTileEffect(map.getTile(l.SolarBallX-2,l.SolarBallY+1),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+2,l.SolarBallY+1),dmg);

            placeTileEffect(map.getTile(l.SolarBallX-1,l.SolarBallY-2),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+1,l.SolarBallY-2),dmg);
            placeTileEffect(map.getTile(l.SolarBallX-1,l.SolarBallY+2),dmg);
            placeTileEffect(map.getTile(l.SolarBallX+1,l.SolarBallY+2),dmg);
        }   
    }

    public bool placeTileEffect (ClickableTile ct, int damage = 8, string element = "Fire", string spellName = "Solar Ball", bool heroTeam = true) {
        if (ct == null) {
            return false;
        }
        TileMap map = ct.map; if (map == null) { return false;}
        ReactionController rc = map.GetComponent<ReactionController>(); if (rc == null) { return false;}
        rc.checkReaction(ct,element,spellName, heroTeam);

        if (ct.characterOnTile != null) {
                //UnityEngine.Debug.Log("Enemy Here!");
                Basic_Character_Class characterGettingHit = ct.characterOnTile.GetComponent<Basic_Character_Class>();
                Enemy_Character_Class enemyGettingHit = ct.characterOnTile.GetComponent<Enemy_Character_Class>();
                if (enemyGettingHit != null) {
                    characterGettingHit.takeMagicDamage(damage,"Fire");
                }
        }

        return true;
    }
}
