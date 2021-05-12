import os, sys, shutil

from pathlib import Path

START_DIR = Path("./InventoryPro/Scripts")
OUT_DIR = Path("./Editor/InventoryPro")

def construct_dir(read_path: Path, write_path: Path, in_editor_folder: bool):
    dirs = os.listdir(read_path)
    
    for fname in dirs:
        new_read = read_path.joinpath(fname)
        
        new_write = write_path.joinpath(fname)
        if fname.lower() == "editor":
            new_write = write_path
        
        print(f"{new_read=}, {in_editor_folder=}")
        if new_read.is_dir():

            if not new_write.exists():
                os.makedirs(new_write)
            
            sub_is_editor_folder = in_editor_folder or fname.lower() == "editor"
            
            construct_dir(new_read, new_write, sub_is_editor_folder)
            
        elif new_read.is_file() and in_editor_folder:
            shutil.move(new_read, new_write)
            
def clear_empty_dirs(dir: Path):
    for fname in os.listdir(dir):
        new_path = dir.joinpath(fname)
        
        if new_path.is_dir():
            clear_empty_dirs(new_path)
        
    if len(os.listdir(dir)) < 1:
        os.rmdir(dir)

construct_dir(START_DIR, OUT_DIR, False)
clear_empty_dirs(START_DIR)
clear_empty_dirs(OUT_DIR)