namespace Misc
{
    public enum ColorType
    {
        Red,
        Green,
        Blue,
        Yellow,
    }
    
    public enum GameState
    {
        MainMenu,
        Waiting,
        Playing,
        Paused,
        BonusCalculation,
        GameOver,
        Win,
        FewerMode
    }
    
    public enum CollectibleType
    {
        Color,
        ColorChanger,
        BonusCollectorStart,
        BonusCollectorEnd,
        Gold,
    }
}