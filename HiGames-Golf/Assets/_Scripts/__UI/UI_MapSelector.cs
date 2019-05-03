using Assets.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MapSelector : MonoBehaviour
{
    //---Variable
    public GameObject UI;
    public GameObject DisplayPrefab;

    public Canvas canvas;
    private float TOP_POSITION;
    private float MIDDLE_POSITION;
    private float DOWN_POSITION;
    private readonly float SWIPE_DISTANCE = 100f;
    private readonly float MAXSPEED = 60;

    //Map Selector
    private Vector3 UpPos, DownPos;
    private Chapter[] chaptersToMove = new Chapter[2];
    private float speed, chapterMoveDistance, distanceSoFar;
    private bool isMoving = false;

    private void Start()
    {
        float height = canvas.GetComponent<RectTransform>().sizeDelta.y;
        TOP_POSITION = +height;
        MIDDLE_POSITION = 0;
        DOWN_POSITION = -height;
    }

    //MS --- Map Selector
    public void Init()
    {
        //Unlocks first level if necessary;
        if (MapManager.Instance.Chapters[0].Displays[0].Locked)
        {
            MapManager.Instance.Chapters[0].Displays[0].Locked = false;
        }
        //Display Map Based on Old Location!
        int curr = MapManager.Instance.CurrentChapterNumber;
        Chapter_Display(curr + 1, TOP_POSITION);
        Chapter_Display(curr + 0, MIDDLE_POSITION);
        Chapter_Display(curr - 1, DOWN_POSITION);

        GameManager.Instance.ActUpdate += DetectSwipe;
    }
    public void Terminate()
    {
        UI.SetActive(false);
        int curr = MapManager.Instance.CurrentChapterNumber;

        Chapter_Destroy(curr - 1);
        Chapter_Destroy(curr + 0);
        Chapter_Destroy(curr + 1);

        if (GameManager.Instance.CurrentMap != MapManager.Instance.Menu)
        {
            GameManager.Instance.Setup_MenuMap();
        }
        else GameManager.Instance.CurrentPlayer.SelectedBall.GoStartingPosition(true);

        GameManager.Instance.ActUpdate -= DetectSwipe;
    }

    public void UnlockNextLevel(int level)
    {
        /* Ter em conta, que o curr é relativo a um index a frente do array
         * Logo para se saber o lugar atual no array é necessario subtrair 1
         * O mesmo se aplica ao level */
        int curr = MapManager.Instance.CurrentChapterNumber;
        Chapter c = MapManager.Instance.Chapters[curr - 1];

        //If last level -> Unlock First level of next chapter
        if (level == c.Maps.Length)
        {
            //Verify if next chapters is available
            if (MapManager.Instance.Chapters[curr] != null)
            {
                MapManager.Instance.Chapters[curr].Displays[0].Locked = false;
            }
            else Debug.Log("No More Chapters");
        }
        else
        {
            c.Displays[level].Locked = false;
        }
    }
    public void BUTTON_StartMap(GetMap gm)
    {
        if (!isMoving && !gm.map.Display.Locked)
        {
            MapManager.Instance.SelectedMap = gm.map;
            UiManager.Instance.CloseInterface_MapSelector();
            GameManager.Instance.Setup_Singleplayer();
        }
    }

    private void Chapter_Display(int num, float pos)
    {
        if (num <= MapManager.Instance.Chapters.Count && num > 0)
        {
            Chapter c = MapManager.Instance.Chapters[num - 1];
            Vector2 posVec = new Vector3(0, pos);

            //Instantiate Background
            c.BackGround = new GameObject("Chapter" + num + "BackGround", typeof(RectTransform));
            c.BackGround.AddComponent<Image>();
            c.BackGround.GetComponent<Image>().sprite = c.Sprite;
            c.BackGround.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            c.BackGround.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            c.BackGround.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            c.BackGround.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            c.BackGround.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            c.BackGround.GetComponent<RectTransform>().position += new Vector3(posVec.x, posVec.y, 0);
            c.BackGround.transform.SetParent(UI.transform.Find("Displays"), false);
            c.BackGround.SetActive(true);

            //Instantiate Displays
            for (int i = 0; i < c.Displays.Length; i++)
            {
                int level = i + 1;
                Display d = c.Displays[i];
                d.GO_Copy = DisplayPrefab;
                d.POS = c.DisplayInfos[i].pos;
                d.GO = Instantiate(d.GO_Copy, d.POS.anchoredPosition + posVec, Quaternion.identity);
                d.GO.transform.name = "Level: " + level;
                d.GO.transform.SetParent(UI.transform.Find("Displays"), false);
                d.GO.SetActive(true);
            }
            c.SetDisplays(c.DisplayInfos);
        }
        else Debug.Log("Chapter number not available!");

        //217900790 - numero reaver codigo aixa direta
    }
    private bool Chapter_Destroy(int num)
    {
        if (num > 0 && num <= MapManager.Instance.Chapters.Count)
        {
            Chapter c = MapManager.Instance.Chapters[num - 1];
            GameObject b = c.BackGround;

            Destroy(b);
            for (int i = 0; i < c.Displays.Length; i++)
            {
                Display d = c.Displays[i];
                Destroy(d.GO);
            }
            return true;
        }
        else return false;
    }
    private void MoveUp()
    {
        int numberOfChapters = MapManager.Instance.Chapters.Count;
        int chapterNum = MapManager.Instance.CurrentChapterNumber;

        if (chapterNum == 1) Debug.Log("Cant go up, no chapters up");
        else if (chapterNum > 1)
        {
            if (chapterNum != 2)
            {
                Chapter_Display(chapterNum - 2, DOWN_POSITION);
            }
            Chapter_Destroy(chapterNum + 1);

            SetupUpAnimation();
            MapManager.Instance.CurrentChapterNumber--;

            GameManager.Instance.ActUpdate += Animate;
        }
    }
    private void MoveDown()
    {
        int numberOfChapters = MapManager.Instance.Chapters.Count;
        int chapterNum = MapManager.Instance.CurrentChapterNumber;

        if (chapterNum == numberOfChapters) Debug.Log("Cant go down, no chapters up");
        else if (chapterNum < numberOfChapters)
        {
            Chapter_Display(chapterNum + 2, TOP_POSITION);

            if (chapterNum != 1)
            {
                Chapter_Destroy(chapterNum - 1);

            }
            SetupDownAnimation();
            MapManager.Instance.CurrentChapterNumber++;

            GameManager.Instance.ActUpdate += Animate;
        }
        else if (chapterNum > numberOfChapters) Debug.Log("Something went wrong, current chapter above number of chapters");
    }
    private void SetupUpAnimation()
    {
        //Number active Chapters
        int curr = MapManager.Instance.CurrentChapterNumber;

        for (int i = 0; i < 2; i++)
        {
            Chapter c = MapManager.Instance.Chapters[(curr - i) - 1];
            chaptersToMove[i] = c;
            foreach (Display display in chaptersToMove[i].Displays)
            {
                display.LAST_POS = display.POS;
            }
        }
        chapterMoveDistance = TOP_POSITION;
        distanceSoFar = 0;
        speed = MAXSPEED;
    }
    private void SetupDownAnimation()
    {
        //Number active Chapters
        int curr = MapManager.Instance.CurrentChapterNumber;

        for (int i = 0; i < 2; i++)
        {
            Chapter c = MapManager.Instance.Chapters[(curr + i) - 1];
            chaptersToMove[i] = c;
            foreach (Display display in chaptersToMove[i].Displays)
            {
                display.LAST_POS = display.POS;
            }
        }
        chapterMoveDistance = -TOP_POSITION;
        distanceSoFar = 0;
        speed = -MAXSPEED;
    }
    private void Animate()
    {
        distanceSoFar += MAXSPEED;
        //Running Down
        if (distanceSoFar <= Math.Abs(chapterMoveDistance))
        {
            foreach (Chapter c in chaptersToMove)
            {
                c.BackGround.GetComponent<RectTransform>().localPosition += new Vector3(0, speed);

                foreach (Display d in c.Displays)
                {
                    d.GO.GetComponent<RectTransform>().offsetMax += new Vector2(0, speed);
                    d.GO.GetComponent<RectTransform>().offsetMin += new Vector2(0, speed);
                }
            }
            isMoving = true;
        }
        //Sthap
        else
        {
            int t = 1;
            for (int i = 0; i < 2; i++)
            {
                chaptersToMove[i].BackGround.GetComponent<RectTransform>().localPosition = new Vector3(0, chapterMoveDistance * t, 0);

                foreach (Display d in chaptersToMove[i].Displays)
                {
                    d.GO.GetComponent<RectTransform>().localPosition = new Vector2(d.LAST_POS.localPosition.x, d.LAST_POS.localPosition.y + chapterMoveDistance * t);
                }

                t--;
            }
            GameManager.Instance.ActUpdate -= Animate;
            isMoving = false;
        }
    }
    
    //Swiping Methods
    private void DetectSwipe()
    {
        foreach (Touch t in Input.touches)
        {
            if(t.phase == TouchPhase.Began)
            {
                UpPos = t.position;
                DownPos = t.position;
            }
            //if(t.phase == TouchPhase.Moved)
            //{
            //    DownPos = t.position;
            //    CheckSwipe();
            //}
            if (t.phase == TouchPhase.Ended)
            {
                DownPos = t.position;
                CheckSwipe();
            }
        }
        //if (Input.GetMouseButtonDown(0) && !down)
        //{
        //    UpPos = Input.mousePosition;
        //    DownPos = Input.mousePosition;
        //    down = true;
        //}
        //if (down)
        //{
        //    DownPos = Input.mousePosition;
        //    activate = true;
        //}
        //if (Input.GetMouseButtonUp(0) && activate)
        //{
        //    down = false;
        //    activate = false;
        //    CheckSwipe();
        //}
    }
    private void CheckSwipe()
    {
        if (VerticalMove() > SWIPE_DISTANCE && VerticalMove() > HorizontalValMove())
        {
            if (!isMoving)
            {
                if (DownPos.y - UpPos.y > 0)
                {
                    MoveUp();
                }
                else if (DownPos.y - UpPos.y < 0)
                {
                    MoveDown();
                }
                UpPos = DownPos;
            }
        }
        //SIDE SWIPING - NOT USED
        ////Check if Horizontal swipe
        //else if (horizontalValMove() > SWIPE_DISTANCE && horizontalValMove() > verticalMove())
        //{
        //    //Debug.Log("Horizontal");
        //    if (DownPos.x - UpPos.x > 0)//Right swipe
        //    {
        //        //Swipe Right
        //        Debug.Log("Swipe Right");
        //    }
        //    else if (DownPos.x - UpPos.x < 0)//Left swipe
        //    {
        //        //Swipe Left
        //        Debug.Log("SwipeLeft");
        //    }
        //    UpPos = DownPos;
        //}
        else
        {
            Debug.Log("No Swipe at All");
        }
    }
    private float VerticalMove()
    {
        return Mathf.Abs(DownPos.y - UpPos.y);
    }
    private float HorizontalValMove()
    {
        return Mathf.Abs(DownPos.x - UpPos.x);
    }
}
