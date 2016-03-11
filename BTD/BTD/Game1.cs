using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BTD
{
    // TODO When building add a hover check on towers that brings up info about the tower. Cost etc.
    // TODO Add rotation to projectiles.
    // TODO Add animation to enemies.

    public enum GameState
    {
        NewGame,
        DifficultySelect,
        HeroSelect,
        Playing,
        GameOver,
        Menu,
        ResolutionChange
    }

    public enum Hero
    {
        Arthus,
        Jaina,
        Pyrus,
        Coldreaver,
        Thrall,
        Arachne,
        Tichondrius,
        Joranda,
        Furion,
        Tyrande
    }

    public static class Variables
    {
        public static int numberLives;
        public static int gold;
        public static int lumber;
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Texture2D testEnemy;

        public const int DEFAULT_WINDOW_WIDTH = 1440;
        public const int DEFAULT_WINDOW_HEIGHT = 810;
        public const int WORLD_TEXTURE_DIMENSIONS = 32;

        public const int STARTING_GOLD = 60;
        public const int STARTING_LUMBER = 0;

        public int windowWidth = DEFAULT_WINDOW_WIDTH;
        public int windowHeight = DEFAULT_WINDOW_HEIGHT;

        public static GameState gameState;
        public static Hero hero1;
        //public static Hero hero2;
        //public static Hero hero3;
        //public static Hero hero4;
        public static bool building = false;

        public bool towerSelected;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
    
        Texture2D mapTexture;
        Map map;
        int mapWidth;
        int mapHeight;

        Texture2D screenOverlay;
        Texture2D menuBackground;
        Texture2D menuButton;
        Menu mainMenu;
        Menu resolutionMenu;
        Menu difficultySelect;
        Menu heroSelect;
        Menu gameOver;
        string menuDescription = "";

        UserInterface userInterface;
        Texture2D miniMap;
        Texture2D miniMapOverlay;
        Texture2D miniMapBorder;
        Texture2D commandsBorder;
        Texture2D commandsBackground;
        Texture2D informationBorder;
        Texture2D heroThumbs;
        Texture2D towerThumbs;
        Texture2D arrow;
        Texture2D buildSellButtons;
        Texture2D towers;
        Texture2D waitingToBuildTowers;
        float interfaceVerticalScale;
        float interfaceHorizontalScale;
        Rectangle miniMapArea;

        Vector2 mouseWorldPos;
        MouseState oldMouseState;
        KeyboardState oldKeyboardState;

        Camera2d Camera;
        Viewport viewport;

        bool buildable;
        Vector2 buildCordinates;
        int buildingTowerIndex;

        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            //graphics.IsFullScreen = true;
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameState = GameState.NewGame;
            map = new Map(WORLD_TEXTURE_DIMENSIONS);
            mapWidth = map.MapWidth;
            mapHeight = map.MapHeight;

            viewport = new Viewport(0, 0, windowWidth, windowHeight);
            Camera = new Camera2d(viewport, mapWidth * WORLD_TEXTURE_DIMENSIONS, mapHeight * WORLD_TEXTURE_DIMENSIONS, 1f);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mapTexture = this.Content.Load<Texture2D>("GrassRoadSheet");
            miniMap = this.Content.Load<Texture2D>("minimap");
            miniMapOverlay = this.Content.Load<Texture2D>("minimapoverlay");
            miniMapBorder = this.Content.Load<Texture2D>("leftborder");
            commandsBorder = this.Content.Load<Texture2D>("rightborder");
            commandsBackground = this.Content.Load<Texture2D>("commandsbackground");
            informationBorder = this.Content.Load<Texture2D>("centerborder");            
            screenOverlay = this.Content.Load<Texture2D>("screenoverlay");
            menuBackground = this.Content.Load<Texture2D>("menubackground");
            menuButton = this.Content.Load<Texture2D>("menubutton");
            buildSellButtons = this.Content.Load<Texture2D>("buildsellbuttons");
            heroThumbs = this.Content.Load<Texture2D>("herothumbs");
            towerThumbs = this.Content.Load<Texture2D>("towerthumbs");
            towers = this.Content.Load<Texture2D>("towers");
            waitingToBuildTowers = this.Content.Load<Texture2D>("waitingtobuildtowers");
            arrow = this.Content.Load<Texture2D>("arrow");
            
            font = this.Content.Load<SpriteFont>("Miramonte");

            testEnemy = this.Content.Load<Texture2D>(@"Enemies\wave1bunnies");
        } 

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {          
            MouseState currentMouseState = Mouse.GetState();
            KeyboardState currentKeyboardState = Keyboard.GetState();

            // Transform mouse input from view to world position
            // I copied this.
            Matrix inverse = Matrix.Invert(Camera.GetTransformation());
            mouseWorldPos = Vector2.Transform(new Vector2(currentMouseState.X, currentMouseState.Y), inverse);

            switch (gameState)
            {
                #region case NewGame
                case GameState.NewGame:
                    building = false;
                    Variables.gold = STARTING_GOLD;
                    Variables.lumber = STARTING_LUMBER;
                    interfaceVerticalScale = (float)windowHeight / 4 / miniMap.Height;
                    interfaceHorizontalScale = (float)windowWidth / DEFAULT_WINDOW_WIDTH;
                    userInterface = new UserInterface(windowWidth, windowHeight, interfaceVerticalScale, interfaceHorizontalScale, mapWidth * WORLD_TEXTURE_DIMENSIONS, mapHeight * WORLD_TEXTURE_DIMENSIONS);
                    UserInterface.interfaceItems.Clear();
                    UserInterface.buildTowerThumbs.Clear();
                    userInterface.CreateInterfaceItems(miniMap, miniMapOverlay, miniMapBorder, commandsBorder, commandsBackground, informationBorder, buildSellButtons);
                    Camera.UpdatePosition(currentMouseState);
                    userInterface.UpdatePositions(Camera.Pos);
                    
                    Tower.Towers.Clear();
                    Tower.BuiltTowers.Clear();
                    Projectiles.ProjectilesList.Clear();

                    Enemy.enemyList.Clear();

                    Map.map.Clear();
                    map.CreateMap(WORLD_TEXTURE_DIMENSIONS);

                    difficultySelect = new Menu(menuBackground, menuButton, font);
                    difficultySelect.CreateButton("Easy - 50 Lives", "Can lose 50 lives before gameover");
                    difficultySelect.CreateButton("Normal - 25 Lives", "Can lose 25 lives before gameover");
                    difficultySelect.CreateButton("Hard - 1 Life", "Can lose 1 life before gameover");
                    //difficultySelect.CreateButton("Insane - 1 Life", "Can lose 1 life before gameover \nLevels are also randomized from 3 possible levels");

                    gameState = GameState.DifficultySelect;

                    break;
                #endregion

                #region case DifficultySelect
                case GameState.DifficultySelect:
                    Button tempButton;
                    Rectangle menuArea = new Rectangle((int)Camera.Pos.X - (menuButton.Width + 20) / 2, (int)Camera.Pos.Y - (windowHeight / 3), menuButton.Width + 20, 0);
                    difficultySelect.UpdatePositions(menuArea);

                    menuDescription = difficultySelect.CheckHover(oldMouseState, currentMouseState, mouseWorldPos);

                    tempButton = difficultySelect.CheckClick(oldMouseState, currentMouseState, mouseWorldPos);
                    if (tempButton != null)
                    {
                        if (tempButton.Label == "Easy - 50 Lives")
                        {
                            Variables.numberLives = 50;
                        }
                        else if (tempButton.Label == "Normal - 25 Lives")
                        {
                            Variables.numberLives = 25;
                        }
                        else if (tempButton.Label == "Hard - 1 Life")
                        {
                            Variables.numberLives = 1;
                        }
                        //else if (tempButton.Label == "Insane - 1 Life")
                        //{
                        //    numberLives = 1;
                        //    // Set random levels
                        //}

                        menuArea = new Rectangle((int)Camera.Pos.X - (menuButton.Width + 20) / 2, (int)Camera.Pos.Y - (windowHeight / 3), menuButton.Width + 20, 0);
                        heroSelect = new Menu(menuBackground, menuButton, font);
                        heroSelect.CreateButton("Arthus - Human", "Focus: Cheap towers\nSecondaryElements: Poison, Holy, Ice\nUltimate Building: Siege Tank");
                        heroSelect.CreateButton("Jaina - Magic", "Focus: Special Abilities\nSecondaryElements: Fire, Ice, Thunder\nUltimate Building: Mind Ripper");
                        heroSelect.CreateButton("Pyrus - Fire", "Focus: Splash damage\nSecondaryElements: Death, Earth, Holy\nUltimate Building: Volcanic Fissure");
                        heroSelect.CreateButton("Coldreaver - Ice", "Focus: Frost damage\nSecondaryElements: Earth, Human, Wind\nUltimate Building: Cryogenics Lab");
                        heroSelect.CreateButton("Thrall - Thunder", "Focus: Range\nSecondaryElements: Holy, Magic, Fire\nUltimate Building: Power Plant");
                        heroSelect.CreateButton("Arachne - Poison", "Focus: Slow Poison and related abilities\nSecondaryElements: Death, Fire, Earth\nUltimate Building: Infection Intensifier");
                        heroSelect.CreateButton("Tichondrius - Death", "Focus: Black Magic\nSecondaryElements: Wind, Thunder, Poison\nUltimate Building: Enfeebler");
                        heroSelect.CreateButton("Joranda - Holy", "Focus: Anti-undead, Defense\nSecondaryElements: Magic, Wind, Human\nUltimate Building: Guardian Angel");
                        heroSelect.CreateButton("Furion - Earth", "Focus: Anti-land, Siege damage\nSecondaryElements: Poison, Thunder, Magic\nUltimate Building: Gold Mine");
                        heroSelect.CreateButton("Tyrande - Wind", "Focus: Anti-air\nSecondaryElements: Human, Ice, Death\nUltimate Building: Sky Dominator");
                        heroSelect.UpdatePositions(menuArea);

                        gameState = GameState.HeroSelect;
                    }

                    break;
                #endregion

                #region case HeroSelect
                case GameState.HeroSelect:                    
                    menuDescription = heroSelect.CheckHover(oldMouseState, currentMouseState, mouseWorldPos);

                    tempButton = null;
                    tempButton = heroSelect.CheckClick(oldMouseState, currentMouseState, mouseWorldPos);
                    if (tempButton != null)
                    {
                        int heroSpacing = 112;
                        if (tempButton.Label == "Arthus - Human")
                        {
                            hero1 = Hero.Arthus;
                            userInterface.CreateHeroThumb(heroThumbs, towerThumbs, Hero.Arthus, new Rectangle(heroSpacing * (int)Hero.Arthus, 0, 112, 117));
                        }
                        else if (tempButton.Label == "Jaina - Magic")
                        {
                            hero1 = Hero.Jaina;
                            userInterface.CreateHeroThumb(heroThumbs, towerThumbs, Hero.Jaina, new Rectangle(heroSpacing * (int)Hero.Jaina, 0, 112, 117));
                        }
                        else if (tempButton.Label == "Pyrus - Fire")
                        {
                            hero1 = Hero.Pyrus;
                            userInterface.CreateHeroThumb(heroThumbs, towerThumbs, Hero.Pyrus, new Rectangle(heroSpacing * (int)Hero.Pyrus, 0, 112, 117));
                        }
                        else if (tempButton.Label == "Coldreaver - Ice")
                        {
                            hero1 = Hero.Coldreaver;
                            userInterface.CreateHeroThumb(heroThumbs, towerThumbs, Hero.Coldreaver, new Rectangle(heroSpacing * (int)Hero.Coldreaver, 0, 112, 117));
                        }
                        else if (tempButton.Label == "Thrall - Thunder")
                        {
                            hero1 = Hero.Thrall;
                            userInterface.CreateHeroThumb(heroThumbs, towerThumbs, Hero.Thrall, new Rectangle(heroSpacing * (int)Hero.Thrall, 0, 112, 117));
                        }
                        else if (tempButton.Label == "Arachne - Poison")
                        {
                            hero1 = Hero.Arachne;
                            userInterface.CreateHeroThumb(heroThumbs, towerThumbs, Hero.Arachne, new Rectangle(heroSpacing * (int)Hero.Arachne, 0, 112, 117));
                        }
                        else if (tempButton.Label == "Tichondrius - Death")
                        {
                            hero1 = Hero.Tichondrius;
                            userInterface.CreateHeroThumb(heroThumbs, towerThumbs, Hero.Tichondrius, new Rectangle(heroSpacing * (int)Hero.Tichondrius, 0, 112, 117));
                        }
                        else if (tempButton.Label == "Joranda - Holy")
                        {
                            hero1 = Hero.Joranda;
                            userInterface.CreateHeroThumb(heroThumbs, towerThumbs, Hero.Joranda, new Rectangle(heroSpacing * (int)Hero.Joranda, 0, 112, 117));
                        }
                        else if (tempButton.Label == "Furion - Earth")
                        {
                            hero1 = Hero.Furion;
                            userInterface.CreateHeroThumb(heroThumbs, towerThumbs, Hero.Furion, new Rectangle(heroSpacing * (int)Hero.Furion, 0, 112, 117));
                        }
                        else if (tempButton.Label == "Tyrande - Wind")
                        {
                            hero1 = Hero.Tyrande;
                            userInterface.CreateHeroThumb(heroThumbs, towerThumbs, Hero.Tyrande, new Rectangle(heroSpacing * (int)Hero.Tyrande, 0, 112, 117));
                        }
                        userInterface.UpdatePositions(Camera.Pos);
                        gameState = GameState.Playing;
                    }

                    break;
                #endregion

                #region case Playing
                case GameState.Playing:

                    if (currentKeyboardState.IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyUp(Keys.Escape) && !building)
                    {
                        mainMenu = new Menu(menuBackground, menuButton, font);
                        mainMenu.CreateButton("Resume Game", "");
                        mainMenu.CreateButton("Restart Game", "");
                        mainMenu.CreateButton("Change Resolution", "");
                        mainMenu.CreateButton("Exit", "");

                        resolutionMenu = new Menu(menuBackground, menuButton, font);
                        resolutionMenu.CreateButton("1024x768", "");
                        resolutionMenu.CreateButton("1366x768", "");
                        resolutionMenu.CreateButton("1440x810", "");
                        resolutionMenu.CreateButton("1920x1080", "");
                        resolutionMenu.CreateButton("Go Windowed", "");
                        resolutionMenu.CreateButton("Go Fullscreen", "");
                        resolutionMenu.CreateButton("Back", "");
                        resolutionMenu.isActive = false;
                        
                        gameState = GameState.Menu;
                    }
                    
                    #region !Building
                    if (!building)
                    {
                        UserInterface.interfaceItems.Remove(userInterface.cancelCommand);

                        if (currentKeyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B))
                        {
                            UserInterface.interfaceItems.Add(userInterface.cancelCommand);
                            oldKeyboardState = currentKeyboardState;
                            building = true;
                        }

                        Rectangle buildCommand = new Rectangle((int)userInterface.buildCommand.Position.X, (int)userInterface.buildCommand.Position.Y, (int)(userInterface.buildCommand.SourceTexture.Width * interfaceHorizontalScale), (int)(userInterface.buildCommand.SourceTexture.Height * interfaceVerticalScale));
                        
                        if (buildCommand.Contains(new Point((int)mouseWorldPos.X, (int)mouseWorldPos.Y)))
                        {
                            if (oldMouseState.LeftButton != ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed)
                            {
                                UserInterface.interfaceItems.Add(userInterface.cancelCommand);
                                oldMouseState = currentMouseState;
                                building = true;
                            }
                        }
                    }
                    #endregion
                    
                    #region Building
                    if (building)
                    {
                        UserInterface.interfaceItems.Remove(userInterface.buildCommand);

                        currentKeyboardState = Keyboard.GetState();
                        currentMouseState = Mouse.GetState();

                        #region towerSelected
                        if (towerSelected)
                        {
                            buildable = false;

                            buildCordinates = new Vector2(mouseWorldPos.X / WORLD_TEXTURE_DIMENSIONS, mouseWorldPos.Y / WORLD_TEXTURE_DIMENSIONS);

                            if (buildCordinates.X < 0)
                                buildCordinates.X = 0;
                            if (buildCordinates.Y < 0)
                                buildCordinates.Y = 0;
                            if (buildCordinates.X > mapWidth - 2)
                                buildCordinates.X = mapWidth - 2;
                            if (buildCordinates.Y > mapHeight - 2)
                                buildCordinates.Y = mapHeight - 2;

                            int index = 0 + (int)buildCordinates.Y * mapWidth + (int)buildCordinates.X;

                            if (Map.map[index] == "G" && Map.map[index + 1] == "G" && Map.map[index + mapWidth] == "G" && Map.map[index + mapWidth + 1] == "G")
                            {
                                buildable = true;
                            }

                            if (buildable)
                            {
                                if (oldMouseState.LeftButton != ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed)
                                {
                                    Map.map[index] = "GBUILT";
                                    Map.map[index + 1] = "GBUILT";
                                    Map.map[index + mapWidth] = "GBUILT";
                                    Map.map[index + mapWidth + 1] = "GBUILT";
                                    Variables.gold -= int.Parse(Tower.Towers[buildingTowerIndex][1]);
                                    Variables.lumber -= int.Parse(Tower.Towers[buildingTowerIndex][2]);
                                    Tower.BuildTower(buildingTowerIndex, new Vector2((int)buildCordinates.X * WORLD_TEXTURE_DIMENSIONS, (int)buildCordinates.Y * WORLD_TEXTURE_DIMENSIONS), arrow);
                                }
                            }

                            if (Variables.gold < Tower.GetSelectedTowerGoldCost(buildingTowerIndex) || Variables.lumber < Tower.GetSelectedTowerLumberCost(buildingTowerIndex))
                            {
                                towerSelected = false;
                            }
                        }

                            
                        #endregion

                        for (int i = 0; i < UserInterface.buildTowerThumbs.Count; i++)
                        {
                            Rectangle towerButton = new Rectangle((int)UserInterface.buildTowerThumbs[i].Position.X, (int)UserInterface.buildTowerThumbs[i].Position.Y, (int)(UserInterface.buildTowerThumbs[i].SourceTexture.Width * interfaceHorizontalScale), (int)(UserInterface.buildTowerThumbs[i].SourceTexture.Height * interfaceVerticalScale));
                            if (towerButton.Contains(new Point((int)mouseWorldPos.X, (int)mouseWorldPos.Y)))
                            {
                                // Add a hover check thing and draw out description?? 
                                if (oldMouseState.LeftButton != ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed)
                                {
                                    if (Variables.gold >= Tower.GetSelectedTowerGoldCost(i) && Variables.lumber >= Tower.GetSelectedTowerLumberCost(i))
                                    {
                                        buildingTowerIndex = i;
                                        towerSelected = true;
                                    }                                                                      
                                }
                            }
                        }

                        Rectangle cancelCommand = new Rectangle((int)userInterface.cancelCommand.Position.X, (int)userInterface.cancelCommand.Position.Y, (int)(userInterface.cancelCommand.SourceTexture.Width * interfaceHorizontalScale), (int)(userInterface.cancelCommand.SourceTexture.Height * interfaceVerticalScale));

                        if (cancelCommand.Contains(new Point((int)mouseWorldPos.X, (int)mouseWorldPos.Y)))
                        {
                            if (oldMouseState.LeftButton != ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed)
                            {
                                towerSelected = false;
                                building = false;
                                UserInterface.interfaceItems.Add(userInterface.buildCommand);
                            }
                        }

                        if (currentKeyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B) || currentKeyboardState.IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyUp(Keys.Escape))
                        {
                            towerSelected = false;
                            building = false;
                            UserInterface.interfaceItems.Add(userInterface.buildCommand);
                        }
                    }
                    #endregion

                    miniMapArea = new Rectangle((int)userInterface.miniMap.Position.X, (int)userInterface.miniMap.Position.Y, (int)(miniMap.Width * interfaceHorizontalScale), (int)(miniMap.Height * interfaceVerticalScale));

                    if (miniMapArea.Contains(new Point((int)mouseWorldPos.X, (int)mouseWorldPos.Y)))
                    {
                        if (oldMouseState.LeftButton != ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed)
                        {
                            Camera.Pos = new Vector2((mouseWorldPos.X - miniMapArea.X) * (float)(mapWidth * WORLD_TEXTURE_DIMENSIONS) / miniMapArea.Width, (mouseWorldPos.Y - miniMapArea.Y) * (float)(mapHeight * WORLD_TEXTURE_DIMENSIONS) / miniMapArea.Height);
                        }
                    }

                    Camera.UpdatePosition(currentMouseState);
                    userInterface.UpdatePositions(Camera.Pos);

#region TEST AREA
                    if (currentKeyboardState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A))
                    {
                        Variables.numberLives = 0;
                    }

                    if (currentKeyboardState.IsKeyDown(Keys.E))
                    {
                        new Enemy(testEnemy, 3, 0, 14, 2);
                        new Enemy(testEnemy, 3, 1, 14, 2);
                        new Enemy(testEnemy, 3, 2, 14, 2);
                        new Enemy(testEnemy, 3, 3, 14, 2);
                    }                   
