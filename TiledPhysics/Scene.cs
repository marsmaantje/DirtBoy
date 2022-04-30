using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;
using GXPEngine.Core;
using Layers;
using Objects;


public class Scene : Pivot
{
    #region variables
    public Player player;
    TiledObject playerObj;
    TiledLoader parser;
    public UI ui;

    //camera movement
    float smoothSpeed = 0.1f;
    GameObject currentLookTarget;
    #endregion

    #region constructor
    public Scene(UI ui)
    {
        this.ui = ui;
    }
    #endregion

    #region Update
    public void Update()
    {
        //if the scene has something to focus on, update its position and scale to do so
        if (currentLookTarget != null)
        {
            Vector2 globalPosition = currentLookTarget.TransformPoint(0, 0);
            Vector2 relativePosition = this.InverseTransformPoint(globalPosition.x, globalPosition.y);
            Vector2 globalTargetScale = currentLookTarget.InverseTransformDirection(1, 1);
            Vector2 localTargetScale = this.TransformDirection(globalTargetScale.x, globalTargetScale.y);
            float deltaX = (game.width / 2f) - relativePosition.x * this.scaleX - this.x;
            float deltaY = (game.height / 2f) - relativePosition.y * this.scaleY - this.y;
            float deltaScaleX = (-this.scaleX + localTargetScale.x) * smoothSpeed;
            float deltaScaleY = (-this.scaleY + localTargetScale.y) * smoothSpeed;

            //update position
            Move(deltaX * smoothSpeed, deltaY * smoothSpeed);

            //update scale
            this.scaleX += deltaScaleX;
            this.scaleY += deltaScaleY;

            //fix position update caused by scale change
            this.x += this.x * deltaScaleX / scaleX;
            this.y += this.y * deltaScaleY / scaleY;
        }
    }
    #endregion

    #region methods
    /// <summary>
    /// Will force the camera to teleport to the target, disregarding the smoothing used in Update()
    /// </summary>
    public void jumpToTarget()
    {
        //if the scene has something to focus on, update its position and scale to do so
        if (currentLookTarget != null)
        {
            Vector2 globalPosition = currentLookTarget.TransformPoint(0, 0);
            Vector2 relativePosition = this.InverseTransformPoint(globalPosition.x, globalPosition.y);
            Vector2 globalTargetScale = currentLookTarget.InverseTransformDirection(1, 1);
            Vector2 localTargetScale = this.TransformDirection(globalTargetScale.x, globalTargetScale.y);
            float deltaX = (game.width / 2f) - relativePosition.x * this.scaleX - this.x;
            float deltaY = (game.height / 2f) - relativePosition.y * this.scaleY - this.y;
            float deltaScaleX = -this.scaleX + localTargetScale.x;
            float deltaScaleY = -this.scaleY + localTargetScale.y;
            Move(deltaX, deltaY);

            this.scaleX += deltaScaleX;
            this.scaleY += deltaScaleY;
        }
    }


    /// <summary>
    /// Will initialize the TiledLoader for mapParsing
    /// </summary>
    /// <param name="mapFile">The name of the tiled map to parse/load</param>
    public void loadMapFile(string mapFile)
    {
        parser = new TiledLoader(mapFile, this);
        parser.OnObjectCreated += objectCreateCallback;
    }


    /// <summary>
    /// Will load the level and set up all the different objects for the player to play
    /// </summary>
    public void createLevel()
    {
        //load all image layers with their respective parallax
        parser.addColliders = false;
        int layerCount = 0;
        if (parser.map.ImageLayers != null)
        {
            layerCount = parser.map.ImageLayers.Length;

            for (int i = 0; i < layerCount; i++)
            {
                float parallaxX = parser.map.ImageLayers[i].parallaxx;
                float parallaxY = parser.map.ImageLayers[i].parallaxy;
                Layers.ImageLayer imageLayer = new Layers.ImageLayer(parser, parser.map.ImageLayers[i], this, parallaxX, parallaxY);
                if (parser.map.ImageLayers[i].GetBoolProperty("isOnTop", false))
                    ui.AddChild(imageLayer);
                else
                    AddChild(imageLayer);
                //parser.rootObject = imageLayer;
                //parser.LoadImageLayers(new int[] { i });
            }
        }

        //instead of loading all tile layers, load each seperately because we dont want collision everywhere

        addTileLayer(0, 1 + layerCount, false);
        addTileLayer(1, 2 + layerCount, true);
        addTileLayer(2, 3 + layerCount, false);

        parser.rootObject = this;
        parser.AddManualType(new string[] { "Player" });
        parser.autoInstance = true;
        parser.addColliders = true;
        parser.LoadObjectGroups();

        foreach (GameObject child in this.GetChildren())
        {
            if (child is CustomObject)
            {
                ((CustomObject)child).initialize(this);
            }
            if (child is Player)
            {
                ((Player)child).initialize(this);
            }
        }

        if (FindObjectOfType<Player>() != null)
            SetChildIndex(FindObjectOfType<Player>().parent, GetChildCount());//render the player on top of all other gameObjects
        else
            this.SetXY(0, 0);
    }

    /// <summary>
    /// Will add a tile Layer to the game
    /// </summary>
    /// <param name="index">The index of the layer to load</param>
    /// <param name="offset">The z offset of the layer, higher is in front of lower numbers</param>
    /// <param name="generateColliders"> Whether to generate colliders so the player can collide with the tiles</param>
    public void addTileLayer(int index, int offset, bool generateColliders)
    {
        if (index >= parser.NumTileLayers) return; //stop the code if the tile layer does not exist
        TileLayer newLayer = new TileLayer(this);
        parser.addColliders = generateColliders;
        parser.rootObject = newLayer;
        parser.LoadTileLayers(new int[] { index });
        newLayer.index = index;
        this.SetChildIndex(newLayer, offset);
    }

    /// <summary>
    /// Sets the object the camera should look at
    /// </summary>
    /// <param name="target">the object to focus</param>
    public void setLookTarget(GameObject target)
    {
        if (target != null)
            currentLookTarget = target;
    }

    /// <summary>
    /// Method used to instantiate custom objects we dont want the tiledLoader to create
    /// </summary>
    /// <param name="sprite">Sprite of the object tiled created</param>
    /// <param name="obj">tiledObject of the object tiled created</param>
    public void objectCreateCallback(Sprite sprite, TiledObject obj)
    {
        if (obj.Type == "Player" && player == null) //instantiate player if there is none thus far
        {
            playerObj = obj;
            player = new Player(obj);
            AddChild(player);
            player.SetXY(obj.X, obj.Y);
        }
    }
    #endregion
}