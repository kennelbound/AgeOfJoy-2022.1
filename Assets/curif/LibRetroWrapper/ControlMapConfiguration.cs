using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Text;



public class ControlMapConfiguration 
{
  public List<Map> maps { get; set; }
  
  public class Action
  {
    public string control = "";
    public string action = "";
    public string path = "";
    public Action ()
    {

    }
    public Action(string control, string action, string path = "")
    {
      this.control = control;
      this.action = action;
      this.path = path;
    }
  }

  public class Map {
    public string mame_control;
    public List<Action> actions { get; set; }
    public Map()
    {

    }
    public Map(string mame_control)
    {
      this.mame_control = mame_control;
      this.actions = new();
    }
    public Action addAction(string control, string action, string path = "")
    {
      Action act = new(control, action, path);
      actions.Add(act);
      return act;
    }

  }
  public string asMarkdown()
  {
    StringBuilder sb = new StringBuilder();
    sb.Append("| MAME | Control map | Action | Unity Path |\n");
    sb.Append("| --- | --- | --- | --- |\n");
    foreach (ControlMapConfiguration.Map m in maps)
    {
      foreach (ControlMapConfiguration.Action a in m.actions)
      {
        sb.Append($"| {m.mame_control} | {a.control} | {a.action} | `{a.path}` |\n");
      }
    }
    return sb.ToString();
  }
  
  public ControlMapConfiguration.Map GetMap(string mame_control)
  {
    foreach (var map in maps)
    {
      if (map.mame_control == mame_control)
        return map;
    }
    return null;
  }

  public void addMap(uint libretroMameCoreID, string controlType, string configAction, string[] mapsToAssign)
  {
    ControlMapConfiguration.Map map;
    string mame_control = LibretroMameCore.getDeviceNameFromID(controlType, libretroMameCoreID);
    if (mame_control == "")
    {
      ConfigManager.WriteConsole($"[ControlMapConfiguration.AddMap] ERROR controlType unknown : {controlType}");
      return;
    }

    map = GetMap(mame_control);
    if (map == null)
    {
      map = new(mame_control);
      maps.Add(map);
    }
    
    foreach (string item in mapsToAssign)
    {
      string path = ControlMapPathDictionary.GetInputPath(item, configAction);
      if (string.IsNullOrEmpty(path))
        ConfigManager.WriteConsole($"[ControlMapConfiguration.AddMap] ERROR path unknown MAME action:{controlType}/{mame_control} mapped control action:{configAction} maped control: {item}");
      else
        map.addAction(item, configAction, path);
    }
    return;
  }
}

public class DefaultControlMap : ControlMapConfiguration
{
  static DefaultControlMap instance = null;

  private DefaultControlMap() : base()
  {
    maps = new();
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_B, "joypad", "b-button", new string[] {"gamepad", "vr-right"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_A, "joypad", "a-button", new string[] {"gamepad", "vr-right"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_X, "joypad", "x-button", new string[] {"gamepad", "vr-left"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_Y, "joypad", "y-button", new string[] {"gamepad", "vr-left"});
    // can't be select because the coin is used.
    //addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_Y, "joypad", "select-button", new string[] {"gamepad", "vr-right"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_START, "joypad", "start-button", new string[] {"gamepad", "vr-left"});

    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_UP, "joypad", "thumbstick-up", new string[] {"vr-left"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_UP, "joypad", "left-thumbstick-up", new string[] {"gamepad"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_DOWN, "joypad", "thumbstick-down", new string[] {"vr-left"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_DOWN, "joypad", "left-thumbstick-down", new string[] {"gamepad"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_LEFT, "joypad", "thumbstick-left", new string[] {"vr-left"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_LEFT, "joypad", "left-thumbstick-left", new string[] {"gamepad"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_RIGHT, "joypad", "thumbstick-right", new string[] {"vr-left"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_RIGHT, "joypad", "left-thumbstick-right", new string[] {"gamepad"});

    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_L, "joypad", "trigger", new string[] {"vr-left"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_L, "joypad", "left-trigger", new string[] {"gamepad"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_R, "joypad", "trigger", new string[] {"vr-right"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_R, "joypad", "right-trigger", new string[] {"gamepad"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_L2, "joypad", "grip", new string[] {"vr-left"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_L2, "joypad", "left-bumper", new string[] {"gamepad"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_R2, "joypad", "grip", new string[] {"vr-right"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_R2, "joypad", "right-bumper", new string[] {"gamepad"});

    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_L3, "joypad", "thumbstick-press", new string[] {"vr-left"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_L3, "joypad", "left-thumbstick-press", new string[] {"gamepad"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_R3, "joypad", "thumbstick-press", new string[] {"vr-right"});
    addMap(LibretroMameCore.RETRO_DEVICE_ID_JOYPAD_R3, "joypad", "right-thumbstick-press", new string[] {"gamepad"});
  }

  public static DefaultControlMap Instance { 
    get
    {
      if (instance == null)
        instance = new();
      return instance;
    }
  }
}

public static class ControlMapInputAction
{
  static bool hasMameControl(string control)
  {
     foreach (var item in LibretroMameCore.deviceIdsJoypad)
     {
       if (item == control)
         return true;
     }
     foreach (var item in LibretroMameCore.deviceIdsMouse)
     {
       if (item == control)
         return true;
     }
     return false;
  }

  public static InputActionMap inputActionMapFromConfiguration(ControlMapConfiguration mapConfig)
  {
    InputActionMap inputActionMap = new();
    foreach (var map in mapConfig.maps)
    {
      if (hasMameControl(map.mame_control))
      {
        //the control is one of the MAME required
        var action = inputActionMap.AddAction(map.mame_control);
        foreach (var mapAction in map.actions)
        {
          var bind = new InputBinding
          {
            path = ControlMapPathDictionary.GetInputPath(mapAction.control, mapAction.action),
            action = map.mame_control
          };
          action.AddBinding(bind);
        }
      }
      else {
        ConfigManager.WriteConsole($"[ControlMapConfiguration.ControlMapInputAction.inputActionMap] ERROR MAME control does not exists: {map.mame_control}");
      }
    }
    inputActionMap.Enable();
    return inputActionMap;
  }

}