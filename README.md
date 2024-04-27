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

We need to set the time after winning the level. This can be added to the `YouWin` function in `Level.cs`. Then we can load the time after the `ShowIntro` function in `Level.cs`. Finally, we can delete the time key at the beginning of the game in the `Start` function. Following the principle of "tell don't ask" we can get the `Timer` class to perform these functions for us.

Add these three functions to `Timer.cs`:
```cs
    public void StoreTimer()
    {
        PlayerPrefs.SetFloat("Time", time);
    }

    public void LoadTimer()
    {
        time = PlayerPrefs.GetFloat("Time", 0.0f);
    }

    public static void ResetTime()
    {
        PlayerPrefs.DeleteKey("Time");
    }
```

Now we can add the call to `StoreTimer` in the function `YouWin` in `Level.cs`:
```cs
    IEnumerator YouWin()
    {
        timer.enabled = false;
        win.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Load(next);
    }
```

And we can add the call to `LoadTimer` and `ResetTimer` at the beginning of `Start`:
```cs
        if (timer != null)
        {
            timer.LoadTimer();
        }
        else
        {
            Timer.ResetTime();
        }
```

Note the capital `T` in the else case. This is how to reference a static function belonging to a class. We can't use the `timer` variable if it hasn't been set. It is not set in the SplashScreen scene, but it is set in each level.

In your own games you will need to find the right place to insert the store, load, and reset functions.

## Static data

Another simple way is to make use of the `static` keyword for the `time` data in our `Timer` class. Static data belongs to the class but not to any particular instance of the data. This means that we can access it from anywhere without needing an instance of the class to reference. We still need to reset the value at the start of the game as with the `PlayerPrefs` method.

In the `Timer.cs` script, change the keywords specifying the `time` value to:
```cs
    public static float time = 0.0f;
```

And add this function:
```cs
    public static void Reset()
    {
        time = 0.0f;
    }
```

And at the beginning of `Start` in `Level.cs`, add:
```cs
        if (timer == null)
        {
            Timer.Reset();
        }
```

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
