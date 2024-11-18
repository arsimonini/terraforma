using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    float xAngle = 58;
    public Vector3 startPos;
    public float tooFarHorizontal = 10;
    public float tooFarVertical = 10;

    public float tooNearHorizontal = -5;

    public float tooNearVertical = -5;

    public GameControllerScript gc;
    public TileMap map;

    float speed = 4f;
    float setDirection = 0;
    public Vector3 target;
    
    int rotationCount = 0; //Counts down the limit
    int rotationDirection = 0;

    int targetCounter = 0;
    public int targetIndex = -1;

    public List<GameObject> lUnits;

    public PauseMenu pm;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        lUnits = new List<GameObject>(gc.playerTeamList);
        xAngle = transform.eulerAngles.x;
        //Debug.Log(xAngle);
    }

    // Update is called once per frame
    void Update()
    {
        //Unit is actively moving
        if (map.currentPath != null) {
            //transform.position = new Vector3(map.selectedUnit.transform.position.x, 7, map.selectedUnit.transform.position.z - 2.3f);
            //Vector3 forward = transform.forward;
            //Vector3 right = transform.right;
            checkRotation();
            if (map.selectedUnit != null){
                target = new Vector3(map.selectedUnit.transform.position.x,transform.position.y,map.selectedUnit.transform.position.z);
            
                //if ((target.x != map.selectedUnit.x) && ())

                //float xChar = map.selectedUnit.transform.position.x;
                //float zChar = map.selectedUnit.transform.position.z;
                //easeToLocation(xChar,zChar);
                target += -3*getForward();
                easeToLocation(target.x,target.z);
            }


        } else if (map.selectedUnit != null) { //Unit is selected but isn't moving, restrict camera so that they are always within shot.
            checkRotation();
            float hInput = 0f;
            float vInput = 0f;
            //float hInput = Input.GetAxis("Horizontal");
            //float vInput = Input.GetAxis("Vertical");
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)){
                hInput = 0.75f;
            }
            else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
                hInput = -0.75f;
            }
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)){
                vInput = 0.75f;
            }
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)){
                vInput = -0.75f;
            }
            GameObject unit = map.selectedUnit;
            Vector3 unitPos = unit.transform.position;

            Vector3 fw = 4*getForward();

            Vector3 newMove = speed*vInput*getForward()*Time.deltaTime + speed*hInput*getRight()*Time.deltaTime;
            Vector3 newPos = transform.position + newMove;

            if ((newPos.x > -5) && (newPos.x < tooFarHorizontal) && (newPos.z > -5) && (newPos.z < tooFarVertical)) {
                transform.position = newPos;
            }
            //Set restrictions
            /*float northWall = unitPos.z+10;
            float southWall = unitPos.z-10;
            float eastWall = unitPos.x+10;
            float westWall = unitPos.x-10;

            //Set movement to 0 if too far in wrong direction and also teleport them back to that position
            if (transform.position.x <= westWall && hInput < 0) {
                hInput = 0;
                transform.position = new Vector3(westWall,transform.position.y,transform.position.z);
            } else if (transform.position.x >= eastWall && hInput > 0) {
                hInput = 0;
                transform.position = new Vector3(eastWall,transform.position.y,transform.position.z);
            }

            if (transform.position.z <= southWall && vInput < 0) {
                vInput = 0;
                transform.position = new Vector3(transform.position.x,transform.position.y,southWall);
            } else if (transform.position.z >= northWall && vInput > 0) {
                vInput = 0;
                transform.position = new Vector3(transform.position.x,transform.position.y,northWall);
            }

            transform.position = transform.position + new Vector3(speed * hInput * Time.deltaTime, 0, speed * vInput * Time.deltaTime);    */
        } else {
            checkRotation();
            checkSwitchTarget();
            float hInput = 0f;
            float vInput = 0f;
            //float hInput = Input.GetAxis("Horizontal");
            //float vInput = Input.GetAxis("Vertical");
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)){
                hInput = 0.75f;
            }
            else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
                hInput = -0.75f;
            }
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)){
                vInput = 0.75f;
            }
            else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)){
                vInput = -0.75f;
                UnityEngine.Debug.Log(vInput);
            }
            Vector3 fw = 4*getForward();

            /*if ((transform.position.x) <= 0 && hInput < 0) {
                hInput = 0;
                transform.position = new Vector3(0,transform.position.y,transform.position.z);
            } else if ((transform.position.x) >= tooFarHorizontal && hInput > 0) {
                hInput = 0;
                transform.position = new Vector3(tooFarHorizontal,transform.position.y,transform.position.z);
            }

            if ((transform.position.z) <= -5 && vInput < 0) {
                vInput = 0;
                transform.position = new Vector3(transform.position.x,transform.position.y,-5);
            } else if ((transform.position.z) >= tooFarVertical && vInput > 0) {
                vInput = 0;
                transform.position = new Vector3(transform.position.x,transform.position.y,tooFarVertical);
            }*/

            //If outside bounds, move to position.
            Vector3 newMove = speed*vInput*getForward()*Time.deltaTime + speed*hInput*getRight()*Time.deltaTime;
            Vector3 newPos = transform.position + newMove;

            if ((newPos.x > tooNearHorizontal) && (newPos.x < tooFarHorizontal) && (newPos.z > tooNearVertical) && (newPos.z < tooFarVertical)) {
                transform.position = newPos;
            }
        }
            
        
    }

    void easeToLocation(float x,float z) {

        if(pm.GameIsPaused != true) {
            float inc = 16;
            //The higher the number, the more gradual the travel speed.

            float xOld = transform.position.x;
            float zOld = transform.position.z;

            float xAvg = (xOld*(inc-1) + x) / inc;
            float zAvg = (zOld*(inc-1) + z) / inc;

            transform.position = new Vector3(xAvg,transform.position.y,zAvg);
        }
    }

    void checkRotation() {
        if(pm.GameIsPaused != true) {
            if (rotationCount <= 0) {
                //Check rotation keys
                bool tL = Input.GetKeyDown(KeyCode.Q); 
                bool tR = Input.GetKeyDown(KeyCode.E);
                int rot = 0;
                
                //If E, set rotate right
                if (tL) {
                    rot = -45;
                    rotationDirection = -5;
                    //Debug.Log("Turn Left");
                    rotationCount = 9;
                }
                //If Q, set rotate left
                if (tR) {
                    rot = 45;
                    rotationDirection = 5;
                    //Debug.Log("Turn Right");
                    rotationCount = 9;
                }



            } else {
                rotationCount --;

                //Increment towards that
                transform.Rotate(-xAngle,0,0);
                transform.Rotate(0,rotationDirection,0);
                transform.Rotate(xAngle,0,0); 

                if (rotationCount == 0) {
                    rotationDirection = 0;

                }
                
            }
        }

        
        //Transform.Rotate(transform.rotation.x,transform.rotation.y+rot,transform.rotation.z, Space.World);
    }

    void checkSwitchTarget () {
        if(pm.GameIsPaused != true) {
            lUnits = new List<GameObject>(gc.playerTeamList);
            lUnits.AddRange(gc.enemyTeamList);

            bool inputTab = Input.GetKeyDown(KeyCode.Tab); 

            if (inputTab) {
                targetIndex ++;

                if ((targetIndex < 0) || (targetIndex >= lUnits.Count)) {
                    targetIndex = 0;
                } 

                targetCounter = 60;

            } else if (targetCounter > 0) {
                GameObject targetObject = lUnits[targetIndex];
                target = targetObject.transform.position - 3*getForward();
                easeToLocation(target.x,target.z);

                targetCounter --;
            } else {
                
            }
        }
    }

    float distance(float x1, float z1, float x2, float z2) {
        return Mathf.Sqrt(Mathf.Pow((x1-x2),2) + Mathf.Pow((z1-z2),2));
    }

    Vector3 getForward() {
        Vector3 forward = transform.forward;

        forward.y = 0;

        forward.Normalize();
        
        return forward;
    }

    Vector3 getRight() {
        Vector3 right = transform.right;

        right.y = 0;

        right.Normalize();
        
        return right;
    }
}