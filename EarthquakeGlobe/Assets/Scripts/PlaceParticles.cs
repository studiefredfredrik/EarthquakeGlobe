using UnityEngine;
using System.Collections;
using System;

public class PlaceParticles : MonoBehaviour {
    private bool bPointsUpdated = false;
    private bool isFirstPass = true;
    private float startTime = 0;

    public ParticleSystem.Particle[] cloud;
    public string url;
    public float globeScale;
    public float reUpAfterSeconds;
    public float scaleFactor;
    public float scaleStart;
    public float maxMag;


    void Update()
    {
        // Render points if updated
        if (bPointsUpdated)
        {
            GetComponent<ParticleSystem>().SetParticles(cloud, cloud.Length);
            bPointsUpdated = false;
        }

        // Get new points 
        if (Time.time - startTime >= reUpAfterSeconds || isFirstPass)
        {
            StartCoroutine(HttpGet(url));   // Coroutine since we are async/yeilding
            startTime = Time.time;
            isFirstPass = false;
        }
    }

    IEnumerator HttpGet(string url)
    {
        WWW www = new WWW(url);
        yield return www;   // Continue untill www is populated
        serverJsonResponseObj serverUnitData = JsonUtility.FromJson<serverJsonResponseObj>(www.text);
        
        cloud = new ParticleSystem.Particle[serverUnitData.features.Length];

        for (int ii = 0; ii < serverUnitData.features.Length; ++ii)
        {
            // Vector3 from LatLng is just a straight line of constant length from origo with lat and lng applied as angles
            Vector3 pointPos = 
                Quaternion.AngleAxis(serverUnitData.features[ii].geometry.coordinates[0], -Vector3.up) * 
                Quaternion.AngleAxis(serverUnitData.features[ii].geometry.coordinates[1], -Vector3.right) * 
                new Vector3(0, 0, globeScale / 2);
            cloud[ii].position = pointPos;

            // Calculate color (0-6 magnitude -> green-red)
            var x = serverUnitData.features[ii].properties.mag / maxMag;
            Color myColor = new Color(2.0f * x, 2.0f * (1 - x), 0.5f);
            cloud[ii].color = myColor;

            // Size also affected by magnitude
            cloud[ii].size = serverUnitData.features[ii].properties.mag * scaleFactor + scaleStart;
        }

        bPointsUpdated = true;
        yield return null;
    }

    #region JSON response objects
    [Serializable]
    class serverJsonResponseObj
    {
        public unitPositionObj[] features;
    }

    [Serializable]
    class unitPositionObj
    {
        public string type;
        public quakeProperties properties;
        public quakeGeometry geometry;
        public string id;
    }

    [Serializable]
    class quakeProperties
    {
        public float mag;
        public int tsunami;
    }

    [Serializable]
    class quakeGeometry
    {
        public string type;
        public float[] coordinates;
    }
    #endregion

}
