using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualsGameObjectList;

    private void Awake()
    {
        plateVisualsGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        GameObject plateGameObject = plateVisualsGameObjectList[plateVisualsGameObjectList.Count - 1];
        plateVisualsGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint.position, Quaternion.identity);

        float plateVisualOffsetY = 0.1f;
        plateVisualTransform.position += new Vector3(0f, plateVisualOffsetY * plateVisualsGameObjectList.Count, 0f);

        plateVisualsGameObjectList.Add(plateVisualTransform.gameObject);

    }
}
