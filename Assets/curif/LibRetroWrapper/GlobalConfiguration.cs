using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.IO;
using System.Reflection;

//[RequireComponent(typeof(FileMonitor))]
public class GlobalConfiguration : MonoBehaviour
{
  public GameObject FileMonitorGameObject;
  public UnityEvent OnGlobalConfigChanged;
  
  //[Tooltip("Global Configuration should use the first File Monitor in attached to the gameobject if there are more than one.")]

  private FileMonitor fileMonitor;
  private static string yamlPath;
  private ConfigInformation configuration;
  public  ConfigInformation Configuration
  {
    get { return configuration; }
    private set 
    { 
      configuration = value; 
      OnGlobalConfigChanged?.Invoke();
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    // Get all FileMonitor components attached to the GameObject
    //FileMonitor[] fileMonitors = GetComponents<FileMonitor>();

    // Get the first FileMonitor component in the array
    fileMonitor = FileMonitorGameObject.GetComponent<FileMonitor>();
    yamlPath = ConfigManager.ConfigDir + "/" + fileMonitor.ConfigFileName;
    OnEnable();
    Load();
  }

  private void Load()
  {
    ConfigInformation config;
    ConfigManager.WriteConsole($"[GlobalConfiguration] loadConfiguration: {yamlPath}");
    if (File.Exists(yamlPath))
    {
      config = ConfigInformation.fromYaml(yamlPath);
      if (config == null) 
      {
        ConfigManager.WriteConsole($"[GlobalConfiguration] ERROR can't read, back to default: {yamlPath}");
        Configuration = ConfigInformation.newDefault();
      }
      else 
      {
        Configuration = config;
      }
    }
    else {
      ConfigManager.WriteConsole($"[GlobalConfiguration] file doesn't exists, create default: {yamlPath}");
      Configuration = ConfigInformation.newDefault();
      configuration.toYaml(yamlPath);
      ConfigManager.WriteConsole($"[GlobalConfiguration] ");
      ConfigManager.WriteConsole(configuration.ToString());
    }
  }

  private void OnFileChanged()
  {
    Load();
  }
  
  void OnEnable()
  {
    // Listen for the config reload message
    fileMonitor?.OnFileChanged.AddListener(OnFileChanged);
  }

  void OnDisable()
  {
    // Stop listening for the config reload message
    fileMonitor?.OnFileChanged.RemoveListener(OnFileChanged);
  }

}
