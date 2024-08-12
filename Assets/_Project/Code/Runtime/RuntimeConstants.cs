namespace Code.Runtime
{
    public static class RuntimeConstants
    {
        public static class Scenes
        {
            public const string MATCH3_GAME = "Match3Game";
            public const string MAIN_MENU = "MainMenu";
        }

        public static class Assets
        {
            public const string UI_SCREEN_VIEWS = "Prefabs/UI/Screens/";
            public const string UI_DIALOG_VIEWS = "Prefabs/UI/Dialogs/";
            public const string SQUEEZE_VFX = "Prefabs/Match3/VFX/FruitDestroyVFX";
            public const string FRAGMENT_VFX = "Prefabs/Match3/VFX/BlockDestroyVFX";
            public const string MATCH3_GRID_SLOT = "Prefabs/Match3/GridSlot";
            public const string MATCH3_GRID_SHAPE = "Prefabs/Match3/GridShape";
            public const string UI_MATCH3_FRUIT_GOAL_SHAPE = "Prefabs/UI/Screens/HungryBat/FruitGoal";
            public const string UI_MATCH3_BLOCK_GOAL_SHAPE = "Prefabs/UI/Screens/BlockCrush/BlockGoal";
            public const string HUNGRY_BAT_ENVIRONMENT = "Prefabs/Match3/HungryBatEnvironment";
            public const string BLOCK_CRUSH_ENVIRONMENT = "Prefabs/Match3/BlockCrushEnvironment";
            public const string HUNGRY_BAT_VFX_CONFIG = "Configs/Match3VFX/HungryBatVFXConfig";
            public const string BLOCK_CRUSH_VFX_CONFIG = "Configs/Match3VFX/BlockCrushVFXConfig";
            public const string HUNGRY_BAT_SHAPE_VIEWS_CONFIG = "Configs/Match3Shapes/HungryBatShapeViewsConfig";
            public const string BLOCK_CRUSH_SHAPE_VIEWS_CONFIG = "Configs/Match3Shapes/BlockCrushShapeViewsConfig";
        }

        public static class Test
        {
            public const float HINT_TIME_DELAY = 2; // Milliseconds
        }
    }
}