#endregion
                    
                    Projectiles.UpdateProjectiles();
                    Enemy.UpdateEnemies();
                    Enemy.UpdateEnemyAnimation(gameTime);
                    Tower.CheckFire(gameTime);
                    

                    if (Variables.numberLives <= 0)
                    {
                        menuArea = new Rectangle((int)Camera.Pos.X - (menuButton.Width + 20) / 2, (int)Camera.Pos.Y - (windowHeight / 3), menuButton.Width + 20, 0);
                        gameOver = new Menu(menuBackground, menuButton, font);
                        gameOver.CreateButton("Try Again?", "Restarts Game");
                        gameOver.CreateButton("Exit", "Exit Game");
                        gameOver.UpdatePositions(menuArea);
                        gameState = GameState.GameOver;
                    }

                    break;
                #endregion

                #region case GameOver
                case GameState.GameOver:
                    tempButton = null;

                    menuDescription = gameOver.CheckHover(oldMouseState, currentMouseState, mouseWorldPos);

                    tempButton = gameOver.CheckClick(oldMouseState, currentMouseState, mouseWorldPos);
                    if (tempButton != null)
                    {
                        if (tempButton.Label == "Try Again?")
                            gameState = GameState.NewGame;
                        else if (tempButton.Label == "Exit")
                            this.Exit();
                    }
                    
                    break;
                #endregion

                #region case Menu
                case GameState.Menu:
                    menuArea = new Rectangle((int)Camera.Pos.X - (menuButton.Width + 20) / 2, (int)Camera.Pos.Y - (windowHeight / 3), menuButton.Width + 20, 0);

                    mainMenu.UpdatePositions(menuArea);
                    resolutionMenu.UpdatePositions(menuArea);

                    tempButton = null;                   

                    if (!mainMenu.isActive)
                    {
                        menuDescription = resolutionMenu.CheckHover(oldMouseState, currentMouseState, mouseWorldPos);

                        tempButton = resolutionMenu.CheckClick(oldMouseState, currentMouseState, mouseWorldPos);
                        if (tempButton != null)
                        {
                            if (tempButton.Label == "1024x768")
                            {
                                windowWidth = 1024;
                                windowHeight = 768;
                                gameState = GameState.ResolutionChange;
                            }
                            else if (tempButton.Label == "1366x768")
                            {
                                windowWidth = 1366;
                                windowHeight = 768;
                                gameState = GameState.ResolutionChange;
                            }
                            else if (tempButton.Label == "1440x810")
                            {
                                windowWidth = 1440;
                                windowHeight = 810;
                                gameState = GameState.ResolutionChange;
                            }
                            else if (tempButton.Label == "1920x1080")
                            {
                                windowWidth = 1920;
                                windowHeight = 1080;
                                gameState = GameState.ResolutionChange;
                            }
                            else if (tempButton.Label == "Back")
                            {
                                resolutionMenu.isActive = false;
                                mainMenu.isActive = true;
                            }
                            else if (tempButton.Label == "Go Windowed")
                            {
                                graphics.IsFullScreen = false;
                                graphics.ApplyChanges();
                            }
                            else if (tempButton.Label == "Go Fullscreen")
                            {
                                graphics.IsFullScreen = true;
                                graphics.ApplyChanges();
                            }
                        }

                        if (currentKeyboardState.IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyUp(Keys.Escape))
                        {
                            resolutionMenu.isActive = false;
                            mainMenu.isActive = true;
                            oldKeyboardState = currentKeyboardState;
                        }
                    }

                    if (mainMenu.isActive)
                    {
                        currentKeyboardState = Keyboard.GetState();

                        if (currentKeyboardState.IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyUp(Keys.Escape))
                        {
                            gameState = GameState.Playing;
                        }

                        menuDescription = mainMenu.CheckHover(oldMouseState, currentMouseState, mouseWorldPos);

                        tempButton = mainMenu.CheckClick(oldMouseState, currentMouseState, mouseWorldPos);
                        if (tempButton != null)
                        {

                            if (tempButton.Label == "Resume Game")
                                gameState = GameState.Playing;
                            if (tempButton.Label == "Restart Game")
                                gameState = GameState.NewGame;
                            if (tempButton.Label == "Change Resolution")
                            {
                                mainMenu.isActive = false;
                                resolutionMenu.isActive = true;
                            }
                            if (tempButton.Label == "Exit")
                                this.Exit();
                        }
                    }
                    
                    break;
                #endregion

                #region case ResolutionChange
                case GameState.ResolutionChange:     
                    graphics.PreferredBackBufferWidth = windowWidth;
                    graphics.PreferredBackBufferHeight = windowHeight;
                    graphics.ApplyChanges();

                    viewport = new Viewport(0, 0, windowWidth, windowHeight);
                    Vector2 tempPos = Camera.Pos;
                    Camera = new Camera2d(viewport, mapWidth * WORLD_TEXTURE_DIMENSIONS, mapHeight * WORLD_TEXTURE_DIMENSIONS, 1f);
                    Camera.Pos = tempPos;

                    interfaceVerticalScale = (float)windowHeight / 4 / miniMap.Height;
                    interfaceHorizontalScale = (float)windowWidth / DEFAULT_WINDOW_WIDTH;

                    InterfaceItem tempHero = userInterface.heroThumb;
                    userInterface = new UserInterface(windowWidth, windowHeight, interfaceVerticalScale, interfaceHorizontalScale, mapWidth * WORLD_TEXTURE_DIMENSIONS, mapHeight * WORLD_TEXTURE_DIMENSIONS);

                    UserInterface.interfaceItems.Clear();
                    userInterface.CreateInterfaceItems(miniMap, miniMapOverlay, miniMapBorder, commandsBorder, commandsBackground, informationBorder, buildSellButtons);
                    
                    if (building)
                    {
                        UserInterface.interfaceItems.Remove(userInterface.buildCommand);
                    }

                    userInterface.heroThumb = tempHero;
                    userInterface.heroThumb.HorizontalScale = interfaceHorizontalScale;
                    userInterface.heroThumb.VerticalScale = interfaceVerticalScale;
                    UserInterface.interfaceItems.Add(tempHero);
                    
                    foreach (InterfaceItem item in UserInterface.buildTowerThumbs)
                    {
                        item.HorizontalScale = interfaceHorizontalScale;
                        item.VerticalScale = interfaceVerticalScale;
                    }

                    userInterface.UpdatePositions(Camera.Pos);

                    gameState = GameState.Menu;

                    break;
                #endregion
            }

            oldKeyboardState = currentKeyboardState;
            oldMouseState = currentMouseState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Camera.GetTransformation());

            map.Draw(spriteBatch, mapTexture);
            userInterface.Draw(spriteBatch);
            spriteBatch.DrawString(font, Variables.numberLives.ToString(), new Vector2(userInterface.informationBorder.Position.X + (225 * interfaceHorizontalScale), userInterface.informationBorder.Position.Y + (3 * interfaceVerticalScale)), Color.White, 0f, Vector2.Zero, new Vector2(interfaceHorizontalScale, interfaceVerticalScale), SpriteEffects.None, 0.01f);
            spriteBatch.DrawString(font, Variables.gold.ToString(), new Vector2(userInterface.informationBorder.Position.X + (311 * interfaceHorizontalScale), userInterface.informationBorder.Position.Y + (3 * interfaceVerticalScale)), Color.White, 0f, Vector2.Zero, new Vector2(interfaceHorizontalScale, interfaceVerticalScale), SpriteEffects.None, 0.01f);
            spriteBatch.DrawString(font, Variables.lumber.ToString(), new Vector2(userInterface.informationBorder.Position.X + (410 * interfaceHorizontalScale), userInterface.informationBorder.Position.Y + (3 * interfaceVerticalScale)), Color.White, 0f, Vector2.Zero, new Vector2(interfaceHorizontalScale, interfaceVerticalScale), SpriteEffects.None, 0.01f);

            Tower.DrawTowers(spriteBatch, towers);
            Enemy.DrawEnemies(spriteBatch);
            Projectiles.DrawProjectiles(spriteBatch);

            switch (gameState)
            {
                #region case DifficultySelect
                case GameState.DifficultySelect:
                    spriteBatch.Draw(screenOverlay, new Rectangle((int)Camera.Pos.X - windowWidth / 2, (int)Camera.Pos.Y - windowHeight / 2, windowWidth, windowHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
                    difficultySelect.Draw(spriteBatch);
                    //spriteBatch.Draw(menuBackground, new Rectangle(difficultySelect.MenuArea.X, difficultySelect.MenuArea.Y - (int)font.MeasureString(menuDescription).Y, (int)font.MeasureString(menuDescription).X, (int)font.MeasureString(menuDescription).Y), Color.Thistle);
                    spriteBatch.DrawString(font, menuDescription, new Vector2(difficultySelect.MenuArea.X, difficultySelect.MenuArea.Y - font.MeasureString(menuDescription).Y), Color.White);

                    break;
                #endregion

                #region case HeroSelect
                case GameState.HeroSelect:
                    spriteBatch.Draw(screenOverlay, new Rectangle((int)Camera.Pos.X - windowWidth / 2, (int)Camera.Pos.Y - windowHeight / 2, windowWidth, windowHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
                    heroSelect.Draw(spriteBatch);
                    spriteBatch.DrawString(font, menuDescription, new Vector2(heroSelect.MenuArea.X, heroSelect.MenuArea.Y - font.MeasureString(menuDescription).Y), Color.White);

                    break;
                #endregion

                #region case Playing
                case GameState.Playing:
                    if (building && towerSelected)
                    {
                        if (buildable)
                        {
                            spriteBatch.Draw(miniMapOverlay, new Rectangle((int)buildCordinates.X * WORLD_TEXTURE_DIMENSIONS, (int)buildCordinates.Y * WORLD_TEXTURE_DIMENSIONS, WORLD_TEXTURE_DIMENSIONS * 2, WORLD_TEXTURE_DIMENSIONS * 2), null, Color.DarkSeaGreen, 0f, Vector2.Zero, SpriteEffects.None, 0.7f);
                        }
                        else
                            spriteBatch.Draw(miniMapOverlay, new Rectangle((int)buildCordinates.X * WORLD_TEXTURE_DIMENSIONS, (int)buildCordinates.Y * WORLD_TEXTURE_DIMENSIONS, WORLD_TEXTURE_DIMENSIONS * 2, WORLD_TEXTURE_DIMENSIONS * 2), null, Color.Red, 0f, Vector2.Zero, SpriteEffects.None, 0.7f);

                        spriteBatch.Draw(waitingToBuildTowers, new Rectangle((int)buildCordinates.X * WORLD_TEXTURE_DIMENSIONS, (int)buildCordinates.Y * WORLD_TEXTURE_DIMENSIONS, WORLD_TEXTURE_DIMENSIONS * 2, WORLD_TEXTURE_DIMENSIONS * 2), new Rectangle(int.Parse(Tower.Towers[buildingTowerIndex][7]), int.Parse(Tower.Towers[buildingTowerIndex][8]), 64, 64), Color.DarkSeaGreen, 0f, Vector2.Zero, SpriteEffects.None, 0.6f);
                    }

                    break;
                #endregion

                #region case Menu
                case GameState.Menu:
                    spriteBatch.Draw(screenOverlay, new Rectangle((int)Camera.Pos.X - windowWidth / 2, (int)Camera.Pos.Y - windowHeight / 2, windowWidth, windowHeight), null,  Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
                    if (mainMenu.isActive)
                    {
                        mainMenu.Draw(spriteBatch);
                        spriteBatch.DrawString(font, menuDescription, new Vector2(mainMenu.MenuArea.X, mainMenu.MenuArea.Y - font.MeasureString(menuDescription).Y), Color.White);
                    }
                    else if (resolutionMenu.isActive)
                    {
                        resolutionMenu.Draw(spriteBatch);
                        spriteBatch.DrawString(font, menuDescription, new Vector2(resolutionMenu.MenuArea.X, resolutionMenu.MenuArea.Y - font.MeasureString(menuDescription).Y), Color.White);
                    }

                    break;
                #endregion

                #region case GameOver
                case GameState.GameOver:
                    spriteBatch.Draw(screenOverlay, new Rectangle((int)Camera.Pos.X - windowWidth / 2, (int)Camera.Pos.Y - windowHeight / 2, windowWidth, windowHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
                    gameOver.Draw(spriteBatch);
                    spriteBatch.DrawString(font, "Game Over", new Vector2(windowWidth / 2 - font.MeasureString("Game Over").X / 2, 0), Color.White);
                    spriteBatch.DrawString(font, menuDescription, new Vector2(gameOver.MenuArea.X, gameOver.MenuArea.Y - font.MeasureString(menuDescription).Y), Color.White);

                    break;
                #endregion
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
