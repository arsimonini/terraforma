using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainSystem : MonoBehaviour
{
    public int phase = 0;
    public Sprite textBox;
    public Sprite bg;
    public Sprite CharacterSprite;
    public Sprite arrow;
    public Canvas canvas;
    public List<Sprite> Wold;
    public List<Sprite> Lancin;
    public List<Sprite> Enemy;
    public GameObject leftImage;
    public GameObject rightImage;
    public GameObject TextBox;
    public GameObject characterScroll;
    public TMP_FontAsset font;
    public GameObject fade;
    private Image fadeImage;
    public bool fading = false;
    public bool fadingOut = false;
    public float fadeVal = 1f;
    public List<DialogueAsset> assets = new List<DialogueAsset>();
    public string sceneToSendTo = "Level1";
    public GameObject arrowObj;
    public bool paused = false;


    // Start is called before the first frame update
    void Start()
    {
        fadeImage = fade.GetComponent<Image>();
        canvas = gameObject.transform.GetChild(0).GetComponent<Canvas>();
        StartCoroutine(dialogue());
    }

    // Update is called once per frame
    void Update()
    {
        if (fading && fadeVal >= 0.0f){
            Color newColor = fadeImage.color;
            newColor.a = fadeVal;
            fadeVal -= 0.01f;
            fadeImage.color = newColor; 
        }
        else{
            fading = false;
        }
        if (fadingOut && fadeVal <= 1.0f){
            Color newColor = fadeImage.color;
            newColor.a = fadeVal;
            fadeVal += 0.02f;
            fadeImage.color = newColor; 
        }
        else {
            fadingOut = false;
        }
        if (Input.GetMouseButtonDown(0) && fading == false && fadingOut == false && paused == false){
            phase++;
        }
        fade.transform.SetAsLastSibling();
    }

    IEnumerator dialogue(){
        spawnBackground();
        fadeIn();
        StartCoroutine(newText(assets[0].character, assets[0].dir, assets[0].message, assets[0].expression));        
        if(assets != null && assets.Count > 0){
            for (int i = 1; i < assets.Count; i++){
                yield return new WaitUntil(() => phase == i);
                if (assets[i].character == "Fade"){
                    StartCoroutine(fadeOutAndIn());
                }
                else if (assets[i].character == "Noise"){
                    spawnNoise(assets[i].message);
                }
                else{
                    StartCoroutine(newText(assets[i].character, assets[i].dir, assets[i].message, assets[i].expression));
                }
            }
        }
        yield return new WaitUntil(() => phase == assets.Count);
        fadeOut();
        yield return new WaitForSecondsRealtime(2.0f);
        SceneManager.LoadScene(sceneToSendTo);
    }

    private void spawnBackground(){
        canvas.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = bg;
    }

    IEnumerator fadeOutAndIn(){
        fadeOut();
        paused = true;
        yield return new WaitForSecondsRealtime(2.0f);
        paused = false;
        fadeIn();
        phase++;
    }

    private void spawnNoise(string message){
        if (TextBox != null){
            Destroy(TextBox);
        }
        GameObject text = new GameObject("Textbox");
        text.transform.SetParent(canvas.transform, false);
        Image img = text.AddComponent<Image>();
        img.sprite = textBox;
        text.GetComponent<RectTransform>().localScale = new Vector3(6f, 1.25f, 1f);
        text.GetComponent<RectTransform>().localPosition = new Vector3(-2.5f, -120f, -610f);
        TextBox = text;
        GameObject newText = new GameObject("Message");
        newText.transform.SetParent(text.transform, false);
        TextMeshProUGUI txt = newText.AddComponent<TextMeshProUGUI>();
        Vector3 inverseScale = new Vector3(0.3f, 0.9f, 0.3f);
        newText.transform.localScale = inverseScale;
        newText.transform.localPosition = new Vector3(0f, 0f, 0f);
        txt.text = message;
        txt.font = font;
        txt.fontSize = 10;
        txt.color = Color.black;
        txt.alignment = TextAlignmentOptions.Center;

        if (arrowObj != null){
            Destroy(arrowObj);
        }

        GameObject nextArrow = new GameObject("nextArrow");
        nextArrow.transform.SetParent(canvas.transform, false);
        Image arrowImg = nextArrow.AddComponent<Image>();
        arrowImg.sprite = arrow;
        arrowImg.GetComponent<RectTransform>().localScale = new Vector3(0.53f, 0.1848f, 1.235f);
        arrowImg.GetComponent<RectTransform>().localPosition = new Vector3(208f, -155f, -610f);
        arrowImg.color = new Color(34f / 255f, 28f / 255f, 28f / 255f);
        arrowObj = nextArrow;
    }

    IEnumerator newText(string character, string dir, string text, string mood){
        switch(character){
            case "Wold":
                spawnImage(Wold[getImgNum(character, mood)], dir, character);
                spawnText(character, text, dir);
                break;     
            case "Lancin":
                spawnImage(Lancin[getImgNum(character, mood)], dir, character);
                spawnText(character, text, dir);
                break;
            case "Enemy":
                spawnImage(Enemy[getImgNum(character, mood)], dir, character);
                spawnText(character, text, dir);
                break;
        }
        yield break;
    }

    public int getImgNum(string character, string mood){
        if (character != "Enemy"){
            switch (mood){
                case "Neutral":
                    return 0;
                case "Angry":
                    return 1;
                case "Happy":
                    return 2;
                case "Surprised":
                    return 3;
                default:
                    return 0;
            }
        }
        else {
            switch (mood){
                case "Neutral":
                    return 0;
                case "Angry":
                    return 1;
                case "Confused":
                    return 2;
                case "Surprised":
                    return 3;
                default:
                    return 0;
            }
        }
        return 0;
    }

    public void spawnImage(Sprite sprite, string dir, string character){
        GameObject image = new GameObject("Image"+dir);
        image.transform.SetParent(canvas.transform, false);
        Image img = image.AddComponent<Image>();
        if (dir == "Left"){
            if (leftImage != null){
                Destroy(leftImage);
            }
            if (character == "Enemy"){
                image.GetComponent<RectTransform>().localScale = new Vector3(-7, 7, 1);
            }
            else {
                image.GetComponent<RectTransform>().localScale = new Vector3(7, 7, 1);
            }
            image.GetComponent<RectTransform>().localPosition = new Vector3(-525, 100, 0);
            leftImage = image;
            greyImage("Right");
        }
        else{
            if (rightImage != null){
                Destroy(rightImage);
            }
            if (character == "Enemy"){
                image.GetComponent<RectTransform>().localScale = new Vector3(7, 7, 1);
            }
            else {
                image.GetComponent<RectTransform>().localScale = new Vector3(-7, 7, 1);
            }
            image.GetComponent<RectTransform>().localPosition = new Vector3(525, 100, 0);
            rightImage = image;
            greyImage("Left");
        }
        img.sprite = sprite;
    }

    public void spawnText(string character, string message, string dir){
        if (TextBox != null){
            Destroy(TextBox);
        }
        GameObject text = new GameObject("Textbox");
        text.transform.SetParent(canvas.transform, false);
        Image img = text.AddComponent<Image>();
        img.sprite = textBox;
        text.GetComponent<RectTransform>().localScale = new Vector3(6f, 1.25f, 1f);
        text.GetComponent<RectTransform>().localPosition = new Vector3(-2.5f, -120f, -610f);
        TextBox = text;
        characterName(character, dir);
        GameObject newText = new GameObject("Message");
        newText.transform.SetParent(text.transform, false);
        TextMeshProUGUI txt = newText.AddComponent<TextMeshProUGUI>();
        Vector3 inverseScale = new Vector3(0.3f, 0.9f, 0.3f);
        newText.transform.localScale = inverseScale;
        newText.transform.localPosition = new Vector3(0f, 0f, 0f);
        txt.text = message;
        txt.font = font;
        txt.fontSize = 10;
        txt.color = Color.black;
        txt.alignment = TextAlignmentOptions.Center;

        if (arrowObj != null){
            Destroy(arrowObj);
        }

        GameObject nextArrow = new GameObject("nextArrow");
        nextArrow.transform.SetParent(canvas.transform, false);
        Image arrowImg = nextArrow.AddComponent<Image>();
        arrowImg.sprite = arrow;
        arrowImg.GetComponent<RectTransform>().localScale = new Vector3(0.53f, 0.1848f, 1.235f);
        arrowImg.GetComponent<RectTransform>().localPosition = new Vector3(208f, -155f, -610f);
        arrowImg.color = new Color(34f / 255f, 28f / 255f, 28f / 255f);
        arrowObj = nextArrow;
    }

    public void characterName(string character, string dir){
        if (characterScroll != null){
            Destroy(characterScroll);
        }
        GameObject characterName = new GameObject(character+"Scroll");
        characterName.transform.SetParent(canvas.transform, false);
        Image img = characterName.AddComponent<Image>();
        img.sprite = CharacterSprite;
        if (dir == "Left"){
            characterName.GetComponent<RectTransform>().localScale = new Vector3(2f, 0.5f, 1f);
            characterName.GetComponent<RectTransform>().localPosition = new Vector3(-130f, -74f, -610f);
        }
        else{
            characterName.GetComponent<RectTransform>().localScale = new Vector3(2f, 0.5f, 1f);
            characterName.GetComponent<RectTransform>().localPosition = new Vector3(125f, -74f, -610f);
        }
        characterScroll = characterName;
        GameObject name = new GameObject(character+"Name");
        name.transform.SetParent(characterName.transform, false);
        TextMeshProUGUI txt = name.AddComponent<TextMeshProUGUI>();
        Vector3 inverseScale = new Vector3(0.56f, 2.24f, 1.12f);
        name.transform.localScale = inverseScale;
        name.transform.localPosition = new Vector3(35f, -13f, 0f);
        txt.text = character;
        txt.font = font;
        txt.fontSize = 24;
        txt.color = Color.black;
    }


    public void greyImage(string side){
        if (side == "Left"){
            if (leftImage != null){
                leftImage.GetComponent<Image>().color = new Color(105f / 255f, 80f / 255f, 80f / 255f);
            }
        }
        else {
            if (rightImage != null){
                rightImage.GetComponent<Image>().color = new Color(105f / 255f, 80f / 255f, 80f / 255f);
            }
        }
    }

    public void fadeIn(){
        fadeVal = 1.0f;
        Color newColor = fadeImage.color;
        newColor.a = fadeVal;
        fadeImage.color = newColor;
        fading = true;
    }

    public void fadeOut(){
        if (TextBox != null){
            Destroy(TextBox);
        }
        if (leftImage != null){
            Destroy(leftImage);
        }
        if (rightImage != null){
            Destroy(rightImage);
        }
        if (characterScroll != null){
            Destroy(characterScroll);
        }
        if (arrowObj != null){
            Destroy(arrowObj);
        }
        fadeVal = 0.0f;
        Color newColor = fadeImage.color;
        newColor.a = fadeVal;
        fadeImage.color = newColor;
        fadingOut = true;
    }
}
