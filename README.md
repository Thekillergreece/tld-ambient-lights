# Ambient lights
This mod aims to modify the default lightning for interiors with windows, making them brighter so you don't need a light source during midday to move around, and also more reflective of what's happening outside (time of day, weather, and sun position).
These lights are just ambiental and not "real" lights, so to craft or do things at night you will still need a lightsource just like in the base game.

## Installation
This compatibility update targets **The Long Dark Survival 2.60**, **MelonLoader 0.7.2**, and **ModSettings 2.2.5** on Windows (IL2CPP, .NET 6).

1. Install [MelonLoader](https://github.com/LavaGang/MelonLoader) and [ModSettings](https://github.com/DigitalzombieTLD/ModSettings). Launch the updated game once so MelonLoader generates its interop assemblies.
2. Close the game. Extract the release ZIP into the game's `Mods` directory. It must contain both `AmbientLights.dll` and the `ambient-lights` directory next to it.
3. Start Survival and open **Options > Mod Settings > Ambient Lights Settings**.

Existing scene mappings are retained. New interiors without a matching `scene_*.json` use the game's lighting; this update does not add Iron Road interior mappings. Wintermute and Episode Five are not validation targets.

## Building

Install the .NET 6 SDK or a newer SDK capable of targeting .NET 6. The default game path is `C:/Program Files (x86)/Steam/steamapps/common/TheLongDark`. For another Steam library, pass the game directory explicitly:

```powershell
dotnet build AmbientLights.csproj -c Release '-p:TheLongDarkPath=E:/SteamLibrary/steamapps/common/TheLongDark'
```

You can also set the `TLD_GAME_PATH` environment variable. The build requires MelonLoader's generated assemblies and `Mods/ModSettings.dll` from your installation. It outputs `bin/Release/net6.0/AmbientLights.dll` and the accompanying `ambient-lights` folder. Building does not install files into the game.

To check Harmony targets and parse the shipped JSON files against a local installation (requires the .NET 9 SDK):

```powershell
dotnet run --project tools/CompatibilityCheck -- 'C:/Program Files (x86)/Steam/steamapps/common/TheLongDark' 'bin/Release/net6.0/AmbientLights.dll'
```

This metadata check does not execute the Unity player. In-game validation should cover entering and leaving mapped interiors, loading a save, changing presets, day/night and weather transitions, and both lighting-manager types. True Sun remains experimental.

## Main Features
* Lighting intensity (how bright the light is), range (how far the light reaches) and color changes depending of what's happening outside.
* Clear and light cloudy weathers will have a warmer color.
* Foggy weathers are greenish, blizzards are more dark blue, and overcast weathers are more gray-ish.
* Makes most interiors brighter by default.
* Mod options to change default intensity and range even mid-game.
* Option to make nights as dark as the base game, or even brighter than the mod's default.
* Window orientations. For example, with clear weather during dawn, the east side of a place will be brighter and will have a warmer color than the west side.
* Window panels, ambient light, fill lights and lightshafts are controlled by the Ambient Lights Manager, so all light sources in the scene are affected by weather and sun position.
* Light shafts now fade out on every weather type except for clear and partly cloudy instead of shining all the time, to reflect the outside environment better.

## Debug Controls
* Debug keys can be enabled in the Mod setting menu option for Ambient Lights.
* Shift+L toggles the ambient lighting on/off

## Known Issues
* Natural interiors such as caves aren't mapped yet.
* Some features don't work on some interiors as they use a different (static & baked) lighting system.
* Some interiors might have a brighter than normal default ambient light.
* Some larger spaces might be too dark/too bright, you can tweak this by going to the scene config in the ambient-lights folder and tweaking the intensity_multiplier and range_multiplier at the bottom of the file.
* Transition to night time with increased night brightness is instant and not progressive.

## Editing the mod's lighting
Since AmbientLights is based on JSON files, it's not complicated to edit the mod's default lighting values.
Inside the ambient-lights folder there are three types of files: scene_*, global_sets, global_periods, and weather_sets.

* global_periods is used to define on what period we are so global_sets can use that data. New periods can be added, but they need to be added to global_sets as well.
* global_sets is used to define the lighting for each period and orientation. Each orientation has the data to render the light: color, intensity and range. There's also a shadow type paramenter, but it's not used yet.
* weather_sets is used to define how each weather affects color and intensity of the lights. rMod, bMod and gMod modify the tone of the light, sMod modifies the saturation of the color, and vMod the brightness/intensity.
* scene_* are the files where the windows are defined (position, orientation data from the period/weather it will use, a cover modifier that reduces intensity for that light and a size modifier that changes the maximum range at which the light is cast), as well as the multipliers for range and intensity for that particular scene (the options object). In this file, the default sets can also be overriden. Inside the "periods" object we can use the same structure as in global_sets.json

## Uninstalling
* Go into your mods folder (<path_to_TheLongDark_installation_folder>/mods) and delete **AmbientLights.dll** and the **ambient-lights** folder.

## Screenshots

[Ambient Lights on imgur](https://imgur.com/a/r1GOzKt)

![Clear dawn at Camp Office](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/example-1.jpg "Clear dawn at Camp Office")

![Weather comparison](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/lighthouse_weathers.jpg "Weather comparison")

![Clear dawn at Trappers](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/example-2.jpg "Clear dawn at Trappers")

![Early dawn at PV Farm](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/example-3.jpg "Early dawn at PV Farm")

![Tweaked aurora powered lights](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/aurora_1.jpg "Tweaked aurora powered lights")

## Updates

### v3.0.1
* Updated for TLD Survival 2.60 and MelonLoader 0.7.2; replaced the removed TinyJSON API with System.Text.Json.
* Fixed invalid Harmony instance arguments and interior-lighting manager initialization.
* Cleared scene state and created lights on unload; deferred lighting updates until weather data is ready.
* Fixed shared orientation values being multiplied once per emitter.
* Repaired missing commas in 11 scene configurations and added build/compatibility checks.

### v2.8.1
* FIX: Tweaked config files for Dam and Lower Dam so windows aren't so bright when using high ambience level settings
* FIX: Added Ash Canyon interiors
* REMOVED: Removed mines and caves configs so the mod isn't active in them

### v2.8
* Fixed a crash in some interiors

### v2.7
* Updated to work with TLD 2.06

### v2.6
* Fixed vanilla lightshafts so they doesn't look like a rave in some locations and so they also adapt properly to outside weather.

### v2.5
* Updated to work with TLD 1.95 - Fury, then silence.

### v2.4
* Mapped Ash Canyon interiors
* Fixed some glass illumination issues whil under sunlight
* Added extra decimal to mod settings
* Added verbose option to settings for troubleshooting

### v2.3
* Fixed decals and glass being too bright in low light environments

### v2.2
* Fixed for TLD 1.93

### v2.1
* Major rewrite to make it work with newer versions
* NEW: Added True Sun Lightshafts, this makes the sun come through the windows, making lightshafts more realistic. Since this manipulates the scene heavily, it can lead to performance hits. It's toggeable in the settings menu. If you enter a scene and it looks burned out, it's because the scene manipulation has failed for some reason (usually due to performance during scene load), this should be fixed by exiting the interior and going in again.
* NEW: Added light flickering to some of the weather types, to represent better what's going on outside.
* FIX: Fixed "aurora ambience" coming up too early, now it should come progressively as the aurora progresses.
* FIX: Tuned values all over the mod.

### v1.0.2
* FIX: Fixed issue that was throwing errors on some locations.

### v1.0.1
* FIX: Fixed issue with some lightshafts not reacting properly to cloudy weathers (they stayed lit like in the base game).

### v1.0.0
Major overhaul:
* Changed time system to be based on game states instead of clock, makes the mod compatible with any other mod that changes day duration like Solstice.
* NEW: Added windows, ambient light, fill lights and lightshafts to the Ambient Lights Manager, so all light sources in the scene are affected by weather and sun position.
* NEW: Added a preset system for quick tweaking. It overwrites the settings, so it's better to select one as base and tweak from there.
* NEW: Added controls to general ambient light level. Setting this to 0 makes all ambient light disapear, so only light sources illuminate the scene (i.e. makes the Lower Dam pitch black except for the offices)
* NEW: Added controls for fill lights. These are those spots of lights, usually coloured, that are used in scenes to highlight parts of it. With the new sliders you can control how much saturated (as in how colorful the light is), and the intensity level of it.
* NEW: Added an experimental feature: making window lights cast shadows. It's still basic, and might make the game take a good performance hit, but with a 970 gtx, I get around 60 FPS with everyhting maxed out and this toggled on.
* NEW: Made the light shafts disappear on every weather type except for clear and partly cloudy, to reflect the outside environment better. There are some scenes where this doesn't work properly, but I'll try to fix in next releases.
* REM: Removed the Aurora Lights tweaking, as I felt they were a bit out of context inside Ambient Lights.

### v0.9.7
* FIX: Updated to work with TLD 1.42
* FIX: Reduced daylight brightness to compensate for interior lighting changes.

### v0.9.5
* FIX: Fixed new issue with incorrect lights loading on some locations.
* NEW: Added multipliers to aurora lights intensity and range manually, on a scene by scene basis.

### v0.9
* FIX: Fixed issue where lighting took an in-game minute to render on some locations.
* NEW: Added options to control aurora powered lights

