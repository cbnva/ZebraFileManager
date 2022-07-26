# Zebra File Manager
Manages files on Zebra printers. Developed/Tested on QLn320 and ZQ620

## State ##
Alpha product. It'll probably work, but no guarantees.

I'm probably not going to fix problems I'm not having, but you're more than welcome to.

I wrote this as an internal tool for work, and it does what I need.

There are very few error checks, comments, or good coding practices. Again, feel free to improve it.

## Features ##
- Connects over IP (LAN/WLAN) to port 9100
- USB connection
  - No Zebra driver required. Interfaces directly with USB Printing Support.
- Drag/drop files to/from printer
- Rename/Delete Files
- "Factory Reset"
  - Deletes all files on E: drive and changes settings back to default.
- Change settings
- Generate settings change script
  
## Planned ##
- Run ZPL/CPCL scripts
  
## Coding ##
- Only references a few NuGet packages; no Zebra dependencies
- Could be used via powershell
  - `[System.Reflection.Assembly]::LoadFile($path)`

## Screenshots ##
![Main Form](/../screenshots/MainForm.png?raw=true "Main Form")
![Settings](/../screenshots/Settings.png?raw=true "Settings")
