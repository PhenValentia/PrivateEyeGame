using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointAndClickManager : MonoBehaviour // Entirely Writen by Phen Valentia (Nicholas Salter)
{
    // UI Elements
    [SerializeField]
    GameObject background;
    [SerializeField]
    GameObject inventoryBox;
    [SerializeField]
    GameObject textButton;
    [SerializeField]
    GameObject flashScreen;
    [SerializeField]
    GameObject blockerImage;
    [SerializeField]
    Canvas mainCanvas;
    [SerializeField]
    GameObject bedroomDoor;
    [SerializeField]
    GameObject castingFloor;

    // Clickable Collections (Still UI)
    [SerializeField]
    GameObject MainNObjs;
    [SerializeField]
    GameObject MainEObjs;
    [SerializeField]
    GameObject BedNObjs;
    [SerializeField]
    GameObject BedEObjs;

    // Audio Elements
    AudioManager AM;

    // World Variables
    public int room = 1;
    public bool ethereal = false;
    bool dialogueLocked = false;

    // Inventory Items
    [SerializeField]
    GameObject Item1;
    Vector2 Item1DefaultAnchor;
    [SerializeField]
    GameObject Item2;
    Vector2 Item2DefaultAnchor;
    [SerializeField]
    GameObject Item3;
    Vector2 Item3DefaultAnchor;
    [SerializeField]
    GameObject equippedItem = null;

    // Story Bools
    [SerializeField]
    bool toldSpell = false;
    [SerializeField]
    bool toldRitual = false;
    [SerializeField]
    bool paperHasSymbol = false;
    [SerializeField]
    bool gainedMarker = false;
    [SerializeField]
    bool gainedPaper = false;
    [SerializeField]
    bool gainedChalk = false;

    // Text Elements
    Hashtable textDB;
    int currentLine = 0;
    string currentArray = null;
    bool currentlyTyping = false;

    // Start is called before the first frame update
    void Start()
    {
        flashScreen.GetComponent<Image>().color = new Color(0,0,0,0);
        background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Backgrounds/MainRoomN");
        ethereal = false;
        room = 1;
        blockerImage.SetActive(false);
        setObjectsTrue(MainNObjs);
        setObjectsFalse(MainEObjs);
        setObjectsFalse(BedNObjs);
        setObjectsFalse(BedEObjs);
        //MainNObjs.SetActive(true);
        //MainEObjs.SetActive(false);
        //BedNObjs.SetActive(false);
        //BedEObjs.SetActive(false);
        AM = GetComponent<AudioManager>();
        Item1DefaultAnchor = Item1.GetComponent<RectTransform>().anchoredPosition;
        Item2DefaultAnchor = Item2.GetComponent<RectTransform>().anchoredPosition;
        Item3DefaultAnchor = Item3.GetComponent<RectTransform>().anchoredPosition;
        Item1.SetActive(false);
        Item2.SetActive(false);
        Item3.SetActive(false);


        // Establish Text Database
        textDB = new Hashtable();
        string[] tempString;

        // Text for Marker
        tempString = new string[] { "These are an odd looking set of drawers.", "Kinda looks like they were hand crafted...", "There's a marker in one of these.", "AddedItem", "Marker" };
        textDB.Add("GetMarker", tempString);
        tempString = new string[] { "The rest of the drawers are empty apart from the marker.", "No dust has gathered either...", "Something feels wrong here.", "It's like she never left." };
        textDB.Add("GottenMarker", tempString);

        // Text for Paper
        tempString = new string[] { "The rug's pretty nice. Whoever lived here is pretty well off.", "Though it looks like something is sticking out.", "Paper? Why would this be here?", "AddedItem", "Paper" };
        textDB.Add("GetPaper", tempString);
        tempString = new string[] { "Moving the rest of rug doesn't reveal much. Just a lot of dust.", "Wait... this dust is white. Chalk dust.", "Is she a wizard herself?", "Let's look around for something a little more concrete." };
        textDB.Add("GottenPaper", tempString);

        // Text for Chalk
        tempString = new string[] { "Just a normal chest of drawers.", "Please forgive me for going through your drawers.... and...", "Nothing out of the ordinary in here.", "Just clothes and few other items.", "Wait there's a box here.", "With chalk... odd", "AddedItem", "Chalk"};
        textDB.Add("GetChalk", tempString);
        tempString = new string[] {  "Chalk in such an ornate box...", "She's definitely a caster of sorts." };
        textDB.Add("GottenChalk", tempString);

        // Text for Combo
        tempString = new string[] { "I don't think I can use that here." };
        textDB.Add("WrongCombo", tempString);
        tempString = new string[] { "If what the being told me is true...", "I can draw the pentagram to get the spell.", "It's a magnetism spell. What can I use that for?", "AddedItem", "Symbol"};
        textDB.Add("PenPaper", tempString);

        // Text for Being
        tempString = new string[] { "'Hmm? Someone's missing you say?' Asks the ethereal being, it's voice deep as a groaning ship lost at sea.", "'Yes I heard them on the other side of this door last night.' It continues.", "'They've been quiet ever since.'", "'Though they do seem to lock their own door a lot.'", "'Say you wouldn't be in the market for a magnetism spell? Would you?' The being continues.", "Perhaps you are you say to it.", "'Good, if you've got a pentagram lying about anywhere, all you have to do is...'", "'Chant the following words.'", "'Magnes Ades Auxilium'", "'Apart from that I can't help you.'"};
        textDB.Add("TellSpell", tempString);
        tempString = new string[] { "'I don't have any more information to offer.'" };
        textDB.Add("Being", tempString);
        tempString = new string[] { "'I do remember her chanting something now that I think about it.'" , "'Draw a chalk circle on the ground and recite the following:'", "'vires ultra patriam me'", "'Though I have no idea what that means...'" };
        textDB.Add("TellRitual", tempString);

        // Text for EndGame
        tempString = new string[] { "vires ultra patriam me","'HAHAHAHA!' The being cackles." , "This creature tricked me.", "I'm stuck in this hellscape", "'Well you've found her.'", "But who's going to find you?! HAHAHAHA", "FinishGame"};
        textDB.Add("EndGame", tempString);

        // Text for Girl
        tempString = new string[] { "'...'", "It looks sad." };
        textDB.Add("Girl", tempString);

        // Text for Bedroom Door
        tempString = new string[] { "Let's see if that being's advice is correct.", "'Magnes Ades Auxilium'", "Hey, the key slid under the door and now I can unlock it.", "Let's hope her room has more clues than out here.", "GoToBedroom" };
        textDB.Add("EnterBedroom", tempString);
        tempString = new string[] { "This must be her bedroom." , "It's locked and looks like it needs a key.", "Looking under the door, I can see a key on the floor...", "She locked it from the inside?", "The window is locked too.", "Lost in her own room? Strange."};
        textDB.Add("BedroomDoor", tempString);

        // Text for Furniture Room 1
        tempString = new string[] { "This is a new book, recently bought with a receipt still in it.", "A Beginner's Guide to Black Magic.", "The information in this book doesn't really mean anything.", "I've never heard of this author.", "Nothing useful here..." };
        textDB.Add("Book", tempString);
        tempString = new string[] { "A generic picture of a house.", "On second thought, it looks quite nice.", "I'm more of a city person to be honest.", "I might change my mind when I'm older and retire." };
        textDB.Add("HousePainting", tempString);
        tempString = new string[] { "It's a plane.", "I don't know anything about planes really.", "How they make planes fly in the first place is magic in itself.", "Maybe when this case is over, I'll go on holiday abroad." };
        textDB.Add("PlanePainting", tempString);
        tempString = new string[] { "Some shoes.", "This girl definitely values comfort.", "Looks full, like no shoes were missing...", "Wearing boots perhaps?" };
        textDB.Add("ShoeRack", tempString);
        tempString = new string[] { "Boo!", "Haha just kidding. Tis me.", "When this case is over, I should get a haircut.", "Nothing unusual here." };
        textDB.Add("Mirror", tempString);
        tempString = new string[] { "I don't know enough about plants to tell what kind of plant it is.", "It's green, but it does look healthy and well taken care of.", "There doesn't seem to be anything unusual here." };
        textDB.Add("Plant", tempString);
        tempString = new string[] { "Smells like mulled wine", "with citrus and some other herb or spices which I don't know the name of.", "Cinnamon, ginger, cloves etc.", "Looks and smells like a normal candle." };
        textDB.Add("Candles", tempString);
        tempString = new string[] { "Just a normal lamp.", "No hidden switches or items in or around it as far as I can tell.", "It works at least." };
        textDB.Add("Lamp", tempString);
        tempString = new string[] { "Just an empty room.", "Doesn't look like it's been used in a while.", "Can't find anything unusual or interesting." };
        textDB.Add("Empty1", tempString);
        tempString = new string[] { "Another empty room.", "Doesn't look like it's been used in a while.", "Nothing of interest here." };
        textDB.Add("Empty2", tempString);
        tempString = new string[] { "It's a ceiling light.", "Nothing unusual about.", "It works like it should do at least." };
        textDB.Add("Light", tempString);

        // Text for Furniture Room 2
        tempString = new string[] { "Apart from the scratches and the bed being unmade, there doesn't seem to be anything unusual.", "Nothing in the pillow covers.", "Nothing under the bed, just a few books and boxes of stuff.", "Nothing seems to be hidden under the mattress either." };
        textDB.Add("Bed", tempString);
        tempString = new string[] { "A few books about the Occult and Magic in general, nothing really stands out.", "The books have been thumbed through a bit, can't seem to find anything worthy of note." };
        textDB.Add("Table", tempString);
        tempString = new string[] { "Glass is so clean it almost looks like it were wide open.", "It's a bit dark outside, I can see the park across the road and that's pretty much all I can make out." };
        textDB.Add("Window", tempString);
        tempString = new string[] { "More books on Magic and the Occult...by the looks of it, the information in them is pretty generic.", "Some novels and fiction books around the theme of the supernatural, but other than that there's nothing of interest here." };
        textDB.Add("Shelf", tempString);
        tempString = new string[] { "Interesting. This scratch mark doesn't look like it can be done by a human. What could've made it in the first place?", "Kinda looks looks like claws ..." };
        textDB.Add("Scratch", tempString);

        // Text for Ethereal
        tempString = new string[] { "Oof. Bad vibes here. I do not like. But gotta push through and find out what happened to her..." };
        textDB.Add("Creepy", tempString);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("EtherealCast") && !dialogueLocked)
        {
            changeEthereal();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }

        if (equippedItem != null)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, Input.mousePosition, mainCanvas.worldCamera, out pos);
            equippedItem.transform.position = mainCanvas.transform.TransformPoint(pos);
        }
        if (Input.GetButtonDown("Fire1") && equippedItem != null)
        {
            if (Vector2.Distance(Item1.GetComponent<RectTransform>().position, Item2.GetComponent<RectTransform>().position) < 10f && toldSpell == true && Item1.activeSelf == true && Item2.activeSelf == true)
            {
                Item2.GetComponent<RectTransform>().anchoredPosition = Item2DefaultAnchor;
                Item1.SetActive(false);
                equippedItem = null;
                paperHasSymbol = true;
                startText("PenPaper");
            }
            else if (paperHasSymbol == true
                && Item2.activeSelf == true
                && bedroomDoor.GetComponent<RectTransform>().offsetMin.x < Item2.GetComponent<RectTransform>().position.x
                && bedroomDoor.GetComponent<RectTransform>().offsetMin.y < Item2.GetComponent<RectTransform>().position.y
                && bedroomDoor.GetComponent<RectTransform>().offsetMax.x > Item2.GetComponent<RectTransform>().position.x
                && bedroomDoor.GetComponent<RectTransform>().offsetMax.y > Item2.GetComponent<RectTransform>().position.y)
            {
                Item2.GetComponent<RectTransform>().anchoredPosition = Item2DefaultAnchor;
                Item2.SetActive(false);
                equippedItem = null;
                startText("EnterBedroom");
            }
            else if (toldRitual == true
                && Item3.activeSelf == true
                && castingFloor.GetComponent<RectTransform>().offsetMin.x < Item3.GetComponent<RectTransform>().position.x
                && castingFloor.GetComponent<RectTransform>().offsetMin.y < Item3.GetComponent<RectTransform>().position.y
                && castingFloor.GetComponent<RectTransform>().offsetMax.x > Item3.GetComponent<RectTransform>().position.x
                && castingFloor.GetComponent<RectTransform>().offsetMax.y > Item3.GetComponent<RectTransform>().position.y)
            {
                Item3.GetComponent<RectTransform>().anchoredPosition = Item3DefaultAnchor;
                Item3.SetActive(false);
                equippedItem = null;
                changeEthereal();
                AM.playEvil();
                startText("EndGame");
            }
            else
            {
                if (equippedItem == Item1)
                {
                    Item1.GetComponent<RectTransform>().anchoredPosition = Item1DefaultAnchor;
                }
                if (equippedItem == Item2)
                {
                    Item2.GetComponent<RectTransform>().anchoredPosition = Item2DefaultAnchor;
                }
                if (equippedItem == Item3)
                {
                    Item3.GetComponent<RectTransform>().anchoredPosition = Item3DefaultAnchor;
                }
                startText("WrongCombo");
            }
            setEquippedItem(0);
        }
    }

    // Allows the being to talk when clicked on
    public void talkToBeing()
    {
        // Tell spell if in room 1 and not been told
        if (!toldSpell && room == 1)
        {
            startText("TellSpell");
            toldSpell = true;
        }
        // Tell nothing if in room 1 and have been told
        else if (toldSpell && room == 1)
        {
            startText("Being");
        }
        // Tell ritual if in room 2 and not been told
        else if (!toldRitual && room == 2)
        {
            startText("TellRitual");
            toldRitual = true;
        }
        // Tell nothing if in room 2 and have been told
        else if (toldRitual && room == 2)
        {
            startText("Being");
        }
    }

    public void getMarker()
    {
        if (!gainedMarker) { startText("GetMarker"); gainedMarker = true; }
        else { startText("GottenMarker"); }
    }

    public void getPaper()
    {
        if (!gainedPaper) { startText("GetPaper"); gainedPaper = true; }
        else { startText("GottenPaper"); }
    }

    public void getChalk()
    {
        if (!gainedChalk) { startText("GetChalk"); gainedChalk = true; }
        else { startText("GottenChalk"); }
    }

    public void tryBedroom()
    {

    }

    public void setEquippedItem(int i)
    {
        if (i == 1)
        { equippedItem = Item1; }
        else if (i == 2)
        { equippedItem = Item2; }
        else if (i == 3)
        { equippedItem = Item3; }
        else 
        { equippedItem = null; }
    }

    void changeRoom(int currentRoom)
    {
        StopAllCoroutines();
        textButton.GetComponentInChildren<Text>().text = "";
        blockerImage.SetActive(false);
        if (currentRoom == 1)
        {
            room = 2;
            background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Backgrounds/BedRoomN");
            ethereal = false;
            setObjectsFalse(MainNObjs);
            setObjectsFalse(MainEObjs);
            setObjectsTrue(BedNObjs);
            setObjectsFalse(BedEObjs);
            flashingScreen();
        }
        else 
        {
            room = 1;
            background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Backgrounds/MainRoomN");
            ethereal = false;
            setObjectsTrue(MainNObjs);
            setObjectsFalse(MainEObjs);
            setObjectsFalse(BedNObjs);
            setObjectsFalse(BedEObjs);
            flashingScreen();
        }
    }

    void changeEthereal()
    {
        StopAllCoroutines();
        textButton.GetComponentInChildren<Text>().text = "";
        blockerImage.SetActive(false);
        ethereal = !ethereal;
        if (ethereal == false && room == 1 && !dialogueLocked)
        {
            flashingScreen();
            background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Backgrounds/MainRoomN");
            setObjectsTrue(MainNObjs);
            setObjectsFalse(MainEObjs);
            setObjectsFalse(BedNObjs);
            setObjectsFalse(BedEObjs);
            //MainNObjs.SetActive(true);
            //MainEObjs.SetActive(false);
            //BedNObjs.SetActive(false);
            //BedEObjs.SetActive(false);
        }
        if (ethereal == true && room == 1 && !dialogueLocked)
        {
            flashingScreen();
            background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Backgrounds/MainRoomE");
            setObjectsFalse(MainNObjs);
            setObjectsTrue(MainEObjs);
            setObjectsFalse(BedNObjs);
            setObjectsFalse(BedEObjs);
            //MainNObjs.SetActive(false);
            //MainEObjs.SetActive(true);
            //BedNObjs.SetActive(false);
            //BedEObjs.SetActive(false);
        }
        if (ethereal == false && room == 2 && !dialogueLocked)
        {
            flashingScreen();
            background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Backgrounds/BedRoomN");
            setObjectsFalse(MainNObjs);
            setObjectsFalse(MainEObjs);
            setObjectsTrue(BedNObjs);
            setObjectsFalse(BedEObjs);
            //MainNObjs.SetActive(false);
            //MainEObjs.SetActive(false);
            //BedNObjs.SetActive(true);
            //BedEObjs.SetActive(false);
        }
        if (ethereal == true && room == 2 && !dialogueLocked)
        {
            flashingScreen();
            background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Backgrounds/BedRoomE");
            setObjectsFalse(MainNObjs);
            setObjectsFalse(MainEObjs);
            setObjectsFalse(BedNObjs);
            setObjectsTrue(BedEObjs);
            //MainNObjs.SetActive(false);
            //MainEObjs.SetActive(false);
            //BedNObjs.SetActive(false);
            //BedEObjs.SetActive(true);
        }
    }

    void flashingScreen()
    {
        flashScreen.GetComponent<Image>().color = Color.white;
        dialogueLocked = true;
        StartCoroutine("flashTheScreen");
    }

    IEnumerator flashTheScreen()
    {
        while (flashScreen.GetComponent<Image>().color.a > 0)
        {
            flashScreen.GetComponent<Image>().color = new Color(1,1,1, flashScreen.GetComponent<Image>().color.a - 0.01f);
            yield return new WaitForSeconds(0.01f);

            if(ethereal)
            {
                AM.NRoomBackgroundMusic.volume -= (AM.currentAudioMax / 100);
                AM.ERoomBackgroundMusic.volume += (AM.currentAudioMax / 100);
            }
            else if (!ethereal)
            {
                AM.NRoomBackgroundMusic.volume += (AM.currentAudioMax / 100);
                AM.ERoomBackgroundMusic.volume -= (AM.currentAudioMax / 100);
            }
        }
        dialogueLocked = false;
    }

    void setObjectsFalse(GameObject o)
    {
        foreach (Transform child in o.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void setObjectsTrue(GameObject o)
    {
        foreach (Transform child in o.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void startText(string textArrayName)
    {
        dialogueLocked = true;
        currentLine = 0;
        currentArray = textArrayName;
        StopCoroutine("typeText");
        IEnumerator inputtedCoroutine = typeText((string[])textDB[textArrayName], currentLine);
        StartCoroutine(inputtedCoroutine);

        blockerImage.SetActive(true);
    }

    IEnumerator typeText(string[] data, int line)
    {
        textButton.GetComponentInChildren<Text>().text = "";
        currentlyTyping = true;
        foreach (char c in data[line])
        {
            textButton.GetComponentInChildren<Text>().text += c;
            yield return new WaitForSeconds(0.03f);
        }
        currentlyTyping = false;
    }

    public void updateText()
    {
        string[] inputStr = (string[])textDB[currentArray];
        if (currentlyTyping == true)
        {
            currentlyTyping = false;
            StopAllCoroutines();
            textButton.GetComponentInChildren<Text>().text = inputStr[currentLine];
        }
        else if (currentLine + 1 < inputStr.Length && inputStr[currentLine + 1] == "AddedItem")
        {
            currentLine++;
            currentLine++;
            if (inputStr[currentLine] == "Marker")
            {
                Item1.SetActive(true);
            }
            else if (inputStr[currentLine] == "Paper")
            {
                Item2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Entities/Paper");
                Item2.SetActive(true);
            }
            else if (inputStr[currentLine] == "Symbol")
            {
                Item2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Entities/Symbol");
                Item2.SetActive(true);
            }
            else if (inputStr[currentLine] == "Chalk")
            {
                Item3.SetActive(true);
            }
            textButton.GetComponentInChildren<Text>().text = "";
            blockerImage.SetActive(false);
            currentLine = 0;
            dialogueLocked = false;
        }
        else if (currentLine + 1 < inputStr.Length && inputStr[currentLine + 1] == "GoToBedroom")
        {
            currentLine++;
            changeRoom(1);
            Debug.Log("Changing Room");
        }
        else if (currentLine + 1 < inputStr.Length && inputStr[currentLine + 1] == "FinishGame")
        {
            Application.LoadLevel(3);
        }
        else if (currentLine + 1 < inputStr.Length)
        {
            currentLine++;
            StopCoroutine("typeText");
            IEnumerator inputtedCoroutine = typeText(inputStr, currentLine);
            StartCoroutine(inputtedCoroutine);
        }
        else
        {
            textButton.GetComponentInChildren<Text>().text = "";
            blockerImage.SetActive(false);
            currentLine = 0;
            dialogueLocked = false;
        }
    }
}
