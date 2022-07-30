# HP Difference Gauge

## Purpose
This is a plugin for [XIVLauncher/Dalamud](https://github.com/goatcorp/FFXIVQuickLauncher) that shows a configurable bar gauge of HP difference between two enemies.  Useful for Hand of Pain-style mechanics, or simultaneous enrages if you don't have functional eyes.  Open the config window to see a sample gauge and move/size it.  Includes a single default gauge config for DSR phase 6.

![Plugin Screenshot](/docs/Images/image1.png)

This is a quick and dirty implementation, and has the following imitations:

- Only works when both enemies are in your aggro list (the gauge won't show when you or one of the enemies are dead).  It's too much work to sort through identically-named actors to bother changing this.
- You have to find the TerritoryType of the instance with the enemies to set it up.  Look in the Dalamud data window, debug windows for other plugins, the sheets, etc.  May add zone dropdown at some point.
- The gauge only looks right at a certain height value for each Dalamud font size.  Set the height that looks good, and then adjust the width as desired.

Like all other third party stuff, SE could discipline you if they find out you're using this plugin.  It's pretty benign (just reads the aggro list data and checks whether those actors have the specified names, then reads their HP), but I don't make any warranty about safety or efficacy.

## Installation
This is not a main repo plugin.  Can be installed from [my third party plugin repo](https://github.com/PunishedPineapple/DalamudPluginRepo).

## License
Code and executable are covered under the [MIT License](../LICENSE).  Final Fantasy XIV (and any associated data used by this plugin) is owned by and copyright Square Enix.
