using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Struct;

public abstract class Display
{
    public GameObject GO;
    public GameObject GO_Copy;
    public RectTransform POS;
    public RectTransform LAST_POS;
    public Map Map;
    public Chapter Chapter;
    public bool Locked = true;
    public int levelNumber;

    public Sprite SpriteLocked;
    public Sprite SpriteUnlocked;
    public Sprite SpriteLevel;

    public abstract void Init(Chapter chapter, Map map, int level, DisplayInfo di);
    public abstract void SetUnlocked();
    public abstract void SetLocked();
}
