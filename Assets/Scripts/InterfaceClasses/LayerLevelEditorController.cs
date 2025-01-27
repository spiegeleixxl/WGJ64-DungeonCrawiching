﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LayerLevelEditorController : LayerController
{

    public List<TileController> possibleTiles;
    private List<GameObject> tileObjectPrefabs;
    public TileController selectedTile;
    public UnityEngine.UI.Image tilePreview;
    public UnityEngine.UI.Text layerText;
    public int selectedLayer;

    public int selectedTileNumber
    {
        get
        {
            return _selectedTileNumber;
        }
        set
        {

            if (value < possibleTiles.Count && value >= 0)
                _selectedTileNumber = value;
            selectedTile = possibleTiles[_selectedTileNumber];


            tilePreview.sprite = tileObjectPrefabs[_selectedTileNumber].GetComponent<TileController>().spriteTexture;

        }
    }
    private int _selectedTileNumber = 0;
    public PlayerScript playerRef;
    public GameManager gm;

    // Use this for initialization
    void Start()
    {
        gm = GameManager.instance;
        playerRef = gm.playerScript;

        tileObjectPrefabs = new List<GameObject>();
        foreach (TileController t in possibleTiles)
        {
            GameObject o = GameObject.Instantiate(t, this.gameObject.transform).gameObject;
            //o.SetActive(false);

            tileObjectPrefabs.Add(o);
        }
        selectedTileNumber = 0;
        selectedLayer = 0;


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (playerRef.inMenu) return;
        if (!playerRef.cheatMode) return;
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Y))
        {
            string saveFile = UnityEditor.EditorUtility.SaveFilePanel("Save current map", "", "map", "json");
            if (saveFile == "")
                return;

            StaticUtilitiesFunction.SaveMapToFile(saveFile);
            Debug.Log("File successfully saved to: " + saveFile);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            string loadFile = UnityEditor.EditorUtility.OpenFilePanel("Load map", "", "json");
            if (loadFile == "")
                return;

            StaticUtilitiesFunction.LoadMapFromFile(loadFile, tileControllerPrefab, true);
        }
#endif
        if (Input.GetKey(KeyCode.Plus))
            Camera.main.orthographicSize += 1;
        if (Input.GetKey(KeyCode.Minus))
            Camera.main.orthographicSize -= 1;
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetAxis("Mouse ScrollWheel") >= 0.99f)
            selectedTileNumber++;
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxis("Mouse ScrollWheel") <= -0.99f)
            selectedTileNumber--;
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            selectedLayer++;
            if (GameManager.instance.layers.Count <= selectedLayer)
            {
                LayerController l = GameObject.Instantiate(GameManager.instance.LayerPrefab, GameManager.instance.GameGrid.transform).GetComponent<LayerController>();
                GameManager.instance.layers.Add(l);
                GameManager.instance.initLayer(GameManager.instance.layers.Count - 1, true);
            }
            layerText.text = "Layer: " + selectedLayer;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
            if (selectedLayer > 0)
            {
                selectedLayer--;
                layerText.text = "Layer: " + selectedLayer;
            }


        if (Input.GetButtonDown("Fire1"))
        {
            if (!playerRef)
                playerRef = gm.playerScript;
            if (!playerRef)
                return;


            GameManager.instance.layers[selectedLayer].tiles[Coordinate2D.Is(playerRef.mapX, playerRef.mapY)].GettingOverwritten(tileObjectPrefabs[selectedTileNumber].GetComponent<TileController>());
        }

    }
}
