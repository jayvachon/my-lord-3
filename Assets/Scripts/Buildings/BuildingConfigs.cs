using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConfig {

	public readonly float height;
	public readonly int value;

	public BuildingConfig(float height, int value) {
		this.height = height;
		this.value = value;
	}
}

public static class BuildingConfigs
{

    static BuildingConfig[] configs = new BuildingConfig[4];

    static BuildingConfigs() {
    	configs[0] = new BuildingConfig(2, 100000);
    	configs[1] = new BuildingConfig(2.5f, 150000);
    	configs[2] = new BuildingConfig(3, 300000);
    	configs[3] = new BuildingConfig(3.5f, 1000000);
    }

    public static BuildingConfig GetRandom() {
        return configs.RandomItem();
    }
}
