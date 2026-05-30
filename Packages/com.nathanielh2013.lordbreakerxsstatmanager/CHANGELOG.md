# Changelog

## [0.1.4] - 2026-05-30

### Added

- Grouping stat modfiers in the build popup
- Stat modifiers in the build popup now have icons taken from their scripts preview thumbnail in the project window.
- Updated editor utilities package and used more flexable BuilderMenu instead of BuilderPopup.

### Changed

- Custom stat modifier attribute allows setting the group of a stat modifier.

### Removed

- Unused stat modifier BuilderPopup now that it been replaced with BuilderMenu

## [0.1.3] - 2026-05-16

### Added

- Editor utilities as a required dependency.
- Stat Modifier BuilderPopup for a more advanced building menu.

### Changed

- Refactored the runtime code.
- Simplified the Stat Holder.


## [0.1.2] - 2026-05-09

### Added

- Package link to wiki documentation on github.
- Package link to this changelog on github.
- The start of the documentation of the Stat Manager.

### Fixed

- Null exception error that breaks stat editor when first opening the editor during a session (#1)
- Editor not showing created profiles after fixing issue 1 (#2).
- Created stats having a blank name instead of its default: "Stat {Stat Index}" (#3).
- Created stat not automatically being renamed when first created (#4).
- Stat profile not being focused and text selected when first created (#5). 

### Changed

- Changed package description to something that describes the package and its features.

### Removed

- Unused documentation files