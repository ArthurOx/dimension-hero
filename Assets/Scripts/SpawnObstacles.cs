using UnityEngine;
using System.Collections;
public class SpawnObstacles : MonoBehaviour
{
    public Level level = null;
    private float distance = 0;
    private float spawnAt = 0;
    private string[] obstacleHistory = new string[] { null, null };
    private int obstacleHistoryIndex = 0;

    void Start()
    {
        distance = 0f;
        spawnAt = 0f;
    }

    void Update()
    {
        distance += Time.deltaTime * level.mainSpeed;
        if (distance < spawnAt)
        {
            return;
        }
        SpawnObstacle();
    }

    private void SpawnObstacle()
    {
        int obstacleIndex;
        ObstacleStuff obstacleStuff = null;
        do
        {
            obstacleIndex = Random.Range(0, level.obstacles.Length);
            obstacleStuff = level.obstacles[obstacleIndex].GetComponent<ObstacleStuff>();
        } while ((obstacleStuff.uniqueName == obstacleHistory[0] && obstacleStuff.uniqueName == obstacleHistory[1]) || obstacleStuff.minSpeed > level.mainSpeed);
        obstacleHistory[obstacleHistoryIndex] = obstacleStuff.uniqueName;
        obstacleHistoryIndex = (obstacleHistoryIndex + 1) % 2;

        GameObject obstacle = (GameObject)Instantiate(level.obstacles[obstacleIndex]);

        obstacle.transform.position = new Vector3(transform.localScale.x / 2, transform.position.y, 0);
        obstacle.transform.localScale = new Vector3(obstacle.transform.localScale.x, RandomSide() * obstacle.transform.localScale.y, obstacle.transform.localScale.z);
        obstacle.GetComponent<MoveRelatively>().level = level;
        obstacle.GetComponent<DestroyOnLeftEdge>().ground = gameObject;

        float minimumSpawnDistance = obstacle.GetComponent<Collider2D>().bounds.size.x * level.mainSpeed / 8.0f + obstacle.GetComponent<ObstacleStuff>().minGap * 0.2f;
        spawnAt = minimumSpawnDistance * (1 + Random.value * 0.5f);
        distance = 0f;
    }

    /**
	 * returns -1 or 1 randomly
     */
    private int RandomSide()
    {
		if(Random.Range(0.0f, 1.0f) < 0.5f) {
			return -1;
		} else {
			return 1;
		}
    }
}
