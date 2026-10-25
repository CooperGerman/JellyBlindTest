# JellyBlindTest

A Jellyfin plugin that adds a **Blind Test** native Home tab and a separate
Dashboard settings page. It chooses local library tracks, plays a short preview,
and asks the player to type the song title.

## Dependencies

- **File Transformation** is required to inject the Home-tab registration script
  without modifying Jellyfin Web files. Install it from the [File Transformation
  repository](https://github.com/IAmParadox27/jellyfin-plugin-file-transformation).
- **Jellyfin Enhanced** is required for its Jellyfin 12 native Home-tab registry.
  Install it from the [Jellyfin Enhanced repository](https://github.com/n00bcodr/Jellyfin-Enhanced).
- **Music Discovery** is optional. Install the [CooperGerman fork](https://github.com/CooperGerman/JellyMusicDiscovery)
  at a version newer than `0.1.15.0` to enable ranked-song modes. Those modes use
  the cached Deezer popularity index; library and playlist modes work without it.

## First version

- Administrators can open **Settings** from the game page to configure rounds,
  preview duration, answer time, and the post-answer playback interval.
- Random tracks are selected from the signed-in user's accessible Jellyfin
  Audio library.
- Tracks do not repeat within a game when enough tracks are available.
- Starting a round immediately plays a preview for the configured duration.
- An answer countdown opens after the preview; unanswered rounds are revealed
  automatically when time expires.
- After reveal, playback continues for the configured interval before the next
  round starts automatically.
- The answer is checked without case, accent, or punctuation differences.
- The song, artist, and album artwork are revealed after each answer when
  artwork is available.

The menu icon's editable SVG source is in `plugin/source/Web/blindtest-icon.svg`.

## Build

Requires the .NET 10 SDK.

```bash
make package
```

The installable ZIP is written to `plugin/JellyBlindTest.zip`. The packaging
target also updates the checksum and timestamp in `manifest.json`.

## Install

Extract `plugin/JellyBlindTest.zip` into a folder named
`JellyBlindTest_<version>` in Jellyfin's plugins directory, using the version in
`meta.json`, then restart Jellyfin.

For catalog publishing, the manifest currently points at the `main` branch of
`CooperGerman/JellyBlindTest`; publish the repository and ZIP there before
relying on catalog installation.