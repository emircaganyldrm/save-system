# Save System

A brief documentation about usage of the save system
#

### Basic Usage:

- Put your save values to SavedData.cs
- Create a SaveController MonoBehaviour in scene
- Set the default values in SaveController
- Set the SavedData value from your script
    
```java 
SaveManager.savedData.exampleVariable = exampleVariable;
```

- Then set the variable to SavedData value

```java
exampleVariable = SaveManager.savedData.exampleVariable;
```
#

### About the system:

#### System mainly uses three classes and one struct:
- Saver.cs
- SaveManager.cs
- SavedData.cs
- SaveController.cs

#

#### Saver.cs


Saver is a static class which holds two generic functions

```java
public static void Save<T>(T data, string savePath)
{
	var save = JsonUtility.ToJson(data);
	save = save.ToEncrypted();

	File.WriteAllText(savePath, save);
	}

	public static T Load<T>(string savePath)
	{
	var save = File.ReadAllText(savePath);
	save = save.FromEncrypted();

	return JsonUtility.FromJson<T>(save);
}
```
#### These two methods does all the work on the OS side

- Takes the class to be saved converts to Json or from Json

- Encrypts or decrypts

- Writes or reads the file

#

#### SaveManager.cs

SaveManager is the main class of this system it initializes whole system holds the SavedData struct and the save path

The Init() function takes a default SavedData which SaveController holds.
Checks if any file exist in the save path uses the defaults if not:

```java
public static void Init(SavedData defaultSavedData)
{
	if (File.Exists(savePath))
	    Load();
	else
	    savedData = defaultSavedData;
}
```

SaveManager also has the referance to the SavedData struct to be called from anywhere


### SavedData.cs

SavedData is the base struct to save to a local file. You should store all your saveable data in this struct.

```java
[Serializable]
public struct SavedData
{
	/*
	 * Define your data defaults in inspector from the save controller mono
	 */

	public ExampleData exampleData;

	public SavedData(ExampleData exampleData)
	{
		this.exampleData = exampleData;
	}	
}

[Serializable]
public struct ExampleData
{
	public int exampleInt;

	public ExampleData(int exampleInt)
	{
		this.exampleInt = exampleInt;
	}
}
```

#

For example if you want to store data from Example.cs:

```java
public class Example : Monobehaviour
{
	private int exampleInt = 5;

	private void IncreaseExampleInt(int amount)
	{
	    exampleInt += amount;

	    SaveManager.savedData.exampleInt = exampleInt;
	}
}
```

#
#### SaveController.cs

SaveController is a Monobehaviour that initializes SaveManager on Awake.
Calls the SaveToLocal function of SaveManager on disable or on application quit.

#


## Editor side of things

- You need to have a SaveController Monobehaviour in every scene that you have something to save or load

![image](https://user-images.githubusercontent.com/70021708/225603103-9c424ca7-97aa-429b-89d7-9bb522685c8c.png)

- Use the save defaults struct to determine the non saved values of variables
- For example if you want to set default playerSpeed to 5 you should put that value in there. Script will load that value to the savedData then you can set your playerSpeed to the SaveManager.savedData.playerSpeed.

### Editor Tools

There are two basic tools that can be used:

![image](https://user-images.githubusercontent.com/70021708/225604029-aa110d49-a3b7-4b4d-bb05-2d06aac3624b.png)


- Show Save Data : 
This window shows every value that has been saved
![image](https://user-images.githubusercontent.com/70021708/225604362-c8ecdd06-8c9c-440f-a02f-815781a54593.png)


- Clear Saves

Works exactly as Clear Playerprefs. Deletes the current save file.


## Encryption

System uses a simple Base64 converter to encrypt the data.

Base64Encryptor.cs

```java
string.ToEncrypted();

string.FromEncrypted();
```