﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    // Singleton
    public static LevelGenerator sharedInstance;

    // todos los bloques que están disponibles
    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();

    public Transform levelStartPoint;

    // bloques que hay en la escena
    public List<LevelBlock> currentBlocks = new List<LevelBlock>();

    public int initialBlockNum = 2;

    public LevelBlock firstBlock;
    
    private void Awake()
    {
        // Singleton, se accede a la clase a traves de LevelGenerator.sharedInstance
        sharedInstance = this;
    }

    private void Start()
    {
        GenerateInitialBlocks();
    }

    public void AddLevelBlock()
    {
        // numero random
        int randomIndex = Random.Range(0, allTheLevelBlocks.Count);

        // crea currentBlock que será de la clase LevelBlock  
        LevelBlock currentBlock;

        // La posición de spawn igualada a 0
        Vector3 spawnPosition = Vector3.zero;

        if(currentBlocks.Count == 0)
        {
            // instanciamos el primer bloque elegido
            currentBlock = (LevelBlock)Instantiate(firstBlock);
            currentBlock.transform.SetParent(this.transform, false);
            // si es el primer block, designamos que la posicion de inicio es levelStartPoint añadida previamente
            spawnPosition = levelStartPoint.position;
        }
        else
        {
            // lo instancia random desde los bloques disponibles
            currentBlock = Instantiate(allTheLevelBlocks[randomIndex]);
            // el bloque será hijo del LevelGenerator
            currentBlock.transform.SetParent(this.transform, false);
            // designamos la posición de inicio del block que sea la final del block anterior
            spawnPosition = currentBlocks[currentBlocks.Count - 1].exitPoint.position;
        }

        //Corrección para el spawn del bloque del nivel
        Vector3 correction = new Vector3(spawnPosition.x-currentBlock.startPoint.position.x,spawnPosition.y-currentBlock.startPoint.position.y,0);

        // asignamos la posicion del inicio del block a la posicion del block
        currentBlock.transform.position = correction;

        // añadimos el block a la lista de los que ya están generados
        currentBlocks.Add(currentBlock);
    }

    public void RemoveOldestLevelBlock()
    {
        Destroy(currentBlocks[0].gameObject);
        currentBlocks.Remove(currentBlocks[0]);
    }

    public void RemoveAllBlocks()
    {
        while (currentBlocks.Count > 0)
        {
            RemoveOldestLevelBlock();
        }
        Debug.Log(currentBlocks);
    }

    public void GenerateInitialBlocks()
    {
        for (int i = 0; i < initialBlockNum; i++)
        {
            AddLevelBlock();
        }
    }
}
