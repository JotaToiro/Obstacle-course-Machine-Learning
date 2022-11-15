using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBaseMap : MonoBehaviour
{
    [SerializeField]
    private GameObject cubeShape;
    [SerializeField]
    private GameObject HexagonShape;
    [SerializeField]
    private GameObject TriangleShape;
    [SerializeField]
    private BlockShape blockShape;

    private List<AnimationTrigger> animationTriggers;

    [SerializeField]
    private int Xsize = 20;
    [SerializeField]
    private int Ysize = 30;

    [SerializeField]
    private float shapeSize;

    public bool isEdit;


    void Start()
    {
        
    }

    public void GenerateMap()
    {
        animationTriggers = new List<AnimationTrigger>();

        GameObject map = null;
        switch (blockShape)
        {
            case BlockShape.Cube:
                map = generateCubeMap();
                break;
            case BlockShape.Hexagon:
                map = generateHexagonMap();
                break;
            case BlockShape.Triangle:
                map = generateTriangleMap();
                break;
        }
        if (map != null)
        {
            map.AddComponent<MapInitializer>();
            GetComponent<PlaceSpawnPoint>().map = map.GetComponent<MapInitializer>();
        }
    }

    private void Update()
    {
        if (isEdit)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                resetAllTilesUp();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                resetAllTilesDown();
            }
        }
    }

    private GameObject generateCubeMap()
    {
        GameObject map = new GameObject("Map");

        for(int y = 0; y < Ysize; y++)
        {
            for(int x = 0; x < Xsize; x++)
            {
                GameObject shape = Instantiate(cubeShape, new Vector3(x * shapeSize, 0, y * shapeSize), Quaternion.identity);
                shape.transform.parent = GameObject.Find("Map").transform;
                shape.transform.localScale = new Vector3(shapeSize, shapeSize, shapeSize);
                animationTriggers.Add(shape.GetComponent<AnimationTrigger>());
            }
        }

        return map;
    }

    private GameObject generateHexagonMap()
    {
        GameObject map = new GameObject("Map");

        float differentColumnXIncrement = shapeSize + shapeSize/2;
        float differentColumnYIncrement = Mathf.Sqrt(shapeSize - Mathf.Pow(shapeSize/2, 2));
        float sameColumnIncrement = differentColumnYIncrement * 2;

        float x = 0;
        float y = 0;
        for(int i = 0; i < Ysize/2; i ++)
        {
            for(int j = 0; j < Xsize; j ++)
            {
                GameObject shape = Instantiate(HexagonShape, new Vector3(x, 0, y), Quaternion.identity);
                shape.transform.parent = GameObject.Find("Map").transform;
                shape.transform.localScale = new Vector3(shapeSize, shapeSize, shapeSize);
                animationTriggers.Add(shape.GetComponent<AnimationTrigger>());
                x += sameColumnIncrement;
            }
            x = 0;
            y += differentColumnXIncrement * 2;
        }


        x = 0;
        y = differentColumnXIncrement;
        for (int i = 0; i < Ysize/2; i++)
        {
            for (int j = 0; j < Xsize; j++)
            {
                GameObject shape = Instantiate(HexagonShape, new Vector3(x + differentColumnYIncrement, 0, y), Quaternion.identity);
                shape.transform.parent = GameObject.Find("Map").transform;
                shape.transform.localScale = new Vector3(shapeSize, shapeSize, shapeSize);
                animationTriggers.Add(shape.GetComponent<AnimationTrigger>());
                x += sameColumnIncrement;
            }
            x = 0;
            y += differentColumnXIncrement * 2;
        }

        return map;
    }

    private GameObject generateTriangleMap()
    {
        GameObject map = new GameObject("Map");

        float lineOfset = Mathf.Sqrt(shapeSize - Mathf.Pow(shapeSize / 2, 2));
        float columnOfset = shapeSize / 2;
        float massCenter = Mathf.Tan(30 * Mathf.Deg2Rad) * shapeSize / 2;

        float x = 0;
        float y = 0;
        for(int i = 0; i < Ysize; i++)
        {
            for(int j = 0; j < Xsize; j++)
            {
                GameObject shape = Instantiate(TriangleShape, new Vector3(x, 0, y), Quaternion.Euler(0, 90, 0));
                shape.transform.parent = GameObject.Find("Map").transform;
                shape.transform.localScale = new Vector3(shapeSize, shapeSize, shapeSize);
                animationTriggers.Add(shape.GetComponent<AnimationTrigger>());

                if ((j + i)%2 != 0)
                {
                    shape.transform.position = new Vector3(shape.transform.position.x + (massCenter - (lineOfset/2)), shape.transform.position.y, shape.transform.position.z);
                    shape.transform.eulerAngles = new Vector3(0, shape.transform.eulerAngles.y + 180, 0);
                }
                else
                {
                    shape.transform.position = new Vector3(shape.transform.position.x - (massCenter - (lineOfset / 2)), shape.transform.position.y, shape.transform.position.z);
                }
                x += lineOfset;
            }
            x = 0;
            y += columnOfset;
        }

        return map;
    }


    private void resetAllTilesUp()
    {
        foreach (AnimationTrigger animationTrigger in animationTriggers)
        {
            animationTrigger.playAnimationUp();
        }
    }

    private void resetAllTilesDown()
    {
        foreach (AnimationTrigger animationTrigger in animationTriggers)
        {
            animationTrigger.playAnimationDown();
        }
    }
}


public enum BlockShape
{
    Cube,
    Hexagon,
    Triangle
}

