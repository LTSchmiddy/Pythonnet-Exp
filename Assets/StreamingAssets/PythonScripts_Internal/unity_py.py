import clr, sys, os, pathlib
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

def load_script_bundles(script_bundles_path: str):
    sb_path = pathlib.Path(script_bundles_path)
    for i in os.listdir(sb_path):
        if not i.endswith(".meta"):
            sys.path.append(sb_path.joinpath(i))
        
    print(f"{sys.path=}")
    