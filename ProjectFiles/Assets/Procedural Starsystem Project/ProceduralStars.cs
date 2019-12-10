using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralStars : MonoBehaviour
{
    public Vector2 minMaxSize;

    public bool useMusic = false;
    public GameObject[] starTypes;
    public int numPoints;
    [Range(0, 1)]
    public float turnFraction;
    [Range(0, 200)]
    public float starDistance;
    public float power;
    List<Vector3> Points = new List<Vector3>();
    List<GameObject> stars = new List<GameObject>();

    int blueStarPercentage = 75;

    int frameCount;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numPoints; i++)
        {
            float _dst = Mathf.Pow(i / (numPoints / -1f), power);
            float _angle = 2 * Mathf.PI * turnFraction * i;
            _dst = _dst * starDistance;
            float _x = _dst * Mathf.Cos(_angle) + starDistance;
            float _z = _dst * Mathf.Sin(_angle) + starDistance;
            Vector3 _spawnPos = new Vector3(_x, 0f, _z);
            Points.Add(_spawnPos);

            int _random = Random.Range(0, starTypes.Length);
            if(_random == 0)
            {
                int _random2 = Random.Range(0, 100);

                if(_random2 > blueStarPercentage)
                {
                    _random = Random.Range(1, starTypes.Length);
                }
            }
            GameObject _temp = Instantiate(starTypes[_random], _spawnPos, Quaternion.identity);
            float _randomScale = Random.Range(minMaxSize.x, minMaxSize.y);
            _temp.transform.localScale = new Vector3(_randomScale, _randomScale, _randomScale);
            _temp.transform.SetParent(this.transform);
            stars.Add(_temp);
           // Gizmos.DrawSphere(_spawnPos, 1f);
           // GameObject _temp = Instantiate(star, _spawnPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCount % 10 == 0)
        {
            if (useMusic)
                turnFraction = (AudioVisualizer.pitchValue - 1) / (0 - 1);

            float maxCount = (AudioVisualizer.pitchValue - 1) / (0 - numPoints);
            Debug.Log(maxCount);
            for (int i = 0; i < numPoints; i++)
            {
                float _dst = Mathf.Pow(i / (numPoints / -1f), power);
                float _angle = 2 * Mathf.PI * turnFraction * i;
                _dst = _dst * starDistance;
                float _x = _dst * Mathf.Cos(_angle);
                float _z = _dst * Mathf.Sin(_angle);
                Vector3 _spawnPos = new Vector3((_x + transform.position.x), transform.position.y, (_z + transform.position.z));
                Points[i] = _spawnPos;

                if (stars[i].transform.position != _spawnPos)
                {
                    stars[i].transform.position = _spawnPos;
                }
            }
        }
        frameCount++;
    }
    //private void OnDrawGizmos()
    //{
    //    if (Points == null)
    //        return;

    //    for (int i = 0; i < Points.Count; i++)
    //    {
    //        Gizmos.DrawSphere(Points[i], .01f);
    //    }
    //}  
}
