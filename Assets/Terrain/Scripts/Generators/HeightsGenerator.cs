using Assets.Scripts.MapGenerator.Maps;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.MapGenerator.Generators
{
    public class HeightsGenerator : MonoBehaviour, IGenerator
    {
        //uses the terrain data to determine things like size and heightmap resolution

        public int Octaves = 4;
        public float Scale = 50f;
        [Range(0, 3)]
        [Tooltip("How spiky the terrain is")]
        public float Lacunarity = 2f;
        [Range(0, 1)]
        public float Persistance = 0.5f;
        public AnimationCurve HeightCurve;
        public float Offset = 100f;
        public float FalloffDirection = 3f;
        public float FalloffRange = 3f;
        public bool UseFalloffMap;
        public bool Randomize;
        public bool AutoUpdate;

        private void OnValidate()
        {
            if (Lacunarity < 1)
            {
                Lacunarity = 1;
            }
            if (Octaves < 0)
            {
                Octaves = 0;
            }
            if (Scale <= 0)
            {
                Scale = 0.0001f;
            }
        }

        //would like a method that generates noise directly at the vertex positions only, as 
        //we don't need the noise to be generated elsewhere
        //

        public void Generate()
        {
            if (Randomize)
            {
                Offset = Random.Range(0f, 9999f);
            }

            TerrainData terrainData = Terrain.activeTerrain.terrainData;

            float[,] falloff = null;
            if (UseFalloffMap)
            {
                falloff = new FalloffMap
                {
                    FalloffDirection = FalloffDirection,
                    FalloffRange = FalloffRange,
                    Size = terrainData.heightmapResolution
                }.Generate();
            }

            float[,] noiseMap = GenerateNoise(falloff);
            terrainData.SetHeights(0, 0, noiseMap);
        }

        float[,] GenerateNoise(float[,] falloffMap = null)
        {
            TerrainData terrainData = Terrain.activeTerrain.terrainData;
            //set a height curve here that we like
            AnimationCurve heightCurve = new AnimationCurve(HeightCurve.keys);



            float maxLocalNoiseHeight;
            float minLocalNoiseHeight;

            float[,] noiseMap = new PerlinMap()
            {
                Size = terrainData.heightmapResolution,
                Octaves = Octaves,
                Scale = Scale * (terrainData.heightmapResolution) / 512,  //multiply by scale here so its size independent
                Offset = Offset,
                Persistance = Persistance,
                Lacunarity = Lacunarity
            }.Generate(out maxLocalNoiseHeight, out minLocalNoiseHeight);

            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                for (int x = 0; x < terrainData.heightmapResolution; x++)
                {
                    var lerp = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);

                    if (falloffMap != null)
                    {
                        lerp -= falloffMap[x, y];
                    }

                    if (lerp >= 0)
                    {
                        noiseMap[x, y] = heightCurve.Evaluate(lerp);

                        Debug.Assert(noiseMap[x, y] <= 1);

                        //could we use a 1 channel texture to go between two height curves?
                        //or instead of texture, use another noise map?
                        //or instead of texture, use another noise map?
                        //one that gives more cliffs (aka step) and one that's more linear?

                    }
                    else
                    {
                        noiseMap[x, y] = 0;
                    }
                }
            }

            return noiseMap;
        }

        public void Clear()
        {
            TerrainData terrainData = Terrain.activeTerrain.terrainData;
            terrainData.SetHeights(0, 0, new float[terrainData.heightmapResolution, terrainData.heightmapResolution]);
        }
    }
}