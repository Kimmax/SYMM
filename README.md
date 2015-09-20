# SYMM
Simple Youtube Music Manager
SYMM is a project which is able to download all videos of a youtube channel or playlist (of course you can download a single video, if you're lame enough) and converts it to high quality audio afterwards using the power of ffmpeg.

To be able to use your own build you have to register at https://console.developers.google.com and generate a "Browser Key" for the Youtube Data API v3. This key needs to be set at "SYMM/src/SYMM Frontend WPF/app.config", block "SYMM_Frontend_WPF.Properties.Settings", key "YTDataAPIv3Key". You can also use the settings editor of your IDE.

This projects aims to replace all these shitty "Youtube converters" with ship with tons of adware and sometimes with malware or aren't functional at all. In the future automatic synchronization for USB devices may be added, as well as id3 tag editing. Also keeping up with new uploads on specified channels would be cool.