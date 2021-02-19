using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

}

public class Grid : MB
{
	int width = 10;
	int length = 4;
	float wSpace = 1.5f;
	float lSpace = 4f;
	float streetWidth = 4f;

	Dictionary<Vector2, Cell> grid = new Dictionary<Vector2, Cell>();
    
    void Awake() {

    	// grid.Add(new Vector2(0, 0), new Cell());
    	// Debug.Log(grid[new Vector2(0, 0)]);
    	
    	float centerX = width * wSpace / 2f;
    	float centerZ = length * lSpace / 2f;

    	bool createdPoliceStation = false;

    	for (int i = 0; i < width; i ++) {
    		for (int j = 0; j < length; j ++) {

    			Cell c = new Cell();

    			grid.Add(new Vector2(i, j), c);

    			float x = i * wSpace - centerX;
    			float z = j * lSpace - centerZ;

    			if (j == 2 && (i == 5 || i == 6)) {
    				if (!createdPoliceStation) {
	    				GameObjectPool.Instantiate("PoliceStation", new Vector3(x + 0.75f, 0.5f, z));
	    				createdPoliceStation = true;
    				}
				} else {
	    			Apartment a = GameObjectPool.Instantiate("Apartment", 
	    				new Vector3(x, 0, z)).GetComponent<Apartment>();
	    			a.Init(BuildingConfigs.GetRandom());
				}
    		}
    	}
    }
}
