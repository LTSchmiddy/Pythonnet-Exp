import clr, sys, multiprocessing
clr.AddReference("UnityEngine")
import UnityEngine

old_stdout = sys.stdout
old_stderr = sys.stderr

class UnityStdout:
    def write(self, s: str):
        if not (s.strip() == ""):
            UnityEngine.Debug.Log(s.strip())
        
    def flush(self, s):
       pass

sys.stdout = UnityStdout()

class UnityStderr:
    def write(self, s: str):
        if not (s.strip() == ""):
            UnityEngine.Debug.LogError(s.strip())
        
    def flush(self, s):
       pass

sys.stderr = UnityStderr()