using UnityEngine;

public class GroundLooper : MonoBehaviour
{
    [Tooltip("Игрок (его Transform)")]
    public Transform player;

    [Tooltip("Плитки пола, которые будем перекидывать вперёд")]
    public Transform[] groundTiles;

    [Tooltip("Длина одной плитки пола в мировых координатах")]
    public float tileLength = 200f;

    private int tilesCount;

    private void Start()
    {
        tilesCount = groundTiles.Length;
    }

    private void Update()
    {
        if (player == null || groundTiles == null || groundTiles.Length == 0)
            return;

        foreach (var tile in groundTiles)
        {
            // если игрок убежал вперёд дальше, чем длина плитки
            if (player.position.z - tile.position.z > tileLength)
            {
                // перекидываем эту плитку вперёд за самую дальнюю
                tile.position += Vector3.forward * tileLength * tilesCount;
            }
        }
    }
}
