using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeOfHole : MonoBehaviour
{
    public enum HoleType { Menu_Singleplayer, Menu_Multiplayer, Menu_Local, Game_FinalHole, Game_TrickHole};
    public HoleType typeOfHole;
}
