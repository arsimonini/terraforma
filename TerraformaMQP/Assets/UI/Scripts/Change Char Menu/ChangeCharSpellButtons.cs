using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharSpellButtons : MonoBehaviour
{

    public string buttonClicked;
    public TeamInfo ti;
    public GameObject go;
    public GameObject thisGO;

    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spellButtonClicked(string s) {
        buttonClicked = s;
        swapSpell(go.GetComponent<ChangeCharMenu>().currChanging, buttonClicked);
        
        
        Destroy(thisGO);
    }

    public void swapSpellSprite(int index, string spell) {
        for(int i = 0; i < go.GetComponent<ChangeCharMenu>().spellSpriteKeys.Length; i++) {
            if(spell == go.GetComponent<ChangeCharMenu>().spellSpriteKeys[i]) {
                //go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
                go.GetComponent<ChangeCharMenu>().spellButtons[index].GetComponent<Image>().sprite = go.GetComponent<ChangeCharMenu>().spellSprites[i];
            }
        }

    }



    public void swapSpell(string place, string what) {
        if(what == "Locked") {
            return;
        }
        switch (place) {
            case "Hero1_Spell1":
            spellCheck(1, 1, what);
            ti.spell1_1 = what;
            swapSpellSprite(0, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero1_Spell2":
            spellCheck(2, 1, what);
            ti.spell1_2 = what;
            swapSpellSprite(1, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero1_Spell3":
            spellCheck(3, 1, what);
            ti.spell1_3 = what;
            swapSpellSprite(2, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero1_Spell4":
            spellCheck(4, 1, what);
            ti.spell1_4 = what;
            swapSpellSprite(3, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero1_Spell5":
            spellCheck(5, 1, what);
            ti.spell1_5 = what;
            swapSpellSprite(4, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero1_Spell6":
            spellCheck(6, 1, what);
            ti.spell1_6 = what;
            swapSpellSprite(5, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;
            
            case "Hero2_Spell1":
            spellCheck(1, 2, what);
            ti.spell2_1 = what;
            swapSpellSprite(6, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero2_Spell2":
            spellCheck(2, 2, what);
            ti.spell2_2 = what;
            swapSpellSprite(7, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero2_Spell3":
            spellCheck(3, 2, what);
            ti.spell2_3 = what;
            swapSpellSprite(8, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero2_Spell4":
            spellCheck(4, 2, what);
            ti.spell2_4 = what;
            swapSpellSprite(9, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero2_Spell5":
            spellCheck(5, 2, what);
            ti.spell2_5 = what;
            swapSpellSprite(10, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero2_Spell6":
            spellCheck(6, 2, what);
            ti.spell2_6 = what;
            swapSpellSprite(11, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero3_Spell1":
            spellCheck(1, 3, what);
            ti.spell3_1 = what;
            swapSpellSprite(12, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero3_Spell2":
            spellCheck(2, 3, what);
            ti.spell3_2 = what;
            swapSpellSprite(13, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero3_Spell3":
            spellCheck(3, 3, what);
            ti.spell3_3 = what;
            swapSpellSprite(14, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero3_Spell4":
            spellCheck(4, 3, what);
            ti.spell3_4 = what;
            swapSpellSprite(15, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero3_Spell5":
            spellCheck(5, 3, what);
            ti.spell3_5 = what;
            swapSpellSprite(16, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;

            case "Hero3_Spell6":
            spellCheck(6, 3, what);
            ti.spell3_6 = what;
            swapSpellSprite(17, what);
            go.GetComponent<ChangeCharMenu>().setTeam();
            break;
        }
    }

    public void spellCheck(int num, int hero, string changingToSpell) {
        if(num == 1) {
            if(hero == 1) {
                if(ti.spell1_2 == changingToSpell) {
                    string tempSpell = ti.spell1_1;
                    ti.spell1_1 = ti.spell1_2;
                    ti.spell1_2 = tempSpell;
                }
                else if(ti.spell1_3 == changingToSpell) {
                    string tempSpell = ti.spell1_1;
                    ti.spell1_1 = ti.spell1_3;
                    ti.spell1_3 = tempSpell;
                }
                else if(ti.spell1_4 == changingToSpell) {
                    string tempSpell = ti.spell1_1;
                    ti.spell1_1 = ti.spell1_4;
                    ti.spell1_4 = tempSpell;
                }
                else if(ti.spell1_5 == changingToSpell) {
                    string tempSpell = ti.spell1_1;
                    ti.spell1_1 = ti.spell1_5;
                    ti.spell1_5 = tempSpell;
                }
                else if(ti.spell1_6 == changingToSpell) {
                    string tempSpell = ti.spell1_1;
                    ti.spell1_1 = ti.spell1_6;
                    ti.spell1_6 = tempSpell;
                }
            }
            else if(hero == 2) {
                if(ti.spell2_2 == changingToSpell) {
                    string tempSpell = ti.spell2_1;
                    ti.spell2_1 = ti.spell2_2;
                    ti.spell2_2 = tempSpell;
                }
                else if(ti.spell2_3 == changingToSpell) {
                    string tempSpell = ti.spell2_1;
                    ti.spell2_1 = ti.spell2_3;
                    ti.spell2_3 = tempSpell;
                }
                else if(ti.spell2_4 == changingToSpell) {
                    string tempSpell = ti.spell2_1;
                    ti.spell2_1 = ti.spell2_4;
                    ti.spell2_4 = tempSpell;
                }
                else if(ti.spell2_5 == changingToSpell) {
                    string tempSpell = ti.spell2_1;
                    ti.spell2_1 = ti.spell2_5;
                    ti.spell2_5 = tempSpell;
                }
                else if(ti.spell2_6 == changingToSpell) {
                    string tempSpell = ti.spell2_1;
                    ti.spell2_1 = ti.spell2_6;
                    ti.spell2_6 = tempSpell;
                }
            }
            else if(hero == 3) {
                if(ti.spell3_2 == changingToSpell) {
                    string tempSpell = ti.spell3_1;
                    ti.spell3_1 = ti.spell3_2;
                    ti.spell3_2 = tempSpell;
                }
                else if(ti.spell3_3 == changingToSpell) {
                    string tempSpell = ti.spell3_1;
                    ti.spell3_1 = ti.spell3_3;
                    ti.spell3_3 = tempSpell;
                }
                else if(ti.spell3_4 == changingToSpell) {
                    string tempSpell = ti.spell3_1;
                    ti.spell3_1 = ti.spell3_4;
                    ti.spell3_4 = tempSpell;
                }
                else if(ti.spell3_5 == changingToSpell) {
                    string tempSpell = ti.spell3_1;
                    ti.spell3_1 = ti.spell3_5;
                    ti.spell3_5 = tempSpell;
                }
                else if(ti.spell3_6 == changingToSpell) {
                    string tempSpell = ti.spell3_1;
                    ti.spell3_1 = ti.spell3_6;
                    ti.spell3_6 = tempSpell;
                }
            }
        } else if(num == 2) {
            if(hero == 1) {
                if(ti.spell1_1 == changingToSpell) {
                    string tempSpell = ti.spell1_2;
                    ti.spell1_2 = ti.spell1_1;
                    ti.spell1_1 = tempSpell;
                }
                else if(ti.spell1_3 == changingToSpell) {
                    string tempSpell = ti.spell1_2;
                    ti.spell1_2 = ti.spell1_3;
                    ti.spell1_3 = tempSpell;
                }
                else if(ti.spell1_4 == changingToSpell) {
                    string tempSpell = ti.spell1_2;
                    ti.spell1_2 = ti.spell1_4;
                    ti.spell1_4 = tempSpell;
                }
                else if(ti.spell1_5 == changingToSpell) {
                    string tempSpell = ti.spell1_2;
                    ti.spell1_2 = ti.spell1_5;
                    ti.spell1_5 = tempSpell;
                }
                else if(ti.spell1_6 == changingToSpell) {
                    string tempSpell = ti.spell1_2;
                    ti.spell1_2 = ti.spell1_6;
                    ti.spell1_6 = tempSpell;
                }
            }
            else if(hero == 2) {
                if(ti.spell2_1 == changingToSpell) {
                    string tempSpell = ti.spell2_2;
                    ti.spell2_2 = ti.spell2_1;
                    ti.spell2_1 = tempSpell;
                }
                else if(ti.spell2_3 == changingToSpell) {
                    string tempSpell = ti.spell2_2;
                    ti.spell2_2 = ti.spell2_3;
                    ti.spell2_3 = tempSpell;
                }
                else if(ti.spell2_4 == changingToSpell) {
                    string tempSpell = ti.spell2_2;
                    ti.spell2_2 = ti.spell2_4;
                    ti.spell2_4 = tempSpell;
                }
                else if(ti.spell2_5 == changingToSpell) {
                    string tempSpell = ti.spell2_2;
                    ti.spell2_2 = ti.spell2_5;
                    ti.spell2_5 = tempSpell;
                }
                else if(ti.spell2_6 == changingToSpell) {
                    string tempSpell = ti.spell2_2;
                    ti.spell2_2 = ti.spell2_6;
                    ti.spell2_6 = tempSpell;
                }
            }
            else if(hero == 3) {
                if(ti.spell3_1 == changingToSpell) {
                    string tempSpell = ti.spell3_2;
                    ti.spell3_2 = ti.spell3_1;
                    ti.spell3_1 = tempSpell;
                }
                else if(ti.spell3_3 == changingToSpell) {
                    string tempSpell = ti.spell3_2;
                    ti.spell3_2 = ti.spell3_3;
                    ti.spell3_3 = tempSpell;
                }
                else if(ti.spell3_4 == changingToSpell) {
                    string tempSpell = ti.spell3_2;
                    ti.spell3_2 = ti.spell3_4;
                    ti.spell3_4 = tempSpell;
                }
                else if(ti.spell3_5 == changingToSpell) {
                    string tempSpell = ti.spell3_2;
                    ti.spell3_2 = ti.spell3_5;
                    ti.spell3_5 = tempSpell;
                }
                else if(ti.spell3_6 == changingToSpell) {
                    string tempSpell = ti.spell3_2;
                    ti.spell3_2 = ti.spell3_6;
                    ti.spell3_6 = tempSpell;
                }
            }
        } else if(num == 3) {
            if(hero == 1) {
                if(ti.spell1_1 == changingToSpell) {
                    string tempSpell = ti.spell1_3;
                    ti.spell1_3 = ti.spell1_1;
                    ti.spell1_1 = tempSpell;
                } else if(ti.spell1_2 == changingToSpell) {
                    string tempSpell = ti.spell1_3;
                    ti.spell1_3 = ti.spell1_2;
                    ti.spell1_2 = tempSpell;
                } else if(ti.spell1_4 == changingToSpell) {
                    string tempSpell = ti.spell1_3;
                    ti.spell1_3 = ti.spell1_4;
                    ti.spell1_4 = tempSpell;
                } else if(ti.spell1_5 == changingToSpell) {
                    string tempSpell = ti.spell1_3;
                    ti.spell1_3 = ti.spell1_5;
                    ti.spell1_5 = tempSpell;
                } else if(ti.spell1_6 == changingToSpell) {
                    string tempSpell = ti.spell1_3;
                    ti.spell1_3 = ti.spell1_6;
                    ti.spell1_6 = tempSpell;
                }
            }
            else if(hero == 2) {
                if(ti.spell2_1 == changingToSpell) {
                        string tempSpell = ti.spell2_3;
                        ti.spell2_3 = ti.spell2_1;
                        ti.spell2_1 = tempSpell;
                }
                else if(ti.spell2_2 == changingToSpell) {
                    string tempSpell = ti.spell2_3;
                    ti.spell2_3 = ti.spell2_2;
                    ti.spell2_2 = tempSpell;
                }
                else if(ti.spell2_4 == changingToSpell) {
                    string tempSpell = ti.spell2_3;
                    ti.spell2_3 = ti.spell2_4;
                    ti.spell2_4 = tempSpell;
                }
                else if(ti.spell2_5 == changingToSpell) {
                    string tempSpell = ti.spell2_3;
                    ti.spell2_3 = ti.spell2_5;
                    ti.spell2_5 = tempSpell;
                }
                else if(ti.spell2_6 == changingToSpell) {
                    string tempSpell = ti.spell2_3;
                    ti.spell2_3 = ti.spell2_6;
                    ti.spell2_6 = tempSpell;
                }
            } else if(hero == 3) {
                if(ti.spell3_1 == changingToSpell) {
                    string tempSpell = ti.spell3_3;
                    ti.spell3_3 = ti.spell3_1;
                    ti.spell3_1 = tempSpell;
                }
                else if(ti.spell3_2 == changingToSpell) {
                    string tempSpell = ti.spell3_3;
                    ti.spell3_3 = ti.spell3_2;
                    ti.spell3_2 = tempSpell;
                }
                else if(ti.spell3_4 == changingToSpell) {
                    string tempSpell = ti.spell3_3;
                    ti.spell3_3 = ti.spell3_4;
                    ti.spell3_4 = tempSpell;
                    }
                    else if(ti.spell3_5 == changingToSpell) {
                        string tempSpell = ti.spell3_3;
                        ti.spell3_3 = ti.spell3_5;
                        ti.spell3_5 = tempSpell;
                    }
                    else if(ti.spell3_6 == changingToSpell) {
                        string tempSpell = ti.spell3_3;
                        ti.spell3_3 = ti.spell3_6;
                        ti.spell3_6 = tempSpell;
                    }
            }
        }else if(num == 4) {
            if(hero == 1) {
                if(ti.spell1_1 == changingToSpell) {
                    string tempSpell = ti.spell1_4;
                    ti.spell1_4 = ti.spell1_1;
                    ti.spell1_1 = tempSpell;
                }
                else if(ti.spell1_2 == changingToSpell) {
                    string tempSpell = ti.spell1_4;
                    ti.spell1_4 = ti.spell1_2;
                    ti.spell1_2 = tempSpell;
                }
                else if(ti.spell1_3 == changingToSpell) {
                    string tempSpell = ti.spell1_4;
                    ti.spell1_4 = ti.spell1_3;
                    ti.spell1_3 = tempSpell;
                }
                else if(ti.spell1_5 == changingToSpell) {
                    string tempSpell = ti.spell1_4;
                    ti.spell1_4 = ti.spell1_5;
                    ti.spell1_5 = tempSpell;
                }
                else if(ti.spell1_6 == changingToSpell) {
                    string tempSpell = ti.spell1_4;
                    ti.spell1_4 = ti.spell1_6;
                    ti.spell1_6 = tempSpell;
                }
            }else if(hero == 2) {
                if(ti.spell2_1 == changingToSpell) {
                    string tempSpell = ti.spell2_4;
                    ti.spell2_4 = ti.spell2_1;
                    ti.spell2_1 = tempSpell;
                }
                else if(ti.spell2_2 == changingToSpell) {
                    string tempSpell = ti.spell2_4;
                    ti.spell2_4 = ti.spell2_2;
                    ti.spell2_2 = tempSpell;
                }
                else if(ti.spell2_3 == changingToSpell) {
                    string tempSpell = ti.spell2_4;
                    ti.spell2_4 = ti.spell2_3;
                    ti.spell2_3 = tempSpell;
                }
                else if(ti.spell2_5 == changingToSpell) {
                    string tempSpell = ti.spell2_4;
                    ti.spell2_4 = ti.spell2_5;
                    ti.spell2_5 = tempSpell;
                }
                else if(ti.spell2_6 == changingToSpell) {
                    string tempSpell = ti.spell2_4;
                    ti.spell2_4 = ti.spell2_6;
                    ti.spell2_6 = tempSpell;
                }
            } else if(hero == 3) {
                if(ti.spell3_1 == changingToSpell) {
                    string tempSpell = ti.spell3_4;
                    ti.spell3_4 = ti.spell3_1;
                    ti.spell3_1 = tempSpell;
                }
                else if(ti.spell3_2 == changingToSpell) {
                    string tempSpell = ti.spell3_4;
                    ti.spell3_4 = ti.spell3_2;
                    ti.spell3_2 = tempSpell;
                }
                else if(ti.spell3_3 == changingToSpell) {
                    string tempSpell = ti.spell3_4;
                    ti.spell3_4 = ti.spell3_3;
                    ti.spell3_3 = tempSpell;
                }
                else if(ti.spell3_5 == changingToSpell) {
                    string tempSpell = ti.spell3_4;
                    ti.spell3_4 = ti.spell3_5;
                    ti.spell3_5 = tempSpell;
                }
                else if(ti.spell3_6 == changingToSpell) {
                    string tempSpell = ti.spell3_4;
                    ti.spell3_4 = ti.spell3_6;
                    ti.spell3_6 = tempSpell;
                }
            }
        }else if(num == 5) {
            if(hero == 1) {
                if(ti.spell1_1 == changingToSpell) {
                    string tempSpell = ti.spell1_5;
                    ti.spell1_5 = ti.spell1_1;
                    ti.spell1_1 = tempSpell;
                }
                else if(ti.spell1_2 == changingToSpell) {
                    string tempSpell = ti.spell1_5;
                    ti.spell1_5 = ti.spell1_2;
                    ti.spell1_2 = tempSpell;
                }
                else if(ti.spell1_3 == changingToSpell) {
                    string tempSpell = ti.spell1_5;
                    ti.spell1_5 = ti.spell1_3;
                    ti.spell1_3 = tempSpell;
                }
                else if(ti.spell1_4 == changingToSpell) {
                    string tempSpell = ti.spell1_5;
                    ti.spell1_5 = ti.spell1_4;
                    ti.spell1_4 = tempSpell;
                }
                else if(ti.spell1_6 == changingToSpell) {
                    string tempSpell = ti.spell1_5;
                    ti.spell1_5 = ti.spell1_6;
                    ti.spell1_6 = tempSpell;
                }
            } else if(hero == 2) {
                if(ti.spell2_1 == changingToSpell) {
                    string tempSpell = ti.spell2_5;
                    ti.spell2_5 = ti.spell2_1;
                    ti.spell2_1 = tempSpell;
                }
                else if(ti.spell2_2 == changingToSpell) {
                    string tempSpell = ti.spell2_5;
                    ti.spell2_5 = ti.spell2_2;
                    ti.spell2_2 = tempSpell;
                }
                else if(ti.spell2_3 == changingToSpell) {
                    string tempSpell = ti.spell2_5;
                    ti.spell2_5 = ti.spell2_3;
                    ti.spell2_3 = tempSpell;
                }
                else if(ti.spell2_4 == changingToSpell) {
                    string tempSpell = ti.spell2_5;
                    ti.spell2_5 = ti.spell2_4;
                    ti.spell2_4 = tempSpell;
                }
                else if(ti.spell2_6 == changingToSpell) {
                    string tempSpell = ti.spell2_5;
                    ti.spell2_5 = ti.spell2_6;
                    ti.spell2_6 = tempSpell;
                }
            } else if(hero == 3) {
                if(ti.spell3_1 == changingToSpell) {
                    string tempSpell = ti.spell3_5;
                    ti.spell3_5 = ti.spell3_1;
                    ti.spell3_1 = tempSpell;
                }
                else if(ti.spell3_2 == changingToSpell) {
                    string tempSpell = ti.spell3_5;
                    ti.spell3_5 = ti.spell3_2;
                    ti.spell3_2 = tempSpell;
                }
                else if(ti.spell3_3 == changingToSpell) {
                    string tempSpell = ti.spell3_5;
                    ti.spell3_5 = ti.spell3_3;
                    ti.spell3_3 = tempSpell;
                }
                else if(ti.spell3_4 == changingToSpell) {
                    string tempSpell = ti.spell3_5;
                    ti.spell3_5 = ti.spell3_4;
                    ti.spell3_4 = tempSpell;
                }
                else if(ti.spell3_6 == changingToSpell) {
                    string tempSpell = ti.spell3_5;
                    ti.spell3_5 = ti.spell3_6;
                    ti.spell3_6 = tempSpell;
                }
            }
        } else if(num == 6) {
            if(hero == 1) {
                if(ti.spell1_1 == changingToSpell) {
                    string tempSpell = ti.spell1_6;
                    ti.spell1_6 = ti.spell1_1;
                    ti.spell1_1 = tempSpell;
                }
                else if(ti.spell1_2 == changingToSpell) {
                    string tempSpell = ti.spell1_6;
                    ti.spell1_6 = ti.spell1_2;
                    ti.spell1_2 = tempSpell;
                }
                else if(ti.spell1_3 == changingToSpell) {
                    string tempSpell = ti.spell1_6;
                    ti.spell1_6 = ti.spell1_3;
                    ti.spell1_3 = tempSpell;
                }
                else if(ti.spell1_4 == changingToSpell) {
                    string tempSpell = ti.spell1_6;
                    ti.spell1_6 = ti.spell1_4;
                    ti.spell1_4 = tempSpell;
                }
                else if(ti.spell1_5 == changingToSpell) {
                    string tempSpell = ti.spell1_6;
                    ti.spell1_6 = ti.spell1_5;
                    ti.spell1_5 = tempSpell;
                }
            } else if(hero == 2) {
                if(ti.spell2_1 == changingToSpell) {
                    string tempSpell = ti.spell2_6;
                    ti.spell2_6 = ti.spell2_1;
                    ti.spell2_1 = tempSpell;
                }
                else if(ti.spell2_2 == changingToSpell) {
                    string tempSpell = ti.spell2_6;
                    ti.spell2_6 = ti.spell2_2;
                    ti.spell2_2 = tempSpell;
                }
                else if(ti.spell2_3 == changingToSpell) {
                    string tempSpell = ti.spell2_6;
                    ti.spell2_6 = ti.spell2_3;
                    ti.spell2_3 = tempSpell;
                }
                else if(ti.spell2_4 == changingToSpell) {
                    string tempSpell = ti.spell2_6;
                    ti.spell2_6 = ti.spell2_4;
                    ti.spell2_4 = tempSpell;
                }
                else if(ti.spell2_5 == changingToSpell) {
                    string tempSpell = ti.spell2_6;
                    ti.spell2_6 = ti.spell2_5;
                    ti.spell2_5 = tempSpell;
                }
            } else if(hero == 3) {
                if(ti.spell3_1 == changingToSpell) {
                    string tempSpell = ti.spell3_6;
                    ti.spell3_6 = ti.spell3_1;
                    ti.spell3_1 = tempSpell;
                }
                else if(ti.spell3_2 == changingToSpell) {
                    string tempSpell = ti.spell3_6;
                    ti.spell3_6 = ti.spell3_2;
                    ti.spell3_2 = tempSpell;
                }
                else if(ti.spell3_3 == changingToSpell) {
                    string tempSpell = ti.spell3_6;
                    ti.spell3_6 = ti.spell3_3;
                    ti.spell3_3 = tempSpell;
                }
                else if(ti.spell3_4 == changingToSpell) {
                    string tempSpell = ti.spell3_6;
                    ti.spell3_6 = ti.spell3_4;
                    ti.spell3_4 = tempSpell;
                }
                else if(ti.spell3_5 == changingToSpell) {
                    string tempSpell = ti.spell3_6;
                    ti.spell3_6 = ti.spell3_5;
                    ti.spell3_5 = tempSpell;
                }
            }
        }
    }

}
