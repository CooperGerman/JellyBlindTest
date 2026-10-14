# JellyBlindTest

A Jellyfin plugin that adds a **Blind Test** page to the main menu. The first
version chooses local library tracks at random, plays a five-second preview,
and asks the player to type the song title.

## First version

- Five rounds per game.
- Random tracks are selected from the signed-in user's accessible Jellyfin
  Audio library.
- Tracks do not repeat within a game when enough tracks are available.
- The preview stops automatically after five seconds; it can be replayed.
- The answer is checked without case, accent, or punctuation differences.
- The song and artist are revealed after each answer.

The five-second preview length and five-round game length are constants in the
page script for now. Multiple choice, genre/popularity filters, and configurable
round settings can be added later.

## Build

Requires the .NET 10 SDK.

```bash
make package
```

The installable ZIP is written to `plugin/JellyBlindTest.zip`. The packaging
target also updates the checksum and timestamp in `manifest.json`.

## Install

Extract `plugin/JellyBlindTest.zip` into a folder named
`JellyBlindTest_0.1.0.0` in Jellyfin's plugins directory, then restart Jellyfin.
The folder name must match the version in `meta.json`.

For catalog publishing, the manifest currently points at the `main` branch of
`CooperGerman/JellyBlindTest`; publish the repository and ZIP there before
relying on catalog installation.