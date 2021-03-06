# OpenVR Volume Control

As of this writing, changing the volume from within VR involves pointing at a 2D volume control. I found that frustrating and difficult to do precisely, as I often wanted the volume at or near the lowest level, so this allows adjusting the volume by dragging a giant virtual slider with haptic feedback. It consists of a plugin to interface with Windows' CoreAudio, and a HapticRack and the LinearDrive example interaction from the SteamVR Unity plugin.

The audio gear on the table is from a paid asset package, and will not be included in this repository. The utility will still work if you don't have it - the gear is entirely cosmetic.

[Here's a video.](https://www.youtube.com/watch?v=vdGikLASFjM)

## Screenshots

![Action Shot](Screenshots/example.jpg)

## License

The Unity plugin I wrote to interface with system volume control is MIT licensed. Code from other sources, such as the [SteamVR Unity plugin](https://github.com/ValveSoftware/steamvr_unity_plugin/blob/master/LICENSE), and [Steam Audio Unity plugin](https://github.com/ValveSoftware/steam-audio/blob/master/LICENSE.md), are subject to their respective licenses.

## Credits

* [Hi-Fi mini components - PSR](https://assetstore.unity.com/packages/3d/props/electronics/hi-fi-mini-component-pack-110452)
* [Noble Savages by Jeris](http://dig.ccmixter.org/files/VJ_Memes/41913) (c) copyright 2013 Licensed under a Creative Commons Attribution Noncommercial  (3.0) license. Ft: NiGiD, Javolenus
* [commonGround by airtone](http://dig.ccmixter.org/files/airtone/58703) (c) copyright 2018 Licensed under a Creative Commons Attribution Noncommercial  (3.0) license.