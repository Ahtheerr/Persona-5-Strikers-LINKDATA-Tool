# Credits
LINKDATA encryption: [Ceathleann](https://github.com/neptuwunium/Cethleann)

# Usage
## LINKDATA

Extracting files:

Make sure LINKDATA.IDX and LINKDATA.BIN are in the same folder with the same name.

``P5SLDT.exe -linkdata LINKDATA.IDX -dec``

Remove the argument ``-dec`` for non-encrypted LINKDATA (Switch and PS4).

Packing files into LINKDATA:

Don't change the names of the files.

``P5SLDT.exe -linkdata [number].dat LINKDATA.IDX -enc``

Remove the argument ``-enc`` for non-encrypted LINKDATA (Switch and PS4).

## Text Files

Converting .dat file into .csv file:

``P5SLDT.exe -dat [number].dat``

Converting .csv to .dat:

``P5SLDT.exe -csv [number].csv``

# Known issues

- It will extract 200mb~ worth of files (It should be the same size as the .BIN), Text files ends at 8185.dat, with some non-text files between 5919.dat and 8185.dat.
- English files until 5912.dat are all multiples of 8, then 8158.dat, 8168.dat and 8178.dat (Not an issue, this helps dealing with the previous issue.)
