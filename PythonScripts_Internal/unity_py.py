import clr, sys, pathlib
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

# Setting sys.executable to the real python, and storing the path to the game executable separately.
# Helps with certain modules such as debugpy and multiprocessing.
# game_executable = sys.executable

# new_exec_path = pathlib.Path(sys.path[0]).joinpath("python.exe")
# print(new_exec_path.exists())

# sys.executable = str(new_exec_path)
# print(sys.executable)