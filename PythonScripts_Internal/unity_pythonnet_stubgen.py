import sys
import clr

# from UnityEngine import *


def run_stub_gen(packages: list[str], debug: bool = False):
    if debug:
        import ptvsd
        ptvsd.enable_attach(address=("127.0.0.1", 8080))
        ptvsd.wait_for_attach()
    
    print(packages)
    for i in packages:
        clr.AddReference(i)
        
        module = __import__(i)
        process_module_object(module)

def process_module_object(module):
    print(module)
    for key, value in module.__dict__.items():
        print(f"{key} = {value} (type: {type(value)})")
        


if __name__ == '__main__':
    asm_list = [
        'UnityEngine'
    ]
    run_stub_gen(asm_list)


