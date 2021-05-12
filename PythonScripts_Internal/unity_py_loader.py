import clr, sys, pickle, unity_pickler
clr.AddReference("UnityEngine")
clr.AddReference("System")
from UnityEngine import Debug

old_stdout = sys.stdout
old_stderr = sys.stderr

class UnityStdout:
    def write(self, s: str):
        if not (s.strip() == ""):
            Debug.Log(s.strip())
        
    def flush(self, s):
       pass

sys.stdout = UnityStdout()
