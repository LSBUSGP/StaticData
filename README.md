# StaticData

This repository demonstrates a small game. What I'd like is for the time taken to accumulate over each level.

How can we transfer data from one scene to the next?

## PlayerPrefs

One simple way to transfer data between scenes is to use `PlayerPrefs`. To do this we need to `Set`, `Get`, and `Delete` key data from the `PlayerPrefs` stored along with our game.

```.cs
PlayerPrefs.SetFloat("Time", time);
PlayerPrefs.GetFloat("Time", 0.0f);
PlayerPrefs.DeleteKey("Time");
```

Where do these calls need to go?

We need to set the time after winning the level. This can be added to the `YouWin` function in `Level.cs`. Then we can load the time after the `ShowIntroduction` function in `Level.cs`. Finally, we can delete the time key at the beginning of the game in the `Start` function. Following the principle of "tell don't ask" we can get the `Timer` class to perform these functions for us.

## Static data

Another simple way is to make use of the `static` keyword for the `time` data in our `Timer` class. Static data belongs to the class but not to any particular instance of the data. This means that we can access it from anywhere without needing an instance of the class to reference. We still need to reset the value at the start of the game as with the `PlayerPrefs` method.

