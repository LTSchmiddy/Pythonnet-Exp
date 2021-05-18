from UnityEngine import Debug


class HelloAlex:
    value: str
    def __init__(self):
        self.value = "poopee"
        
        print("ALEX CLASS");
        
    def check(self):
        Debug.Log(f"{self.value=}")