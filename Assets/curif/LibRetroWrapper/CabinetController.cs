/*
This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;


public class CabinetController : MonoBehaviour
{
  public CabinetPosition game;
  // public int priority;

  void Start()
  {
    StartCoroutine(load());
  }

  IEnumerator load()
  {

    while (game == null || game.CabInfo == null)
      yield return new WaitForSeconds(1f);

    Cabinet cab;
    try
    {
      //cabinet inception
      ConfigManager.WriteConsole($"[CabinetController] Deploy cabinet {game.CabInfo.name} #{game.Position}");
      cab = CabinetFactory.fromInformation(game.CabInfo, game.Room, game.Position, transform.position, transform.rotation, transform.parent);
    }
    catch (System.Exception ex)
    {
      ConfigManager.WriteConsole($"[CabinetController] ERROR loading cabinet from description {game.CabInfo.name}: {ex}");
      cab = null;
    }
    if (cab != null && game.CabInfo.Parts != null) 
    {
//      var t = Task.Run(async() => await CabinetFactory.skinFromInformationAsync(cab, game.CabInfo));
//      yield return new WaitUntil(() => t.IsCompleted);
      ConfigManager.WriteConsole($"[CabinetControlle] {game.CabInfo.name} texture parts");
      foreach (CabinetInformation.Part part in game.CabInfo.Parts)
      {
        CabinetFactory.skinCabinetPart(cab, game.CabInfo, part);
        yield return null;
      }
    }
    gameObject.SetActive(false);
    ConfigManager.WriteConsole($"[CabinetController] Cabinet deployed  {game.CabInfo.name} ******");
  }
}
