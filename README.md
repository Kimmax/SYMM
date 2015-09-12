# SYMM
Simple Youtube Music Manager
SYMM is a project which is able to download all videos of a youtube channel or playlist (of course you can download a single video, if you're lame enough) and converts it to high quality audio afterwards using the power of ffmpeg.

To be able to use your own build you have to register at https://console.developers.google.com and generate a "Browser Key" for the Youtube Data API v3. This key needs to be the first and only parameter for "SYMMHandler" at "SYMM/src/SYMM Frontend WPF/Pages/Downloaden.xaml", column 18, "INSERT-API-KEY-HERE". This will change in the future and be moved to a central configuration file.