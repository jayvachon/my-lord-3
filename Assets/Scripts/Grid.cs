using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Cell {

	public Building Building { get; private set; }

	public void Init(Building building) {
		Building = building;
	}
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
    	
    	float centerX = width * wSpace / 2f;
    	float centerZ = length * lSpace / 2f;

    	bool createdPoliceStation = false;
        bool createdCourt = false;

    	// Create buildings
    	for (int i = 0; i < width; i ++) {
    		for (int j = 0; j < length; j ++) {

    			Cell cell = new Cell();
    			float x = i * wSpace - centerX;
    			float z = j * lSpace - centerZ;

                if (j == 1 && (i == 2 || i == 3)) {
                    if (!createdCourt) {
                        Building b = GameObjectPool.Instantiate("Court", new Vector3(x + 0.75f, 0.5f, z)).GetComponent<Building>();
                        cell.Init(b);
                        createdCourt = true;
                    }
                }
    			else if (j == 2 && (i == 5 || i == 6)) {
    				if (!createdPoliceStation) {
	    				Building b = GameObjectPool.Instantiate("PoliceStation", new Vector3(x + 0.75f, 0.5f, z)).GetComponent<Building>();
	    				cell.Init(b);
	    				createdPoliceStation = true;
    				}
				} else {
	    			Apartment a = GameObjectPool.Instantiate("Apartment", 
	    				new Vector3(x, 0, z)).GetComponent<Apartment>();
	    			a.Init(BuildingConfigs.GetRandom());
	    			cell.Init(a);
				}

    			grid.Add(new Vector2(i, j), cell);
    		}
    	}

    	// Set neighbor values
    	for (int i = 0; i < width; i ++) {
    		for (int j = 0; j < length; j ++) {
    			Building b;
    			if (TryGetBuildingAtCell(i, j, out b)) {
    				List<float> neighborValues = new List<float>();
    				bool onLeftEdge = i == 0;
    				bool onRightEdge = i == width-1;
    				bool onTopEdge = j == 0;
    				bool onBottomEdge = j == length-1;
    				if (!onLeftEdge) {
    					Building b1;
    					if (TryGetBuildingAtCell(i-1, j, out b1)) {
    						neighborValues.Add(b1.PropertyValue);
    					}
    					if (!onTopEdge) {
    						Building b2;
    						if (TryGetBuildingAtCell(i-1, j-1, out b2)) {
    							neighborValues.Add(b2.PropertyValue);
    						}
    					}
    					if (!onBottomEdge) {
    						Building b2;
    						if (TryGetBuildingAtCell(i-1, j+1, out b2)) {
    							neighborValues.Add(b2.PropertyValue);
    						}
    					}
    				}
    				if (!onRightEdge) {
    					Building b1;
    					if (TryGetBuildingAtCell(i+1, j, out b1)) {
    						neighborValues.Add(b1.PropertyValue);
    					}
    					if (!onTopEdge) {
    						Building b2;
    						if (TryGetBuildingAtCell(i+1, j-1, out b2)) {
    							neighborValues.Add(b2.PropertyValue);
    						}
    					}
    					if (!onBottomEdge) {
    						Building b2;
    						if (TryGetBuildingAtCell(i+1, j+1, out b2)) {
    							neighborValues.Add(b2.PropertyValue);
    						}
    					}
    				}
    				if (!onTopEdge) {
    					Building b1;
    					if (TryGetBuildingAtCell(i, j-1, out b1)) {
    						neighborValues.Add(b1.PropertyValue);
    					}
    				}
    				if (!onBottomEdge) {
    					Building b1;
    					if (TryGetBuildingAtCell(i, j+1, out b1)) {
    						neighborValues.Add(b1.PropertyValue);
    					}
    				}
    				b.PropertyNeighborModifier = neighborValues.Average();
    			}
    		}
    	}
    }

    bool TryGetBuildingAtCell(int x, int z, out Building building) {
    	Cell c = grid[new Vector2(x, z)];
    	if (c.Building != null) {
    		building = c.Building;
    		return true;
    	}
    	building = null;
    	return false;
    }

    public Building GetRandomBuilding() {
        bool hasBuilding = false;
        Building randomBuilding = new Building();
        while (!hasBuilding) {
            int x = Random.Range(0, width);
            int z = Random.Range(0, length);
            if (TryGetBuildingAtCell(x, z, out randomBuilding)) {
                hasBuilding = true;
                return randomBuilding;
            }
        }
        return randomBuilding;
    }

    public Apartment GetRandomApartment() {
        bool hasApartment = false;
        Apartment randomApartment = new Apartment();
        while (!hasApartment) {
            Building b = GetRandomBuilding();
            if (b is Apartment) {
                randomApartment = b as Apartment;
                hasApartment = true;
            }
        }
        return randomApartment;
    }

    public Apartment GetHighestValueApartment() {
        return grid.Where(c => c.Value.Building is Apartment)
            .OrderByDescending(c => c.Value.Building.PropertyValue).FirstOrDefault()
            .Value.Building as Apartment;
    }
}
