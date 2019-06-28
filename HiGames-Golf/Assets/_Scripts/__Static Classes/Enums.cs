
public static class Enums
{
    #region Game Structs
    public enum GameType
    {
        Menu,
        OneShot,
        Waypoint,
        FreeForm
    };
    public enum GameState
    {
        Resumed,
        Paused
    };
    public enum GameMode
    {
        Menu,
        Singleplayer,
        Multiplayer,
        Localgame
    };
    #endregion

    public enum SkyboxType
    {
        Garage,
        LivingRoom,
        Outdoor
    }; //Map Skyboxes

    public enum Tab
    {
        Ball,
        Hat,
        Trail,
        Hand,
        Acessories
    }; //Skin Menu Tabs

    public enum CameraDirection { West, East, South, North }

    public enum ColorPalette { Default, two }

}
