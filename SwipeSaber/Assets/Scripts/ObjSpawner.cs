using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSpawner : MonoBehaviour
{
    public GameObject blockPrefab;

    public Transform gameObjPoolParent;

    public Transform spawnPointTransform;

    PhaseDetails currentPhase;

    Dictionary<ObstacleType, float> obstaclesAllowed;
    List<Direction> directionsAllowedInPhase = new List<Direction>();
   
    List<Direction> pastDirections = new List<Direction>();
    List<Transform> pastSpawnPoints = new List<Transform>();
    List<Transform> pastObjs = new List<Transform>();

    GameObject[] gameObjPool;
    List<Transform> spawnPoints = new List<Transform>();

    float chanceDeduction = 2.0f;
    float blockSpeed;

    float doubleSidedSpawnChance;
    float obstacleChance;
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
            firstSpawnPoint = ChooseSpawnPoint(pastSpawnPoints, spawnPoints);

            if (obstaclesAllowed.Count != 0)
            {
                if (CalculateChance(obstacleChance))
                {
                    CreateBlock(firstSpawnPoint, true);
                }
            }
            else
            {
                CreateBlock(firstSpawnPoint);
                objSpawned = true;
            }

            if (pastSpawnPoints.Count > 5)
            {
                pastSpawnPoints.RemoveAt(pastSpawnPoints.Count - 1);
                pastSpawnPoints.Add(firstSpawnPoint);
            }
        }

        Transform secondSpawnPoint = null;

        if (firstSpawnPoint != null)
        {
            List<Transform> leftOverSpawnPoints = new List<Transform>(spawnPoints);
            leftOverSpawnPoints.Remove(firstSpawnPoint);
            secondSpawnPoint = ChooseSpawnPoint(pastSpawnPoints, leftOverSpawnPoints);
            CreateBlock(secondSpawnPoint);
            objSpawned = true;
        }
        else
        {
            secondSpawnPoint = ChooseSpawnPoint(pastSpawnPoints, spawnPoints);
            CreateBlock(secondSpawnPoint);
            objSpawned = true;
        }

        if (pastSpawnPoints.Count > 5)
        {
            pastSpawnPoints.RemoveAt(pastSpawnPoints.Count - 1);
            pastSpawnPoints.Add(secondSpawnPoint);
        }

        return objSpawned;        
    }

    void CreateBlock(Transform spawnPos, bool isObstacle = false)
    {
        Direction blockDir = ChooseBlockDirection(pastDirections, directionsAllowedInPhase);

        GameObject gameObjToSpawn = RetrieveGameObject(gameObjPool);

        if (gameObjToSpawn != null)
        {
            gameObjToSpawn.transform.localPosition = spawnPos.position;

            if (!isObstacle)
            {
                ObstacleType newObstacle = ChooseObstacle(obstaclesAllowed);

                gameObjToSpawn.GetComponent<Block>().Init(blockDir, blockSpeed, true, newObstacle);
            }
            else
            {
                gameObjToSpawn.GetComponent<Block>().Init(blockDir, blockSpeed, true);

            }
            gameObjToSpawn.SetActive(true);
        }
        else
        {
            Debug.Log("No GameObject avaible in pool");
        }
    }

    public void PhaseChange(PhaseDetails newPhase)
    {
        directionsAllowedInPhase.Clear();
        directionsAllowedInPhase = new List<Direction>(newPhase.AllowedDirectoins);
        blockSpeed = newPhase.BlockSpeed;
        obstacleChance = newPhase.ObstacleChance;
        doubleSidedSpawnChance = newPhase.DoubleSidedSpawnChance;
        obstaclesAllowed = newPhase.AllowedObjects;
    }

    void FillPool()
    {
        gameObjPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            gameObjPool[i] = Instantiate(blockPrefab, gameObjPoolParent);
            gameObjPool[i].name = gameObjPool[i].name + " " + i;
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


    ObstacleType ChooseObstacle(Dictionary<ObstacleType, float> allowedObstacles)
    {
        ObstacleType newDirection = ObstacleType.Null;

        //turn chances into a direction float dictionary 
        //Dictionary<Direction, float> chances = new Dictionary<Direction, float>(allowedDirections.Count);

        float highestPercentage = 0;

        foreach (var item in allowedObstacles)
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
