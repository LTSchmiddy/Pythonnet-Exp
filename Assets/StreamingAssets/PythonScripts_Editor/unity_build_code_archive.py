import zipfile, pathlib, os

# import clr
# clr.AddReference("UnityEditor")

from System import *

def assemble_python_code_archive(rootDirectory: str, outputPath: str):
    added_inits = []
    if not pathlib.Path(outputPath).parent.exists():
        os.makedirs(pathlib.Path(outputPath).parent)
    
    outArchive = zipfile.ZipFile(outputPath, 'w');
    
    for pathName, dirs, fileNames in os.walk(rootDirectory):
        # Skip StreamingAssets:
        if pathName.replace("\\", "/").startswith("./Assets/StreamingAssets"):
            continue
        
        path = pathlib.Path(pathName)
        # if pathlib.Path("StreamingAssets") in path.parents:

        
        for fileName in fileNames:
            if not fileName.endswith(".py"):
                continue
            
            file = path.joinpath(fileName)
            
            outArchive.write(file, str(file)[len("Assets/"):])
            
            # compile_file = outArchive.open(str(file), 'w')
            # py_compile.compile(file, compile_file, dfile= file, doraise=False)
    
    # We'll construct inits in after all the real files are created:
    for pathName, dirs, fileNames in os.walk(rootDirectory):
        # Skip StreamingAssets:
        if pathName.replace("\\", "/").startswith("./Assets/StreamingAssets"):
            continue
        
        path = pathlib.Path(pathName)
        
        has_py = False
        for fileName in fileNames:
            if fileName.endswith(".py"):
                has_py = True
                break
            
        # We need to have module init files for all sub-folders in the archive.
        # If a given folder doesn't have one, we'll create a blank on in the archive:
        if has_py and "__init__.py" not in fileNames:
            construct_inits(path, outArchive, added_inits)
            # for i in outArchive.namelist():
                # print(i)
            for i in path.parents:
                if str(i).strip() != ".":
                    # print(i)
                    construct_inits(i, outArchive, added_inits)
                
    # print(outArchive.namelist())
    outArchive.close()            

def construct_inits(dir_path: pathlib.Path, archive: zipfile.ZipFile, added_inits: list[str]):
    init_file = dir_path.joinpath("__init__.py")
    archive_dest = str(init_file)[len("Assets/"):]
    
    if not archive_dest in added_inits:
        # print(f"Creating '{init_file}' in archive...")
        archive.writestr(archive_dest, "")
        added_inits.append(archive_dest)
    # else:
        # print (f"{archive_dest} in archive")
    
    
    
    