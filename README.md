# Ambient lights mod
This mod aims to modify the default lightning for interiors with windows, making them brighter so you don't need a light source during midday to move around, and also more reflective of what's happening outside (time of day, weather, and sun position).
These lights are just ambiental and not "real" lights, so to craft or do things at night you will still need a lightsource just like in the base game.

## Updates
### v0.9
* Fixed issue where lighting took an in-game minute to render on some locations.
* Added options to control aurora powered lights

## Main Features
* Lighting intensity (how bright the light is), range (how far the light reaches) and color changes depending of what's happening outside.
* Clear and light cloudy weathers will have a warmer color.
* Foggy weathers are greenish, blizzards are more dark blue, and overcast weathers are more gray-ish.
* Makes most interiors brighter by default.
* Mod options to change default intensity and range even mid-game.
* Option to make nights as dark as the base game, or even brighter than the mod's default.
* Window orientations. For example, with clear weather during dawn, the east side of a place will be brighter and will have a warmer color than the west side.
* Options to change intensity and range of aurora powerd lights.
* Option to disable flicker of lights during aurora.
* Optional debugging keys to check if everything's working and to make easier the process of changing the lighting settings (disabled by default).

## Installation
* If you haven't already done so, install the [Mod Loader](https://github.com/zeobviouslyfakeacc/ModLoaderInstaller) by **zeobviouslyfakeacc** and patch your game.
* You should also have [ModSettings](https://github.com/zeobviouslyfakeacc/ModSettings/releases/tag/v1.1) by **zeo** installed in order to be able to change the mod's default options. Download the .dll and put it in your mods folder.
* Head over to the [Releases](https://github.com/Xpazeman/tld-ambient-lights/releases/) page and download Ambient_Lights_vX.X.zip
* Unzip this file and move AmbientLights.dll and the ambient-lights folder into your mods directory (it's in <path_to_TheLongDark_installation_folder>/mods ), it should look like this (of course you might have more mods installed):
* Make sure your game brightness it's set to default, otherwise it might be too bright.

![Your mod folder](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/folder.jpg "Your mod folder")

## Debug Controls
* Debug keys can be enabled in the Mod setting menu option for Ambient Lights.
* L key toggles the ambient lighting on/off
* LeftCtrl+L reloads the lighting data, useful to make changes to the json and see them quickly.

## Known Issues
* There is a problem with transitions when there's a period and a weather change happen close to each other.
* Natural interiors such as caves aren't mapped yet.

## Editing the mod's lighting
Since AmbientLights is based on JSON files, it's not complicated to edit the mod's default lighting values.
Inside the ambient-lights folder there are three types of files: scene_*, global_sets, and global_periods.

* global_periods is used to define on what period we are so global_sets can use that data. New periods can be added, but they need to be added to global_sets as well.
* global_sets is used to define the lighting for each period, weather and orientation. Each orientation has the data to render the light: color, intensity and range. There's also a shadow type paramenter, but it's not used yet.
* scene_* are the files where the windows are defined (position, orientation data from the period/weather it will use, a cover modifier that reduces intensity for that light and a size modifier that changes the maximum range at which the light is cast), as well as the multipliers for range and intensity for that particular scene. In this file, the default sets can also be overriden. Inside the "periods" object we can use the same structure as in global_sets.json

## Screenshots

[Ambient Lights on imgur](https://imgur.com/a/a3T82ZK)

![Clear dawn at Camp Office](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/example-1.jpg "Clear dawn at Camp Office")

![Weather comparison](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/lighthouse_weathers.jpg "Weather comparison")

![Clear dawn at Trappers](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/example-2.jpg "Clear dawn at Trappers")

![Early dawn at PV Farm](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/example-3.jpg "Early dawn at PV Farm")

![Tweaked aurora powered lights](https://raw.githubusercontent.com/Xpazeman/tld-ambient-lights/master/screenshots/aurora_1.jpg "Tweaked aurora powered lights")
