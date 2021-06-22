import pickle, io
from typing import TextIO

from System import Byte


class UnityPickler(pickle.Pickler):
    pass

class UnityUnpickler(pickle.Unpickler):
    pass


def get_fullname(obj: object):
    klass = obj.__class__
    module = klass.__module__
    if module == 'builtins':
        return klass.__qualname__ # avoid outputs like 'builtins.str'
    return module + '.' + klass.__qualname__

def get_module(obj: object):
    klass = obj.__class__
    module = klass.__module__
    return module



def dumps(inst, *args, **kwargs) -> bytes:
    dumpto = io.BytesIO()
    
    unityPickler = UnityPickler(dumpto)
    unityPickler.dump(inst)
    # print(dump.getvalue())
    
    return dumpto.getvalue()
    
    
def loads(data: list[int], *args, **kwargs):
    loadbytes = io.BytesIO()
    for i in data:
        loadbytes.write(i.to_bytes(1, 'little'))

    loadbytes.flush()
    loadbytes.seek(0, io.SEEK_SET)
    unityUnpickler = UnityUnpickler(loadbytes)
    
    return unityUnpickler.load()

def loads_into(data: list[int], obj: object, *args, **kwargs):
    loadbytes = io.BytesIO()
    for i in data:
        loadbytes.write(i.to_bytes(1, 'little'))

    loadbytes.flush()
    loadbytes.seek(0, io.SEEK_SET)
    unityUnpickler = UnityUnpickler(loadbytes)
    
    obj.__dict__.update(unityUnpickler.load().__dict__)