# Installation

## Requirements

- Jellyfin 12.1.x on .NET 10.

## Manual installation

1. Build with `make package`, or download `JellyBlindTest.zip` from a plugin
   catalog.
2. Extract the ZIP into a folder named `JellyBlindTest_0.1.12.0` in Jellyfin's
   plugins directory.
3. Restart Jellyfin and open **Blind Test** from the main menu.

The folder name must match the version in `meta.json`. The game only selects
tracks visible to the current Jellyfin user.