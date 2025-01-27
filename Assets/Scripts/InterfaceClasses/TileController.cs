﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum TileTriggers
{
    Damage,
    Move,
    Others,
    SpawnPoint,
    NextFloorStairs,
    Item,
    Trap
}

public class TileController : MonoBehaviour
{
    public Sprite spriteTexture
    {
        get
        {
            fetchSpriteRenderer();
            return spriteRenderer.sprite;
        }
        set
        {
            fetchSpriteRenderer();
            spriteRenderer.sprite = value;
            if (spriteRenderer)
                spriteName = spriteRenderer.sprite.name;
        }
    }
    public SpriteRenderer spriteRenderer;

    public string spriteName
    {
        get
        {
            fetchSpriteRenderer();
            _spriteName = spriteRenderer.sprite.name;
            return spriteRenderer.sprite.name;
        }
        set
        {
            _spriteName = value;
            fetchSpriteRenderer();
            spriteRenderer.sprite = StaticResourceProvider.GetSpriteObject(value);
        }
    }
    public string _spriteName;
    public bool walkableByPlayer;
    public bool walkableByEnemy;
    public bool walkableByGlitch;
    /*public delegate void trigger();
    public event trigger TileTouchTrigger;*/
    public bool isTriggering;
    public TileTriggers triggerType;
    public object triggerParameters;

    // Use this for initialization
    void Start()
    {
        fetchSpriteRenderer();

        /*if (spriteRenderer)
            spriteName = spriteRenderer.sprite.name; */
    }

    void fetchSpriteRenderer()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        if (!spriteRenderer)
            Debug.Log("Could not create a spriteRenderer?!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup()
    {
        spriteName = _spriteName;
    }

    public void GettingOverwritten(TileController src)
    {
        walkableByEnemy = src.walkableByEnemy;
        walkableByGlitch = src.walkableByGlitch;
        walkableByPlayer = src.walkableByPlayer;

        spriteTexture = src.spriteTexture;
        tag = src.tag;
        triggerType = src.triggerType;
        isTriggering = src.isTriggering;
        ItemScript iscr = src.GetComponent<ItemScript>();
        if (iscr)
        {
            ItemScript myiscr = this.GetComponent<ItemScript>();
            if (!myiscr)
                myiscr = this.gameObject.AddComponent<ItemScript>();

            myiscr.itemType = iscr.itemType;
            myiscr.itemEffect = iscr.itemEffect;
            myiscr.itemSprite = iscr.itemSprite;
            myiscr.itemDisplayName = iscr.itemDisplayName;
            myiscr.itemEffectValue = iscr.itemEffectValue;

        }
    }


}
