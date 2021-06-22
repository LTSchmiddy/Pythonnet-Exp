import sys, imp, anon_func

from PythonEngine import PythonEvent


def setup_python_event(eventObj: PythonEvent):
    new_module = imp.new_module('inline_module')
    exec(eventObj.importCode, new_module.__dict__)
    new_function = anon_func.func(("objectRefs", "self=None"), eventObj.eventCode, False, __globals = new_module.__dict__)
    
    eventObj.eventModule = new_module
    eventObj.eventFunction = new_function
    