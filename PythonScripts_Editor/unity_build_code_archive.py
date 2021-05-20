import zipfile, pathlib, os

def assemble_python_code_archive(rootDirectory: str, outputPath: str):
    outArchive = zipfile.ZipFile(outputPath, 'w');
    
    for pathName, dirs, fileNames in os.walk(rootDirectory):
        path = pathlib.Path(pathName)
        for fileName in fileNames:
            if not fileName.endswith(".py"):
                continue
            
            file = path.joinpath(fileName)
            
            outArchive.write(file, str(file)[len("Assets/"):])
            
            # compile_file = outArchive.open(str(file), 'w')
            # py_compile.compile(file, compile_file, dfile= file, doraise=False)
            
    outArchive.close()
    