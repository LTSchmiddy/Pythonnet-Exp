import importlib
import importlib.util
import inspect
import types
from System.Collections.Generic import List
from PythonEngine import PythonFile



def analyse_PythonFile(file: PythonFile, src: str):
    spec = importlib.util.spec_from_file_location(file.moduleName, src)
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    
    definedFunctions = []
    definedClasses = []
    
    for name, item in mod.__dict__.items():
        if isinstance(item, types.FunctionType) and item.__module__ == file.moduleName:
            # print(f"{name}: {type(item)}")
            definedFunctions.append(name)
            
        if inspect.isclass(item) and item.__module__ == file.moduleName:
            # print(f"{name}: {type(item)}")
            definedClasses.append(name)
            
    return definedFunctions, definedClasses