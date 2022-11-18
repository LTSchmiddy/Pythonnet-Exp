import clr
from UnityEngine import *
import Cinemachine
from GameUniverse import GlobalManager

def OnGUI(go: GameObject):
    if GUILayout.Button("New Game"):
        GlobalManager.LoadNewGame()
        
    if GUILayout.Button("Load Game"):
        GlobalManager.LoadIntoGame()
        
        
    
    
    