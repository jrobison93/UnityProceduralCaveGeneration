using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

		ProcessMap();

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

	List<List<Coord>> GetRegions(int tileType)
	{
		List<List<Coord>> regions = new List<List<Coord>>();
		int[,] mapFlags = new int[width, height];

		for(int i = 0; i < width; i++)
		{
			for(int j = 0; j < height; j++)
			{
				if(mapFlags[i, j] == 0 && map[i, j] == tileType)
				{
					List<Coord> newRegion = GetRegionTiles(i, j);
					regions.Add(newRegion);

					foreach(Coord tile in newRegion)
					{
						mapFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}

		return regions;
	}

	void ProcessMap()
	{
		List<List<Coord>> wallRegions = GetRegions(1);

		int wallThresholdSize = 100;

		foreach(List<Coord> wallRegion in wallRegions)
		{
			if(wallRegion.Count < wallThresholdSize)
			{
				foreach(Coord tile in wallRegion)
				{
					map[tile.tileX, tile.tileY] = 0;
				}
			}
		}

		List<List<Coord>> roomRegions = GetRegions(1);
		
		int roomThresholdSize = 200;
		
		foreach(List<Coord> roomRegion in roomRegions)
		{
			if(roomRegion.Count < wallThresholdSize)
			{
				foreach(Coord tile in roomRegion)
				{
					map[tile.tileX, tile.tileY] = 1;
				}
			}
		}
	}

	List<Coord> GetRegionTiles(int startX, int startY)
	{
		List<Coord> tiles = new List<Coord>();
		int[,] mapFlags = new int[width, height];
		int tileType = map[startX, startY];

		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue(new Coord(startX, startY));
		mapFlags[startX, startY] = 1;

		while(queue.Count > 0) 
		{
			Coord tile = queue.Dequeue();
			tiles.Add(tile);

			for(int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
			{
				for(int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
				{
					if(IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
					{
						if(mapFlags[x, y] == 0 && map[x, y] == tileType)
						{
							mapFlags[x, y] = 1;
							queue.Enqueue(new Coord(x, y));
						}
					}
				}
			}
		}

		return tiles;
	}

	bool IsInMapRange(int x, int y)
	{
		return x >= 0 && x < width && y >= 0 && y < height;
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
				if(IsInMapRange(i, j))
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


	struct Coord
	{
		public int tileX;
		public int tileY;

		public Coord(int x, int y)
		{
			tileX = x;
			tileY = y;
		}
	}


}
