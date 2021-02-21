# PboViewer
Cross-platform PBO maker / unpacker

## GUI
The GUI is availble to all the platform.

Here is a screenshot of the window: 

![alt text](https://i.ibb.co/XDgpwTn/Pbo-Viewer-S4ywf-ENT6-M.png)

## CLI
You have two command for the CLI:
- packFolder, arguments: --path or -p, path of the folder to pack
- unpackFolder, arguments: --path or -p, path of the PBO to unpack

Here is one example for each command:
- `./PboViewer packFolder --path="C:\test"`
- `./PboViewer unpackFolder --path="C:\test.pbo"`
- `./PboViewer listFiles --path="C:\test.pbo"`

The return of `listFiles` is in this format: `Path: fileName, Size: fileSize, Data size: dataSize, Timestamp: timestamp, Packing method: packingMethod`
