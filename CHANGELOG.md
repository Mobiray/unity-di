# Changelog

## [1.3.1] - 2026-10-30
### Fixed
- Static state (installer flag and DI container) is reset on Play Mode start, so entering Play Mode with Domain Reload disabled no longer breaks registration

## [1.3.0] - 2026-10-30
### Changed
- `GameInstaller` alone now handles early injection: place it on every scene and the duplicate injects its scene in `Awake` before destroying itself
### Removed
- Removed `SceneLoadDetector`; no longer needed

## [1.2.3] - 2026-10-30
### Fixed
- Restored injection before `Awake`/`OnEnable` (regression since 1.2.0): the installer now injects its own scene during its `Awake`, and the restored `SceneLoadDetector` component injects any other scene before its objects run `Awake`
- An object is no longer injected twice when a scene is injected early and again on scene load

## [1.2.2] - 2026-10-28
### Removed
- Removed `RegisterSingleton<T>()` from `GameInstaller`; use `Register<T, T>()` instead
- Removed public manual injection methods (`InjectScene`, `InjectGameObject`, `InjectCurrentScene`) from `GameInstaller`
- Removed the `Dont Destroy On Load` toggle from `GameInstaller`; the installer now always persists between scenes (disabling it broke injection in later scenes)
- Removed unused internal code (`Resolve<T>`, `Clear`, static `DIContainer.Instance`)

### Fixed
- Corrected README usage examples and documented installer settings

## [1.2.1] - 2025-11-22
### Changed
- Version bump

## [1.2.0] - 2025-01-15
### Added
- Added logging control settings

## [1.1.1] - 2024-XX-XX
### Fixed
- Minor fixes

## [1.1.0] - 2024-XX-XX
### Added
- Added injecting on scene change

## [1.0.0] - 2024-XX-XX
### Added
- Initial release