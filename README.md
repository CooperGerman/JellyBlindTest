# JellyBlindTest

A Jellyfin plugin that adds a **Blind Test** link to Jellyfin's regular user
navigation and a separate Dashboard settings page. It chooses local library tracks at random,
plays a short preview, and asks the player to type the song title.

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

Multiple choice and genre/popularity filters can be added later.

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
`JellyBlindTest_0.1.27.0` in Jellyfin's plugins directory, then restart Jellyfin.
The folder name must match the version in `meta.json`.

For catalog publishing, the manifest currently points at the `main` branch of
`CooperGerman/JellyBlindTest`; publish the repository and ZIP there before
relying on catalog installation.