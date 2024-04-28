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

Also, when returning to the `Start` scene we need to make sure that the `GameObject` is not duplicated. We can do this by checking if the `GameObject` already exists and destroying any duplicates. To do this we can use a static variable pointing to the already created instance. This is also an example of the Singleton pattern in the GoF Design Patterns book.

![image](https://github.com/LSBUSGP/StaticData/assets/3679392/8282f72c-e9ba-4846-a8a1-7dc633212134)

The intent of the Singleton pattern is:
> Singleton is a creational design pattern that lets you ensure that a class has only one instance, while providing a global access point to this instance.

Let's start by creating a new game object in the `Start` scene. Call this object `GameData` and then create a new script also called `GameData` and add it to the object. To begin with, let's just mark the object with `DontDestroyOnLoad` when it is created:
```cs
using UnityEngine;

public class GameData : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
```

Now when you run the game and start playing level 1, you should see the `GameData` object (the same one from the `Start` scene) is still in the Hierarchy Window:

![image](https://github.com/LSBUSGP/StaticData/assets/3679392/50d3572b-f819-4f44-bfb3-39ae39865309)

So, with this we can move the `time` variable from the `Timer` class into the `GameData` class. Now, we could make the variable public and access from the `Timer` class, but that would be breaking our "tell don't ask" rule. So instead, we can move the `Update` functionality from the `Timer` class and put that into the GameData class too. To do this, we need to create a new function in `GameData.cs` and because we don't have access to the `Timer`s `text` object from the `GameData` class we'll have to pass that in as a parameter (and we might as well pass in `deltaTime` while we are at it:
```cs
    public void UpdateTimerText(TMP_Text text, float deltaTime)
    {
        time += deltaTime;
        TimeSpan span = TimeSpan.FromSeconds(time);
        text.text = $"TIME: {span.Minutes:D2}:{span.Seconds:D2}.{span.Milliseconds/10:D2}";
    }
```

Note, you will also need to add some `using` statements to the top of the file, `System` for `TimeSpan` and `TMPro` for `TMP_Text`:
```cs
using System;
using TMPro;
```

Now, in `Timer.cs` we need to access the `GameData` object to update the timer text. For this we need the second part of the Singleton pattern: to provide a global access point to the instance. We can do that by adding a static `instance` variable and modifying the `Start` function to set it:
```cs
    public static GameData instance;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
```

With this in place, we can now access the `UpdateTimerText` function from `Timer.cs`:
```cs
public class Timer : MonoBehaviour
{
    public TMP_Text text;

    void Update()
    {
        GameData.instance.UpdateTimerText(text, Time.deltaTime);
    }
}
```

Now, if you run the program, you will find that it will work, but has a couple of issues. Firstly, although the `time` value is maintained between levels, and is reset when going back to the `Start` scene, a new instance of `GameData` is getting created each time. Secondly, if you try running the `Level1` scene or `Level2` scene, you will get an exception. To solve these issues, we need to implement the first intent of the Singleton pattern. We need to ensure that there is only one instance.

One way to do this is to catch the duplication when the scene is loaded and the new object is created. We can do this from the `GameData.cs` script `Start` function:
```cs
    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
```

Now you can add copies of the `GameData` object to `Level1` and `Level2` scenes, but when you run, the objects won't get duplicated.

Unfortunately, this re-introduces the issue that we had with the earlier methods, in that the timer doesn not reset when going back to the `Start` scene. But we can fix this in a similar way too. In `GameData.cs` we can add the function:
```cs
    public void ResetTimer()
    {
        time = 0.0f;
    }
```

Then call it from `Level.cs` `Start` function:
```cs
        if (timer == null)
        {
            GameData.instance.ResetTimer();
        }
```

Note, there is a potential issue here with using an `instance` variable in a `Start` function when the `instance` is also set in a `Start` function. The program will only work if the `GameData` script is processed before the `Level` script. There are two easy ways to fix this. You can change the order of execution for the `GameData` script and make it execute earlier than the `Level` script (or make the `Level` script execute after the `GameData` script.) Or, you can change the `GameData` `Start` function into `Awake`.

## ScriptableObject

The final way I'm going to demonstrate is the recommended approach. That is to use a `ScriptableObject`. This is a Unity specific way to store data between scenes in a way that is still editable within the Unity Editor. To do this we need to create a new `ScriptableObject` asset and store our data in it. We can then access this data from any script in our game. Again we have to remember to reset the timer when starting a new game.

When creating a scriptable object, you need to create the script first. So create a new script `GameData.cs`:
```cs
using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
}
```

Now, you can create an instance of the `GameData` scriptable object in your project folder by selecting `Game Data`:

![image](https://github.com/LSBUSGP/StaticData/assets/3679392/db61d1d8-2ded-494b-af67-89344c248088)

Now we can move the `time` variable and functions into `GameData.cs` as we did with the previous method:
```cs
using System;
using TMPro;
using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
	[SerializeField] float time = 0.0f;

	public void UpdateTimerText(TMP_Text text, float deltaTime)
	{
		time += deltaTime;
		TimeSpan span = TimeSpan.FromSeconds(time);
		text.text = $"TIME: {span.Minutes:D2}:{span.Seconds:D2}.{span.Milliseconds / 10:D2}";
	}

	public void ResetTimer()
	{
		time = 0.0f;
	}
}
```

I have added the keyword `[SerializeField]` to the time variable as I want it to be private, but still allow it to be modified within the Unity Editor.

As with the previous method, we can provide a global access point to this instance. But in this case, no matter how many times we change scenes, we still only have one instance to deal with. So here we don't need to deal with duplicates. Because this is a `ScriptableObject` rather than a `MonoBehaviour` we can put our instance code in the `OnEnable` function which always gets called whenever the object is loaded. Since it gets loaded once when the game loads, and is not normally unloaded, this is a good place to initialise our global static instance variable.

```cs
    public static GameData instance;
	void OnEnable()
	{
		instance = this;
	}
```

Now we can call the functions from `Timer.cs`:
```cs
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMP_Text text;

    void Update()
    {
        GameData.instance.UpdateTimerText(text, Time.deltaTime);
    }
}
```

And from the `Level.cs` `Start` function as before:
```cs
        if (timer == null)
        {
            GameData.instance.ResetTimer();
        }
```

Unlike the `DontDestroyOnLoad` example, we don't need to add anything to the scenes but we can still run any scene independently.

## Pros and Cons

| Method | Pros | Cons |
| --- | --- | --- |
| PlayerPrefs | simple to implement<br>less code for small data | doesn't scale well<br>persists outside of the game<br>hard to debug<br>risk of key collision |
| Static data | simple to implement<br>can scale up<br>doesn't persist outside of the game | hard to debug<br>only editable within code |
| DontDestroyOnLoad | editable within the Unity Editor<br>scales well<br>doesn't persist outside of the game | harder to implement<br>difficult to debug<br>requires a boot scene or duplicate data objects |
| ScriptableObject | simple to implement<br>editable within the Unity Editor<br>scales well<br>doesn't persist outside of the game<br>does persist within the Editor | creates more coupling |

## Other methods

It is also possible to additively load scenes and keep certain data between scene loads. This is a more complex approach and is not covered here. It can be useful if large amounts of data need to be loaded asynchronously during the boot process.
