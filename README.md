# ReGame Cognitive Project

Unity Version: 2019.4.3f1
This project is both a VR and PC application. Production scene is "Cognitive". 

For the VR version:</br>
1. On the [StudyManager] gameobject make sure you **check** the "Is VR Version" checkbox.
2. **Disable** the Camera gameobject.
3. Make sure the OVRPlayerController is **enabled**.
4. Go to Build Settings and make sure you have the Android platform selected.
5. Go to Project Settings, player settings, and XR Settings to make sure 'Virtual Reality Supported' is **checked** and Oculus SDK is enabled. This should automatically change when enabling player controller and platform. 

For the PC Version:</br> 
1. On the [StudyManager] gameobject make sure you **uncheck** the "Is VR Version" checkbox.
2. **Enable** the Camera gameobject.
3. Make sure the OVRPlayerController is **disabled**.
4. Go to Build Settings and make sure you have the PC, Mac, & Linux Standalone platform selected.
5. Go to Project Settings, player settings, and XR Settings to make sure 'Virtual Reality Supported' is **unchecked**. 

**DATA LOCATION**

Data file paths for PC is:</br> 
*C:\Users\ [USERNAME] \AppData\LocalLow\VantaReality\ReGameCognitive*

In order to access the AppData folder you will have to enable "Show hidden File" in your File Explorer settings. 

Data file path for VR is:</br>
*PC\Quest\Internal shared storage\Android\data\com.VantaReality.ReGameCognitive\files*

Remember to restart your Oculus Quest after a session otherwise the data will not display in the file folder. 

**CONTROLS**

All controls in the VR version will be handled by the user. For the PC version there is:</br>

Left Arrow key - incorrect answer
Right Arrow Key - correct answer
Escape key - Exit
The instructions on the screen will display to notify you which button to press for the next level. For example, 'B' for blue, etc.

**CODEBOOK**

For the PC version the researcher will need to know the correct sequence in order to grade them in real time. 
We have created an automatic system that will generate the codebook documents each time so if you make a change to the sequences they will update automatically on start. They are located at:</br>

*C:\Users\ [USERNAME] \Unity Projects\ReGameCognitive\ReGameCognitive\Assets\Resources*

```
Level 1

[BLUE] 
[BLUE] [RED] 
[BLUE] [RED] [RED] 
[BLUE] [RED] [RED] [GREEN] 
[BLUE] [RED] [RED] [GREEN] [BLUE] 
...
```

If you ever want to change the predetermined sequences for the PC version you can go to the Scriptable Objects we have created for the level difficulties.
You can find them at *Assets\_ProjectFolder\SciptableObjects\Difficulty*. In the Inspector you will see a button that will set them. On the difficulty scriptable objects you can also change the time limit per level. 

Color hex codes used for each level:</br>

Blue (level 1) - #000CFF</br>
Green (level 2) - #00FF13</br>
Red (level 3) - #FF0000</br>
Yellow (level 4) - #FCFF00</br>
Orange (level 5) - #FF9A00</br>
</br>
**DATA SHEET LEGEND**</br>

**currentRound** - This is the total amount of rounds.</br>
**difficultyLevel** - The current difficulty level. There are 5 levels.</br>
**sequenceAttempted** - This is the sequence that was given to the player for them to replicate.</br>
**buttonMissed** - If the player entered the sequence incorrectly it is displayed in this column. This allows
you to identify the exact point the player entered the sequence incorrectly.</br>
**timeSpentInSequence** - This is the total amount of time attempting the sequence. Timer starts when the
“Begin” audio clip is played. This could possibly be used to show when the player is confused and
possibly guessing if the total time taken seems significantly longer.</br>
**totalSequencesAttemptedInRound** - This is the total amount of sequences attempted in the current
round.</br>
**totalSequencesCorrectInRound** - This is the total amount of sequences correct in the current round.</br>
**sequenceSuccessPercentageInRound** - This is the success percentage in the current round. This
resets after every round.</br>
