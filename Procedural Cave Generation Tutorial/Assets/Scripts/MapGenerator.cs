using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour 
{
	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	[Range(0, 100)]
	public int randomFillPercent;

	private int[,] map;

	void Start()
	{
		GenerateMap();
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			GenerateMap();
		}
	}

	void GenerateMap()
	{
		map = new int[width, height];
		RandomFillMap();

		for(int i = 0; i < 5; i++)
		{
			SmoothMap();
		}

		int borderSize = 1;
		int[,] borderMap = new int[width + borderSize * 2, height + borderSize * 2];

		for(int i = 0; i < borderMap.GetLength(0); i++)
		{
			for(int j = 0; j < borderMap.GetLength(1); j++)
			{
				if(i >= borderSize && i < width + borderSize && j >= borderSize && j < height + borderSize)
				{
					borderMap[i, j] = map[i - borderSize, j - borderSize];
				}
				else
				{
					borderMap[i, j] = 1;
				}
			}
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh(borderMap, 1);
	}

	void RandomFillMap()
	{
		if(useRandomSeed)
		{
			seed = DateTime.Now.ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());
		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < height; j++)
			{
				if(i == 0 || i == width - 1 || j == 0 || j == height - 1)
				{
					map[i, j] = 1;
				}
				else
				{
					map[i, j] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0; 
				}
			}
		}
	}

	void SmoothMap()
	{
		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < height; j++)
			{
				int neighborWallTiles = GetSurroundingWallCount(i, j);

				if(neighborWallTiles > 4)
				{
					map[i, j] = 1;
				}
				else if(neighborWallTiles < 4)
				{
					map[i, j] = 0;
				}
			}
		}
	}

	int GetSurroundingWallCount(int x, int y)
	{
		int wallCount = 0;

		for(int i = x - 1; i <= x + 1; i++)
		{
			for(int j = y - 1; j <= y + 1; j++)
			{
				if(i >= 0 && i < width && j >= 0 && j < height)
				{
					if(i != x || j != y)
					{
						wallCount += map[i, j];
					}
				}
				else
				{
					wallCount++;
				}
			}
		}

		return wallCount;
	}

	void OnDrawGizmos()
	{
//		if(map != null)
//		{
//			for(int i = 0; i < width; i++)
//			{
//				for(int j = 0; j < height; j++)
//				{
//					Gizmos.color = (map[i, j] == 1) ? Color.black : Color.white;
//					Vector3 pos = new Vector3(-width / 2 + i + 0.5f, 0, -height / 2 + j + 0.5f);
//					Gizmos.DrawCube(pos, Vector3.one);
//				}
//			}
//		}
	}


}
