using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;


    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager instance { get; private set; }
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the Scene");
        }
        instance = this;
    }

    public void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        //LoadGame();

    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    //public void LoadGame()
    //{
    //    // Load save file data using data handler
    //    this.gameData = dataHandler.Load();

    //    // If no data to load, initialize new game
    //    if (this.gameData == null)
    //    {
    //        Debug.LogError("No Data was found. Initializing data to defaults.");
    //        NewGame();
    //    }

    //    // Push the loaded data to all other scripts
    //    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
    //    {
    //        dataPersistenceObj.LoadData(gameData);
    //    }
    //}

    //public void SaveGame()
    //{
    //    // Pass data to other scripts so they can update it

    //    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
    //    {
    //        dataPersistenceObj.SaveData(ref gameData);
    //    }

    //    // Save the data to a file using data handler
    //    dataHandler.Save(gameData);
    //}

    private void OnApplicationQuit()
    {
        //SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);

    }
}
