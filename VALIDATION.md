# AmbientLights 3.0.1 compatibility validation

Validated locally on November 8, 2026 against the installed Windows game:

| Component | Version |
| --- | --- |
| The Long Dark Survival | 2.60 |
| Unity | 6000.0.60f1 |
| MelonLoader | 0.7.2 Open-Beta, net6 |
| ModSettings | 2.2.5 |

## Completed checks

* Release build against the installed game's generated interop assemblies: zero warnings and zero errors.
* Metadata inspection: all 25 Harmony patch targets resolve uniquely, and their injected parameters match the current method signatures.
* System.Text.Json parsing: all 99 packaged JSON files parse successfully, including the repaired scene files. Comments and trailing commas are supported.
* Installed the DLL and configuration directory into the local game's `Mods` folder and launched the game.
* `MelonLoader/Latest.log` reports `Game Version: 2.60` and `AmbientLights v3.0.1`; startup progressed through support-module loading and mod initialization without a reported AmbientLights exception or patch error.
* Installed DLL SHA-256 matches the packaged build: `5CD9DA890D7B16C4286065A428F774A59EE63EC7D827C4868A7852D7D9DAD5AB`.

## Still needs gameplay verification

The computer-use helper could not connect (`Computer Use native pipe is unavailable`, OS error 2), including after retrying and resetting its session. No playable interior was inspected. Startup success and metadata checks do not establish visual correctness.

Manual verification should cover:

1. Enter a mapped interior such as Camp Office and confirm the emitters load and settings affect brightness.
2. Exit, re-enter, then save/reload to check that lights neither duplicate nor retain the previous scene's state.
3. Check day/night, weather, and aurora transitions.
4. Test both InteriorLightingManager and DarkLightingManager interiors; compare True Sun enabled and disabled.
5. Enter an unmapped interior and confirm the game's lighting remains in use.

Iron Road interiors have not been newly mapped. Wintermute and Episode Five have not been validated.
