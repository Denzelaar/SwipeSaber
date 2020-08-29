using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSpawner : MonoBehaviour
{
    public GameObject blockPrefab;

    public Transform gameObjPoolParent;

    public Transform spawnPointTransform;

    PhaseDetails currentPhase;

    List<Direction> directionsAllowedInPhase = new List<Direction>();

    List<Direction> pastDirections = new List<Direction>();
    List<Transform> pastSpawnPoints = new List<Transform>();
    List<Transform> pastObjs = new List<Transform>();

    GameObject[] gameObjPool;
    List<Transform> spawnPoints = new List<Transform>();

    float chanceDeduction = 2.0f;
    float blockSpeed;

    float doubleSidedSpawnChance;

    float obstChance;

    int poolSize = 5;


    // Start is called before the first frame update
    public void Init(int poolSize)
    {
        for (int i = 0; i < spawnPointTransform.childCount; i++)
        {
            if(spawnPointTransform.GetChild(i).gameObject.activeSelf == true)
            {
                spawnPoints.Add(spawnPointTransform.GetChild(i));
            }
        }

        this.poolSize = poolSize;
        FillPool();

    }

    public bool SpawnObject()
    {
        bool objSpawned = false;
        Transform firstSpawnPoint = null;
        if (CalculateChance(doubleSidedSpawnChance))
        {
            if (obstChance != 0)
            {
                if (CalculateChance(obstChance))
                {
                    //spawn Obsrtalcesc
                }
            }
            else
            {
                firstSpawnPoint = CreateBlock(spawnPoints);
                objSpawned = true;
            }
        }

        if(firstSpawnPoint != null)
        {
            List<Transform> leftOverSpawnPoints = new List<Transform>(spawnPoints);
            leftOverSpawnPoints.Remove(firstSpawnPoint);
            CreateBlock(leftOverSpawnPoints);
            objSpawned = true;
        }
        else
        {
            CreateBlock(spawnPoints);
            objSpawned = true;
        }

        return objSpawned;        
    }

    Transform CreateBlock(List<Transform> spawnPoints)
    {
        Transform spawnPos = ChooseSpawnPoint(pastSpawnPoints, spawnPoints);
        Direction blockDir = ChooseBlockDirection(pastDirections, directionsAllowedInPhase);

        GameObject gameObjToSpawn = RetrieveGameObject(gameObjPool);

        if (gameObjToSpawn != null)
        {
            gameObjToSpawn.transform.localPosition = spawnPos.position;
            gameObjToSpawn.GetComponent<Block>().Init(blockDir, blockSpeed, true);
            gameObjToSpawn.SetActive(true);
        }
        else
        {
            Debug.Log("No GameObject avaible in pool");
        }

        if(pastSpawnPoints.Count > 5)
        {
            pastSpawnPoints.RemoveAt(pastSpawnPoints.Count - 1);
            pastSpawnPoints.Add(spawnPos);
        }

        if(pastDirections.Count > 5)
        {
            pastDirections.RemoveAt(pastDirections.Count - 1);
            pastDirections.Add(blockDir);
        }


        return spawnPos;
    }

    public void PhaseChange(PhaseDetails newPhase)
    {
        directionsAllowedInPhase.Clear();
        directionsAllowedInPhase = new List<Direction>(newPhase.AllowedDirectoins);
        this.blockSpeed = newPhase.BlockSpeed;
        obstChance = newPhase.ObstacleChance;
        this.doubleSidedSpawnChance = newPhase.DoubleSidedSpawnChance;
    }

    void FillPool()
    {
        gameObjPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            gameObjPool[i] = Instantiate(blockPrefab, gameObjPoolParent);
            gameObjPool[i].SetActive(false);
        }
    }

    GameObject RetrieveGameObject(GameObject[] pool)
    {
        GameObject gameObjToSpawn = null;

        foreach (var item in pool)
        {
            if (item.activeSelf == false)
            {
                gameObjToSpawn = item;
                break;

            }
        }

        return gameObjToSpawn;
    }

    bool NextObjIsObstacle()
    {
        bool nextObjIsObstacle = false;


        return nextObjIsObstacle;
    }

    Direction ChooseBlockDirection(List<Direction> pastDirections, List<Direction> allowedDirections)
    {
        Direction newDirection = Direction.Null;


        //turn chances into a direction float dictionary 

        Dictionary<Direction, float> chances = new Dictionary<Direction, float>(allowedDirections.Count);

        foreach (var item in allowedDirections)
        {
            chances.Add(item, (1.0f / allowedDirections.Count));
        }

        foreach (var item in chances)
        {
            int i = 0;

            for (int u = 0; u < pastDirections.Count; u++)
            {
                if (pastDirections[u] == allowedDirections[i])
                {
                    chances[item.Key] -= chanceDeduction;

                    for (int p = 0; p < chances.Count; p++)
                    {
                        if (p != i)
                        {
                            chances[item.Key] += (chanceDeduction * (1 / chances.Count));
                        }
                    }
                }
            }

            i++;
        }

        float highestPercentage = 0;
        Direction highestChangeWinner = 0;

        foreach (var item in chances)
        {
            float random = Random.Range(0, item.Value);

            if (random > highestPercentage)
            {
                highestPercentage = random;
                newDirection = item.Key;
            }
        }

        return newDirection;
    }

    Transform ChooseSpawnPoint(List<Transform> lastPoints, List<Transform> availablepawnPoints)
    {
        Transform newSpawnPoint = null;

        Dictionary<Transform, float> chances = new Dictionary<Transform, float>(availablepawnPoints.Count);

        foreach (var item in availablepawnPoints)
        {
            chances.Add(item, 1.0f / availablepawnPoints.Count);
        }

        foreach (var item in chances)
        {
            int i = 0;
            for (int u = 0; u < lastPoints.Count; u++)
            {
                if (lastPoints[u].gameObject.transform.position == item.Key.position)
                {
                    chances[item.Key] -= chanceDeduction;

                    for (int p = 0; p < chances.Count; p++)
                    {
                        if (p != i)
                        {
                            chances[item.Key] += (chanceDeduction * (1 / chances.Count));
                        }
                    }
                }

            }
            i++;
        }

        float highestPercentage = 0;
        float highestChangeWinner = 0;

        foreach (var item in chances)
        {
            float random = Random.Range(0, item.Value);

            if (random > highestPercentage)
            {
                highestPercentage = random;
                newSpawnPoint = item.Key;
            }
        }

        return newSpawnPoint;
    }

    bool CalculateChance(float chance)
    {
        bool returnBool = false;
        float random = Random.Range(0, 1.0f);

        if (random < chance)
        {
            returnBool = true;
        }

        return returnBool;
    }
}
