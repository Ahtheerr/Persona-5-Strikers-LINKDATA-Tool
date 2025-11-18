# P5SLDT Utility Tool

A command-line utility for managing and converting **LINKDATA** and **Text Files** for a game (likely *Persona 5 Strikers* based on the name).

## Credits

| Component | Author | Repository |
| :--- | :--- | :--- |
| **LINKDATA** Encryption | Ceathleann | [neptuwunium/Cethleann](https://github.com/neptuwunium/Cethleann) |

-----

## 🛠️ Usage

### 📦 LINKDATA Operations

The primary executable is `P5SLDT.exe`.

#### **Extracting Files**

1.  Ensure **`LINKDATA.IDX`** and **`LINKDATA.BIN`** are in the same folder and have the exact same base name.

2.  Run the extraction command:

    ```bash
    P5SLDT.exe -linkdata LINKDATA.IDX -dec
    ```

| Argument | Description | Platform Note |
| :--- | :--- | :--- |
| **`-linkdata LINKDATA.IDX`** | Specifies the index file to use. | Required |
| **`-dec`** | **Decrypts** the files before extraction. | **Only needed for PC LINKDATA.** |
| **`-everything`** | Extracts all files, ignoring file structure checks (includes broken, redundant, and non-text files). | Optional |
| **`-eng`** | Extracts only files identified as **English** text files. | Optional |

#### **Packing Files**

1.  **Do not change the names** of the extracted files you wish to pack.

2.  Run the packing command:

    ```bash
    P5SLDT.exe -linkdata [number].dat LINKDATA.IDX -enc
    ```

<!-- end list -->

  * **Encryption Note:** Remove the argument **`-enc`** if you are packing for **Switch** or **PS4** (non-encrypted) **LINKDATA**.

-----

### 📝 Text File Conversion

The tool handles conversion between the game's `.dat` format and standard `.csv` for easier editing.

  * **Converting `.dat` to `.csv`:**

    ```bash
    P5SLDT.exe -dat [number].dat
    ```

  * **Converting `.csv` to `.dat`:**

    ```bash
    P5SLDT.exe -csv [number].csv
    ```

-----

## ⚠️ Known Issues & File Structure Notes

  * **Extraction Size:** The tool extracts approximately **200 MB** worth of files (this size should match the size of the original `.BIN` file).
  * **Text File Range:** Text files generally end at **`8185.dat`**.
      * There are a few **non-text files** interspersed between `5919.dat` and `8185.dat`.
  * **English File Indexing (Helpful Pattern):**
      * English files up to `5912.dat` are all files whose index numbers are **multiples of 8**.
      * After this, the notable English files are `8158.dat`, `8168.dat`, and `8178.dat`. (This pattern can be useful for targeted extraction.)