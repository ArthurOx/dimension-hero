using UnityEngine;
using System.Collections;
public class SpawnCollectables : MonoBehaviour
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
        SpawnCoin();
    }

    private void SpawnCoin()
    {

        GameObject collectables = (GameObject)Instantiate(level.collectables[0]);

        collectables.transform.position = new Vector3(transform.localScale.x / 2, transform.position.y + RandomYAxisPosition(), 0);
        collectables.transform.localScale = new Vector3(collectables.transform.localScale.x, collectables.transform.localScale.y, collectables.transform.localScale.z);
        collectables.GetComponent<MoveRelatively>().level = level;
        collectables.GetComponent<DestroyOnLeftEdge>().ground = gameObject;

        float minimumSpawnDistance = collectables.GetComponent<Collider2D>().bounds.size.x * level.mainSpeed / 8.0f + collectables.GetComponent<ObstacleStuff>().minGap * 0.2f;
        spawnAt = minimumSpawnDistance * (1 + Random.value * 0.5f);
        distance = 0f;
    }

    /**
	 * returns int within -4 to 4
     */
    private int RandomYAxisPosition()
    {
        System.Random ran = new System.Random();
        int power = ran.Next(0, 8) - 4;
        return power;
}
}