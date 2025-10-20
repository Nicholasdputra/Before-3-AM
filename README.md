<table width="100%">
  <thead>
    <tr>    
      <th colspan="2">
        <h1>Gameplay Footage - May Take Time To Load</h1>
      </th>
    </tr>
  </thead>
  <tbody> 
    <tr>
      <td align="center"> 
        <img src="https://github.com/user-attachments/assets/97b5049e-1ec6-4436-a246-63e4134bd859" alt="Before 3 A.M. Voting" width="100%">
      </td>
      <td align="center">
        <img src="https://github.com/user-attachments/assets/b25d0736-1dbf-4303-93a7-9d3383e8a359" alt="Before 3 A.M. Dialogue" width="100%"> 
      </td>
    </tr>
    <tr>
      <td align="center">
        <img src="https://github.com/user-attachments/assets/c084e6a9-c929-4f7f-add1-460c7345e4f2" alt="Before 3 A.M. Notes" width="100%"> 
      </td>
      <td align="center"> 
        <img src="https://github.com/user-attachments/assets/8bb0dc31-1c4a-43bc-ad32-e8d485122e31" alt="Before 3 A.M. Archives" width="100%">
      </td>
    </tr>
  </tbody>
</table>

# 🕝 About
Before 3 A.M. is a horror story game where you and three other people find yourself locked in a room with seemingly no exit and a note that tells you that one of you is responsible for having kidnapped everyone here. The note also says that the four of you have the power to 'vote' for who you think is responsible for all this and that if you figure it out, everyone lives, otherwise... Regardless, it says you have until before 3 A.M. to talk with everyone and rally everyone together and vote. Sadly, everyone seems to suspect each other so it's solely on you to be the decision maker.

---

# 📜 Scripts Breakdown

<table width="100%">
  <thead>
    <tr>
      <th width="33%">
        <h4>
          <a>Script Name</a>
        </h4>
      </th>
      <th width="67%">
        <h4>
          <a>Script Description</a>
        </h4>
      </th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>
        📚 ArchivesScript.cs
      </td>
      <td>
        Handles the game's ending unlocks for its archives
      </td>
    </tr>
    <tr>
      <td>
        🎵 AudioManager.cs
      </td>
      <td>
        Responsible for everything sound related in the game
      </td>
    </tr>
    <tr>
      <td>
        ✍️ DialogueDataReformatter.cs
      </td>
      <td>
        Reformats dialogues that contain player name to include said player name as well as detemines the time it'll take to go through a dialogue option
      </td>
    </tr>
    <tr>
      <td>
        💬 DialogueDataSO.cs
      </td>
      <td>
        Scriptable object for all dialogue in the game
      </td>
    </tr>
    <tr>
      <td>
        👁️‍🗨️ DialogueViewScript.cs
      </td>
      <td>
        Handles dialogue flow, with the basic flow of a character's dialogue being 'Dialogue Data' that contains 'Dialogue' that might have 'Player Choices' as a response to that and 'Follow Up Player Choice' as the NPC's response for the player's choice
      </td>
    </tr>
    <tr>
      <td>
        🎮 GameManagerScript.cs
      </td>
      <td>
        Script for handling time, scene changes and panels
      </td>
    </tr>
    <tr>
      <td>
        ⏰ MainMenuScript.cs
      </td>
      <td>
        Handles everything in the main menu
      </td>
    </tr>
    <tr>
      <td>
        🧑‍🦲 NPC.cs
      </td>
      <td>
        Script that holds an NPC's attributes and 'mode' (there's conversation mode where you focus in on one person and the other is room mode where they're just standing there)
      </td>
    </tr>
    <tr>
      <td>
        🎥 PreGameScript.cs
      </td>
      <td>
        Handles the warning text and username input in a pregame scene that's between the main menu and the actual game
      </td>
    </tr>
    <tr>
      <td>
        ⚙️ Settings.cs
      </td>
      <td>
        Handles resolutions and volume settings for the game
      </td>
    </tr>
    <tr>
      <td>
        👍 VoteViewScript.cs
      </td>
      <td>
        Handles the vote mechanic 
      </td>
    </tr>
  </tbody>
</table>

---
# 🕹️ Controls
| Button | Actions |
|---|---|
| Left Click | Used for picking dialogue options |

---
# 💻 My Contributions

* Chacter Dialogues
* Voting
* Ending Archives
* Music & SFX
* UI
* Time Consumption Per Dialogue

---
# 📰Team Members
* Leonardi - Game Artist 🖌️
* Nicholas Dwi Putra - Game Programmer 💻
* Rafael Wirasana Wijaya - Game Designer 📃
