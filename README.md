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

## DontDestroyOnLoad

With this method we use a `GameObject` in the scene to store our time data. Unlike other `GameObject`s it won't be destroyed when the scene changes. This means we can continue to access its data between scenes. In this case we need to make sure that the `GameObject` is in the `Start` scene but not in the other scenes. This also means that the other scenes will not work without being loaded after the `Start` scene.

Also, when returning to the `Start` scene we need to make sure that the `GameObject` is not duplicated. We can do this by checking if the `GameObject` already exists and destroying any duplicates. To do this we can use a static variable pointing to the already created instance. This is also an example of the Singleton pattern in the GoF book.

## ScriptableObject

The final way I'm going to demonstrate is the recommended approach. That is to use a `ScriptableObject`. This is a Unity specific way to store data between scenes in a way that is still editable within the Unity Editor. To do this we need to create a new `ScriptableObject` asset and store our data in it. We can then access this data from any script in our game. Again we have to remember to reset the timer when starting a new game.

## Pros and Cons

| Method | Pros | Cons |
| --- | --- | --- |
| PlayerPrefs | simple to implement<br>less code for small data | doesn't scale well<br>persists outside of the game<br>hard to debug<br>risk of key collision |
| Static data | simple to implement<br>can scale up<br>doesn't persist outside of the game | hard to debug<br>only editable within code |
| DontDestroyOnLoad | editable within the Unity Editor<br>scales well<br>doesn't persist outside of the game | harder to implement<br>difficult to debug<br>requires a boot scene |
| ScriptableObject | simple to implement<br>editable within the Unity Editor<br>scales well<br>doesn't persist outside of the game<br>does persist within the Editor | requires more configuration |

## Other methods

It is also possible to additively load scenes and keep certain data between scene loads. This is a more complex approach and is not covered here. It can be useful if large amounts of data need to be loaded asynchronously during the boot process.
